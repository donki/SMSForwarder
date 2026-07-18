using System.Globalization;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace SMSForwarder.Pages
{
    public partial class AboutPage : ContentPage
    {
        // CONFIGURACIÓN
        private const string ContactEmail = "jsoladelarosa@gmail.com";
        private const string DonationUrl = "https://ko-fi.com/josepsola";
        private const string EmailSubject = "Contacto desde SMS Forwarder";

        // Versión leída de AppInfo (= ApplicationDisplayVersion del csproj), nunca hardcodeada.
        private static string AppVersion => AppInfo.Current.VersionString;

        public AboutPage()
        {
            InitializeComponent();
            ApplyLanguage(GetCurrentLanguage());
        }

        private static string GetCurrentLanguage()
            => Preferences.Get("AppLanguage", GetSystemLanguage());

        private static string GetSystemLanguage()
        {
            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            return culture == "es" ? "es" : "en";
        }

        private void ApplyLanguage(string language)
        {
            bool spanish = language == "es";

            if (spanish)
                SetSpanishTexts();
            else
                SetEnglishTexts();

            UpdateLanguageButtons(spanish);
        }

        private void SetSpanishTexts()
        {
            Title = "Acerca de";

            VersionLabel.Text = $"Versión {AppVersion}";
            DescriptionLabel.Text = "Reenvía automáticamente tus SMS a otros números de teléfono";

            ContactTitleLabel.Text = "Contacto";
            ContactInstructionLabel.Text = "Toca para enviar un correo electrónico";

            SupportTitleLabel.Text = "Apoya el Desarrollo";
            DonationButton.Text = "Ko-fi.com - Invítame un café";
            SupportDescLabel.Text = "Tu apoyo ayuda a mantener y mejorar la aplicación";

            LanguageTitleLabel.Text = "Idioma";
            LanguageDescLabel.Text = "Selecciona tu idioma preferido";

            PrivacyTitleLabel.Text = "Privacidad";
            PrivacyTextLabel.Text = "Esta aplicación no recopila tus datos personales ni los envía a los desarrolladores. La información se procesa en tu dispositivo para la función propia de la app.";

            LicenseTitleLabel.Text = "Licencia";
            LicenseTextLabel.Text = "Esta aplicación es software libre distribuido bajo licencia MIT.";

            LegalTitleLabel.Text = "Aviso Legal";
            LegalText1Label.Text = "Este software se proporciona «tal cual», sin garantías de ningún tipo. El usuario es responsable del uso adecuado de la aplicación y del cumplimiento de las leyes locales.";
            LegalText2Label.Text = "En ningún caso los autores serán responsables de daños directos, indirectos, incidentales o consecuentes que resulten del uso de este software.";
            WarningLabel.Text = "⚠️ Uso bajo su propio riesgo";

            BackButton.Text = "← Volver";
        }

        private void SetEnglishTexts()
        {
            Title = "About";

            VersionLabel.Text = $"Version {AppVersion}";
            DescriptionLabel.Text = "Automatically forward your SMS to other phone numbers";

            ContactTitleLabel.Text = "Contact";
            ContactInstructionLabel.Text = "Tap to send an email";

            SupportTitleLabel.Text = "Support Development";
            DonationButton.Text = "Ko-fi.com - Buy me a coffee";
            SupportDescLabel.Text = "Your support helps maintain and improve the app";

            LanguageTitleLabel.Text = "Language";
            LanguageDescLabel.Text = "Select your preferred language";

            PrivacyTitleLabel.Text = "Privacy";
            PrivacyTextLabel.Text = "This app does not collect your personal data or send it to the developers. Information is processed on your device for the app's own purpose.";

            LicenseTitleLabel.Text = "License";
            LicenseTextLabel.Text = "This app is free software distributed under the MIT license.";

            LegalTitleLabel.Text = "Legal Notice";
            LegalText1Label.Text = "This software is provided 'as is', without warranty of any kind. The user is responsible for proper use of the app and compliance with local laws.";
            LegalText2Label.Text = "In no event shall the authors be liable for any direct, indirect, incidental or consequential damages arising from the use of this software.";
            WarningLabel.Text = "⚠️ Use at your own risk";

            BackButton.Text = "← Back";
        }

        // Resalta el botón de idioma activo con el estilo primario y el inactivo con el
        // estilo outline, usando referencias directas por x:Name. NUNCA se navega el árbol
        // visual por índices ni se castea a Frame/Border (eso lanzaba InvalidCastException).
        private void UpdateLanguageButtons(bool spanishActive)
        {
            var primaryStyle = GetResourceStyle("PrimaryButton");
            var outlineStyle = GetResourceStyle("OutlineButton");

            if (primaryStyle is null || outlineStyle is null)
                return;

            SpanishButton.Style = spanishActive ? primaryStyle : outlineStyle;
            EnglishButton.Style = spanishActive ? outlineStyle : primaryStyle;
        }

        private static Style? GetResourceStyle(string key)
        {
            if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Style style)
                return style;
            return null;
        }

        private void OnSpanishClicked(object? sender, EventArgs e)
        {
            Preferences.Set("AppLanguage", "es");
            ApplyLanguage("es");
        }

        private void OnEnglishClicked(object? sender, EventArgs e)
        {
            Preferences.Set("AppLanguage", "en");
            ApplyLanguage("en");
        }

        private async void OnBackClicked(object? sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnContactEmailClicked(object? sender, EventArgs e)
        {
            try
            {
                var currentLanguage = GetCurrentLanguage();

                // Crear el cuerpo del email con información de la aplicación
                var appName = "SMS Forwarder";
                var appVersion = AppVersion;
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
                var currentLanguage = GetCurrentLanguage();
                var errorTitle = currentLanguage == "es" ? "Error" : "Error";
                var errorMessage = currentLanguage == "es"
                    ? "Cliente de correo no disponible en este dispositivo"
                    : "Email client not available on this device";

                await DisplayAlert(errorTitle, errorMessage, "OK");
            }
            catch (Exception ex)
            {
                var currentLanguage = GetCurrentLanguage();
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
                    var currentLanguage = GetCurrentLanguage();
                    var title = currentLanguage == "es" ? "Navegador no disponible" : "Browser not available";
                    var message = currentLanguage == "es"
                        ? $"Enlace copiado al portapapeles:\n{DonationUrl}"
                        : $"Link copied to clipboard:\n{DonationUrl}";

                    await DisplayAlert(title, message, "OK");
                }
                catch
                {
                    var currentLanguage = GetCurrentLanguage();
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
                    var currentLanguage = GetCurrentLanguage();
                    var title = currentLanguage == "es" ? "Error al abrir enlace" : "Error opening link";
                    var message = currentLanguage == "es"
                        ? $"No se pudo abrir el navegador ({ex.Message}), enlace copiado al portapapeles."
                        : $"Could not open browser ({ex.Message}), link copied to clipboard.";

                    await DisplayAlert(title, message, "OK");
                }
                catch
                {
                    var currentLanguage = GetCurrentLanguage();
                    var errorTitle = currentLanguage == "es" ? "Error" : "Error";
                    var errorMessage = currentLanguage == "es"
                        ? $"No se pudo abrir el enlace: {DonationUrl}"
                        : $"Could not open link: {DonationUrl}";

                    await DisplayAlert(errorTitle, errorMessage, "OK");
                }
            }
        }
    }
}
