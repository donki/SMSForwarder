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

            builder.Services.AddSingleton<ILoggingService, LoggingService>();
            builder.Services.AddSingleton<IContactService, ContactService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<DiagnosticsPage>();
            builder.Services.AddTransient<ContactsPage>();

            // Habilitar todos los niveles de registro en modo debug
#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
#endif

            return builder.Build();
        }
    }
}


