using Microsoft.Maui.Authentication;

public class SmsPermissions
{
    // Clase para manejar permisos de autostart
    public class AutoStartPermission : Permissions.BasePermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
#if ANDROID
            try
            {
                var context = Platform.CurrentActivity ?? Android.App.Application.Context;
                var packageManager = context.PackageManager;
                var packageName = context.PackageName;
                
                // Verificar si la app está en la lista blanca de autostart
                // Esto es aproximado ya que cada fabricante tiene su propio sistema
                return Task.FromResult(PermissionStatus.Unknown);
            }
            catch
            {
                return Task.FromResult(PermissionStatus.Unknown);
            }
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override async Task<PermissionStatus> RequestAsync()
        {
#if ANDROID
            try
            {
                var context = Platform.CurrentActivity ?? Android.App.Application.Context;
                
                // Intentar abrir configuración de autostart según el fabricante
                var intent = new Android.Content.Intent();
                var packageName = context.PackageName;
                
                // Xiaomi
                if (IsManufacturer("xiaomi"))
                {
                    intent.SetComponent(new Android.Content.ComponentName("com.miui.securitycenter", 
                        "com.miui.permcenter.autostart.AutoStartManagementActivity"));
                }
                // Huawei
                else if (IsManufacturer("huawei"))
                {
                    intent.SetComponent(new Android.Content.ComponentName("com.huawei.systemmanager", 
                        "com.huawei.systemmanager.startupmgr.ui.StartupNormalAppListActivity"));
                }
                // OPPO
                else if (IsManufacturer("oppo"))
                {
                    intent.SetComponent(new Android.Content.ComponentName("com.coloros.safecenter", 
                        "com.coloros.safecenter.permission.startup.StartupAppListActivity"));
                }
                // Vivo
                else if (IsManufacturer("vivo"))
                {
                    intent.SetComponent(new Android.Content.ComponentName("com.vivo.permissionmanager", 
                        "com.vivo.permissionmanager.activity.BgStartUpManagerActivity"));
                }
                // Samsung
                else if (IsManufacturer("samsung"))
                {
                    intent.SetComponent(new Android.Content.ComponentName("com.samsung.android.lool", 
                        "com.samsung.android.sm.ui.battery.BatteryActivity"));
                }
                // OnePlus
                else if (IsManufacturer("oneplus"))
                {
                    intent.SetComponent(new Android.Content.ComponentName("com.oneplus.security", 
                        "com.oneplus.security.chainlaunch.view.ChainLaunchAppListActivity"));
                }
                else
                {
                    // Configuración general de aplicaciones
                    intent.SetAction(Android.Provider.Settings.ActionApplicationDetailsSettings);
                    intent.SetData(Android.Net.Uri.Parse($"package:{packageName}"));
                }

                intent.AddFlags(Android.Content.ActivityFlags.NewTask);
                context.StartActivity(intent);
                
                return PermissionStatus.Unknown; // No podemos verificar automáticamente
            }
            catch
            {
                return PermissionStatus.Denied;
            }
#else
            return PermissionStatus.Granted;
#endif
        }

        public override bool ShouldShowRationale()
        {
            return true;
        }

        public override void EnsureDeclared()
        {
            // No requiere declaración en manifest
        }

        private bool IsManufacturer(string manufacturer)
        {
#if ANDROID
            return Android.OS.Build.Manufacturer?.ToLower().Contains(manufacturer) == true;
#else
            return false;
#endif
        }
    }

    // Clase para manejar optimización de batería
    public class BatteryOptimizationPermission : Permissions.BasePermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
#if ANDROID
            try
            {
                var context = Platform.CurrentActivity ?? Android.App.Application.Context;
                var powerManager = context.GetSystemService(Android.Content.Context.PowerService) as Android.OS.PowerManager;
                
                if (powerManager != null && Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
                {
                    var packageName = context.PackageName;
                    bool isIgnoringOptimizations = powerManager.IsIgnoringBatteryOptimizations(packageName);
                    return Task.FromResult(isIgnoringOptimizations ? PermissionStatus.Granted : PermissionStatus.Denied);
                }
                
                return Task.FromResult(PermissionStatus.Granted);
            }
            catch
            {
                return Task.FromResult(PermissionStatus.Unknown);
            }
#else
            return Task.FromResult(PermissionStatus.Granted);
#endif
        }

        public override async Task<PermissionStatus> RequestAsync()
        {
#if ANDROID
            try
            {
                var context = Platform.CurrentActivity ?? Android.App.Application.Context;
                
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
                {
                    var intent = new Android.Content.Intent();
                    var packageName = context.PackageName;
                    
                    intent.SetAction(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
                    intent.SetData(Android.Net.Uri.Parse($"package:{packageName}"));
                    intent.AddFlags(Android.Content.ActivityFlags.NewTask);
                    
                    context.StartActivity(intent);
                    
                    // Esperar un momento para que el usuario pueda interactuar
                    await Task.Delay(1000);
                    
                    // Verificar el estado después de la solicitud
                    return await CheckStatusAsync();
                }
                
                return PermissionStatus.Granted;
            }
            catch
            {
                return PermissionStatus.Denied;
            }
#else
            return PermissionStatus.Granted;
#endif
        }

        public override bool ShouldShowRationale()
        {
            return true;
        }

        public override void EnsureDeclared()
        {
            // Requiere permiso en manifest
        }
    }

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