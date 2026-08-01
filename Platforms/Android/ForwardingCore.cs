using Android.App;
using Android.Content;
using System.Text.Json;
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
        private static string? _lastSender;
        private static string? _lastBody;
        private static DateTime _lastReceived = DateTime.MinValue;

        public static void Forward(Context context, string sender, string messageBody)
        {
            // Evita reenvios duplicados en un corto periodo de tiempo (multipart, doble broadcast, etc.)
            if (_lastSender == sender && _lastBody == messageBody && (DateTime.Now - _lastReceived).TotalSeconds < 5)
            {
                SafeLog("Mensaje duplicado detectado, no se reenvia.");
                return;
            }
            _lastSender = sender;
            _lastBody = messageBody;
            _lastReceived = DateTime.Now;

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

                var phonesJson = prefs.GetString("phones", null);
                if (string.IsNullOrEmpty(phonesJson))
                {
                    SafeLog("No hay numeros guardados en preferencias");
                    return;
                }

                List<string>? phones;
                try
                {
                    phones = JsonSerializer.Deserialize<List<string>>(phonesJson);
                    if (phones == null || phones.Count == 0)
                    {
                        SafeLog("No hay numeros para reenviar");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    SafeLog($"Error deserializando numeros: {ex.Message}");
                    return;
                }

                // PREVENCION DE BUCLES: no reenviar si el remitente es uno de los numeros de reenvio
                var cleanSender = CleanPhoneNumber(sender);
                var isFromForwardingNumber = phones.Any(phone => ArePhoneNumbersEqual(cleanSender, CleanPhoneNumber(phone)));
                if (isFromForwardingNumber)
                {
                    SafeLog($"BUCLE DETECTADO: mensaje desde un numero de reenvio ({sender}). No se reenvia.");
                    return;
                }
                if (IsForwardedMessage(messageBody))
                {
                    SafeLog("BUCLE DETECTADO: el mensaje parece un reenvio de SMSForwarder. No se reenvia.");
                    return;
                }

                SafeLog($"Procesando reenvio a {phones.Count} numeros");

                var forwardedMessage = $"[SMSForwarder] De: {sender}\n{messageBody}";
                if (forwardedMessage.Length > 160)
                {
                    var maxBodyLength = 160 - "[SMSForwarder] De: ".Length - sender.Length - 4;
                    var truncatedBody = messageBody.Length > maxBodyLength
                        ? messageBody.Substring(0, Math.Max(0, maxBodyLength)) + "..."
                        : messageBody;
                    forwardedMessage = $"[SMSForwarder] De: {sender}\n{truncatedBody}";
                }

                var successCount = 0;
                var errorCount = 0;
                foreach (var phone in phones.Where(p => !string.IsNullOrWhiteSpace(p)))
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

        private static string CleanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return "";
            return phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "")
                              .Replace(")", "").Replace(".", "").Replace("+", "").Trim();
        }

        private static bool ArePhoneNumbersEqual(string phone1, string phone2)
        {
            if (string.IsNullOrWhiteSpace(phone1) || string.IsNullOrWhiteSpace(phone2)) return false;
            if (phone1 == phone2) return true;
            var minLength = Math.Min(phone1.Length, phone2.Length);
            if (minLength >= 9)
                return phone1.Substring(phone1.Length - 9) == phone2.Substring(phone2.Length - 9);
            return false;
        }

        private static bool IsForwardedMessage(string messageBody)
        {
            if (string.IsNullOrWhiteSpace(messageBody)) return false;
            if (messageBody.StartsWith("[SMSForwarder]", StringComparison.OrdinalIgnoreCase)) return true;
            var forwardPatterns = new[] { "De:", "From:", "Reenviado:", "Forwarded:", "SMS de:" };
            var messageStart = messageBody.Substring(0, Math.Min(30, messageBody.Length)).ToLower();
            return forwardPatterns.Any(pattern => messageStart.Contains(pattern.ToLower()));
        }
    }
}
