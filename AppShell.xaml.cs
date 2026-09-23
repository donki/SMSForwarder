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
            Routing.RegisterRoute(nameof(MessagesPage), typeof(MessagesPage));
            Routing.RegisterRoute(nameof(ComposePage), typeof(ComposePage));
            Routing.RegisterRoute(nameof(MessageDetailPage), typeof(MessageDetailPage));
            Routing.RegisterRoute(nameof(DestinationPage), typeof(DestinationPage));

            // Obtener el servicio de localización desde el contenedor de servicios
            _localizationService = MauiApplication.Current?.Services.GetRequiredService<ILocalizationService>() ?? new LocalizationService();
            _localizationService.LanguageChanged += OnLanguageChanged;

            // Actualizar strings localizados
            UpdateLocalizedStrings();
        }

        /// <summary>
        /// Estando en el modo de seleccion multiple del buzon, el boton de atras sale del modo en
        /// vez de cerrar la pantalla. Va aqui y no en la pagina porque el Shell se queda el gesto
        /// de atras de su pagina raiz: el <c>OnBackButtonPressed</c> de la pagina no llega a correr.
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            if (CurrentPage is MessagesPage { IsSelecting: true } messages)
            {
                messages.CancelSelection();
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(UpdateLocalizedStrings);
        }

        private void UpdateLocalizedStrings()
        {
            // Actualizar header
            HeaderTitle.Text = "SMS Forwarder";

            // Actualizar items del menú
            MessagesItem.Title = _localizationService.GetString("menu.messages");
            SettingsItem.Title = _localizationService.GetString("menu.settings");
            DiagnosticsItem.Title = _localizationService.GetString("menu.diagnostics");
            AboutItem.Title = _localizationService.GetString("menu.about");

            // Actualizar footer
            // La version se lee del paquete: escrita a mano se quedaba vieja (decia 2026.08.02.0).
            FooterVersion.Text = $"v{AppInfo.Current.VersionString}";
        }
    }
}
