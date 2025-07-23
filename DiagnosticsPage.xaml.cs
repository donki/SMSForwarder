using SMSForwarder.Services;
using System.Text.Json;

namespace SMSForwarder
{
    public partial class DiagnosticsPage : ContentPage
    {
        private readonly ILoggingService _loggingService;

        public DiagnosticsPage(ILoggingService loggingService)
        {
            InitializeComponent();
            _loggingService = loggingService;
            RefreshStatus();
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            await RefreshStatus();
        }

        private async Task RefreshStatus()
        {
            try
            {
                // Verificar permisos
                var receiveSmsStatus = await Permissions.CheckStatusAsync<SmsPermissions.ReceiveSms>();
                var sendSmsStatus = await Permissions.CheckStatusAsync<SmsPermissions.SendSms>();
                var readSmsStatus = await Permissions.CheckStatusAsync<SmsPermissions.ReadSms>();

                PermissionsStatus.Text = $"Recibir SMS: {receiveSmsStatus}\n" +
                                       $"Enviar SMS: {sendSmsStatus}\n" +
                                       $"Leer SMS: {readSmsStatus}";

                // Contar números configurados
                var phonesJson = Preferences.Default.Get("phones", "[]");
                var phones = JsonSerializer.Deserialize<List<string>>(phonesJson);
                PhonesCount.Text = $"{phones?.Count ?? 0} números configurados";

                // Cargar logs
                LogsLabel.Text = _loggingService.GetLogContents();

                _loggingService.LogInfo("Estado de diagnósticos actualizado");
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al actualizar diagnósticos", ex);
                await DisplayAlert("Error", "Error al actualizar el estado", "OK");
            }
        }

        private async void OnClearLogsClicked(object sender, EventArgs e)
        {
            try
            {
                var logFile = Path.Combine(FileSystem.AppDataDirectory, "sms_forwarder.log");
                if (File.Exists(logFile))
                {
                    File.Delete(logFile);
                }
                LogsLabel.Text = "Logs limpiados";
                _loggingService.LogInfo("Logs limpiados por el usuario");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al limpiar logs: {ex.Message}", "OK");
            }
        }

        private async void OnTestSmsClicked(object sender, EventArgs e)
        {
            try
            {
                var phonesJson = Preferences.Default.Get("phones", "[]");
                var phones = JsonSerializer.Deserialize<List<string>>(phonesJson);

                if (phones == null || phones.Count == 0)
                {
                    await DisplayAlert("Sin números", "No hay números configurados para enviar SMS de prueba", "OK");
                    return;
                }

                var testMessage = $"SMS de prueba desde SMSForwarder - {DateTime.Now:HH:mm:ss}";
                
#if ANDROID
                var smsManager = Android.Telephony.SmsManager.Default;
                foreach (var phone in phones)
                {
                    try
                    {
                        smsManager.SendTextMessage(phone, null, testMessage, null, null);
                        _loggingService.LogInfo($"SMS de prueba enviado a {phone}");
                    }
                    catch (Exception ex)
                    {
                        _loggingService.LogError($"Error enviando SMS de prueba a {phone}", ex);
                    }
                }
#endif
                await DisplayAlert("SMS de prueba", $"SMS de prueba enviado a {phones.Count} números", "OK");
                await RefreshStatus();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error en SMS de prueba", ex);
                await DisplayAlert("Error", $"Error al enviar SMS de prueba: {ex.Message}", "OK");
            }
        }
    }
}