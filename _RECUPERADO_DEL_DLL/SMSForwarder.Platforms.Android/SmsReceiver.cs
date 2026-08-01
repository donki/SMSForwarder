using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Java.Lang;

namespace SMSForwarder.Platforms.Android;

/// <summary>
/// Camino de entrada cuando la app NO es la de SMS por defecto: broadcast SMS_RECEIVED.
/// Si la app SI es la predeterminada, este receptor se inhibe porque el sistema entrega
/// el mensaje via SMS_DELIVER a <see cref="T:SMSForwarder.Platforms.Android.SmsDeliverReceiver" /> (evita doble reenvio).
/// El reenvio real vive en <see cref="T:SMSForwarder.Platforms.Android.ForwardingCore" />.
/// </summary>
[Register("com.socratic.smsforwarder.SMSReceiver")]
[BroadcastReceiver(Enabled = true, Exported = true, Label = "SMS Receiver", Name = "com.socratic.smsforwarder.SMSReceiver")]
[IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" }, Categories = new string[] { "android.intent.category.DEFAULT" }, Priority = 1000)]
public class SmsReceiver : BroadcastReceiver
{
	public override void OnReceive(Context? context, Intent? intent)
	{
		try
		{
			if (context == null || intent == null || intent.Action != "android.provider.Telephony.SMS_RECEIVED" || MessageStore.IsAppDefault(context))
			{
				return;
			}
			Bundle extras = intent.Extras;
			Object val = ((extras != null) ? ((BaseBundle)extras).Get("pdus") : null);
			if (val == null)
			{
				return;
			}
			Object[] array;
			try
			{
				array = (Object[])val;
			}
			catch
			{
				return;
			}
			if (array == null || array.Length == 0)
			{
				return;
			}
			string text = ((BaseBundle)extras).GetString("format") ?? "3gpp";
			string text2 = "Desconocido";
			StringBuilder stringBuilder = new StringBuilder();
			Object[] array2 = array;
			foreach (Object val2 in array2)
			{
				try
				{
					byte[] array3 = (byte[])val2;
					SmsMessage val3;
					try
					{
						val3 = SmsMessage.CreateFromPdu(array3, text);
					}
					catch
					{
						val3 = SmsMessage.CreateFromPdu(array3);
					}
					if (val3 != null)
					{
						text2 = val3.OriginatingAddress ?? text2;
						stringBuilder.Append(val3.MessageBody ?? "");
					}
				}
				catch
				{
				}
			}
			string text3 = stringBuilder.ToString();
			try
			{
				Notifier.NotifyIncoming(context, text2, text3);
			}
			catch (Exception)
			{
			}
			ForwardingCore.Forward(context, text2, text3);
		}
		catch (Exception)
		{
		}
	}
}
