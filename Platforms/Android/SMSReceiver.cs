using Android.App;
using Android.Content;
using Android.Runtime;
using AndroidSmsMessage = Android.Telephony.SmsMessage;

[assembly: UsesPermission(Android.Manifest.Permission.ReceiveSms)]
[assembly: UsesPermission(Android.Manifest.Permission.SendSms)]
[assembly: UsesPermission(Android.Manifest.Permission.ReadSms)]
namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Camino de entrada cuando la app NO es la de SMS por defecto: broadcast SMS_RECEIVED.
    /// Si la app SI es la predeterminada, este receptor se inhibe porque el sistema entrega
    /// el mensaje via SMS_DELIVER a <see cref="SmsDeliverReceiver"/> (evita doble reenvio).
    /// El reenvio real vive en <see cref="ForwardingCore"/>.
    /// </summary>
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
        public override void OnReceive(Context? context, Intent? intent)
        {
            try
            {
                if (context == null || intent == null ||
                    intent.Action != "android.provider.Telephony.SMS_RECEIVED")
                    return;

                // Si somos la app de SMS por defecto, SMS_DELIVER se encarga: no duplicar.
                if (MessageStore.IsAppDefault(context))
                    return;

                var bundle = intent.Extras;
                var pdusObj = bundle?.Get("pdus");
                if (pdusObj == null) return;

                Java.Lang.Object[]? pdus;
                try { pdus = (Java.Lang.Object[]?)pdusObj; }
                catch { return; }
                if (pdus == null || pdus.Length == 0) return;

                var format = bundle!.GetString("format") ?? "3gpp";

                // Reensambla el cuerpo (multipart) y toma el remitente del primer PDU.
                string sender = "Desconocido";
                var body = new System.Text.StringBuilder();
                foreach (var pdu in pdus)
                {
                    try
                    {
                        var bytes = (byte[])pdu;
                        AndroidSmsMessage? sms;
                        try
                        {
#pragma warning disable CS0618
                            sms = AndroidSmsMessage.CreateFromPdu(bytes, format);
#pragma warning restore CS0618
                        }
                        catch
                        {
#pragma warning disable CS0618
                            sms = AndroidSmsMessage.CreateFromPdu(bytes);
#pragma warning restore CS0618
                        }
                        if (sms == null) continue;
                        sender = sms.OriginatingAddress ?? sender;
                        body.Append(sms.MessageBody ?? "");
                    }
                    catch { }
                }

                var text = body.ToString();

                // Notificar al usuario del SMS entrante.
                try { Notifier.NotifyIncoming(context, sender, text); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[SmsReceiver] notify: {ex.Message}"); }

                ForwardingCore.Forward(context, sender, text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SmsReceiver] Error: {ex.Message}");
            }
        }
    }
}
