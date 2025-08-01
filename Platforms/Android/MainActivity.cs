using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using SMSForwarder.Platforms.Android;

namespace SMSForwarder
{

    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
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

                // Solicitar permiso para leer SMS
                var statusReadSms = await Permissions.RequestAsync<SmsPermissions.ReadSms>();
                if (statusReadSms != PermissionStatus.Granted)
                {
                    Console.WriteLine("Permiso READ_SMS no otorgado.");
                    return false;
                }

                // Solicitar permiso para SMS premium
                var statusPremiumSms = await Permissions.RequestAsync<SmsPremiumPermission>();
                if (statusPremiumSms != PermissionStatus.Granted)
                {
                    Console.WriteLine("Permiso SEND_SMS_NO_CONFIRMATION no otorgado.");
                    // Puedes mostrar un diálogo explicativo aquí si lo deseas
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

            // Inicializar componentes básicos primero
            intentFilter_SMS_RECEIVED = new IntentFilter("android.provider.Telephony.SMS_RECEIVED")
            {
                Priority = (int)IntentFilterPriority.HighPriority
            };
            smsReceiver = new SmsReceiver();

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
    }
}

