using Android.App;
using Android.Content;
using Android.OS;

namespace SMSForwarder.Platforms.Android
{
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
            try
            {
                var nm = (NotificationManager?)context.GetSystemService(Context.NotificationService);
                if (nm == null) return;

                if (Build.VERSION.SdkInt >= BuildVersionCodes.O && nm.GetNotificationChannel(ChannelId) == null)
                {
                    var ch = new NotificationChannel(ChannelId, ChannelName, NotificationImportance.High)
                    {
                        Description = "Avisos de nuevos SMS recibidos"
                    };
                    nm.CreateNotificationChannel(ch);
                }

                var launch = context.PackageManager?.GetLaunchIntentForPackage(context.PackageName);
                PendingIntent? pi = null;
                if (launch != null)
                {
                    launch.AddFlags(ActivityFlags.NewTask | ActivityFlags.SingleTop);
                    pi = PendingIntent.GetActivity(context, 0, launch,
                        PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);
                }

                var title = string.IsNullOrWhiteSpace(sender) ? "Nuevo SMS" : sender;

                Notification.Builder b;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    b = new Notification.Builder(context, ChannelId);
                }
                else
                {
#pragma warning disable CS0618
                    b = new Notification.Builder(context);
#pragma warning restore CS0618
                }

                b.SetContentTitle(title)
                 .SetContentText(body)
                 .SetSmallIcon(context.ApplicationInfo!.Icon)
                 .SetAutoCancel(true)
                 .SetStyle(new Notification.BigTextStyle().BigText(body));
                if (pi != null) b.SetContentIntent(pi);

                nm.Notify(_next++, b.Build());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Notifier] {ex.Message}");
            }
        }
    }
}
