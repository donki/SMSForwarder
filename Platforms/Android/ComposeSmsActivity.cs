using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Activity que responde a SENDTO/VIEW sobre esquemas sms/smsto/mms/mmsto. Obligatoria para
    /// calificar como app de SMS por defecto (el sistema/otros apps la usan para "enviar mensaje").
    /// SMS Forwarder no ofrece redaccion completa: redirige a la pantalla principal (Mensajes).
    /// </summary>
    [Register("com.socratic.smsforwarder.ComposeSmsActivity")]
    [Activity(
        Label = "SMS Forwarder",
        Exported = true,
        Theme = "@android:style/Theme.Translucent.NoTitleBar",
        LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionSendto },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "sms", "smsto", "mms", "mmsto" })]
    [IntentFilter(
        new[] { Intent.ActionView, Intent.ActionSend },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "sms", "smsto", "mms", "mmsto" })]
    public class ComposeSmsActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                var main = new Intent(this, typeof(MainActivity));
                main.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                StartActivity(main);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ComposeSms] {ex.Message}");
            }
            Finish();
        }
    }
}
