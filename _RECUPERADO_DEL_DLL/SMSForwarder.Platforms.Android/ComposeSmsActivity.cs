using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace SMSForwarder.Platforms.Android;

/// <summary>
/// Activity que responde a SENDTO/VIEW sobre esquemas sms/smsto/mms/mmsto. Obligatoria para
/// calificar como app de SMS por defecto (el sistema/otros apps la usan para "enviar mensaje").
/// SMS Forwarder no ofrece redaccion completa: redirige a la pantalla principal (Mensajes).
/// </summary>
[Register("com.socratic.smsforwarder.ComposeSmsActivity")]
[Activity(/*Could not decode attribute arguments.*/)]
[IntentFilter(new string[] { "android.intent.action.SENDTO" }, Categories = new string[] { "android.intent.category.DEFAULT", "android.intent.category.BROWSABLE" }, DataSchemes = new string[] { "sms", "smsto", "mms", "mmsto" })]
[IntentFilter(new string[] { "android.intent.action.VIEW", "android.intent.action.SEND" }, Categories = new string[] { "android.intent.category.DEFAULT", "android.intent.category.BROWSABLE" }, DataSchemes = new string[] { "sms", "smsto", "mms", "mmsto" })]
public class ComposeSmsActivity : Activity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		((Activity)this).OnCreate(savedInstanceState);
		try
		{
			Intent val = new Intent((Context)(object)this, typeof(MainActivity));
			val.AddFlags((ActivityFlags)335544320);
			((Context)this).StartActivity(val);
		}
		catch (Exception)
		{
		}
		((Activity)this).Finish();
	}
}
