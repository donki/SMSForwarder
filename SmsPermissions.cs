public class SmsPermissions
{
    public class ReceiveSms : Permissions.BasePermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
#if ANDROID
            return Permissions.CheckStatusAsync<Permissions.Sms>();
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override Task<PermissionStatus> RequestAsync()
        {
#if ANDROID
            return Permissions.RequestAsync<Permissions.Sms>();
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override bool ShouldShowRationale()
        {
#if ANDROID
            return Permissions.ShouldShowRationale<Permissions.Sms>();
#else
            return false;
#endif
        }

        public override void EnsureDeclared()
        {
            // No es necesario en MAUI moderno
        }
    }

    public class SendSms : Permissions.BasePermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
#if ANDROID
            return Permissions.CheckStatusAsync<Permissions.Sms>();
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override Task<PermissionStatus> RequestAsync()
        {
#if ANDROID
            return Permissions.RequestAsync<Permissions.Sms>();
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override bool ShouldShowRationale()
        {
#if ANDROID
            return Permissions.ShouldShowRationale<Permissions.Sms>();
#else
            return false;
#endif
        }

        public override void EnsureDeclared()
        {
            // No es necesario en MAUI moderno
        }
    }

    public class ReadSms : Permissions.BasePermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
#if ANDROID
            return Permissions.CheckStatusAsync<Permissions.Sms>();
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override Task<PermissionStatus> RequestAsync()
        {
#if ANDROID
            return Permissions.RequestAsync<Permissions.Sms>();
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override bool ShouldShowRationale()
        {
#if ANDROID
            return Permissions.ShouldShowRationale<Permissions.Sms>();
#else
            return false;
#endif
        }

        public override void EnsureDeclared()
        {
            // No es necesario en MAUI moderno
        }
    }

    public class BroadCastSms : Permissions.BasePermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
#if ANDROID
            return Permissions.CheckStatusAsync<Permissions.Sms>();
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override Task<PermissionStatus> RequestAsync()
        {
#if ANDROID
            return Permissions.RequestAsync<Permissions.Sms>();
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override bool ShouldShowRationale()
        {
#if ANDROID
            return Permissions.ShouldShowRationale<Permissions.Sms>();
#else
            return false;
#endif
        }

        public override void EnsureDeclared()
        {
            // No es necesario en MAUI moderno
        }
    }
}