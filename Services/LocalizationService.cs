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
                { "messages.inbox", "Entrada" },
                { "messages.sent", "Enviados" },
                { "messages.compose", "Nuevo mensaje" },
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
                { "messages.detail_title", "Mensaje" },
                { "messages.reply", "Responder" },
                { "messages.copy_text", "Copiar el texto" },
                { "messages.copied_title", "Copiado" },
                { "messages.copied_number", "El número está en el portapapeles." },
                { "messages.copied_text", "El texto del mensaje está en el portapapeles." },
                { "messages.link_error", "No se ha podido abrir el enlace." },
                { "messages.forward_rule", "Reenv\u00edo autom\u00e1tico" },
                { "messages.forward_to_whom", "\u00bfA qu\u00e9 n\u00famero reenviar los SMS de {0}?" },
                { "messages.forward_new_number", "Otro n\u00famero\u2026" },
                { "messages.forward_new_number_hint", "Escribe el n\u00famero al que reenviar estos SMS." },
                { "messages.forward_invalid_number", "Ese n\u00famero no vale (7-15 d\u00edgitos)." },
                { "messages.forward_what", "\u00bfQu\u00e9 se le manda a {0}?" },
                { "messages.forward_all_from", "Todos los SMS de {0}" },
                { "messages.forward_with_word", "Solo los que contengan una palabra\u2026" },
                { "messages.forward_word_hint", "Se reenviar\u00e1n los SMS de este remitente que contengan lo que escribas. No distingue may\u00fasculas ni acentos." },
                { "messages.forward_limits", "A {0} ahora le llegan TODOS los SMS. Al poner esta condici\u00f3n, solo recibir\u00e1 lo que la cumpla." },
                { "messages.forward_done", "{0} recibir\u00e1 los SMS de {1}." },
                { "messages.forward_done_word", "{0} recibir\u00e1 los SMS de {1} que contengan \u00ab{2}\u00bb." },
                { "messages.forward_done_words", "{0} recibir\u00e1 los SMS de {1} que contengan alguna de las palabras que ya ten\u00eda puestas: {2}." },
                { "messages.forward_now", "Reenviar" },
                { "messages.forward_now_to", "\u00bfA qui\u00e9n le reenv\u00edas este mensaje?" },
                { "messages.forward_other_recipient", "Otro destinatario\u2026" },
                { "messages.forward_body", "De: {0}\n{1}" },
                { "messages.delete_selected", "Eliminar" },
                { "messages.delete_selected_count", "Eliminar ({0})" },
                { "messages.confirm_delete_many", "\u00bfEliminar {0} mensajes del tel\u00e9fono?" },
                { "messages.delete_partial", "Se han borrado {0} de {1}." },

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
                { "main.add_number", "Agregar número" },
                { "main.from_contacts", "Contactos" },
                { "main.numbers_list", "Números configurados" },
                { "main.no_numbers", "No hay números configurados" },
                { "main.delete", "Eliminar" },
                { "main.confirm_delete", "¿Eliminar este número?" },
                { "main.delete_confirm_button", "Sí, eliminar" },
                { "main.cancel", "Cancelar" },
                { "main.language", "Idioma" },
                { "main.language_hint", "Elige el idioma de la aplicación" },
                { "destination.title", "Qué SMS recibe" },
                { "destination.all", "Todos los SMS" },
                { "destination.all_hint", "Apagado, a este número solo le llegan los SMS que cumplan lo de abajo. Volver a encenderlo borra los remitentes y las palabras." },
                { "destination.senders", "Solo de estos remitentes" },
                { "destination.senders_hint", "Vacío: de cualquiera. Con varios, vale con que venga de uno." },
                { "destination.keywords", "Que contenga estas palabras o frases" },
                { "destination.keywords_hint", "Vacío: diga lo que diga. Con varias, vale con que aparezca una. No distingue mayúsculas ni acentos." },
                { "destination.keyword_placeholder", "Ej: código, factura, pedido enviado" },
                { "destination.add", "Añadir" },
                { "destination.duplicate", "Eso ya está en la lista." },
                { "destination.summary_all", "Le llegan todos los SMS" },
                { "destination.summary_senders", "Solo de {0} remitente(s)" },
                { "destination.summary_keywords", "con {0} palabra(s) o frase(s)" },

                // DiagnosticsPage
                { "diagnostics.title", "Diagnósticos" },
                { "diagnostics.subtitle", "Estado de la aplicación y registro de actividad" },
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

                // DiagnosticsPage y PermissionService (antes escritos a mano en castellano)
                { "main.info_title", "Información" },
                { "diagnostics.permissions", "Permisos" },
                { "diagnostics.numbers", "Números" },
                { "diagnostics.checking", "Comprobando..." },
                { "diagnostics.permissions_setup", "Configuración de permisos" },
                { "diagnostics.check_permissions", "Ver el estado de los permisos" },
                { "diagnostics.configure_all", "Configurar todos los permisos" },
                { "diagnostics.battery", "Batería" },
                { "diagnostics.autostart", "Inicio automático" },
                { "diagnostics.hint", "Para que funcione bien, configure todos los permisos: así la aplicación puede recibir y reenviar SMS aunque esté en segundo plano." },
                { "diagnostics.tools", "Herramientas de diagnóstico" },
                { "diagnostics.refresh", "Actualizar estado" },
                { "diagnostics.activity_log", "Registro de actividad" },
                { "diagnostics.no_activity", "Sin actividad reciente..." },
                { "diagnostics.clear_log", "Limpiar registro" },
                { "diagnostics.log_cleared", "Registro limpiado" },
                { "diagnostics.receive_sms", "Recibir SMS: {0}" },
                { "diagnostics.send_sms", "Enviar SMS: {0}" },
                { "diagnostics.numbers_count", "{0} números configurados" },
                { "diagnostics.numbers_count_one", "1 número configurado" },
                { "diagnostics.not_decided", "Sin decidir" },
                { "diagnostics.refresh_error", "No se pudo actualizar el estado." },
                { "diagnostics.clear_error", "No se pudo limpiar el registro: {0}" },
                { "diagnostics.check_error", "No se pudo comprobar el estado de los permisos." },
                { "diagnostics.configure_error", "No se pudieron configurar los permisos." },
                { "diagnostics.all_configured", "Todos los permisos están configurados." },
                { "diagnostics.some_not_configured", "Algunos permisos no se han podido configurar. Revíselos a mano en los ajustes del teléfono." },
                { "diagnostics.attention", "Atención" },
                { "diagnostics.battery_ok_title", "Batería" },
                { "diagnostics.battery_ok", "La optimización de batería está desactivada, como debe ser." },
                { "diagnostics.battery_title", "Optimización de batería" },
                { "diagnostics.battery_on", "La optimización de batería está activada y puede impedir que la aplicación funcione en segundo plano.\n\n¿Abrir la configuración?" },
                { "diagnostics.battery_error", "No se pudo abrir la configuración de batería." },
                { "diagnostics.autostart_title", "Inicio automático" },
                { "diagnostics.autostart_text", "Se abrirá la configuración de inicio automático. Busque «SMS Forwarder» en la lista y actívelo para que la aplicación siga funcionando después de reiniciar el teléfono." },
                { "diagnostics.autostart_error", "No se pudo abrir la configuración de inicio automático." },
                { "perm.sms_title", "Permiso de SMS" },
                { "perm.sms_text", "La aplicación necesita el permiso de SMS para funcionar. ¿Concederlo?" },
                { "perm.sms_error", "No se pudo comprobar el permiso de SMS: {0}" },
                { "perm.battery_text", "Para que la aplicación funcione bien en segundo plano, conviene desactivar la optimización de batería.\n\n¿Abrir la configuración?" },
                { "perm.battery_warn", "Si la optimización de batería sigue activada, la aplicación podría no recibir mensajes en segundo plano." },
                { "perm.battery_error", "No se pudo comprobar la optimización de batería: {0}" },
                { "perm.info", "Información" },
                { "perm.autostart_prefix", "Para que la aplicación siga funcionando después de reiniciar el teléfono, " },
                { "perm.autostart_xiaomi", "vaya a Ajustes > Aplicaciones > Administrar aplicaciones > SMS Forwarder > Inicio automático y actívelo." },
                { "perm.autostart_huawei", "vaya a Ajustes > Aplicaciones > SMS Forwarder > Inicio automático y actívelo." },
                { "perm.autostart_perms", "vaya a Ajustes > Aplicaciones > SMS Forwarder > Permisos > Inicio automático y actívelo." },
                { "perm.autostart_samsung", "vaya a Ajustes > Aplicaciones > SMS Forwarder > Batería > Optimizar uso de batería y desactívelo." },
                { "perm.autostart_other", "compruebe que la aplicación puede ejecutarse en segundo plano." },
                { "perm.autostart_open", "¿Abrir la configuración ahora?" },
                { "perm.autostart_error", "No se pudo mostrar la información de inicio automático: {0}" },
                { "perm.status_title", "Estado de los permisos" },
                { "perm.status_text", "SMS: {0}\nOptimización de batería: {1}\nFabricante: {2}\n\nPara que funcione bien, todos los permisos deben estar concedidos." },
                { "perm.battery_off", "desactivada" },
                { "perm.battery_on", "activada" },
                { "perm.status_error", "No se pudo comprobar el estado: {0}" },
                { "common.not_now", "Ahora no" },
                { "common.understood", "Entendido" },
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
                { "messages.inbox", "Inbox" },
                { "messages.sent", "Sent" },
                { "messages.compose", "New message" },
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
                { "messages.detail_title", "Message" },
                { "messages.reply", "Reply" },
                { "messages.copy_text", "Copy the text" },
                { "messages.copied_title", "Copied" },
                { "messages.copied_number", "The number is on the clipboard." },
                { "messages.copied_text", "The message text is on the clipboard." },
                { "messages.link_error", "The link could not be opened." },
                { "messages.forward_rule", "Auto-forward" },
                { "messages.forward_to_whom", "Which number should get the SMS from {0}?" },
                { "messages.forward_new_number", "Another number\u2026" },
                { "messages.forward_new_number_hint", "Enter the number these SMS should be forwarded to." },
                { "messages.forward_invalid_number", "That number is not valid (7-15 digits)." },
                { "messages.forward_what", "What should {0} get?" },
                { "messages.forward_all_from", "Every SMS from {0}" },
                { "messages.forward_with_word", "Only the ones containing a word\u2026" },
                { "messages.forward_word_hint", "SMS from this sender containing what you type will be forwarded. Case and accents are ignored." },
                { "messages.forward_limits", "{0} currently gets ALL your SMS. With this condition it will only get the ones that match." },
                { "messages.forward_done", "{0} will get the SMS from {1}." },
                { "messages.forward_done_word", "{0} will get the SMS from {1} containing \u201c{2}\u201d." },
                { "messages.forward_done_words", "{0} will get the SMS from {1} containing one of the words already set: {2}." },
                { "messages.forward_now", "Forward" },
                { "messages.forward_now_to", "Who should this message be forwarded to?" },
                { "messages.forward_other_recipient", "Another recipient\u2026" },
                { "messages.forward_body", "From: {0}\n{1}" },
                { "messages.delete_selected", "Delete" },
                { "messages.delete_selected_count", "Delete ({0})" },
                { "messages.confirm_delete_many", "Delete {0} messages from the phone?" },
                { "messages.delete_partial", "{0} of {1} messages were deleted." },

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
                { "main.add_number", "Add number" },
                { "main.from_contacts", "Contacts" },
                { "main.numbers_list", "Configured numbers" },
                { "main.no_numbers", "No numbers configured" },
                { "main.delete", "Delete" },
                { "main.confirm_delete", "Delete this number?" },
                { "main.delete_confirm_button", "Yes, delete" },
                { "main.cancel", "Cancel" },
                { "main.language", "Language" },
                { "main.language_hint", "Choose the app language" },
                { "destination.title", "Which SMS it receives" },
                { "destination.all", "All SMS" },
                { "destination.all_hint", "When off, this number only gets the SMS that match the rules below. Turning it back on clears the senders and the words." },
                { "destination.senders", "Only from these senders" },
                { "destination.senders_hint", "Empty: from anyone. With several, matching one is enough." },
                { "destination.keywords", "Containing these words or phrases" },
                { "destination.keywords_hint", "Empty: whatever it says. With several, matching one is enough. Case and accents are ignored." },
                { "destination.keyword_placeholder", "E.g: code, invoice, order shipped" },
                { "destination.add", "Add" },
                { "destination.duplicate", "That is already in the list." },
                { "destination.summary_all", "Gets every SMS" },
                { "destination.summary_senders", "Only from {0} sender(s)" },
                { "destination.summary_keywords", "with {0} word(s) or phrase(s)" },

                // DiagnosticsPage
                { "diagnostics.title", "Diagnostics" },
                { "diagnostics.subtitle", "App status and activity log" },
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

                // DiagnosticsPage and PermissionService (hard-coded in Spanish before)
                { "main.info_title", "Information" },
                { "diagnostics.permissions", "Permissions" },
                { "diagnostics.numbers", "Numbers" },
                { "diagnostics.checking", "Checking..." },
                { "diagnostics.permissions_setup", "Permission setup" },
                { "diagnostics.check_permissions", "Check permission status" },
                { "diagnostics.configure_all", "Set up all permissions" },
                { "diagnostics.battery", "Battery" },
                { "diagnostics.autostart", "Autostart" },
                { "diagnostics.hint", "For the best results, set up every permission: that way the app can receive and forward SMS even while it runs in the background." },
                { "diagnostics.tools", "Diagnostic tools" },
                { "diagnostics.refresh", "Refresh status" },
                { "diagnostics.activity_log", "Activity log" },
                { "diagnostics.no_activity", "No recent activity..." },
                { "diagnostics.clear_log", "Clear log" },
                { "diagnostics.log_cleared", "Log cleared" },
                { "diagnostics.receive_sms", "Receive SMS: {0}" },
                { "diagnostics.send_sms", "Send SMS: {0}" },
                { "diagnostics.numbers_count", "{0} numbers configured" },
                { "diagnostics.numbers_count_one", "1 number configured" },
                { "diagnostics.not_decided", "Not decided" },
                { "diagnostics.refresh_error", "The status could not be refreshed." },
                { "diagnostics.clear_error", "The log could not be cleared: {0}" },
                { "diagnostics.check_error", "The permission status could not be checked." },
                { "diagnostics.configure_error", "The permissions could not be set up." },
                { "diagnostics.all_configured", "All permissions are set up." },
                { "diagnostics.some_not_configured", "Some permissions could not be set up. Check them manually in the phone settings." },
                { "diagnostics.attention", "Attention" },
                { "diagnostics.battery_ok_title", "Battery" },
                { "diagnostics.battery_ok", "Battery optimization is off, as it should be." },
                { "diagnostics.battery_title", "Battery optimization" },
                { "diagnostics.battery_on", "Battery optimization is on and may stop the app from working in the background.\n\nOpen the settings?" },
                { "diagnostics.battery_error", "The battery settings could not be opened." },
                { "diagnostics.autostart_title", "Autostart" },
                { "diagnostics.autostart_text", "The autostart settings will open. Find “SMS Forwarder” in the list and turn it on so the app keeps working after the phone restarts." },
                { "diagnostics.autostart_error", "The autostart settings could not be opened." },
                { "perm.sms_title", "SMS permission" },
                { "perm.sms_text", "The app needs the SMS permission to work. Grant it?" },
                { "perm.sms_error", "The SMS permission could not be checked: {0}" },
                { "perm.battery_text", "For the app to work well in the background, battery optimization should be turned off.\n\nOpen the settings?" },
                { "perm.battery_warn", "If battery optimization stays on, the app may not receive messages in the background." },
                { "perm.battery_error", "Battery optimization could not be checked: {0}" },
                { "perm.info", "Information" },
                { "perm.autostart_prefix", "For the app to keep working after the phone restarts, " },
                { "perm.autostart_xiaomi", "go to Settings > Apps > Manage apps > SMS Forwarder > Autostart and turn it on." },
                { "perm.autostart_huawei", "go to Settings > Apps > SMS Forwarder > Autostart and turn it on." },
                { "perm.autostart_perms", "go to Settings > Apps > SMS Forwarder > Permissions > Autostart and turn it on." },
                { "perm.autostart_samsung", "go to Settings > Apps > SMS Forwarder > Battery > Optimize battery usage and turn it off." },
                { "perm.autostart_other", "make sure the app is allowed to run in the background." },
                { "perm.autostart_open", "Open the settings now?" },
                { "perm.autostart_error", "The autostart information could not be shown: {0}" },
                { "perm.status_title", "Permission status" },
                { "perm.status_text", "SMS: {0}\nBattery optimization: {1}\nManufacturer: {2}\n\nFor the best results, every permission should be granted." },
                { "perm.battery_off", "off" },
                { "perm.battery_on", "on" },
                { "perm.status_error", "The status could not be checked: {0}" },
                { "common.not_now", "Not now" },
                { "common.understood", "Got it" },
            };
        }
    }
}
