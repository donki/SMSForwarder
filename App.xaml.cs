namespace SMSForwarder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new SplashPage()));
        }

        public static void NavigateToMainApp()
        {
            if (Current?.Windows?.FirstOrDefault() is Window window)
            {
                window.Page = new AppShell();
            }
        }
    }
}
