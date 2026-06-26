using Microsoft.Maui.Controls;

namespace SMSForwarder.Services
{
    public class PermissionService
    {
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
                var receiveSmsStatus = await new SmsPermissions.ReceiveSms().CheckStatusAsync();
                var sendSmsStatus = await new SmsPermissions.SendSms().CheckStatusAsync();

                if (receiveSmsStatus != PermissionStatus.Granted ||
                    sendSmsStatus != PermissionStatus.Granted)
                {
                    var result = await Application.Current.MainPage.DisplayAlert(
                        "Permisos SMS Requeridos",
                        "Esta aplicación necesita permisos SMS para funcionar correctamente. ¿Desea conceder los permisos?",
                        "Sí", "No");

                    if (result)
                    {
                        await new SmsPermissions.ReceiveSms().RequestAsync();
                        await new SmsPermissions.SendSms().RequestAsync();

                        // Verificar nuevamente
                        receiveSmsStatus = await new SmsPermissions.ReceiveSms().CheckStatusAsync();
                        sendSmsStatus = await new SmsPermissions.SendSms().CheckStatusAsync();

                        return receiveSmsStatus == PermissionStatus.Granted &&
                               sendSmsStatus == PermissionStatus.Granted;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al verificar permisos SMS: {ex.Message}", "OK");
                return false;
            }
        }

        private async Task<bool> CheckAndRequestBatteryOptimizationAsync()
        {
            try
            {
                var batteryPermission = new SmsPermissions.BatteryOptimizationPermission();
                var status = await batteryPermission.CheckStatusAsync();

                if (status != PermissionStatus.Granted)
                {
                    var result = await Application.Current.MainPage.DisplayAlert(
                        "Optimización de Batería",
                        "Para que la aplicación funcione correctamente en segundo plano, es recomendable desactivar la optimización de batería.\n\n¿Desea abrir la configuración?",
                        "Sí", "Ahora no");

                    if (result)
                    {
                        await batteryPermission.RequestAsync();
                        
                        // Esperar un poco y verificar nuevamente
                        await Task.Delay(2000);
                        status = await batteryPermission.CheckStatusAsync();
                        
                        if (status != PermissionStatus.Granted)
                        {
                            await Application.Current.MainPage.DisplayAlert(
                                "Información",
                                "Si no desactivó la optimización de batería, la aplicación podría no recibir mensajes cuando esté en segundo plano.",
                                "Entendido");
                        }
                    }
                }
                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al verificar optimización de batería: {ex.Message}", "OK");
                return false;
            }
        }

        private async Task ShowAutostartInformationAsync()
        {
            try
            {
                var manufacturer = GetManufacturer().ToLower();
                string message = "Para que la aplicación funcione correctamente después de reiniciar el dispositivo, ";

                switch (manufacturer)
                {
                    case "xiaomi":
                        message += "vaya a Configuración > Aplicaciones > Administrar aplicaciones > SMS Forwarder > Inicio automático y actívelo.";
                        break;
                    case "huawei":
                        message += "vaya a Configuración > Aplicaciones > SMS Forwarder > Inicio automático y actívelo.";
                        break;
                    case "oppo":
                        message += "vaya a Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático y actívelo.";
                        break;
                    case "vivo":
                        message += "vaya a Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático y actívelo.";
                        break;
                    case "samsung":
                        message += "vaya a Configuración > Aplicaciones > SMS Forwarder > Batería > Optimizar uso de batería y desactívelo.";
                        break;
                    case "oneplus":
                        message += "vaya a Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático y actívelo.";
                        break;
                    default:
                        message += "asegúrese de que la aplicación tenga permisos para ejecutarse en segundo plano.";
                        break;
                }

                var result = await Application.Current.MainPage.DisplayAlert(
                    "Configuración de Autostart",
                    message + "\n\n¿Desea abrir la configuración ahora?",
                    "Sí", "Ahora no");

                if (result)
                {
                    var autostartPermission = new SmsPermissions.AutoStartPermission();
                    await autostartPermission.RequestAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al mostrar información de autostart: {ex.Message}", "OK");
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
                var status = await batteryPermission.CheckStatusAsync();
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
                var smsStatus = await new SmsPermissions.ReceiveSms().CheckStatusAsync();
                var batteryStatus = await CheckBatteryOptimizationStatusAsync();
                var manufacturer = GetManufacturer();

                var message = $"Estado de permisos:\n\n" +
                             $"📱 SMS: {(smsStatus == PermissionStatus.Granted ? "✅ Concedido" : "❌ Denegado")}\n" +
                             $"🔋 Optimización batería: {(batteryStatus ? "✅ Desactivada" : "❌ Activada")}\n" +
                             $"🚀 Fabricante: {manufacturer}\n\n" +
                             $"Para un funcionamiento óptimo, todos los permisos deben estar concedidos.";

                await Application.Current.MainPage.DisplayAlert("Estado de Permisos", message, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al verificar estado: {ex.Message}", "OK");
            }
        }
    }
}
