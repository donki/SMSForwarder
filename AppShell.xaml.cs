using SMSForwarder.Pages;
using SMSForwarder.Services;

namespace SMSForwarder
{
    public partial class AppShell : Shell
    {
        private readonly ILocalizationService _localizationService;

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(DiagnosticsPage), typeof(DiagnosticsPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));

            // Obtener el servicio de localización desde el contenedor de servicios
            _localizationService = MauiApplication.Current?.Services.GetRequiredService<ILocalizationService>() ?? new LocalizationService();
            _localizationService.LanguageChanged += OnLanguageChanged;

            // Actualizar strings localizados
            UpdateLocalizedStrings();
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(UpdateLocalizedStrings);
        }

        private void UpdateLocalizedStrings()
        {
            // Actualizar header
            HeaderTitle.Text = "SMS Forwarder";
            HeaderSubtitle.Text = _localizationService.GetString("menu.header.subtitle");

            // Actualizar items del menú
            SettingsItem.Title = _localizationService.GetString("menu.settings");
            DiagnosticsItem.Title = _localizationService.GetString("menu.diagnostics");
            AboutItem.Title = _localizationService.GetString("menu.about");

            // Actualizar footer
            FooterVersion.Text = $"v2026.06.26.0 - sOCratic";
        }
    }
}
