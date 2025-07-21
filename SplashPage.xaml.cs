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
        
        // Navegar a la aplicación principal
        App.NavigateToMainApp();
    }
}