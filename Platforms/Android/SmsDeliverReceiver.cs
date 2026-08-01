using Android.App;
using Android.Content;
using Android.Runtime;
using AndroidTelephony = Android.Provider.Telephony;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Receptor SMS_DELIVER: el sistema SOLO lo entrega a la app de SMS por defecto.
    /// Es uno de los 4 componentes obligatorios para poder ser la app predeterminada.
    /// Como app por defecto somos responsables de PERSISTIR el mensaje en el proveedor
    /// del sistema (buzon de entrada) para que el usuario no lo pierda, ademas de reenviarlo.
    /// </summary>
    [Register("com.socratic.smsforwarder.SmsDeliverReceiver")]
    [BroadcastReceiver(
        Enabled = true,
        Exported = true,
        Label = "SMS Deliver",
        Permission = "android.permission.BROADCAST_SMS")]
    [IntentFilter(new[] { "android.provider.Telephony.SMS_DELIVER" })]
    public class SmsDeliverReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            try
            {
                if (context == null || intent == null ||
                    intent.Action != "android.provider.Telephony.SMS_DELIVER")
                    return;

                var msgs = AndroidTelephony.Sms.Intents.GetMessagesFromIntent(intent);
                if (msgs == null || msgs.Length == 0) return;

                var sender = msgs[0]?.OriginatingAddress ?? "Desconocido";
                var body = new System.Text.StringBuilder();
                long ts = 0;
                foreach (var m in msgs)
                {
                    if (m == null) continue;
                    body.Append(m.MessageBody ?? "");
                    if (ts == 0) ts = m.TimestampMillis;
                }
                var text = body.ToString();

                // 1) Persistir en el buzon del sistema (somos la app por defecto).
                try { MessageStore.PersistInbox(context, sender, text, ts); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[SmsDeliver] persist: {ex.Message}"); }

                // 2) Notificar al usuario del SMS entrante.
                try { Notifier.NotifyIncoming(context, sender, text); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[SmsDeliver] notify: {ex.Message}"); }

                // 3) Reenviar segun la configuracion del usuario.
                try { ForwardingCore.Forward(context, sender, text); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[SmsDeliver] forward: {ex.Message}"); }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SmsDeliver] Error: {ex.Message}");
            }
        }
    }
}
