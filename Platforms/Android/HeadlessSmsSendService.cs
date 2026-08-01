using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Servicio que responde a RESPOND_VIA_MESSAGE ("responder con mensaje" a una llamada entrante).
    /// Obligatorio para calificar como app de SMS por defecto. SMS Forwarder no implementa la
    /// respuesta rapida, asi que es un no-op: solo debe existir y estar declarado.
    /// </summary>
    [Register("com.socratic.smsforwarder.HeadlessSmsSendService")]
    [Service(
        Exported = true,
        Permission = "android.permission.SEND_RESPOND_VIA_MESSAGE")]
    [IntentFilter(
        new[] { "android.intent.action.RESPOND_VIA_MESSAGE" },
        Categories = new[] { Intent.CategoryDefault },
        DataSchemes = new[] { "sms", "smsto", "mms", "mmsto" })]
    public class HeadlessSmsSendService : Service
    {
        public override IBinder? OnBind(Intent? intent) => null;

        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            StopSelf(startId);
            return StartCommandResult.NotSticky;
        }
    }
}
