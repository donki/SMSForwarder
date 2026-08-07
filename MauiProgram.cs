using Microsoft.Extensions.Logging;
using SMSForwarder.Services;

namespace SMSForwarder
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
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

            // Habilitar todos los niveles de registro en modo debug
#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
#endif

            // Inicializar localización
            var app = builder.Build();
            var localizationService = app.Services.GetRequiredService<ILocalizationService>();
            localizationService.Initialize();

            return app;
        }
    }
}


