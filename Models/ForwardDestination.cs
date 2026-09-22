using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SMSForwarder.Models
{
    /// <summary>
    /// Un numero al que se reenvian los SMS, con sus condiciones. Sin condiciones (lo que sale por
    /// defecto al anadir un numero) le llega TODO; en cuanto se le pone algun remitente o alguna
    /// palabra, solo le llegan los mensajes que casen.
    /// </summary>
    public class ForwardDestination : INotifyPropertyChanged
    {
        public string Phone { get; set; } = "";

        /// <summary>Remitentes permitidos. Vacio = de cualquiera.</summary>
        public List<string> Senders { get; set; } = [];

        /// <summary>Palabras o frases que puede contener el mensaje. Vacio = diga lo que diga.</summary>
        public List<string> Keywords { get; set; } = [];

        /// <summary>Sin filtros: le llega todo (el comportamiento por defecto).</summary>
        [JsonIgnore]
        public bool ForwardsEverything => Senders.Count == 0 && Keywords.Count == 0;

        private string _summary = "";
        /// <summary>
        /// Que le llega, en una frase, para la fila de la lista. No se guarda: lo compone la
        /// interfaz, que es la que sabe de idioma.
        /// </summary>
        [JsonIgnore]
        public string Summary
        {
            get => _summary;
            set
            {
                if (_summary == value) return;
                _summary = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Si este destino tiene que recibir el mensaje. Con las dos listas puestas hay que cumplir
        /// las dos cosas: que venga de uno de esos remitentes Y que contenga una de esas palabras.
        /// </summary>
        public bool Matches(string sender, string body)
        {
            if (Senders.Count > 0 && !Senders.Any(s => PhoneNumbers.AreEqual(s, sender)))
                return false;
            if (Keywords.Count > 0 && !Keywords.Any(k => ContainsText(body, k)))
                return false;
            return true;
        }

        /// <summary>Busca la palabra o la frase sin distinguir mayusculas ni acentos («PAGO» encuentra «pagó»).</summary>
        public static bool ContainsText(string body, string needle)
        {
            if (string.IsNullOrWhiteSpace(needle))
                return true;
            if (string.IsNullOrEmpty(body))
                return false;
            return Normalize(body).Contains(Normalize(needle), StringComparison.Ordinal);
        }

        private static string Normalize(string text)
        {
            var decomposed = text.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(decomposed.Length);
            foreach (var c in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }
            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }

    /// <summary>Comparacion de numeros de telefono, la misma en la interfaz y en el reenvio.</summary>
    public static class PhoneNumbers
    {
        public static string Clean(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return "";
            return phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "")
                              .Replace(")", "").Replace(".", "").Replace("+", "").Trim();
        }

        /// <summary>
        /// Iguales si coinciden enteros o en sus ultimos 9 digitos: el mismo numero llega unas veces
        /// con prefijo de pais y otras sin el.
        /// </summary>
        public static bool AreEqual(string a, string b)
        {
            var x = Clean(a);
            var y = Clean(b);
            if (string.IsNullOrWhiteSpace(x) || string.IsNullOrWhiteSpace(y)) return false;
            if (x == y) return true;
            var min = Math.Min(x.Length, y.Length);
            return min >= 9 && x[^9..] == y[^9..];
        }
    }

    /// <summary>
    /// Lectura y escritura de los destinos. Se guardan dos claves a la vez: <c>destinations</c> con
    /// los destinos completos y <c>phones</c> con solo los numeros, que es lo que habia hasta la
    /// 2026.09.22.0 (una version vieja instalada encima sigue reenviando a todos).
    /// </summary>
    public static class ForwardDestinations
    {
        public const string DestinationsKey = "destinations";
        public const string PhonesKey = "phones";

        private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

        /// <summary>Convierte el JSON guardado en destinos; acepta el formato viejo (lista de numeros).</summary>
        public static List<ForwardDestination> Parse(string? destinationsJson, string? phonesJson)
        {
            if (!string.IsNullOrWhiteSpace(destinationsJson))
            {
                try
                {
                    var list = JsonSerializer.Deserialize<List<ForwardDestination>>(destinationsJson, Options);
                    if (list is not null)
                        return list.Where(d => !string.IsNullOrWhiteSpace(d.Phone)).ToList();
                }
                catch (Exception) { /* se intenta con el formato viejo */ }
            }
            if (!string.IsNullOrWhiteSpace(phonesJson))
            {
                try
                {
                    var phones = JsonSerializer.Deserialize<List<string>>(phonesJson, Options);
                    if (phones is not null)
                        return phones.Where(p => !string.IsNullOrWhiteSpace(p))
                                     .Select(p => new ForwardDestination { Phone = p })
                                     .ToList();
                }
                catch (Exception) { }
            }
            return [];
        }

        public static string ToJson(IEnumerable<ForwardDestination> destinations) => JsonSerializer.Serialize(destinations);

        public static string ToPhonesJson(IEnumerable<ForwardDestination> destinations) => JsonSerializer.Serialize(destinations.Select(d => d.Phone).ToList());
    }
}
