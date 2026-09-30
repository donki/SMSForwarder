using Android.App;
using Android.Content;
using SMSForwarder.Models;
using SMSForwarder.Services;
using AndroidSmsManager = Android.Telephony.SmsManager;
using Application = Android.App.Application;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Nucleo de reenvio compartido por los dos caminos de entrada de SMS:
    ///  - <see cref="SmsReceiver"/> (broadcast SMS_RECEIVED) cuando la app NO es la de SMS por defecto.
    ///  - <see cref="SmsDeliverReceiver"/> (broadcast SMS_DELIVER) cuando SI es la app por defecto.
    /// Contiene la deteccion de duplicados/bucles y el envio. Una sola fuente de verdad.
    /// </summary>
    public static class ForwardingCore
    {
        private static readonly DuplicateFilter Duplicates = new(TimeSpan.FromSeconds(5));

        public static void Forward(Context context, string sender, string messageBody)
        {
            // Evita reenvios duplicados en un corto periodo de tiempo (multipart, doble broadcast, etc.)
            if (Duplicates.IsDuplicate(sender, messageBody, DateTime.Now))
            {
                SafeLog("Mensaje duplicado detectado, no se reenvia.");
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(messageBody))
                {
                    SafeLog("Mensaje vacio, no se reenvia");
                    return;
                }

                var packageName = context.PackageName;
                var prefsName = $"{packageName}_preferences";
                var prefs = context.GetSharedPreferences(prefsName, FileCreationMode.Private);
                if (prefs == null)
                {
                    SafeLog("Error: No se pudo acceder a las preferencias");
                    return;
                }

                // Los destinos completos (con sus filtros); si solo hay el formato viejo, se leen los numeros.
                var destinations = ForwardDestinations.Parse(
                    prefs.GetString(ForwardDestinations.DestinationsKey, null),
                    prefs.GetString(ForwardDestinations.PhonesKey, null));

                // Quien recibe el mensaje, bucles incluidos, lo decide ForwardingRules (codigo puro, con pruebas).
                var (decision, targets) = ForwardingRules.Decide(destinations, sender, messageBody);
                if (decision != ForwardDecision.Forward)
                {
                    SafeLog($"No se reenvia: {decision} ({destinations.Count} destinos configurados, remitente {sender}).");
                    return;
                }
                SafeLog($"Procesando reenvio a {targets.Count} de {destinations.Count} numeros");

                var forwardedMessage = ForwardingRules.BuildMessage(sender, messageBody);

                var successCount = 0;
                var errorCount = 0;
                foreach (var phone in targets.Select(d => d.Phone))
                {
                    try
                    {
                        SendSms(phone, forwardedMessage);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        SafeLog($"Error enviando a {phone}: {ex.Message}");
                    }
                }
                SafeLog($"Reenvio completado - Exitos: {successCount}, Errores: {errorCount}");
            }
            catch (Exception ex)
            {
                SafeLog($"Error general en Forward: {ex.Message}");
            }
        }

        private static void SendSms(string phoneNumber, string message)
        {
#pragma warning disable CS0618
            using var smsManager = AndroidSmsManager.Default;
#pragma warning restore CS0618
            if (smsManager == null)
            {
                SafeLog("No se pudo obtener el SmsManager");
                return;
            }

            var sentIntent = PendingIntent.GetBroadcast(
                Application.Context, 0, new Intent("SMS_SENT"),
                PendingIntentFlags.OneShot | PendingIntentFlags.Immutable);

            if (message.Length > 160)
            {
                var parts = smsManager.DivideMessage(message);
                if (parts != null && parts.Count > 0)
                {
                    var sentIntents = new List<PendingIntent>();
                    for (int i = 0; i < parts.Count; i++)
                    {
                        sentIntents.Add(PendingIntent.GetBroadcast(
                            Application.Context, i, new Intent("SMS_SENT"),
                            PendingIntentFlags.OneShot | PendingIntentFlags.Immutable)!);
                    }
                    smsManager.SendMultipartTextMessage(phoneNumber, null, parts, sentIntents, null);
                }
            }
            else
            {
                smsManager.SendTextMessage(phoneNumber, null, message, sentIntent, null);
            }
            SafeLog($"SMS enviado a {phoneNumber}");
        }

        private static void SafeLog(string message)
        {
            try { System.Diagnostics.Debug.WriteLine($"[Forwarding] {DateTime.Now:HH:mm:ss}: {message}"); }
            catch { }
        }
    }
}
