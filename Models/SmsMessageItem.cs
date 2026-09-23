using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SMSForwarder.Models
{
    /// <summary>
    /// Un mensaje SMS leido del proveedor del sistema (buzon de entrada o enviados).
    /// Modelo neutro de plataforma para poder mostrarlo en la UI de MAUI.
    /// </summary>
    public class SmsMessageItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public long Id { get; set; }
        public string Address { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public bool IsInbox { get; set; }   // true = recibido, false = enviado

        /// <summary>
        /// Marcado en el modo de seleccion multiple del buzon. Notifica porque la casilla de cada
        /// fila se enlaza en los dos sentidos y hay que poder desmarcarlas todas desde la pagina.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public string DateText => Date == DateTime.MinValue ? "" : Date.ToString("dd/MM/yyyy HH:mm");

        /// <summary>
        /// La fecha para la lista, corta: la hora si es de hoy y el dia si no. La fecha larga
        /// ocupaba media fila y dejaba el remitente en «+…» con la letra del sistema grande.
        /// </summary>
        public string ListDateText
        {
            get
            {
                if (Date == DateTime.MinValue) return "";
                if (Date.Date == DateTime.Today) return Date.ToString("HH:mm");
                return Date.Year == DateTime.Today.Year ? Date.ToString("dd/MM") : Date.ToString("dd/MM/yy");
            }
        }
        /// <summary>Icono plano de la bandeja (Resources/Images): recibido o enviado.</summary>
        public string DirectionIcon => IsInbox ? "ic_inbox.png" : "ic_outbox.png";
        public string Snippet => Body.Length > 100 ? Body.Substring(0, 100) + "…" : Body;
        public string DisplayAddress => string.IsNullOrWhiteSpace(Address) ? "(desconocido)" : Address;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
