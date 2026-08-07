using System.Text.RegularExpressions;
using SMSForwarder.Services;

namespace SMSForwarder.Pages
{
    /// <summary>
    /// Redaccion y envio de un SMS. Es el destino de <c>ComposeSmsActivity</c> cuando otra app pide
    /// "enviar mensaje" (intents sms:/smsto:) y de la respuesta a un mensaje del buzon, asi que
    /// acepta destinatario y cuerpo prellenados por la ruta de Shell.
    /// </summary>
    public partial class ComposePage : ContentPage, IQueryAttributable
    {
        private const int SmsPartLength = 160;

        private readonly IMessageStore _store;
        private readonly IContactPicker _contactPicker;
        private readonly ILocalizationService _localization;

        public ComposePage(IMessageStore store, IContactPicker contactPicker, ILocalizationService localization)
        {
            InitializeComponent();
            _store = store;
            _contactPicker = contactPicker;
            _localization = localization;

            UpdateLocalizedStrings();
            UpdateCounter();
            _localization.LanguageChanged += OnLanguageChanged;
        }

        /// <summary>
        /// Destinatario y texto prellenados por quien navega hasta aqui: la respuesta a un mensaje
        /// del buzon o un intent "enviar mensaje" de otra app (ver <c>ComposeSmsActivity</c>).
        /// </summary>
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var to = query.TryGetValue("to", out var toValue) ? toValue as string : null;
            var body = query.TryGetValue("body", out var bodyValue) ? bodyValue as string : null;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!string.IsNullOrWhiteSpace(to)) ToEntry.Text = to;
                if (!string.IsNullOrEmpty(body)) BodyEditor.Text = body;
                UpdateCounter();

                // Si ya sabemos a quien escribir, el foco va directo al texto.
                if (!string.IsNullOrWhiteSpace(to)) BodyEditor.Focus();
            });
        }

        // ------------------------------------------------------------- acciones

        private async void OnPickContactClicked(object? sender, EventArgs e)
        {
            try
            {
                var number = await _contactPicker.PickPhoneNumberAsync();
                if (!string.IsNullOrWhiteSpace(number))
                    ToEntry.Text = number;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Compose] PickContact: {ex.Message}");
            }
        }

        private void OnBodyTextChanged(object? sender, TextChangedEventArgs e) => UpdateCounter();

        private async void OnSendClicked(object? sender, EventArgs e)
        {
            var address = (ToEntry.Text ?? "").Trim();
            var body = BodyEditor.Text ?? "";

            if (!IsValidNumber(address))
            {
                await SocShared.ModernDialog.AlertAsync(this,
                    _localization.GetString("common.error"),
                    _localization.GetString("compose.invalid_number"),
                    _localization.GetString("common.ok"));
                return;
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                await SocShared.ModernDialog.AlertAsync(this,
                    _localization.GetString("common.error"),
                    _localization.GetString("compose.empty_body"),
                    _localization.GetString("common.ok"));
                return;
            }

            if (!await EnsureSmsPermissionAsync())
            {
                await SocShared.ModernDialog.AlertAsync(this,
                    _localization.GetString("common.error"),
                    _localization.GetString("compose.no_permission"),
                    _localization.GetString("common.ok"));
                return;
            }

            SendButton.IsEnabled = false;
            try
            {
                if (await _store.SendAsync(address, body))
                {
                    BodyEditor.Text = "";
                    UpdateCounter();
                    await SocShared.ModernDialog.AlertAsync(this,
                        _localization.GetString("common.success"),
                        _localization.GetString("compose.sent"),
                        _localization.GetString("common.ok"));
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await SocShared.ModernDialog.AlertAsync(this,
                        _localization.GetString("common.error"),
                        _localization.GetString("compose.send_error"),
                        _localization.GetString("common.ok"));
                }
            }
            catch (Exception ex)
            {
                await SocShared.ModernDialog.AlertAsync(this,
                    _localization.GetString("common.error"),
                    $"{_localization.GetString("compose.send_error")}: {ex.Message}",
                    _localization.GetString("common.ok"));
            }
            finally
            {
                SendButton.IsEnabled = true;
            }
        }

        // --------------------------------------------------------------- estado

        private static bool IsValidNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return false;
            var digits = Regex.Replace(number, @"[\s\-\(\)\.]", "");
            return Regex.IsMatch(digits, @"^\+?\d{7,15}$");
        }

        private static async Task<bool> EnsureSmsPermissionAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Sms>();
                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<Permissions.Sms>();
                return status == PermissionStatus.Granted;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Caracteres usados y numero de SMS en que se partira el mensaje.</summary>
        private void UpdateCounter()
        {
            var length = (BodyEditor.Text ?? "").Length;
            var parts = length == 0 ? 1 : (length + SmsPartLength - 1) / SmsPartLength;
            CounterLabel.Text = parts > 1
                ? $"{length}/{SmsPartLength} · {parts} SMS"
                : $"{length}/{SmsPartLength}";
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
            => MainThread.BeginInvokeOnMainThread(UpdateLocalizedStrings);

        private void UpdateLocalizedStrings()
        {
            Title = _localization.GetString("compose.title");
            ToLabel.Text = _localization.GetString("compose.to");
            BodyLabel.Text = _localization.GetString("compose.body");
            SendButton.Text = _localization.GetString("compose.send");
            ToEntry.Placeholder = _localization.GetString("main.placeholder");
            BodyEditor.Placeholder = _localization.GetString("compose.body_placeholder");
        }
    }
}
