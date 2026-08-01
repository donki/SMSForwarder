using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View;
using SMSForwarder.Platforms.Android;
using AndroidView = Android.Views.View;

namespace SMSForwarder
{

    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MauiAppCompatActivity
    {
        private SmsReceiver smsReceiver;
        private IntentFilter intentFilter_SMS_RECEIVED;


        private void CheckAndRequestAutostart()
        {
            Context context = Platform.AppContext;
            //AutostartHelper.OpenAutostartSettings(context);
        }
        public async Task<bool> SolicitarPermisosAsync()
        {
            try
            {
                // Solicitar permiso para recibir SMS
                var statusReceiveSms = await Permissions.RequestAsync<SmsPermissions.ReceiveSms>();
                if (statusReceiveSms != PermissionStatus.Granted)
                {
                    Console.WriteLine("Permiso RECEIVE_SMS no otorgado.");
                    return false;
                }

                // Solicitar permiso para enviar SMS
                var statusSendSms = await Permissions.RequestAsync<SmsPermissions.SendSms>();
                if (statusSendSms != PermissionStatus.Granted)
                {
                    Console.WriteLine("Permiso SEND_SMS no otorgado.");
                    return false;
                }

                Console.WriteLine("Todos los permisos fueron otorgados.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al solicitar permisos: {ex.Message}");
                return false;
            }
        }
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ApplySystemBarInsets();

            // Inicializar componentes básicos primero
            intentFilter_SMS_RECEIVED = new IntentFilter("android.provider.Telephony.SMS_RECEIVED")
            {
                Priority = (int)IntentFilterPriority.HighPriority
            };
            smsReceiver = new SmsReceiver();

            // Permiso de notificaciones (Android 13+) para avisar de SMS entrantes. No bloquea.
            try
            {
                if ((int)Build.VERSION.SdkInt >= 33)
                    await Permissions.RequestAsync<Permissions.PostNotifications>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Permiso de notificaciones no concedido: {ex.Message}");
            }

            // Solicitar permisos y registrar el receptor solo si se obtienen
            bool permisosOtorgados = await SolicitarPermisosAsync();
            if (permisosOtorgados)
            {
                try
                {
                    RegisterReceiver(smsReceiver, intentFilter_SMS_RECEIVED);
                    Console.WriteLine("SmsReceiver registrado correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al registrar SmsReceiver: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No se registró SmsReceiver por falta de permisos.");
            }

            CheckAndRequestAutostart();
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Resultado del selector de contactos del sistema (ver ContactPicker).
            ContactPicker.HandleActivityResult(requestCode, resultCode, data);

            // Resultado de la peticion de "app de SMS por defecto" (ver MessageStore).
            MessageStore.HandleActivityResult(requestCode, this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Desregistrar el receptor de SMS
            if (smsReceiver != null)
            {
                UnregisterReceiver(smsReceiver);
                Console.WriteLine("SmsReceiver desregistrado correctamente.");
            }
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
            FinishAndRemoveTask();
        }

        // Android 15 dibuja de borde a borde: separa el contenido del reloj y de la barra inferior.
        private void ApplySystemBarInsets()
        {
            var content = FindViewById(global::Android.Resource.Id.Content);
            if (content is null) return;
            content.SetBackgroundColor(global::Android.Graphics.Color.ParseColor("#2A1CB8")); // indigo de marca
            ViewCompat.SetOnApplyWindowInsetsListener(content, new SystemBarInsetsListener());
            var controller = Window is not null ? WindowCompat.GetInsetsController(Window, Window.DecorView) : null;
            if (controller is not null)
            {
                controller.AppearanceLightStatusBars = false;
                controller.AppearanceLightNavigationBars = false;
            }
        }

        private class SystemBarInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
        {
            public WindowInsetsCompat OnApplyWindowInsets(AndroidView? view, WindowInsetsCompat? insets)
            {
                var consumed = WindowInsetsCompat.Consumed!;
                if (view is null || insets is null) return consumed;
                var bars = insets.GetInsets(WindowInsetsCompat.Type.SystemBars() | WindowInsetsCompat.Type.DisplayCutout());
                if (bars is not null) view.SetPadding(bars.Left, bars.Top, bars.Right, bars.Bottom);
                return consumed;
            }
        }
    }
}

