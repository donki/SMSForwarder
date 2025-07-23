using SMSForwarder.Services;

namespace SMSForwarder;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
        StartAnimations();
        NavigateToMainPage();
    }

    private async void StartAnimations()
    {
        // Animación de entrada del logo
        LogoFrame.Scale = 0;
        await LogoFrame.ScaleTo(1, 800, Easing.BounceOut);

        // Animación de fade in para el título
        TitleLabel.Opacity = 0;
        await TitleLabel.FadeTo(1, 600);

        // Animación de fade in para el subtítulo
        SubtitleLabel.Opacity = 0;
        await SubtitleLabel.FadeTo(1, 600);
    }

    private async void NavigateToMainPage()
    {
        // Esperar 3 segundos antes de navegar
        await Task.Delay(3000);
        
        // Solicitar permisos básicos al iniciar la aplicación
        try
        {
            var permissionService = new PermissionService();
            
            // Solo solicitar permisos SMS automáticamente, los otros se configuran manualmente
            var smsReceiveStatus = await new SmsPermissions.ReceiveSms().CheckStatusAsync();
            var smsSendStatus = await new SmsPermissions.SendSms().CheckStatusAsync();
            var smsReadStatus = await new SmsPermissions.ReadSms().CheckStatusAsync();

            if (smsReceiveStatus != PermissionStatus.Granted ||
                smsSendStatus != PermissionStatus.Granted ||
                smsReadStatus != PermissionStatus.Granted)
            {
                var result = await DisplayAlert(
                    "Permisos Requeridos",
                    "SMS Forwarder necesita permisos SMS para funcionar. ¿Desea concederlos ahora?",
                    "Sí", "Ahora no");

                if (result)
                {
                    await new SmsPermissions.ReceiveSms().RequestAsync();
                    await new SmsPermissions.SendSms().RequestAsync();
                    await new SmsPermissions.ReadSms().RequestAsync();
                }
            }
        }
        catch (Exception ex)
        {
            // Si hay error con permisos, continuar sin mostrar error al usuario
            System.Diagnostics.Debug.WriteLine($"Error al solicitar permisos: {ex.Message}");
        }
        
        // Navegar a la aplicación principal
        App.NavigateToMainApp();
    }
}