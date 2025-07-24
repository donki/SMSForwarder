using Android.App;
using Android.Content;
using Android.Runtime;
using System.Text.Json;
using AndroidSmsManager = Android.Telephony.SmsManager;
using AndroidSmsMessage = Android.Telephony.SmsMessage;
using Application = Android.App.Application;

[assembly: UsesPermission(Android.Manifest.Permission.ReceiveSms)]
[assembly: UsesPermission(Android.Manifest.Permission.SendSms)]
[assembly: UsesPermission(Android.Manifest.Permission.ReadSms)]
namespace SMSForwarder.Platforms.Android
{
    [Register("com.organiccoating.smsforwarder.SMSReceiver")]
    [BroadcastReceiver(
        Enabled = true,
        Exported = true,
        Label = "SMS Receiver",
        Name = "com.organiccoating.smsforwarder.SMSReceiver")]
    [IntentFilter(
        new[] { "android.provider.Telephony.SMS_RECEIVED" },
        Categories = new[] { "android.intent.category.DEFAULT" },
        Priority = (int)IntentFilterPriority.HighPriority)]
    public class SmsReceiver : BroadcastReceiver
    {
        private static string? _lastSender;
        private static string? _lastBody;
        private static DateTime _lastReceived = DateTime.MinValue;

        public override void OnReceive(Context? context, Intent? intent)
        {
            try
            {
                if (context == null || intent == null || intent.Action != "android.provider.Telephony.SMS_RECEIVED")
                {
                    return;
                }

                SafeLog("SMS recibido - procesando");

                var bundle = intent.Extras;
                if (bundle == null) return;

                var pdusObj = bundle.Get("pdus");
                if (pdusObj == null) return;

                // Conversión segura de PDUs
                Java.Lang.Object[]? pdus = null;
                try
                {
                    pdus = (Java.Lang.Object[]?)pdusObj;
                }
                catch
                {
                    SafeLog("Error al convertir PDUs");
                    return;
                }
                if (pdus != null && pdus.Length > 0)
                {
                    var format = bundle.GetString("format") ?? "3gpp";

                    foreach (var pdu in pdus)
                    {
                        ProcessPdu(context, pdu, format);
                    }
                }
            }
            catch (Exception ex)
            {
                SafeLog($"Error en OnReceive: {ex.Message}");
            }
        }

        private void ProcessPdu(Context context, Java.Lang.Object pdu, string format)
        {
            try
            {
                if (pdu == null) return;

                var pduBytes = (byte[])pdu;
                AndroidSmsMessage? sms = null;

                // Crear mensaje SMS de forma compatible
                try
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    sms = AndroidSmsMessage.CreateFromPdu(pduBytes, format);
#pragma warning restore CS0618 // Type or member is obsolete
                }
                catch
                {
                    try
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        sms = AndroidSmsMessage.CreateFromPdu(pduBytes);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                    catch (Exception ex)
                    {
                        SafeLog($"Error creando SMS: {ex.Message}");
                        return;
                    }
                }

                if (sms == null) return;

                var sender = sms.OriginatingAddress ?? "Desconocido";
                var messageBody = sms.MessageBody ?? "";

                SafeLog($"SMS de {sender}: {messageBody.Substring(0, Math.Min(30, messageBody.Length))}...");

                ForwardMessage(context, sender, messageBody);
            }
            catch (Exception ex)
            {
                SafeLog($"Error procesando PDU: {ex.Message}");
            }
        }

        private void ForwardMessage(Context context, string sender, string messageBody)
        {
            // Evita reenvíos duplicados en un corto periodo de tiempo
            if (_lastSender == sender && _lastBody == messageBody && (DateTime.Now - _lastReceived).TotalSeconds < 5)
            {
                SafeLog("Mensaje duplicado detectado, no se reenvía.");
                return;
            }
            _lastSender = sender;
            _lastBody = messageBody;
            _lastReceived = DateTime.Now;

            try
            {
                if (string.IsNullOrEmpty(messageBody))
                {
                    SafeLog("Mensaje vacío, no se reenvía");
                    return;
                }

                // Usar el nombre de paquete correcto para acceder a las preferencias
                var packageName = context.PackageName;
                var prefsName = $"{packageName}_preferences";

                SafeLog($"Accediendo a preferencias: {prefsName}");

                var prefs = context.GetSharedPreferences(prefsName, FileCreationMode.Private);
                if (prefs == null)
                {
                    SafeLog("Error: No se pudo acceder a las preferencias");
                    return;
                }

                var phonesJson = prefs.GetString("phones", null);
                if (string.IsNullOrEmpty(phonesJson))
                {
                    SafeLog("No hay números guardados en preferencias");
                    return;
                }

                SafeLog($"Datos recuperados: {phonesJson}");
                List<string> phones;

                try
                {
                    phones = JsonSerializer.Deserialize<List<string>>(phonesJson);
                    if (phones == null || phones.Count == 0)
                    {
                        SafeLog("No hay números para reenviar");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    SafeLog($"Error deserializando números: {ex.Message}");
                    return;
                }

                SafeLog($"Procesando reenvío a {phones.Count} números: {string.Join(", ", phones)}");

                var forwardedMessage = $"De: {sender}\n{messageBody}";
                if (forwardedMessage.Length > 160)
                {
                    forwardedMessage = forwardedMessage.Substring(0, 157) + "...";
                }

                var tasks = new List<Task>();
                var successCount = 0;
                var errorCount = 0;

                foreach (var phone in phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    try
                    {
                        SendSms(phone, forwardedMessage);
                        successCount++;
                        SafeLog($"SMS enviado exitosamente a {phone}");
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        SafeLog($"Error enviando a {phone}: {ex.Message}");
                    }
                }

                SafeLog($"Reenvío completado - Éxitos: {successCount}, Errores: {errorCount}");
            }
            catch (Exception ex)
            {
                SafeLog($"Error general en ForwardMessage: {ex.Message}");
                if (ex.InnerException != null)
                {
                    SafeLog($"Detalle: {ex.InnerException.Message}");
                }
            }
        }

        private void SendSms(string phoneNumber, string message)
        {
            try
            {
#pragma warning disable CS0618
                using var smsManager = AndroidSmsManager.Default;
#pragma warning restore CS0618

                if (smsManager == null)
                {
                    SafeLog("No se pudo obtener el SmsManager");
                    return;
                }

                // Crear PendingIntents para monitorear el estado del envío
                var sentIntent = PendingIntent.GetBroadcast(
                    Application.Context,
                    0,
                    new Intent("SMS_SENT"),
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
                                Application.Context,
                                i,
                                new Intent("SMS_SENT"),
                                PendingIntentFlags.OneShot | PendingIntentFlags.Immutable));
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
            catch (Exception ex)
            {
                SafeLog($"Error enviando SMS a {phoneNumber}: {ex.Message}");
                throw; // Re-lanzar la excepción para que se maneje en ForwardMessage
            }
        }

        private void SafeLog(string message)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[SMSReceiver] {DateTime.Now:HH:mm:ss}: {message}");
            }
            catch { }
        }
    }
}