namespace SMSForwarder.Services
{
    /// <summary>
    /// Interfaz para gestionar la localización y strings de la aplicación
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Se dispara cuando cambia el idioma actual
        /// </summary>
        event EventHandler? LanguageChanged;

        /// <summary>
        /// Obtiene el idioma actual (es-ES, en-US, etc.)
        /// </summary>
        string CurrentLanguage { get; }

        /// <summary>
        /// Obtiene un string localizado por su clave
        /// </summary>
        string GetString(string key);

        /// <summary>
        /// Inicializa el servicio con el idioma del dispositivo
        /// </summary>
        void Initialize();

        /// <summary>
        /// Cambia el idioma de la aplicación
        /// </summary>
        void SetLanguage(string languageCode);
    }
}
