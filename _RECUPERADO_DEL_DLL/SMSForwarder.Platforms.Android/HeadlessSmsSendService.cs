using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace SMSForwarder.Platforms.Android;

/// <summary>
/// Servicio que responde a RESPOND_VIA_MESSAGE ("responder con mensaje" a una llamada entrante).
/// Obligatorio para calificar como app de SMS por defecto. SMS Forwarder no implementa la
/// respuesta rapida, asi que es un no-op: solo debe existir y estar declarado.
/// </summary>
[Register("com.socratic.smsforwarder.HeadlessSmsSendService")]
[Service(Exported = true, Permission = "android.permission.SEND_RESPOND_VIA_MESSAGE")]
[IntentFilter(new string[] { "android.intent.action.RESPOND_VIA_MESSAGE" }, Categories = new string[] { "android.intent.category.DEFAULT" }, DataSchemes = new string[] { "sms", "smsto", "mms", "mmsto" })]
public class HeadlessSmsSendService : Service
{
	public override IBinder? OnBind(Intent? intent)
	{
		return null;
	}

	public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
	{
		((Service)this).StopSelf(startId);
		return (StartCommandResult)2;
	}
}
