using System.Collections.ObjectModel;
using SMSForwarder.Models;
using SMSForwarder.Services;

namespace SMSForwarder.Pages
{
    /// <summary>
    /// Buzon de mensajes: lista los SMS recibidos y enviados del proveedor del sistema, permite
    /// marcarlos como leidos, borrarlos y responder. Es la pantalla que justifica READ_SMS y el
    /// rol de app de SMS por defecto: sin ella la app no gestionaria mensajes, solo los reenviaria.
    /// </summary>
    public partial class MessagesPage : ContentPage
    {
        private readonly IMessageStore _store;
        private readonly ILocalizationService _localization;
        private readonly ObservableCollection<SmsMessageItem> _messages = new();
        private bool _showingInbox = true;
        private bool _loading;

        public MessagesPage(IMessageStore store, ILocalizationService localization)
        {
            InitializeComponent();
            _store = store;
            _localization = localization;

            MessagesList.ItemsSource = _messages;

            UpdateLocalizedStrings();
            UpdateTabButtons();
            _localization.LanguageChanged += OnLanguageChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            UpdateDefaultAppBanner();
            await LoadAsync();
        }

        // ---------------------------------------------------------------- carga

        private async Task LoadAsync()
        {
            if (_loading) return;
            _loading = true;
            try
            {
                if (!await EnsureSmsPermissionAsync())
                {
                    _messages.Clear();
                    return;
                }

                var items = _showingInbox
                    ? await _store.GetInboxAsync()
                    : await _store.GetSentAsync();

                _messages.Clear();
                foreach (var item in items)
                    _messages.Add(item);
            }
            catch (Exception ex)
            {
                await SocShared.ModernDialog.AlertAsync(this,
                    _localization.GetString("common.error"),
                    $"{_localization.GetString("messages.load_error")}: {ex.Message}",
                    _localization.GetString("common.ok"));
            }
            finally
            {
                _loading = false;
            }
        }

        /// <summary>Pide el permiso de SMS en el momento en que se abre el buzon, no al arrancar.</summary>
        private async Task<bool> EnsureSmsPermissionAsync()
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

        // ------------------------------------------------------------- acciones

        private void OnInboxClicked(object? sender, EventArgs e) => SwitchTo(inbox: true);

        private void OnSentClicked(object? sender, EventArgs e) => SwitchTo(inbox: false);

        private async void SwitchTo(bool inbox)
        {
            if (_showingInbox == inbox) return;
            _showingInbox = inbox;
            UpdateTabButtons();
            UpdateLocalizedStrings();
            await LoadAsync();
        }

        private async void OnRefreshClicked(object? sender, EventArgs e) => await LoadAsync();

        private async void OnComposeClicked(object? sender, EventArgs e)
            => await GoToComposeAsync(null);

        /// <summary>Al tocar un mensaje: se marca como leido y se abre la respuesta al remitente.</summary>
        private async void OnMessageSelected(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not SmsMessageItem message) return;

            // Se limpia la seleccion para poder volver a abrir el mismo mensaje al regresar.
            MessagesList.SelectedItem = null;

            if (message.IsInbox && !message.IsRead)
            {
                try
                {
                    if (await _store.MarkReadAsync(message))
                        message.IsRead = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Messages] MarkRead: {ex.Message}");
                }
            }

            await GoToComposeAsync(message.Address);
        }

        private async void OnDeleteMessageClicked(object? sender, EventArgs e)
        {
            if (sender is not SwipeItem { CommandParameter: SmsMessageItem message }) return;

            // Borrar del proveedor del sistema solo funciona siendo la app de SMS por defecto.
            if (!_store.IsDefaultSmsApp)
            {
                await SocShared.ModernDialog.AlertAsync(this,
                    _localization.GetString("messages.default_title"),
                    _localization.GetString("messages.delete_needs_default"),
                    _localization.GetString("common.ok"));
                return;
            }

            var confirmed = await SocShared.ModernDialog.AlertAsync(this,
                _localization.GetString("common.delete"),
                _localization.GetString("messages.confirm_delete"),
                _localization.GetString("common.yes"),
                _localization.GetString("common.cancel"));
            if (!confirmed) return;

            try
            {
                if (await _store.DeleteAsync(message))
                    _messages.Remove(message);
                else
                    await SocShared.ModernDialog.AlertAsync(this,
                        _localization.GetString("common.error"),
                        _localization.GetString("messages.delete_error"),
                        _localization.GetString("common.ok"));
            }
            catch (Exception ex)
            {
                await SocShared.ModernDialog.AlertAsync(this,
                    _localization.GetString("common.error"),
                    $"{_localization.GetString("messages.delete_error")}: {ex.Message}",
                    _localization.GetString("common.ok"));
            }
        }

        private async void OnMakeDefaultClicked(object? sender, EventArgs e)
        {
            try
            {
                await _store.RequestDefaultAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Messages] RequestDefault: {ex.Message}");
            }
            UpdateDefaultAppBanner();
            await LoadAsync();
        }

        private static async Task GoToComposeAsync(string? address)
        {
            // ShellNavigationQueryParameters pasa los valores tal cual, sin codificarlos en la ruta.
            var parameters = new ShellNavigationQueryParameters();
            if (!string.IsNullOrWhiteSpace(address)) parameters["to"] = address;
            await Shell.Current.GoToAsync(nameof(ComposePage), parameters);
        }

        // --------------------------------------------------------------- estado

        /// <summary>El aviso solo aparece si el dispositivo admite el rol y aun no lo tenemos.</summary>
        private void UpdateDefaultAppBanner()
        {
            try
            {
                DefaultAppBanner.IsVisible = _store.IsSupported && _store.CanBeDefault && !_store.IsDefaultSmsApp;
            }
            catch
            {
                DefaultAppBanner.IsVisible = false;
            }
        }

        private void UpdateTabButtons()
        {
            InboxButton.Style = LookupStyle(_showingInbox ? "PrimaryButton" : "OutlineButton");
            SentButton.Style = LookupStyle(_showingInbox ? "OutlineButton" : "PrimaryButton");
        }

        private static Style? LookupStyle(string key)
            => Application.Current?.Resources.TryGetValue(key, out var s) == true ? s as Style : null;

        private void OnLanguageChanged(object? sender, EventArgs e)
            => MainThread.BeginInvokeOnMainThread(UpdateLocalizedStrings);

        private void UpdateLocalizedStrings()
        {
            Title = _localization.GetString("messages.title");
            InboxButton.Text = _localization.GetString("messages.inbox");
            SentButton.Text = _localization.GetString("messages.sent");
            ComposeButton.Text = _localization.GetString("messages.compose");
            DefaultAppTitle.Text = _localization.GetString("messages.default_title");
            DefaultAppText.Text = _localization.GetString("messages.default_text");
            DefaultAppButton.Text = _localization.GetString("messages.default_button");
            EmptyLabel.Text = _localization.GetString("messages.empty");
            EmptyHintLabel.Text = _localization.GetString(
                _showingInbox ? "messages.empty_inbox_hint" : "messages.empty_sent_hint");
        }
    }
}
