using System.Globalization;

namespace SMSForwarder.Pages
{
    public partial class AboutPage : ContentPage
    {
        // CONFIGURACIÓN
        private const string ContactEmail = "jsoladelarosa@gmail.com";
        private const string DonationUrl = "https://ko-fi.com/smsforwarder";
        private const string EmailSubject = "Contacto desde SMS Forwarder";

        public AboutPage()
        {
            InitializeComponent();
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            var currentLanguage = Microsoft.Maui.Storage.Preferences.Get("AppLanguage", GetSystemLanguage());

            if (currentLanguage == "es")
            {
                SetSpanishTexts();
            }
            else
            {
                SetEnglishTexts();
            }
        }

        private string GetSystemLanguage()
        {
            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            return culture == "es" ? "es" : "en";
        }

        private void SetSpanishTexts()
        {
            Title = "Acerca de";
            // Los textos ya están en español en el XAML
        }

        private void SetEnglishTexts()
        {
            // Los textos se resuelven por x:Name, no navegando el arbol visual por indices: al
            // migrar el diseno (Frame -> Border) esos casts posicionales se rompian con
            // InvalidCastException (constitucion, seccion 8 y anexo A.9).
            Title = "About";

            VersionLabel.Text = "Version 2026.07.17.0";
            DescriptionLabel.Text = "Automatically forward your SMS to other phone numbers";

            ContactTitleLabel.Text = "📧 Contact";
            ContactInstructionLabel.Text = "Tap to send an email";

            SupportTitleLabel.Text = "☕ Support Development";
            DonationButton.Text = "Ko-fi.com - Buy me a coffee";
            SupportDescLabel.Text = "Your support helps maintain and improve the app";

            LegalTitleLabel.Text = "⚖️ Legal Notice";
            LegalText1Label.Text = "This software is provided 'as is', without warranties of any kind. The user is responsible for proper use of the application and compliance with local laws.";
            LegalText2Label.Text = "In no event shall the authors be liable for direct, indirect, incidental or consequential damages resulting from the use of this software.";
            WarningLabel.Text = "⚠️ Use at your own risk";

            LanguageDescLabel.Text = "Select your preferred language";

            BackButton.Text = "← Back";
        }

        private async void OnBackClicked(object? sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnContactEmailClicked(object? sender, EventArgs e)
        {
            try
            {
                var currentLanguage = Microsoft.Maui.Storage.Preferences.Get("AppLanguage", GetSystemLanguage());

                // Crear el cuerpo del email con información de la aplicación
                var appName = "SMS Forwarder";
                var appVersion = "2026.06.13.2";
                var emailBody = currentLanguage == "es"
                    ? $"Hola,\n\nMe pongo en contacto desde la aplicación {appName} (versión {appVersion}).\n\n[Escribe tu mensaje aquí]\n\nSaludos."
                    : $"Hello,\n\nI'm contacting you from the {appName} app (version {appVersion}).\n\n[Write your message here]\n\nBest regards.";

#if ANDROID
                // Usar Intent de Android directamente
                var context = Platform.CurrentActivity ?? Android.App.Application.Context;
                if (context != null)
                {
                    var emailIntent = new Android.Content.Intent(Android.Content.Intent.ActionSendto);
                    emailIntent.SetData(Android.Net.Uri.Parse($"mailto:{ContactEmail}"));
                    emailIntent.PutExtra(Android.Content.Intent.ExtraSubject, EmailSubject);
                    emailIntent.PutExtra(Android.Content.Intent.ExtraText, emailBody);

                    // Crear chooser para mostrar todas las aplicaciones de email disponibles
                    var chooserTitle = currentLanguage == "es" ? "Enviar email con:" : "Send email with:";
                    var chooser = Android.Content.Intent.CreateChooser(emailIntent, chooserTitle);

                    if (chooser != null)
                    {
                        chooser.AddFlags(Android.Content.ActivityFlags.NewTask);
                        context.StartActivity(chooser);
                        return;
                    }
                }
#endif

                // Fallback usando MAUI Essentials si el Intent no funciona
                var message = new Microsoft.Maui.ApplicationModel.Communication.EmailMessage
                {
                    Subject = EmailSubject,
                    Body = emailBody,
                    To = new List<string> { ContactEmail }
                };

                await Microsoft.Maui.ApplicationModel.Communication.Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException)
            {
                var currentLanguage = Microsoft.Maui.Storage.Preferences.Get("AppLanguage", GetSystemLanguage());
                var errorTitle = currentLanguage == "es" ? "Error" : "Error";
                var errorMessage = currentLanguage == "es"
                    ? "Cliente de correo no disponible en este dispositivo"
                    : "Email client not available on this device";

                await DisplayAlert(errorTitle, errorMessage, "OK");
            }
            catch (Exception ex)
            {
                var currentLanguage = Microsoft.Maui.Storage.Preferences.Get("AppLanguage", GetSystemLanguage());
                var errorTitle = currentLanguage == "es" ? "Error" : "Error";
                var errorMessage = currentLanguage == "es"
                    ? $"No se pudo abrir el cliente de correo: {ex.Message}"
                    : $"Could not open email client: {ex.Message}";

                await DisplayAlert(errorTitle, errorMessage, "OK");
            }
        }

        private async void OnDonationClicked(object? sender, EventArgs e)
        {
            try
            {
                var uri = new Uri(DonationUrl);
                var browserLaunchOptions = new Microsoft.Maui.ApplicationModel.BrowserLaunchOptions
                {
                    LaunchMode = Microsoft.Maui.ApplicationModel.BrowserLaunchMode.SystemPreferred,
                    TitleMode = Microsoft.Maui.ApplicationModel.BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.FromArgb("#E67E22"),
                    PreferredControlColor = Color.FromArgb("#FFFFFF")
                };

                await Microsoft.Maui.ApplicationModel.Browser.OpenAsync(uri, browserLaunchOptions);
            }
            catch (FeatureNotSupportedException)
            {
                try
                {
                    await Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.SetTextAsync(DonationUrl);
                    var currentLanguage = Microsoft.Maui.Storage.Preferences.Get("AppLanguage", GetSystemLanguage());
                    var title = currentLanguage == "es" ? "Navegador no disponible" : "Browser not available";
                    var message = currentLanguage == "es"
                        ? $"Enlace copiado al portapapeles:\n{DonationUrl}"
                        : $"Link copied to clipboard:\n{DonationUrl}";

                    await DisplayAlert(title, message, "OK");
                }
                catch
                {
                    var currentLanguage = Microsoft.Maui.Storage.Preferences.Get("AppLanguage", GetSystemLanguage());
                    var errorTitle = currentLanguage == "es" ? "Error" : "Error";
                    var errorMessage = currentLanguage == "es"
                        ? $"No se pudo abrir el navegador: {DonationUrl}"
                        : $"Could not open browser: {DonationUrl}";

                    await DisplayAlert(errorTitle, errorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    await Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.SetTextAsync(DonationUrl);
                    var currentLanguage = Microsoft.Maui.Storage.Preferences.Get("AppLanguage", GetSystemLanguage());
                    var title = currentLanguage == "es" ? "Error al abrir enlace" : "Error opening link";
                    var message = currentLanguage == "es"
                        ? $"No se pudo abrir el navegador ({ex.Message}), enlace copiado al portapapeles."
                        : $"Could not open browser ({ex.Message}), link copied to clipboard.";

                    await DisplayAlert(title, message, "OK");
                }
                catch
                {
                    var currentLanguage = Microsoft.Maui.Storage.Preferences.Get("AppLanguage", GetSystemLanguage());
                    var errorTitle = currentLanguage == "es" ? "Error" : "Error";
                    var errorMessage = currentLanguage == "es"
                        ? $"No se pudo abrir el enlace: {DonationUrl}"
                        : $"Could not open link: {DonationUrl}";

                    await DisplayAlert(errorTitle, errorMessage, "OK");
                }
            }
        }

        private void OnSpanishClicked(object? sender, EventArgs e)
        {
            Microsoft.Maui.Storage.Preferences.Set("AppLanguage", "es");
            SetSpanishTexts();

            // Actualizar botones de idioma
            var languageCard = (Frame)((StackLayout)((ScrollView)Content).Content).Children[4];
            var languageStack = (StackLayout)languageCard.Content;
            var buttonGrid = (Grid)languageStack.Children[1];

            var spanishButton = (Button)buttonGrid.Children[0];
            var englishButton = (Button)buttonGrid.Children[1];

            // Español activo
            spanishButton.BackgroundColor = Color.FromArgb("#9B59B6");
            spanishButton.TextColor = Colors.White;
            spanishButton.BorderWidth = 0;

            // Inglés inactivo
            englishButton.BackgroundColor = Colors.Transparent;
            englishButton.TextColor = Color.FromArgb("#9B59B6");
            englishButton.BorderColor = Color.FromArgb("#9B59B6");
            englishButton.BorderWidth = 1;
        }

        private void OnEnglishClicked(object? sender, EventArgs e)
        {
            Microsoft.Maui.Storage.Preferences.Set("AppLanguage", "en");
            SetEnglishTexts();

            // Actualizar botones de idioma
            var languageCard = (Frame)((StackLayout)((ScrollView)Content).Content).Children[4];
            var languageStack = (StackLayout)languageCard.Content;
            var buttonGrid = (Grid)languageStack.Children[1];

            var spanishButton = (Button)buttonGrid.Children[0];
            var englishButton = (Button)buttonGrid.Children[1];

            // Inglés activo
            englishButton.BackgroundColor = Color.FromArgb("#9B59B6");
            englishButton.TextColor = Colors.White;
            englishButton.BorderWidth = 0;

            // Español inactivo
            spanishButton.BackgroundColor = Colors.Transparent;
            spanishButton.TextColor = Color.FromArgb("#9B59B6");
            spanishButton.BorderColor = Color.FromArgb("#9B59B6");
            spanishButton.BorderWidth = 1;
        }
    }
}