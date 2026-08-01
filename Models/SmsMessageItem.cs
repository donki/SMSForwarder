namespace SMSForwarder.Models
{
    /// <summary>
    /// Un mensaje SMS leido del proveedor del sistema (buzon de entrada o enviados).
    /// Modelo neutro de plataforma para poder mostrarlo en la UI de MAUI.
    /// </summary>
    public class SmsMessageItem
    {
        public long Id { get; set; }
        public string Address { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public bool IsInbox { get; set; }   // true = recibido, false = enviado

        public string DateText => Date == DateTime.MinValue ? "" : Date.ToString("dd/MM/yyyy HH:mm");
        public string DirectionIcon => IsInbox ? "📥" : "📤";
        public string Snippet => Body.Length > 100 ? Body.Substring(0, 100) + "…" : Body;
        public string DisplayAddress => string.IsNullOrWhiteSpace(Address) ? "(desconocido)" : Address;
    }
}
