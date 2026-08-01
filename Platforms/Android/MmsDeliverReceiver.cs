using Android.App;
using Android.Content;
using Android.Runtime;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Receptor WAP_PUSH_DELIVER (MMS). Obligatorio para calificar como app de SMS por defecto.
    /// SMS Forwarder no gestiona MMS, asi que es un no-op: solo debe existir y estar declarado.
    /// </summary>
    [Register("com.socratic.smsforwarder.MmsDeliverReceiver")]
    [BroadcastReceiver(
        Enabled = true,
        Exported = true,
        Label = "MMS Deliver",
        Permission = "android.permission.BROADCAST_WAP_PUSH")]
    [IntentFilter(
        new[] { "android.provider.Telephony.WAP_PUSH_DELIVER" },
        DataMimeType = "application/vnd.wap.mms-message")]
    public class MmsDeliverReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            // MMS no soportado: no se hace nada.
        }
    }
}
