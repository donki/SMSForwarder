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
    [Register("com.socratic.smsforwarder.SMSReceiver")]
    [BroadcastReceiver(
        Enabled = true,
        Exported = true,
        Label = "SMS Receiver",
        Name = "com.socratic.smsforwarder.SMSReceiver")]
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

                // PREVENCIÓN DE BUCLES INFINITOS
                // Verificar si el remitente está en nuestra lista de números de reenvío
                var cleanSender = CleanPhoneNumber(sender);
                var isFromForwardingNumber = phones.Any(phone => 
                {
                    var cleanPhone = CleanPhoneNumber(phone);
                    return ArePhoneNumbersEqual(cleanSender, cleanPhone);
                });

                if (isFromForwardingNumber)
                {
                    SafeLog($"BUCLE DETECTADO: El mensaje proviene de un número en la lista de reenvío ({sender}). No se reenvía para evitar bucle infinito.");
                    return;
                }

                // Verificar si el mensaje parece ser un reenvío de nuestra aplicación
                if (IsForwardedMessage(messageBody))
                {
                    SafeLog($"BUCLE DETECTADO: El mensaje parece ser un reenvío de SMSForwarder. No se reenvía para evitar bucle infinito.");
                    return;
                }

                SafeLog($"Procesando reenvío a {phones.Count} números: {string.Join(", ", phones)}");

                // Crear mensaje con formato identificable para prevenir bucles
                var forwardedMessage = $"[SMSForwarder] De: {sender}\n{messageBody}";
                if (forwardedMessage.Length > 160)
                {
                    // Asegurar que el identificador siempre esté presente
                    var maxBodyLength = 160 - "[SMSForwarder] De: ".Length - sender.Length - 4; // 4 para \n y ...
                    var truncatedBody = messageBody.Length > maxBodyLength 
                        ? messageBody.Substring(0, maxBodyLength) + "..."
                        : messageBody;
                    forwardedMessage = $"[SMSForwarder] De: {sender}\n{truncatedBody}";
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

        /// <summary>
        /// Limpia un número de teléfono removiendo espacios, guiones y otros caracteres
        /// </summary>
        private string CleanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return "";

            // Remover espacios, guiones, paréntesis y otros caracteres
            var cleaned = phoneNumber.Replace(" ", "")
                                   .Replace("-", "")
                                   .Replace("(", "")
                                   .Replace(")", "")
                                   .Replace(".", "")
                                   .Replace("+", "")
                                   .Trim();

            return cleaned;
        }

        /// <summary>
        /// Compara dos números de teléfono para ver si son equivalentes
        /// </summary>
        private bool ArePhoneNumbersEqual(string phone1, string phone2)
        {
            if (string.IsNullOrWhiteSpace(phone1) || string.IsNullOrWhiteSpace(phone2))
                return false;

            // Si los números son exactamente iguales
            if (phone1 == phone2)
                return true;

            // Comparar los últimos 9 dígitos (para manejar códigos de país)
            var minLength = Math.Min(phone1.Length, phone2.Length);
            if (minLength >= 9)
            {
                var suffix1 = phone1.Substring(phone1.Length - 9);
                var suffix2 = phone2.Substring(phone2.Length - 9);
                return suffix1 == suffix2;
            }

            return false;
        }

        /// <summary>
        /// Detecta si un mensaje parece ser un reenvío de SMSForwarder
        /// </summary>
        private bool IsForwardedMessage(string messageBody)
        {
            if (string.IsNullOrWhiteSpace(messageBody))
                return false;

            // Buscar nuestro identificador específico primero
            if (messageBody.StartsWith("[SMSForwarder]", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Buscar otros patrones típicos de mensajes reenviados
            var forwardPatterns = new[]
            {
                "De:",           // Nuestro formato anterior: "De: +34123456789"
                "From:",         // Formato en inglés
                "Reenviado:",    // Posible formato en español
                "Forwarded:",    // Formato en inglés
                "SMS de:",       // Otro posible formato
            };

            var messageStart = messageBody.Substring(0, Math.Min(30, messageBody.Length)).ToLower();
            
            return forwardPatterns.Any(pattern => 
                messageStart.Contains(pattern.ToLower()));
        }
    }
}