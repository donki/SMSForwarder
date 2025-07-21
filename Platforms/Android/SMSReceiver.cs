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
        private readonly LoggingService _logger = new LoggingService();

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action == "android.provider.Telephony.SMS_RECEIVED")
                {
                    _logger.LogInfo("SMS recibido - iniciando procesamiento");

                    var bundle = intent.Extras;
                    if (bundle != null)
                    {
                        var pdus = (Java.Lang.Object[])bundle.Get("pdus");
                        var format = bundle.GetString("format");

                        if (pdus != null)
                        {
                            _logger.LogInfo($"Procesando {pdus.Length} PDUs");
                            foreach (var pdu in pdus)
                            {
                                try
                                {
                                    var sms = SmsMessage.CreateFromPdu((byte[])pdu, format);
                                    var sender = sms.OriginatingAddress;
                                    var messageBody = sms.MessageBody;

                                    _logger.LogInfo($"Mensaje recibido de {sender}: {messageBody?.Substring(0, Math.Min(50, messageBody?.Length ?? 0))}...");

                                    // Reenvía el mensaje a los números configurados
                                    ForwardMessage(context, sender, messageBody);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError($"Error al procesar un mensaje PDU", ex);
                                }
                            }
                        }
                        else
                        {
                            _logger.LogWarning("No se encontraron PDUs en el mensaje");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("El bundle recibido está vacío");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error crítico en OnReceive", ex);
            }
        }

        private void ForwardMessage(Context context, string sender, string messageBody)
        {
            try
            {
                // Usar el mismo almacén que MAUI Preferences
                var prefs = context.GetSharedPreferences($"{context.PackageName}.microsoft.maui.essentials.preferences", FileCreationMode.Private);
                var phonesJson = prefs.GetString("phones", "[]");
                var phones = JsonSerializer.Deserialize<List<string>>(phonesJson);

                _logger.LogInfo($"Números configurados para reenvío: {phones?.Count ?? 0}");

                if (phones != null && phones.Count > 0)
                {
                    var smsManager = SmsManager.Default;
                    int successCount = 0;
                    int errorCount = 0;

                    foreach (var phone in phones)
                    {
                        try
                        {
                            // Enviar el mensaje a cada número de la lista
                            var forwardedMessage = $"De: {sender}\n{messageBody}";
                            smsManager.SendTextMessage(phone, null, forwardedMessage, null, null);
                            _logger.LogInfo($"Mensaje reenviado exitosamente a {phone}");
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error al enviar mensaje a {phone}", ex);
                            errorCount++;
                        }
                    }

                    _logger.LogInfo($"Reenvío completado: {successCount} exitosos, {errorCount} errores");
                }
                else
                {
                    _logger.LogWarning("No hay números de teléfono guardados para reenviar el mensaje");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error crítico al reenviar el mensaje", ex);
            }
        }
    }
}
