using System.Globalization;
using Microsoft.Maui.Storage;

namespace SMSForwarder.Services
{
    /// <summary>
    /// Servicio de localización que maneja strings en múltiples idiomas
    /// Detecta automáticamente el idioma del dispositivo (español o inglés)
    /// </summary>
    public class LocalizationService : ILocalizationService
    {
        private Dictionary<string, Dictionary<string, string>> _strings = [];
        private string _currentLanguage = "en-US";

        public event EventHandler? LanguageChanged;

        public string CurrentLanguage => _currentLanguage;

        public LocalizationService()
        {
            InitializeStrings();
        }

        public void Initialize()
        {
            var savedLanguage = Preferences.Default.Get("app_language", string.Empty);
            if (!string.IsNullOrWhiteSpace(savedLanguage) && _strings.ContainsKey(savedLanguage))
            {
                _currentLanguage = savedLanguage;
                return;
            }

            // Detectar el idioma del dispositivo
            var deviceLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();

            if (deviceLanguage == "es")
            {
                _currentLanguage = "es-ES";
            }
            else
            {
                _currentLanguage = "en-US";
            }
        }

        public void SetLanguage(string languageCode)
        {
            if (_strings.ContainsKey(languageCode) && _currentLanguage != languageCode)
            {
                _currentLanguage = languageCode;
                Preferences.Default.Set("app_language", languageCode);
                LanguageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string GetString(string key)
        {
            if (_strings.ContainsKey(_currentLanguage) &&
                _strings[_currentLanguage].ContainsKey(key))
            {
                return _strings[_currentLanguage][key];
            }

            // Fallback a inglés si la clave no existe en el idioma actual
            if (_strings.ContainsKey("en-US") &&
                _strings["en-US"].ContainsKey(key))
            {
                return _strings["en-US"][key];
            }

            return key; // Retornar la clave si no hay traducción
        }

        private void InitializeStrings()
        {
            // Strings en Español
            _strings["es-ES"] = new Dictionary<string, string>
            {
                // Menú y navegación
                { "menu.settings", "Configuración" },
                { "menu.diagnostics", "Diagnósticos" },
                { "menu.about", "Acerca de" },
                { "menu.contacts", "Contactos" },
                { "menu.messages", "Mensajes" },
                { "menu.header.subtitle", "Control de mensajes" },

                // MessagesPage - Buzón
                { "messages.title", "Mensajes" },
                { "messages.inbox", "Recibidos" },
                { "messages.sent", "Enviados" },
                { "messages.compose", "✏️ Nuevo mensaje" },
                { "messages.empty", "No hay mensajes" },
                { "messages.empty_inbox_hint", "Los SMS que recibas aparecerán aquí." },
                { "messages.empty_sent_hint", "Los SMS que envíes aparecerán aquí." },
                { "messages.confirm_delete", "¿Eliminar este mensaje del teléfono?" },
                { "messages.delete_error", "No se pudo eliminar el mensaje" },
                { "messages.load_error", "No se pudieron cargar los mensajes" },
                { "messages.default_title", "Hazla tu app de mensajes" },
                { "messages.default_text", "Para escribir, borrar y marcar como leídos los mensajes, SMS Forwarder tiene que ser tu aplicación de SMS predeterminada." },
                { "messages.default_button", "Usar como predeterminada" },
                { "messages.delete_needs_default", "Android solo permite borrar mensajes a la aplicación de SMS predeterminada." },

                // ComposePage - Redacción
                { "compose.title", "Nuevo mensaje" },
                { "compose.to", "Para" },
                { "compose.body", "Mensaje" },
                { "compose.body_placeholder", "Escribe tu mensaje" },
                { "compose.send", "Enviar" },
                { "compose.sent", "Mensaje enviado" },
                { "compose.send_error", "No se pudo enviar el mensaje" },
                { "compose.invalid_number", "Introduce un número de teléfono válido (7-15 dígitos)." },
                { "compose.empty_body", "Escribe el texto del mensaje." },
                { "compose.no_permission", "Sin el permiso de SMS no se puede enviar el mensaje." },

                // MainPage - Configuración
                { "main.title", "Configuración" },
                { "main.subtitle", "Configura los números donde reenviar SMS" },
                { "main.placeholder", "Ej: +34 600 123 456" },
                { "main.add_number", "📝 Agregar Número" },
                { "main.from_contacts", "👥 Contactos" },
                { "main.numbers_list", "Números configurados" },
                { "main.no_numbers", "No hay números configurados" },
                { "main.delete", "Eliminar" },
                { "main.confirm_delete", "¿Eliminar este número?" },
                { "main.delete_confirm_button", "Sí, eliminar" },
                { "main.cancel", "Cancelar" },
                { "main.language", "Idioma" },
                { "main.language_hint", "Elige el idioma de la aplicación" },

                // DiagnosticsPage
                { "diagnostics.title", "Diagnósticos" },
                { "diagnostics.subtitle", "Estado y logs de la aplicación" },
                { "diagnostics.permissions_status", "Estado de permisos" },
                { "diagnostics.logs", "Logs de la aplicación" },
                { "diagnostics.clear_logs", "Limpiar logs" },
                { "diagnostics.copy_logs", "Copiar logs" },
                { "diagnostics.export_logs", "Exportar logs" },
                { "diagnostics.permission_sms", "Permiso SMS" },
                { "diagnostics.permission_contacts", "Permiso Contactos" },
                { "diagnostics.permission_phone", "Permiso Teléfono" },
                { "diagnostics.granted", "Concedido" },
                { "diagnostics.denied", "Denegado" },
                { "diagnostics.version", "Versión" },

                // AboutPage
                { "about.title", "Acerca de" },
                { "about.description", "Acerca de SMS Forwarder" },
                { "about.app_name", "SMS Forwarder" },
                { "about.version", "Versión" },
                { "about.description_text", "Aplicación Android que reenvía automáticamente SMS recibidos a números configurados" },
                { "about.features_title", "Características" },
                { "about.feature_1", "Reenvío automático de SMS" },
                { "about.feature_2", "Privacidad: datos locales en el dispositivo" },
                { "about.feature_3", "Sin registro ni seguimiento" },
                { "about.license", "Licencia MIT" },
                { "about.repository", "Repositorio" },

                // SplashPage
                { "splash.loading", "Cargando..." },

                // Mensajes comunes
                { "common.ok", "OK" },
                { "common.cancel", "Cancelar" },
                { "common.yes", "Sí" },
                { "common.no", "No" },
                { "common.save", "Guardar" },
                { "common.delete", "Eliminar" },
                { "common.edit", "Editar" },
                { "common.close", "Cerrar" },
                { "common.back", "Atrás" },
                { "common.next", "Siguiente" },
                { "common.error", "Error" },
                { "common.success", "Éxito" },
                { "common.loading", "Cargando..." },
            };

            // Strings en Inglés
            _strings["en-US"] = new Dictionary<string, string>
            {
                // Menu and navigation
                { "menu.settings", "Settings" },
                { "menu.diagnostics", "Diagnostics" },
                { "menu.about", "About" },
                { "menu.contacts", "Contacts" },
                { "menu.messages", "Messages" },
                { "menu.header.subtitle", "Message control" },

                // MessagesPage - Inbox
                { "messages.title", "Messages" },
                { "messages.inbox", "Received" },
                { "messages.sent", "Sent" },
                { "messages.compose", "✏️ New message" },
                { "messages.empty", "No messages" },
                { "messages.empty_inbox_hint", "The SMS you receive will show up here." },
                { "messages.empty_sent_hint", "The SMS you send will show up here." },
                { "messages.confirm_delete", "Delete this message from the phone?" },
                { "messages.delete_error", "The message could not be deleted" },
                { "messages.load_error", "The messages could not be loaded" },
                { "messages.default_title", "Make it your messaging app" },
                { "messages.default_text", "To write, delete and mark messages as read, SMS Forwarder must be your default SMS application." },
                { "messages.default_button", "Set as default" },
                { "messages.delete_needs_default", "Android only lets the default SMS application delete messages." },

                // ComposePage - Compose
                { "compose.title", "New message" },
                { "compose.to", "To" },
                { "compose.body", "Message" },
                { "compose.body_placeholder", "Write your message" },
                { "compose.send", "Send" },
                { "compose.sent", "Message sent" },
                { "compose.send_error", "The message could not be sent" },
                { "compose.invalid_number", "Enter a valid phone number (7-15 digits)." },
                { "compose.empty_body", "Write the message text." },
                { "compose.no_permission", "The SMS permission is required to send the message." },

                // MainPage - Settings
                { "main.title", "Settings" },
                { "main.subtitle", "Configure the numbers to forward SMS" },
                { "main.placeholder", "E.g: +1 555 123 456" },
                { "main.add_number", "📝 Add Number" },
                { "main.from_contacts", "👥 Contacts" },
                { "main.numbers_list", "Configured numbers" },
                { "main.no_numbers", "No numbers configured" },
                { "main.delete", "Delete" },
                { "main.confirm_delete", "Delete this number?" },
                { "main.delete_confirm_button", "Yes, delete" },
                { "main.cancel", "Cancel" },
                { "main.language", "Language" },
                { "main.language_hint", "Choose the app language" },

                // DiagnosticsPage
                { "diagnostics.title", "Diagnostics" },
                { "diagnostics.subtitle", "Application status and logs" },
                { "diagnostics.permissions_status", "Permissions status" },
                { "diagnostics.logs", "Application logs" },
                { "diagnostics.clear_logs", "Clear logs" },
                { "diagnostics.copy_logs", "Copy logs" },
                { "diagnostics.export_logs", "Export logs" },
                { "diagnostics.permission_sms", "SMS Permission" },
                { "diagnostics.permission_contacts", "Contacts Permission" },
                { "diagnostics.permission_phone", "Phone Permission" },
                { "diagnostics.granted", "Granted" },
                { "diagnostics.denied", "Denied" },
                { "diagnostics.version", "Version" },

                // AboutPage
                { "about.title", "About" },
                { "about.description", "About SMS Forwarder" },
                { "about.app_name", "SMS Forwarder" },
                { "about.version", "Version" },
                { "about.description_text", "Android application that automatically forwards received SMS to configured numbers" },
                { "about.features_title", "Features" },
                { "about.feature_1", "Automatic SMS forwarding" },
                { "about.feature_2", "Privacy: local data on device" },
                { "about.feature_3", "No registration or tracking" },
                { "about.license", "MIT License" },
                { "about.repository", "Repository" },

                // SplashPage
                { "splash.loading", "Loading..." },

                // Common messages
                { "common.ok", "OK" },
                { "common.cancel", "Cancel" },
                { "common.yes", "Yes" },
                { "common.no", "No" },
                { "common.save", "Save" },
                { "common.delete", "Delete" },
                { "common.edit", "Edit" },
                { "common.close", "Close" },
                { "common.back", "Back" },
                { "common.next", "Next" },
                { "common.error", "Error" },
                { "common.success", "Success" },
                { "common.loading", "Loading..." },
            };
        }
    }
}
