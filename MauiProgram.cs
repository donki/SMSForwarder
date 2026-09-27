using Microsoft.Extensions.Logging;
using SMSForwarder.Services;

namespace SMSForwarder
{
    public static class MauiProgram
    {
        // El idioma elegido dentro de la app, para el aviso del gestor de excepciones.
        private static ILocalizationService? _localization;

        public static MauiApp CreateMauiApp()
        {
            // Gestor global de excepciones (General 6.12): un error inesperado se registra en
            // crash.log, se avisa en el idioma de la app y la app sigue.
            SocShared.CrashGuard.Install("SMS Forwarder", language: () =>
                _localization?.CurrentLanguage.StartsWith("es", StringComparison.OrdinalIgnoreCase) switch
                {
                    true => "es",
                    false => "en",
                    null => null,
                });

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registrar servicios
            builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
            builder.Services.AddSingleton<ILoggingService, LoggingService>();
#if ANDROID
            builder.Services.AddSingleton<IContactPicker, Platforms.Android.ContactPicker>();
            builder.Services.AddSingleton<IMessageStore, Platforms.Android.MessageStore>();
#endif
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<DiagnosticsPage>();
            builder.Services.AddTransient<Pages.MessagesPage>();
            builder.Services.AddTransient<Pages.ComposePage>();
            builder.Services.AddTransient<Pages.MessageDetailPage>();
            builder.Services.AddTransient<Pages.DestinationPage>();

            // Habilitar todos los niveles de registro en modo debug
#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
#endif

            // Inicializar localización
            var app = builder.Build();
            var localizationService = app.Services.GetRequiredService<ILocalizationService>();
            localizationService.Initialize();
            _localization = localizationService;

            return app;
        }
    }
}


