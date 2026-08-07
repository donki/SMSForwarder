using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Respuesta rapida con mensaje a una llamada entrante (RESPOND_VIA_MESSAGE). Es uno de los
    /// cuatro componentes obligatorios para poder ser la app de SMS por defecto.
    ///
    /// El sistema entrega el destinatario en la URI del intent y el texto elegido por el usuario
    /// en EXTRA_TEXT; aqui se envia el SMS y se guarda en Enviados, sin abrir interfaz.
    /// </summary>
    [Register("com.socratic.smsforwarder.HeadlessSmsSendService")]
    [Service(
        Exported = true,
        Permission = "android.permission.SEND_RESPOND_VIA_MESSAGE")]
    [IntentFilter(
        new[] { "android.intent.action.RESPOND_VIA_MESSAGE" },
        Categories = new[] { Intent.CategoryDefault },
        DataSchemes = new[] { "sms", "smsto", "mms", "mmsto" })]
    public class HeadlessSmsSendService : Service
    {
        public override IBinder? OnBind(Intent? intent) => null;

        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            try
            {
                var address = ComposeSmsActivity.ExtractRecipient(intent);
                var body = intent?.GetStringExtra(Intent.ExtraText);

                if (!string.IsNullOrWhiteSpace(address) && !string.IsNullOrEmpty(body))
                {
                    // Sin bloquear el hilo del servicio: el envio y la escritura en Enviados
                    // ocurren en segundo plano y el servicio se detiene al terminar.
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await new MessageStore().SendAsync(address!, body!);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[QuickReply] send: {ex.Message}");
                        }
                        finally
                        {
                            StopSelf(startId);
                        }
                    });
                    return StartCommandResult.NotSticky;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[QuickReply] {ex.Message}");
            }

            StopSelf(startId);
            return StartCommandResult.NotSticky;
        }
    }
}
