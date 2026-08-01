using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.Telephony;

namespace SMSForwarder.Platforms.Android;

/// <summary>
/// Receptor SMS_DELIVER: el sistema SOLO lo entrega a la app de SMS por defecto.
/// Es uno de los 4 componentes obligatorios para poder ser la app predeterminada.
/// Como app por defecto somos responsables de PERSISTIR el mensaje en el proveedor
/// del sistema (buzon de entrada) para que el usuario no lo pierda, ademas de reenviarlo.
/// </summary>
[Register("com.socratic.smsforwarder.SmsDeliverReceiver")]
[BroadcastReceiver(Enabled = true, Exported = true, Label = "SMS Deliver", Permission = "android.permission.BROADCAST_SMS")]
[IntentFilter(new string[] { "android.provider.Telephony.SMS_DELIVER" })]
public class SmsDeliverReceiver : BroadcastReceiver
{
	public override void OnReceive(Context? context, Intent? intent)
	{
		try
		{
			if (context == null || intent == null || intent.Action != "android.provider.Telephony.SMS_DELIVER")
			{
				return;
			}
			SmsMessage[] messagesFromIntent = Intents.GetMessagesFromIntent(intent);
			if (messagesFromIntent == null || messagesFromIntent.Length == 0)
			{
				return;
			}
			SmsMessage obj = messagesFromIntent[0];
			string sender = ((obj != null) ? obj.OriginatingAddress : null) ?? "Desconocido";
			StringBuilder stringBuilder = new StringBuilder();
			long num = 0L;
			SmsMessage[] array = messagesFromIntent;
			foreach (SmsMessage val in array)
			{
				if (val != null)
				{
					stringBuilder.Append(val.MessageBody ?? "");
					if (num == 0L)
					{
						num = val.TimestampMillis;
					}
				}
			}
			string text = stringBuilder.ToString();
			try
			{
				MessageStore.PersistInbox(context, sender, text, num);
			}
			catch (Exception)
			{
			}
			try
			{
				Notifier.NotifyIncoming(context, sender, text);
			}
			catch (Exception)
			{
			}
			try
			{
				ForwardingCore.Forward(context, sender, text);
			}
			catch (Exception)
			{
			}
		}
		catch (Exception)
		{
		}
	}
}
