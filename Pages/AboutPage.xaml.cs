using System.Globalization;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace SMSForwarder.Pages
{
    public partial class AboutPage : ContentPage
    {
        // CONFIGURACIÓN
        private const string ContactEmail = "jsoladelarosa@gmail.com";
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

            ContactTitleLabel.Text = "Contacto";
            ContactInstructionLabel.Text = "Toca para enviar un correo electrónico";


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
        }

        private void SetEnglishTexts()
        {
            Title = "About";

            VersionLabel.Text = $"Version {AppVersion}";

            ContactTitleLabel.Text = "Contact";
            ContactInstructionLabel.Text = "Tap to send an email";


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

                await SocShared.ModernDialog.AlertAsync(this,errorTitle, errorMessage, "OK");
            }
            catch (Exception ex)
            {
                var currentLanguage = GetCurrentLanguage();
                var errorTitle = currentLanguage == "es" ? "Error" : "Error";
                var errorMessage = currentLanguage == "es"
                    ? $"No se pudo abrir el cliente de correo: {ex.Message}"
                    : $"Could not open email client: {ex.Message}";

                await SocShared.ModernDialog.AlertAsync(this,errorTitle, errorMessage, "OK");
            }
        }

    }
}
