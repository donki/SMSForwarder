using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Punto de entrada de "enviar mensaje" desde otras apps y desde el sistema: responde a
    /// SENDTO/VIEW/SEND sobre los esquemas sms/smsto/mms/mmsto. Es uno de los cuatro componentes
    /// obligatorios para poder ser la app de SMS por defecto.
    ///
    /// Extrae el destinatario y el texto del intent y abre la pantalla de redaccion de la app
    /// (<c>ComposePage</c>) con ambos campos rellenos, via <see cref="PendingCompose"/>.
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
                PendingCompose.Set(ExtractRecipient(Intent), ExtractBody(Intent));

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

        /// <summary>
        /// Numero de destino del intent. Viene en la parte especifica del esquema
        /// (<c>smsto:+34600123456</c>), que puede traer varios destinatarios separados por coma
        /// y una query con el cuerpo; nos quedamos con el primer numero.
        /// </summary>
        internal static string? ExtractRecipient(Intent? intent)
        {
            var data = intent?.Data;
            if (data == null) return null;

            var raw = data.SchemeSpecificPart;
            if (string.IsNullOrWhiteSpace(raw)) return null;

            var queryStart = raw.IndexOf('?');
            if (queryStart >= 0) raw = raw.Substring(0, queryStart);

            var first = raw.Split(',', ';')[0].Trim();
            if (string.IsNullOrWhiteSpace(first)) return null;

            return global::Android.Net.Uri.Decode(first)?.Trim();
        }

        /// <summary>
        /// Texto propuesto: extra <c>sms_body</c> (convencion de Android), el <c>EXTRA_TEXT</c> de
        /// un ACTION_SEND, o el parametro <c>body</c> de la propia URI.
        /// </summary>
        internal static string? ExtractBody(Intent? intent)
        {
            if (intent == null) return null;

            var body = intent.GetStringExtra("sms_body");
            if (!string.IsNullOrEmpty(body)) return body;

            body = intent.GetStringExtra(Intent.ExtraText);
            if (!string.IsNullOrEmpty(body)) return body;

            try
            {
                return intent.Data?.GetQueryParameter("body");
            }
            catch
            {
                // Las URIs opacas (sms:...) no admiten consulta de parametros.
                return null;
            }
        }
    }
}
