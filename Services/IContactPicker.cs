namespace SMSForwarder.Services
{
    /// <summary>
    /// Selector de contactos del sistema.
    ///
    /// La app nunca lee la agenda: abre el selector del sistema y recibe unicamente el
    /// numero que el usuario elige. Por eso no necesita READ_CONTACTS, que es lo que exige
    /// la politica de Google Play sobre acceso amplio a contactos (obligatoria 2026-10-28).
    /// </summary>
    public interface IContactPicker
    {
        /// <summary>
        /// Abre el selector del sistema y devuelve el numero elegido,
        /// o null si el usuario cancela o el numero no se puede leer.
        /// </summary>
        Task<string?> PickPhoneNumberAsync();
    }
}
