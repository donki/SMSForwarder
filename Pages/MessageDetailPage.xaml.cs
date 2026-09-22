using System.Text.RegularExpressions;
using SMSForwarder.Models;
using SMSForwarder.Services;
using SocShared;

namespace SMSForwarder.Pages
{
    /// <summary>
    /// Un mensaje completo. Existe porque en la lista solo cabe un extracto de 100 caracteres y
    /// tocar el mensaje saltaba directo a responder: no habia forma de leer un SMS largo, ni de
    /// copiar el numero, ni de abrir un enlace (notas de autor del 2026-08-24).
    /// </summary>
    public partial class MessageDetailPage : ContentPage, IQueryAttributable
    {
        /// <summary>
        /// Enlaces del cuerpo. Se reconoce <c>http(s)://</c> y tambien el <c>www.</c> suelto, que
        /// es como los escriben la mitad de los SMS comerciales.
        /// </summary>
        private static readonly Regex LinkPattern = new(
            @"(https?://[^\s]+)|(www\.[^\s]+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly ILocalizationService _localization;
        private readonly ILoggingService _logging;
        private SmsMessageItem? _message;

        public MessageDetailPage(ILocalizationService localization, ILoggingService logging)
        {
            InitializeComponent();
            _localization = localization;
            _logging = logging;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // El mensaje viaja como objeto, no dentro de la ruta: un cuerpo de SMS con «&» o «%»
            // romperia la cadena de navegacion.
            if (query.TryGetValue("message", out var value) && value is SmsMessageItem message)
            {
                _message = message;
                Show();
            }
        }

        private void Show()
        {
            if (_message is null) return;

            Title = _localization.GetString("messages.detail_title");
            AddressLabel.Text = _message.DisplayAddress;
            DateLabel.Text = _message.DateText;
            CopyBodyButton.Text = _localization.GetString("messages.copy_text");
            ReplyButton.Text = _localization.GetString("messages.reply");
            ForwardRuleButton.Text = _localization.GetString("messages.forward_rule");

            // Sin numero no hay a quien responder ni que copiar (puede pasar en mensajes de servicio).
            var hasAddress = !string.IsNullOrWhiteSpace(_message.Address);
            CopyAddressButton.IsEnabled = hasAddress;
            ReplyButton.IsEnabled = hasAddress;
            ForwardRuleButton.IsEnabled = hasAddress;

            BodyLabel.FormattedText = BuildBody(_message.Body);
        }

        /// <summary>
        /// Compone el cuerpo troceandolo en texto normal y enlaces. Los enlaces van subrayados y
        /// con su propio gesto: un <c>Label</c> de MAUI no detecta URL por su cuenta.
        /// </summary>
        private FormattedString BuildBody(string body)
        {
            var formatted = new FormattedString();
            var position = 0;

            foreach (Match match in LinkPattern.Matches(body))
            {
                if (match.Index > position)
                    formatted.Spans.Add(new Span { Text = body[position..match.Index] });

                formatted.Spans.Add(BuildLinkSpan(match.Value));
                position = match.Index + match.Length;
            }

            if (position < body.Length)
                formatted.Spans.Add(new Span { Text = body[position..] });

            return formatted;
        }

        private Span BuildLinkSpan(string link)
        {
            var span = new Span
            {
                Text = link,
                TextColor = (Color)Application.Current!.Resources["Primary"],
                TextDecorations = TextDecorations.Underline,
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (_, _) => await OpenLinkAsync(link);
            span.GestureRecognizers.Add(tap);

            return span;
        }

        private async Task OpenLinkAsync(string link)
        {
            // Un «www.» suelto no es una URI absoluta: el navegador necesita el esquema.
            var url = link.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? "https://" + link : link;

            try
            {
                await Launcher.Default.OpenAsync(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MessageDetail] OpenLink: {ex.Message}");
                await SocShared.ModernDialog.AlertAsync(this,
                    _localization.GetString("common.error"),
                    _localization.GetString("messages.link_error"),
                    _localization.GetString("common.ok"));
            }
        }

        private async void OnCopyAddressClicked(object? sender, EventArgs e)
        {
            if (_message is null) return;

            await Clipboard.Default.SetTextAsync(_message.Address);
            await SocShared.ModernDialog.AlertAsync(this,
                _localization.GetString("messages.copied_title"),
                _localization.GetString("messages.copied_number"),
                _localization.GetString("common.ok"));
        }

        private async void OnCopyBodyClicked(object? sender, EventArgs e)
        {
            if (_message is null) return;

            await Clipboard.Default.SetTextAsync(_message.Body);
            await SocShared.ModernDialog.AlertAsync(this,
                _localization.GetString("messages.copied_title"),
                _localization.GetString("messages.copied_text"),
                _localization.GetString("common.ok"));
        }

        /// <summary>
        /// Deja configurado que los SMS de este remitente se reenvien a alguien: se elige a quien (de
        /// los numeros ya configurados o uno nuevo) y si se le manda todo lo de este remitente o solo
        /// lo que contenga una palabra. Es un atajo a Configuracion > el numero > «Que SMS recibe».
        /// </summary>
        private async void OnForwardRuleClicked(object? sender, EventArgs e)
        {
            if (_message is null || string.IsNullOrWhiteSpace(_message.Address)) return;
            var sender_ = _message.Address.Trim();
            var culture = System.Globalization.CultureInfo.CurrentCulture;

            try
            {
                DestinationStore.Load();

                // 1. A quien.
                var newNumber = _localization.GetString("messages.forward_new_number");
                var options = DestinationStore.Items
                    .Select(d => d.Phone)
                    .Append(newNumber)
                    .ToArray();
                var chosen = await ModernDialog.ActionSheetAsync(this,
                    string.Format(culture, _localization.GetString("messages.forward_to_whom"), sender_),
                    _localization.GetString("common.cancel"), options);
                if (chosen is null || chosen == _localization.GetString("common.cancel")) return;

                ForwardDestination? destination;
                if (chosen == newNumber)
                {
                    var typed = await ModernDialog.PromptAsync(this,
                        _localization.GetString("messages.forward_rule"),
                        _localization.GetString("messages.forward_new_number_hint"),
                        _localization.GetString("common.ok"), _localization.GetString("common.cancel"),
                        placeholder: _localization.GetString("main.placeholder"));
                    var number = (typed ?? string.Empty).Replace(" ", "").Trim();
                    if (number.Length == 0) return;
                    if (!IsValidPhoneNumber(number))
                    {
                        await ModernDialog.AlertAsync(this, _localization.GetString("common.error"),
                            _localization.GetString("messages.forward_invalid_number"), _localization.GetString("common.ok"));
                        return;
                    }
                    destination = DestinationStore.Items.FirstOrDefault(d => PhoneNumbers.AreEqual(d.Phone, number));
                    if (destination is null)
                    {
                        destination = new ForwardDestination { Phone = number };
                        DestinationStore.Items.Add(destination);
                    }
                }
                else
                {
                    destination = DestinationStore.Items.FirstOrDefault(d => d.Phone == chosen);
                }
                if (destination is null) return;

                // 2. Que se le manda de este remitente.
                var everything = string.Format(culture, _localization.GetString("messages.forward_all_from"), sender_);
                var withWord = _localization.GetString("messages.forward_with_word");
                var what = await ModernDialog.ActionSheetAsync(this,
                    string.Format(culture, _localization.GetString("messages.forward_what"), destination.Phone),
                    _localization.GetString("common.cancel"), everything, withWord);
                if (what is null || what == _localization.GetString("common.cancel")) return;

                string? keyword = null;
                if (what == withWord)
                {
                    keyword = (await ModernDialog.PromptAsync(this,
                        _localization.GetString("messages.forward_with_word"),
                        _localization.GetString("messages.forward_word_hint"),
                        _localization.GetString("common.ok"), _localization.GetString("common.cancel"),
                        placeholder: _localization.GetString("destination.keyword_placeholder")))?.Trim();
                    if (string.IsNullOrWhiteSpace(keyword)) return;
                }

                // 3. Si hasta ahora le llegaba todo, se avisa: al poner condiciones deja de llegarle.
                if (destination.ForwardsEverything)
                {
                    var goOn = await ModernDialog.AlertAsync(this,
                        _localization.GetString("messages.forward_rule"),
                        string.Format(culture, _localization.GetString("messages.forward_limits"), destination.Phone),
                        _localization.GetString("common.ok"), _localization.GetString("common.cancel"));
                    if (!goOn) return;
                }

                if (!destination.Senders.Any(s => PhoneNumbers.AreEqual(s, sender_)))
                    destination.Senders.Add(sender_);
                if (keyword is { Length: > 0 } && !destination.Keywords.Any(k => string.Equals(k, keyword, StringComparison.CurrentCultureIgnoreCase)))
                    destination.Keywords.Add(keyword);
                DestinationStore.Save(_logging);

                // Si ese numero ya tenia palabras puestas, lo de este remitente tambien tendra que
                // cumplirlas: se dice, que si no parece que le vaya a llegar todo.
                var message = keyword is { Length: > 0 }
                    ? string.Format(culture, _localization.GetString("messages.forward_done_word"), destination.Phone, sender_, keyword)
                    : destination.Keywords.Count > 0
                        ? string.Format(culture, _localization.GetString("messages.forward_done_words"), destination.Phone, sender_, string.Join(", ", destination.Keywords))
                        : string.Format(culture, _localization.GetString("messages.forward_done"), destination.Phone, sender_);
                await ModernDialog.AlertAsync(this, _localization.GetString("messages.forward_rule"), message, _localization.GetString("common.ok"));
            }
            catch (Exception ex)
            {
                _logging.LogError("Error configurando el reenvio desde el detalle", ex);
                await ModernDialog.AlertAsync(this, _localization.GetString("common.error"), ex.Message, _localization.GetString("common.ok"));
            }
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            var clean = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            return Regex.IsMatch(clean, @"^\+?[1-9]\d{6,14}$");
        }

        private async void OnReplyClicked(object? sender, EventArgs e)
        {
            if (_message is null) return;

            var parameters = new ShellNavigationQueryParameters { ["to"] = _message.Address };
            await Shell.Current.GoToAsync(nameof(ComposePage), parameters);
        }
    }
}
