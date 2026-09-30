using SMSForwarder.Models;

namespace SMSForwarder.Services
{
    /// <summary>Por que un mensaje se reenvia o no. Lo usa el nucleo de reenvio para su registro.</summary>
    public enum ForwardDecision
    {
        Forward,
        EmptyMessage,
        NoDestinations,
        FromForwardingNumber,
        AlreadyForwarded,
        NoDestinationMatches,
    }

    /// <summary>
    /// Reglas del reenvio que no dependen de Android: a quien se manda un SMS, como se evitan los
    /// bucles y los duplicados, y el texto que se envia. El receptor de Android solo lee los PDU y
    /// llama al <c>SmsManager</c>; todo lo que decide esta aqui, con sus pruebas.
    /// </summary>
    public static class ForwardingRules
    {
        public const string Prefix = "[SMSForwarder] De: ";

        /// <summary>Tamano de un SMS de texto: por encima se trocea y se paga por partes.</summary>
        public const int SmsLength = 160;

        /// <summary>
        /// Encabezados con los que empiezan los reenvios (de esta app o de otras parecidas). Solo
        /// cuentan al principio del mensaje: en medio son texto normal.
        /// </summary>
        private static readonly string[] ForwardHeaders = ["De:", "From:", "Reenviado:", "Forwarded:", "SMS de:"];

        /// <summary>Destinos a los que hay que mandar el mensaje, con el motivo si no hay ninguno.</summary>
        public static (ForwardDecision Decision, List<ForwardDestination> Targets) Decide(
            IReadOnlyList<ForwardDestination> destinations, string sender, string body)
        {
            if (string.IsNullOrEmpty(body))
                return (ForwardDecision.EmptyMessage, []);
            if (destinations.Count == 0)
                return (ForwardDecision.NoDestinations, []);

            // Prevencion de bucles: lo que manda uno de los numeros de reenvio no se reenvia.
            if (destinations.Any(d => PhoneNumbers.AreEqual(d.Phone, sender)))
                return (ForwardDecision.FromForwardingNumber, []);
            if (IsForwardedMessage(body))
                return (ForwardDecision.AlreadyForwarded, []);

            // Cada destino decide: sin filtros le llega todo; con remitentes o palabras, solo lo que casa.
            var targets = destinations.Where(d => !string.IsNullOrWhiteSpace(d.Phone) && d.Matches(sender, body)).ToList();
            return targets.Count == 0 ? (ForwardDecision.NoDestinationMatches, targets) : (ForwardDecision.Forward, targets);
        }

        /// <summary>
        /// Si el mensaje ya es un reenvio. Antes bastaba con que «De:» o «From:» aparecieran en los
        /// 30 primeros caracteres, y se quedaban sin reenviar mensajes normales como «Compra de: 23 EUR»
        /// o «Codigo de: acceso». Ahora el encabezado tiene que ir al principio.
        /// </summary>
        public static bool IsForwardedMessage(string messageBody)
        {
            if (string.IsNullOrWhiteSpace(messageBody)) return false;
            var start = messageBody.TrimStart();
            if (start.StartsWith("[SMSForwarder]", StringComparison.OrdinalIgnoreCase)) return true;
            return ForwardHeaders.Any(header => start.StartsWith(header, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Texto que se reenvia: el remitente y el mensaje. Si no cabe en un SMS se recorta el
        /// mensaje con «...» para que el reenvio sea un solo SMS.
        /// </summary>
        public static string BuildMessage(string sender, string body)
        {
            var message = $"{Prefix}{sender}\n{body}";
            if (message.Length <= SmsLength)
                return message;

            var maxBodyLength = SmsLength - Prefix.Length - sender.Length - 4;
            var truncated = body.Length > maxBodyLength
                ? body.Substring(0, Math.Max(0, maxBodyLength)) + "..."
                : body;
            return $"{Prefix}{sender}\n{truncated}";
        }
    }

    /// <summary>
    /// Filtro de duplicados: el mismo mensaje del mismo remitente en menos de unos segundos es el
    /// mismo SMS que llega dos veces (multiparte, doble broadcast), y no se reenvia dos veces.
    /// </summary>
    public sealed class DuplicateFilter
    {
        private readonly TimeSpan _window;
        private string? _lastSender;
        private string? _lastBody;
        private DateTime _lastReceived = DateTime.MinValue;

        public DuplicateFilter(TimeSpan window) => _window = window;

        /// <summary>True si es repetido; si no, lo apunta como el ultimo visto.</summary>
        public bool IsDuplicate(string sender, string body, DateTime now)
        {
            if (_lastSender == sender && _lastBody == body && now - _lastReceived < _window)
                return true;

            _lastSender = sender;
            _lastBody = body;
            _lastReceived = now;
            return false;
        }
    }
}
