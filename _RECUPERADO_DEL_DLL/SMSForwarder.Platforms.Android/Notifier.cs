using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace SMSForwarder.Platforms.Android;

/// <summary>
/// Publica una notificacion local cuando llega un SMS nuevo (tanto si la app es la de SMS por
/// defecto -SMS_DELIVER- como si no lo es -SMS_RECEIVED-). Requiere POST_NOTIFICATIONS en
/// Android 13+; si el usuario no lo concede, simplemente no se muestra (no falla).
/// </summary>
public static class Notifier
{
	private const string ChannelId = "sms_incoming";

	private const string ChannelName = "SMS recibidos";

	private static int _next = 2000;

	public static void NotifyIncoming(Context context, string sender, string body)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Invalid comparison between Unknown and I4
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			NotificationManager val = (NotificationManager)context.GetSystemService("notification");
			if (val != null)
			{
				if ((int)VERSION.SdkInt >= 26 && val.GetNotificationChannel("sms_incoming") == null)
				{
					NotificationChannel val2 = new NotificationChannel("sms_incoming", "SMS recibidos", (NotificationImportance)4)
					{
						Description = "Avisos de nuevos SMS recibidos"
					};
					val.CreateNotificationChannel(val2);
				}
				PackageManager packageManager = context.PackageManager;
				Intent val3 = ((packageManager != null) ? packageManager.GetLaunchIntentForPackage(context.PackageName) : null);
				PendingIntent val4 = null;
				if (val3 != null)
				{
					val3.AddFlags((ActivityFlags)805306368);
					val4 = PendingIntent.GetActivity(context, 0, val3, (PendingIntentFlags)201326592);
				}
				string contentTitle = (string.IsNullOrWhiteSpace(sender) ? "Nuevo SMS" : sender);
				Builder val5 = (((int)VERSION.SdkInt < 26) ? new Builder(context) : new Builder(context, "sms_incoming"));
				val5.SetContentTitle(contentTitle).SetContentText(body).SetSmallIcon(((PackageItemInfo)context.ApplicationInfo).Icon)
					.SetAutoCancel(true)
					.SetStyle((Style)(object)new BigTextStyle().BigText(body));
				if (val4 != null)
				{
					val5.SetContentIntent(val4);
				}
				val.Notify(_next++, val5.Build());
			}
		}
		catch (Exception)
		{
		}
	}
}
