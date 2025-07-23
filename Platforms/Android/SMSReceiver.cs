using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Telephony;
using System.Text.Json;
using SMSForwarder.Services;
using SmsMessage = Android.Telephony.SmsMessage;

namespace SMSForwarder.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" })]
    public class SmsReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (context == null || intent == null)
            {
                return;
            }

            try
            {
                if (intent.Action == "android.provider.Telephony.SMS_RECEIVED")
                {
                    LogInfo("SMS recibido - iniciando procesamiento");

                    var bundle = intent.Extras;
                    if (bundle != null)
                    {
                        var pdus = (Java.Lang.Object[]?)bundle.Get("pdus");
                        var format = bundle.GetString("format");

                        if (pdus != null && pdus.Length > 0)
                        {
                            LogInfo($"Procesando {pdus.Length} PDUs");
                            foreach (var pdu in pdus)
                            {
                                try
                                {
                                    if (pdu != null)
                                    {
                                        var pduBytes = (byte[])pdu;
                                        var sms = SmsMessage.CreateFromPdu(pduBytes, format);
                                        if (sms != null)
                                        {
                                            var sender = sms.OriginatingAddress ?? "Desconocido";
                                            var messageBody = sms.MessageBody ?? "";

                                            LogInfo($"Mensaje recibido de {sender}: {(messageBody.Length > 50 ? messageBody.Substring(0, 50) + "..." : messageBody)}");

                                            // Reenvía el mensaje a los números configurados
                                            ForwardMessage(context, sender, messageBody);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogError($"Error al procesar un mensaje PDU: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            LogWarning("No se encontraron PDUs en el mensaje");
                        }
                    }
                    else
                    {
                        LogWarning("El bundle recibido está vacío");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error crítico en OnReceive: {ex.Message}");
            }
        }

        private void ForwardMessage(Context context, string sender, string messageBody)
        {
            try
            {
                // Usar el mismo almacén que MAUI Preferences
                var prefs = context.GetSharedPreferences($"{context.PackageName}.microsoft.maui.essentials.preferences", FileCreationMode.Private);
                var phonesJson = prefs?.GetString("phones", "[]") ?? "[]";
                var phones = JsonSerializer.Deserialize<List<string>>(phonesJson);

                LogInfo($"Números configurados para reenvío: {phones?.Count ?? 0}");

                if (phones != null && phones.Count > 0)
                {
                    var smsManager = SmsManager.Default;
                    int successCount = 0;
                    int errorCount = 0;

                    foreach (var phone in phones)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(phone))
                            {
                                // Enviar el mensaje a cada número de la lista
                                var forwardedMessage = $"De: {sender}\n{messageBody}";
                                smsManager?.SendTextMessage(phone, null, forwardedMessage, null, null);
                                LogInfo($"Mensaje reenviado exitosamente a {phone}");
                                successCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogError($"Error al enviar mensaje a {phone}: {ex.Message}");
                            errorCount++;
                        }
                    }

                    LogInfo($"Reenvío completado: {successCount} exitosos, {errorCount} errores");
                }
                else
                {
                    LogWarning("No hay números de teléfono guardados para reenviar el mensaje");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error crítico al reenviar el mensaje: {ex.Message}");
            }
        }

        private void LogInfo(string message)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[SMSReceiver] INFO: {message}");
            }
            catch { /* Ignore logging errors */ }
        }

        private void LogWarning(string message)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[SMSReceiver] WARNING: {message}");
            }
            catch { /* Ignore logging errors */ }
        }

        private void LogError(string message)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[SMSReceiver] ERROR: {message}");
            }
            catch { /* Ignore logging errors */ }
        }
    }
}
