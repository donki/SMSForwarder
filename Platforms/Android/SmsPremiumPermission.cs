using Android.App;
using Android.Content.PM;
using Microsoft.Maui.ApplicationModel;

namespace SMSForwarder.Platforms.Android
{
    public class SmsPremiumPermission : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new[]
        {
            ("android.permission.SEND_SMS_NO_CONFIRMATION", true)
        };

        public override async Task<PermissionStatus> CheckStatusAsync()
        {
            var status = await base.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                return status;
            }

            // Verificación adicional para SMS premium
            var context = Platform.CurrentActivity;
            if (context == null)
            {
                return PermissionStatus.Unknown;
            }

            var hasPremiumPermission = context.CheckSelfPermission("android.permission.SEND_SMS_NO_CONFIRMATION");
            return hasPremiumPermission == Permission.Granted ? PermissionStatus.Granted : PermissionStatus.Denied;
        }
    }
}
