using Microsoft.Maui.Controls;

namespace SMSForwarder.Services
{
    public class PermissionService
    {
        private readonly ILocalizationService _l;

        // Los textos van por los recursos de idioma: hasta la 2026.09.27.0 estaban en castellano.
        public PermissionService(ILocalizationService? localization = null)
        {
            _l = localization
                 ?? IPlatformApplication.Current?.Services.GetService<ILocalizationService>()
                 ?? new LocalizationService();
        }

        private string T(string key) => _l.GetString(key);

        // ModernDialog necesita una ContentPage donde poner el aviso: con el Shell,
        // Application.Current.MainPage es el propio Shell, el aviso no salia y la tarea quedaba
        // esperando para siempre («Ver el estado de los permisos» no hacia nada, 2026-09-27).
        private static Page Host =>
            Shell.Current?.CurrentPage
            ?? Application.Current?.Windows.FirstOrDefault()?.Page
            ?? throw new InvalidOperationException("No hay pagina donde mostrar el aviso.");

        public async Task<bool> CheckAndRequestAllPermissionsAsync()
        {
            var results = new List<bool>();

            // 1. Permisos SMS básicos
            results.Add(await CheckAndRequestSmsPermissionsAsync());

            // 2. Permiso de optimización de batería
            results.Add(await CheckAndRequestBatteryOptimizationAsync());

            // 3. Permiso de autostart (solo mostrar diálogo informativo)
            await ShowAutostartInformationAsync();

            return results.All(r => r);
        }

        private async Task<bool> CheckAndRequestSmsPermissionsAsync()
        {
            try
            {
                var receiveSmsStatus = await AppPlatform.CheckPermission(new SmsPermissions.ReceiveSms());
                var sendSmsStatus = await AppPlatform.CheckPermission(new SmsPermissions.SendSms());

                if (receiveSmsStatus != PermissionStatus.Granted ||
                    sendSmsStatus != PermissionStatus.Granted)
                {
                    var result = await AppPlatform.AlertAsync(Host,
                        T("perm.sms_title"),
                        T("perm.sms_text"),
                        T("common.yes"), T("common.no"));

                    if (result)
                    {
                        await AppPlatform.RequestPermission(new SmsPermissions.ReceiveSms());
                        await AppPlatform.RequestPermission(new SmsPermissions.SendSms());

                        // Verificar nuevamente
                        receiveSmsStatus = await AppPlatform.CheckPermission(new SmsPermissions.ReceiveSms());
                        sendSmsStatus = await AppPlatform.CheckPermission(new SmsPermissions.SendSms());

                        return receiveSmsStatus == PermissionStatus.Granted &&
                               sendSmsStatus == PermissionStatus.Granted;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                await AppPlatform.AlertAsync(Host, T("common.error"), string.Format(T("perm.sms_error"), ex.Message), T("common.ok"));
                return false;
            }
        }

        private async Task<bool> CheckAndRequestBatteryOptimizationAsync()
        {
            try
            {
                var batteryPermission = new SmsPermissions.BatteryOptimizationPermission();
                var status = await AppPlatform.CheckPermission(batteryPermission);

                if (status != PermissionStatus.Granted)
                {
                    var result = await AppPlatform.AlertAsync(Host,
                        T("diagnostics.battery_title"),
                        T("perm.battery_text"),
                        T("common.yes"), T("common.not_now"));

                    if (result)
                    {
                        await AppPlatform.RequestPermission(batteryPermission);

                        // Esperar un poco y verificar nuevamente
                        await Task.Delay(2000);
                        status = await AppPlatform.CheckPermission(batteryPermission);

                        if (status != PermissionStatus.Granted)
                        {
                            await AppPlatform.AlertAsync(Host,
                                T("perm.info"),
                                T("perm.battery_warn"),
                                T("common.understood"));
                        }
                    }
                }
                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                await AppPlatform.AlertAsync(Host, T("common.error"), string.Format(T("perm.battery_error"), ex.Message), T("common.ok"));
                return false;
            }
        }

        private async Task ShowAutostartInformationAsync()
        {
            try
            {
                var manufacturer = GetManufacturer().ToLower();
                string message = T("perm.autostart_prefix");

                switch (manufacturer)
                {
                    case "xiaomi":
                        message += T("perm.autostart_xiaomi");
                        break;
                    case "huawei":
                        message += T("perm.autostart_huawei");
                        break;
                    case "oppo":
                    case "vivo":
                    case "oneplus":
                        message += T("perm.autostart_perms");
                        break;
                    case "samsung":
                        message += T("perm.autostart_samsung");
                        break;
                    default:
                        message += T("perm.autostart_other");
                        break;
                }

                var result = await AppPlatform.AlertAsync(Host,
                    T("diagnostics.autostart_title"),
                    message + "\n\n" + T("perm.autostart_open"),
                    T("common.yes"), T("common.not_now"));

                if (result)
                {
                    var autostartPermission = new SmsPermissions.AutoStartPermission();
                    await AppPlatform.RequestPermission(autostartPermission);
                }
            }
            catch (Exception ex)
            {
                await AppPlatform.AlertAsync(Host, T("common.error"), string.Format(T("perm.autostart_error"), ex.Message), T("common.ok"));
            }
        }

        private string GetManufacturer()
        {
#if ANDROID
            return Android.OS.Build.Manufacturer ?? "unknown";
#else
            return "unknown";
#endif
        }

        public async Task<bool> CheckBatteryOptimizationStatusAsync()
        {
            try
            {
                var batteryPermission = new SmsPermissions.BatteryOptimizationPermission();
                var status = await AppPlatform.CheckPermission(batteryPermission);
                return status == PermissionStatus.Granted;
            }
            catch
            {
                return false;
            }
        }

        public async Task ShowPermissionStatusAsync()
        {
            try
            {
                var smsStatus = await AppPlatform.CheckPermission(new SmsPermissions.ReceiveSms());
                var batteryStatus = await CheckBatteryOptimizationStatusAsync();
                var manufacturer = GetManufacturer();

                // Sin emoji: el texto de un diálogo no lleva iconos (van en los botones, y son SVG).
                var message = string.Format(T("perm.status_text"),
                    T(smsStatus == PermissionStatus.Granted ? "diagnostics.granted" : "diagnostics.denied"),
                    T(batteryStatus ? "perm.battery_off" : "perm.battery_on"),
                    manufacturer);

                await AppPlatform.AlertAsync(Host, T("perm.status_title"), message, T("common.ok"));
            }
            catch (Exception ex)
            {
                await AppPlatform.AlertAsync(Host, T("common.error"), string.Format(T("perm.status_error"), ex.Message), T("common.ok"));
            }
        }
    }
}
