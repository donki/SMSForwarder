using System.Text.RegularExpressions;
using SMSForwarder.Models;
using SMSForwarder.Services;

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
        private SmsMessageItem? _message;

        public MessageDetailPage(ILocalizationService localization)
        {
            InitializeComponent();
            _localization = localization;
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

            // Sin numero no hay a quien responder ni que copiar (puede pasar en mensajes de servicio).
            var hasAddress = !string.IsNullOrWhiteSpace(_message.Address);
            CopyAddressButton.IsEnabled = hasAddress;
            ReplyButton.IsEnabled = hasAddress;

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

        private async void OnReplyClicked(object? sender, EventArgs e)
        {
            if (_message is null) return;

            var parameters = new ShellNavigationQueryParameters { ["to"] = _message.Address };
            await Shell.Current.GoToAsync(nameof(ComposePage), parameters);
        }
    }
}
