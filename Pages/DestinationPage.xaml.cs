using System.Collections.ObjectModel;
using SMSForwarder.Models;
using SMSForwarder.Services;

namespace SMSForwarder.Pages
{
    /// <summary>
    /// Que SMS recibe un numero de la lista. Por defecto le llegan todos; aqui se le pueden poner
    /// remitentes y palabras o frases, y entonces solo le llega lo que case (con las dos cosas
    /// puestas, hay que cumplir las dos). Se guarda al momento: no hay boton de guardar.
    /// </summary>
    public partial class DestinationPage : ContentPage, IQueryAttributable
    {
        private readonly ILocalizationService _localization;
        private readonly IContactPicker _contactPicker;
        private readonly ILoggingService _logging;
        private readonly ObservableCollection<string> _senders = [];
        private readonly ObservableCollection<string> _keywords = [];
        private ForwardDestination? _destination;
        private bool _loading;

        public DestinationPage(ILocalizationService localization, IContactPicker contactPicker, ILoggingService logging)
        {
            InitializeComponent();
            _localization = localization;
            _contactPicker = contactPicker;
            _logging = logging;
            SendersList.ItemsSource = _senders;
            KeywordsList.ItemsSource = _keywords;
            ApplyTexts();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // El destino viaja como objeto: un numero con «+» dentro de la ruta se romperia.
            if (query.TryGetValue("destination", out var value) && value is ForwardDestination destination)
            {
                _destination = destination;
                Show();
            }
        }

        private void ApplyTexts()
        {
            Title = _localization.GetString("destination.title");
            AllLabel.Text = _localization.GetString("destination.all");
            AllHint.Text = _localization.GetString("destination.all_hint");
            SendersTitle.Text = _localization.GetString("destination.senders");
            SendersHint.Text = _localization.GetString("destination.senders_hint");
            SenderEntry.Placeholder = _localization.GetString("main.placeholder");
            KeywordsTitle.Text = _localization.GetString("destination.keywords");
            KeywordsHint.Text = _localization.GetString("destination.keywords_hint");
            KeywordEntry.Placeholder = _localization.GetString("destination.keyword_placeholder");
            SemanticProperties.SetDescription(AddSenderButton, _localization.GetString("destination.add"));
            SemanticProperties.SetDescription(PickSenderButton, _localization.GetString("main.from_contacts"));
            SemanticProperties.SetDescription(AddKeywordButton, _localization.GetString("destination.add"));
        }

        private void Show()
        {
            if (_destination is null) return;
            _loading = true;
            PhoneLabel.Text = _destination.Phone;
            _senders.Clear();
            foreach (var s in _destination.Senders) _senders.Add(s);
            _keywords.Clear();
            foreach (var k in _destination.Keywords) _keywords.Add(k);
            AllSwitch.IsToggled = _destination.ForwardsEverything;
            FiltersPanel.IsVisible = !AllSwitch.IsToggled;
            _loading = false;
            UpdateSummary();
        }

        private void OnAllToggled(object? sender, ToggledEventArgs e)
        {
            FiltersPanel.IsVisible = !e.Value;
            if (_loading || _destination is null) return;
            if (e.Value)
            {
                // «Todos los SMS»: se quitan los filtros (lo que estaba escrito se pierde, y se avisa
                // en el texto de ayuda).
                _senders.Clear();
                _keywords.Clear();
            }
            Save();
        }

        // ------------------------------------------------------------------ remitentes

        private async void OnAddSenderClicked(object? sender, EventArgs e)
        {
            var number = (SenderEntry.Text ?? string.Empty).Replace(" ", "").Trim();
            if (number.Length == 0) return;
            if (_senders.Any(s => PhoneNumbers.AreEqual(s, number)))
            {
                await SocShared.ModernDialog.AlertAsync(this, _localization.GetString("destination.title"), _localization.GetString("destination.duplicate"), "OK");
                return;
            }
            _senders.Add(number);
            Clear(SenderEntry);
            Save();
        }

        private async void OnPickSenderClicked(object? sender, EventArgs e)
        {
            try
            {
                var number = await _contactPicker.PickPhoneNumberAsync();
                if (string.IsNullOrWhiteSpace(number)) return;
                SenderEntry.Text = number.Replace(" ", "").Trim();
                OnAddSenderClicked(sender, e);
            }
            catch (Exception ex)
            {
                _logging.LogError("Error al abrir contactos", ex);
            }
        }

        private void OnRemoveSenderClicked(object? sender, EventArgs e)
        {
            if (sender is ImageButton { CommandParameter: string number })
            {
                _senders.Remove(number);
                Save();
            }
        }

        // ------------------------------------------------------------------ palabras y frases

        private async void OnAddKeywordClicked(object? sender, EventArgs e)
        {
            var word = (KeywordEntry.Text ?? string.Empty).Trim();
            if (word.Length == 0) return;
            if (_keywords.Any(k => string.Equals(k, word, StringComparison.CurrentCultureIgnoreCase)))
            {
                await SocShared.ModernDialog.AlertAsync(this, _localization.GetString("destination.title"), _localization.GetString("destination.duplicate"), "OK");
                return;
            }
            _keywords.Add(word);
            Clear(KeywordEntry);
            Save();
        }

        /// <summary>
        /// Vaciar la casilla. Va en el siguiente turno del hilo de la interfaz: puesto a secas dentro
        /// del manejador, el teclado de Android vuelve a escribir lo que habia y el texto se queda.
        /// </summary>
        private void Clear(Entry entry) => Dispatcher.Dispatch(() =>
        {
            entry.Text = string.Empty;
            entry.Unfocus();
        });

        private void OnRemoveKeywordClicked(object? sender, EventArgs e)
        {
            if (sender is ImageButton { CommandParameter: string word })
            {
                _keywords.Remove(word);
                Save();
            }
        }

        // ------------------------------------------------------------------ guardar

        private void Save()
        {
            if (_destination is null) return;
            _destination.Senders = [.. _senders];
            _destination.Keywords = [.. _keywords];
            // Con el interruptor apagado y todavia sin filtros, el destino sigue recibiendolo todo
            // (sin condiciones no hay nada que descartar): el interruptor se queda como lo dejo el
            // usuario y el resumen de abajo lo dice.
            DestinationStore.Save(_logging);
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            if (_destination is null) return;
            SummaryLabel.Text = DestinationStore.Describe(_destination, _localization);
        }
    }
}
