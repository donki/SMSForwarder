using SMSForwarder.Services;
using System.Text.Json;

namespace SMSForwarder
{
    public partial class DiagnosticsPage : ContentPage
    {
        private readonly ILoggingService _loggingService;
        private readonly ILocalizationService _l;

        public DiagnosticsPage(ILoggingService loggingService, ILocalizationService localizationService)
        {
            InitializeComponent();
            _loggingService = loggingService;
            _l = localizationService;

            // Hasta la 2026.09.27.0 la pagina estaba escrita a mano en castellano: con el movil en
            // ingles salia todo en castellano y el estado de los permisos, en ingles («Granted»).
            UpdateLocalizedStrings();
            _l.LanguageChanged += OnLanguageChanged;
            _ = RefreshStatus();
        }

        private string T(string key) => _l.GetString(key);

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            AppPlatform.BeginInvokeOnMainThread(async () =>
            {
                UpdateLocalizedStrings();
                await RefreshStatus();
            });
        }

        private void UpdateLocalizedStrings()
        {
            Title = T("diagnostics.title");
            TitleLabel.Text = T("diagnostics.title");
            SubtitleLabel.Text = T("diagnostics.subtitle");
            PermissionsLabel.Text = T("diagnostics.permissions");
            NumbersLabel.Text = T("diagnostics.numbers");
            PermissionsSetupLabel.Text = T("diagnostics.permissions_setup");
            CheckPermissionsButton.Text = T("diagnostics.check_permissions");
            ConfigureAllButton.Text = T("diagnostics.configure_all");
            BatteryButton.Text = T("diagnostics.battery");
            AutostartButton.Text = T("diagnostics.autostart");
            HintLabel.Text = T("diagnostics.hint");
            ToolsLabel.Text = T("diagnostics.tools");
            RefreshButton.Text = T("diagnostics.refresh");
            ActivityLogLabel.Text = T("diagnostics.activity_log");
            ClearLogButton.Text = T("diagnostics.clear_log");
            if (string.IsNullOrEmpty(PermissionsStatus.Text)) PermissionsStatus.Text = T("diagnostics.checking");
            if (string.IsNullOrEmpty(LogsLabel.Text)) LogsLabel.Text = T("diagnostics.no_activity");
        }

        /// <summary>El estado de un permiso en el idioma de la aplicación («Concedido» / «Granted»).</summary>
        private string StatusText(PermissionStatus status) => status switch
        {
            PermissionStatus.Granted => T("diagnostics.granted"),
            PermissionStatus.Unknown => T("diagnostics.not_decided"),
            _ => T("diagnostics.denied"),
        };

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            await RefreshStatus();
        }

        private async Task RefreshStatus()
        {
            try
            {
                // Verificar permisos
                var receiveSmsStatus = await AppPlatform.CheckStatusAsync<SmsPermissions.ReceiveSms>();
                var sendSmsStatus = await AppPlatform.CheckStatusAsync<SmsPermissions.SendSms>();

                PermissionsStatus.Text = string.Format(T("diagnostics.receive_sms"), StatusText(receiveSmsStatus)) + "\n" +
                                         string.Format(T("diagnostics.send_sms"), StatusText(sendSmsStatus));

                // Contar números configurados
                var phonesJson = Preferences.Default.Get("phones", "[]");
                var phones = JsonSerializer.Deserialize<List<string>>(phonesJson);
                var count = phones?.Count ?? 0;
                PhonesCount.Text = count == 1 ? T("diagnostics.numbers_count_one") : string.Format(T("diagnostics.numbers_count"), count);

                // Cargar logs
                var logs = _loggingService.GetLogContents();
                LogsLabel.Text = string.IsNullOrWhiteSpace(logs) ? T("diagnostics.no_activity") : logs;

                _loggingService.LogInfo("Estado de diagnósticos actualizado");
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al actualizar diagnósticos", ex);
                await AppPlatform.AlertAsync(this, T("common.error"), T("diagnostics.refresh_error"), T("common.ok"));
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
                LogsLabel.Text = T("diagnostics.log_cleared");
                _loggingService.LogInfo("Logs limpiados por el usuario");
            }
            catch (Exception ex)
            {
                await AppPlatform.AlertAsync(this, T("common.error"), string.Format(T("diagnostics.clear_error"), ex.Message), T("common.ok"));
            }
        }

        // Métodos para manejo de permisos (movidos desde MainPage)
        private async void OnCheckPermissionsClicked(object sender, EventArgs e)
        {
            try
            {
                var permissionService = new PermissionService(_l);
                await permissionService.ShowPermissionStatusAsync();
                await RefreshStatus(); // Actualizar estado después de verificar
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al verificar permisos", ex);
                await AppPlatform.AlertAsync(this, T("common.error"), T("diagnostics.check_error"), T("common.ok"));
            }
        }

        private async void OnConfigureAllPermissionsClicked(object sender, EventArgs e)
        {
            try
            {
                var permissionService = new PermissionService(_l);
                var result = await permissionService.CheckAndRequestAllPermissionsAsync();

                if (result)
                {
                    await AppPlatform.AlertAsync(this, T("common.success"), T("diagnostics.all_configured"), T("common.ok"));
                }
                else
                {
                    await AppPlatform.AlertAsync(this, T("diagnostics.attention"), T("diagnostics.some_not_configured"), T("common.ok"));
                }

                await RefreshStatus(); // Actualizar estado después de configurar
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al configurar permisos", ex);
                await AppPlatform.AlertAsync(this, T("common.error"), T("diagnostics.configure_error"), T("common.ok"));
            }
        }

        private async void OnBatteryOptimizationClicked(object sender, EventArgs e)
        {
            try
            {
                var batteryPermission = new SmsPermissions.BatteryOptimizationPermission();
                var status = await AppPlatform.CheckPermission(batteryPermission);

                if (status == PermissionStatus.Granted)
                {
                    await AppPlatform.AlertAsync(this, T("diagnostics.battery_ok_title"), T("diagnostics.battery_ok"), T("common.ok"));
                }
                else
                {
                    var result = await AppPlatform.AlertAsync(this,
                        T("diagnostics.battery_title"),
                        T("diagnostics.battery_on"),
                        T("common.yes"), T("common.no"));

                    if (result)
                    {
                        await AppPlatform.RequestPermission(batteryPermission);
                    }
                }

                await RefreshStatus(); // Actualizar estado después de gestionar batería
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al gestionar optimización de batería", ex);
                await AppPlatform.AlertAsync(this, T("common.error"), T("diagnostics.battery_error"), T("common.ok"));
            }
        }

        private async void OnAutostartClicked(object sender, EventArgs e)
        {
            try
            {
                var autostartPermission = new SmsPermissions.AutoStartPermission();

                await AppPlatform.AlertAsync(this,
                    T("diagnostics.autostart_title"),
                    T("diagnostics.autostart_text"),
                    T("common.understood"));

                await AppPlatform.RequestPermission(autostartPermission);
                await RefreshStatus(); // Actualizar estado después de gestionar autostart
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al gestionar autostart", ex);
                await AppPlatform.AlertAsync(this, T("common.error"), T("diagnostics.autostart_error"), T("common.ok"));
            }
        }
    }
}
