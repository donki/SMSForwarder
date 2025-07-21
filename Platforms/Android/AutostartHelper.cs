using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;

namespace SMSForwarder.Platforms.Android
{
    public static class AutostartHelper
    {
        private static string GetAutostartSettingIntent(Context context)
        {
            string packageName = context.PackageName;
            string manufacturer = Build.Manufacturer.ToLowerInvariant();

            Log.Debug("AutostartHelper", $"Manufacturer: {manufacturer}, Package: {packageName}");

            switch (manufacturer)
            {
                case "samsung":
                    return "com.samsung.android.sm_poppup";
                case "huawei":
                    return "com.huawei.systemmanager/.startupmanager.ui.StartupNormalAppListActivity";
                case "xiaomi":
                    return "com.miui.securitycenter/com.miui.permcenter.autostart.AutoStartManagementActivity";
                case "oppo":
                    return "com.coloros.safecenter/.startupapp.StartupAppListActivity";
                case "vivo":
                    return "com.vivo.permissionmanager/.manager.BgStartUpManager";
                case "oneplus":
                    return "com.oneplus.security/.bootstart.BootStartManagement";
                default:
                    // Intenta una configuración genérica o informa que no se puede abrir directamente
                    return "android.settings.SETTINGS"; // O quizás ACTION_APPLICATION_DETAILS_SETTINGS con el package name
            }
        }

        public static void OpenAutostartSettings(Context context)
        {
            try
            {
                string intentString = GetAutostartSettingIntent(context);
                Intent intent = new Intent(intentString);
                intent.SetFlags(ActivityFlags.NewTask);
                if (!string.IsNullOrEmpty(intentString) && IsIntentCallable(context, intent))
                {
                    context.StartActivity(intent);
                }
                else
                {
                    // Informar al usuario que debe buscar la configuración manualmente
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Información", "Por favor, configurar el inicio automático para SMSForwarder, para que se envien los SMS mientras está en segundo plano.", "Aceptar");
                        await Application.Current.MainPage.DisplayAlert("Información", "Por favor, si está usando Google Message u otro gestor de SMS y no SMSForwarder no reenvia los mensajes, desinstale el gestor. Hay gestores de SMS que no permiten capturar los SMS y SMSForwarder no funcionará.", "Aceptar");
                        //Intent appDetailsIntent = new Intent(Settings.settings);
                        //appDetailsIntent.SetFlags(ActivityFlags.NewTask);
                        //appDetailsIntent.AddCategory(Intent.CategoryDefault);
                        //appDetailsIntent.SetData(Uri.Parse("package:" + context.PackageName));
                        //context.StartActivity(appDetailsIntent);
                    });
                }
            }
            catch (ActivityNotFoundException e)
            {
                Log.Error("AutostartHelper", "Activity not found: " + e.Message);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Error", "No se pudo abrir la configuración de inicio automático. Por favor, busca la configuración manualmente en tu dispositivo.", "Aceptar");
                });
            }
        }

        private static bool IsIntentCallable(Context context, Intent intent)
        {
            var list = context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return list?.Any() == true;
        }
    }
}