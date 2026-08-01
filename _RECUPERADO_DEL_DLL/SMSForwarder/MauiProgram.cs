using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using SMSForwarder.Platforms.Android;
using SMSForwarder.Services;

namespace SMSForwarder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		MauiAppBuilder obj = MauiApp.CreateBuilder(true);
		FontsMauiAppBuilderExtensions.ConfigureFonts(AppHostBuilderExtensions.UseMauiApp<App>(obj), (Action<IFontCollection>)delegate(IFontCollection fonts)
		{
			FontCollectionExtensions.AddFont(fonts, "OpenSans-Regular.ttf", "OpenSansRegular");
			FontCollectionExtensions.AddFont(fonts, "OpenSans-Semibold.ttf", "OpenSansSemibold");
		});
		obj.Services.AddSingleton<ILocalizationService, LocalizationService>();
		obj.Services.AddSingleton<ILoggingService, LoggingService>();
		obj.Services.AddSingleton<IContactPicker, ContactPicker>();
		obj.Services.AddSingleton<IMessageStore, MessageStore>();
		obj.Services.AddSingleton<MainPage>();
		obj.Services.AddTransient<DiagnosticsPage>();
		obj.Services.AddTransient<MessagesPage>();
		MauiApp obj2 = obj.Build();
		obj2.Services.GetRequiredService<ILocalizationService>().Initialize();
		return obj2;
	}
}
