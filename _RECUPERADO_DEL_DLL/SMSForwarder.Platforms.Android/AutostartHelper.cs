using System;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Java.Lang;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SocShared;

namespace SMSForwarder.Platforms.Android;

public static class AutostartHelper
{
	private static string GetAutostartSettingIntent(Context context)
	{
		string packageName = context.PackageName;
		string text = Build.Manufacturer.ToLowerInvariant();
		Log.Debug("AutostartHelper", "Manufacturer: " + text + ", Package: " + packageName);
		return text switch
		{
			"samsung" => "com.samsung.android.sm_poppup", 
			"huawei" => "com.huawei.systemmanager/.startupmanager.ui.StartupNormalAppListActivity", 
			"xiaomi" => "com.miui.securitycenter/com.miui.permcenter.autostart.AutoStartManagementActivity", 
			"oppo" => "com.coloros.safecenter/.startupapp.StartupAppListActivity", 
			"vivo" => "com.vivo.permissionmanager/.manager.BgStartUpManager", 
			"oneplus" => "com.oneplus.security/.bootstart.BootStartManagement", 
			_ => "android.settings.SETTINGS", 
		};
	}

	public static void OpenAutostartSettings(Context context)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0059: Expected O, but got Unknown
		try
		{
			string autostartSettingIntent = GetAutostartSettingIntent(context);
			Intent val = new Intent(autostartSettingIntent);
			val.SetFlags((ActivityFlags)268435456);
			if (!string.IsNullOrEmpty(autostartSettingIntent) && IsIntentCallable(context, val))
			{
				context.StartActivity(val);
				return;
			}
			MainThread.BeginInvokeOnMainThread((Action)async delegate
			{
				await ModernDialog.AlertAsync(Application.Current.MainPage, "Información", "Por favor, configurar el inicio automático para SMSForwarder, para que se envien los SMS mientras está en segundo plano.", "Aceptar");
				await ModernDialog.AlertAsync(Application.Current.MainPage, "Información", "Por favor, si está usando Google Message u otro gestor de SMS y no SMSForwarder no reenvia los mensajes, desinstale el gestor. Hay gestores de SMS que no permiten capturar los SMS y SMSForwarder no funcionará.", "Aceptar");
			});
		}
		catch (ActivityNotFoundException ex)
		{
			ActivityNotFoundException ex2 = ex;
			Log.Error("AutostartHelper", "Activity not found: " + ((Throwable)ex2).Message);
			MainThread.BeginInvokeOnMainThread((Action)delegate
			{
				ModernDialog.AlertAsync(Application.Current.MainPage, "Error", "No se pudo abrir la configuración de inicio automático. Por favor, busca la configuración manualmente en tu dispositivo.", "Aceptar");
			});
		}
	}

	private static bool IsIntentCallable(Context context, Intent intent)
	{
		return context.PackageManager.QueryIntentActivities(intent, (PackageInfoFlags)65536)?.Any() ?? false;
	}
}
