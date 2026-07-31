using SMSForwarder.Services;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SMSForwarder
{
    // MainPage.xaml.cs


    public partial class MainPage : ContentPage
    {
        private readonly ILoggingService _loggingService;
        private readonly IContactPicker _contactPicker;
        private readonly ILocalizationService _localizationService;
        private bool _isApplyingLanguageSelection;
        private ObservableCollection<string> phones = new();

        public MainPage(ILoggingService loggingService, IContactPicker contactPicker, ILocalizationService localizationService)
        {
            InitializeComponent();
            _loggingService = loggingService;
            _contactPicker = contactPicker;
            _localizationService = localizationService;

            var json = Preferences.Default.Get("phones", "[]");
            var list = JsonSerializer.Deserialize<List<string>>(json);
            if (list != null)
            {
                foreach (var phone in list)
                    phones.Add(phone);
            }
            PhoneList.ItemsSource = phones;

            // Actualizar strings localizados
            UpdateLocalizedStrings();
            _localizationService.LanguageChanged += OnLanguageChanged;
            UpdateLanguageButtons();
        }

        private void UpdateLocalizedStrings()
        {
            TitleLabel.Text = _localizationService.GetString("main.title");
            SubtitleLabel.Text = _localizationService.GetString("main.subtitle");
            LanguageLabel.Text = _localizationService.GetString("main.language");
            PhoneEntry.Placeholder = _localizationService.GetString("main.placeholder");
            AddButton.Text = _localizationService.GetString("main.add_number");
            ContactsButton.Text = _localizationService.GetString("main.from_contacts");
            NumbersListLabel.Text = _localizationService.GetString("main.numbers_list");
            InfoTitle.Text = "💡 " + _localizationService.GetString("menu.settings");

            // Actualizar información de ayuda según idioma
            if (_localizationService.CurrentLanguage == "es-ES")
            {
                InfoText.Text = "• Los SMS recibidos se reenviarán automáticamente a estos números\n• Puedes escribir números manualmente o seleccionarlos desde tus contactos\n• Para configurar permisos avanzados, ve a la sección Diagnósticos\n• Desliza hacia la izquierda en un número para eliminarlo";
            }
            else
            {
                InfoText.Text = "• Received SMS will be automatically forwarded to these numbers\n• You can enter numbers manually or select them from your contacts\n• For advanced permission settings, go to the Diagnostics section\n• Swipe left on a number to delete it";
            }
        }

        private void OnSpanishClicked(object? sender, EventArgs e) => ApplyLanguage("es-ES");

        private void OnEnglishClicked(object? sender, EventArgs e) => ApplyLanguage("en-US");

        private void ApplyLanguage(string languageCode)
        {
            if (_isApplyingLanguageSelection || languageCode == _localizationService.CurrentLanguage)
            {
                return;
            }

            // El servicio persiste el idioma y dispara LanguageChanged.
            _localizationService.SetLanguage(languageCode);
            UpdateLocalizedStrings();
        }

        // Resalta el idioma activo con PrimaryButton (indigo relleno) y el otro con OutlineButton
        // (contorno indigo). Mismo patron visual que FileManager.
        private void UpdateLanguageButtons()
        {
            _isApplyingLanguageSelection = true;

            var isSpanish = _localizationService.CurrentLanguage == "es-ES";
            SpanishButton.Style = LookupStyle(isSpanish ? "PrimaryButton" : "OutlineButton");
            EnglishButton.Style = LookupStyle(isSpanish ? "OutlineButton" : "PrimaryButton");

            _isApplyingLanguageSelection = false;
        }

        private static Style? LookupStyle(string key)
            => Application.Current?.Resources.TryGetValue(key, out var s) == true ? s as Style : null;

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateLanguageButtons();
                UpdateLocalizedStrings();
            });
        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(PhoneEntry.Text))
                {
                    // Elimina todos los espacios del número ingresado
                    var cleanNumber = PhoneEntry.Text.Replace(" ", "").Trim();
                    if (IsValidPhoneNumber(cleanNumber))
                    {
                        if (!phones.Contains(cleanNumber))
                        {
                            phones.Add(cleanNumber);
                            SavePhones();
                            PhoneEntry.Text = string.Empty;
                            _loggingService.LogInfo($"Número agregado: {cleanNumber}");
                        }
                        else
                        {
                            if (_localizationService.CurrentLanguage == "es-ES")
                                SocShared.ModernDialog.AlertAsync(this,"Número duplicado", "Este número ya está en la lista.", "OK");
                            else
                                SocShared.ModernDialog.AlertAsync(this,"Duplicate number", "This number is already in the list.", "OK");
                        }
                    }
                    else
                    {
                        if (_localizationService.CurrentLanguage == "es-ES")
                            SocShared.ModernDialog.AlertAsync(this,"Número no válido", "Por favor, introduce un número de teléfono válido (7-15 dígitos).", "OK");
                        else
                            SocShared.ModernDialog.AlertAsync(this,"Invalid number", "Please enter a valid phone number (7-15 digits).", "OK");
                        _loggingService.LogWarning($"Intento de agregar número inválido: {cleanNumber}");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al agregar número", ex);
                if (_localizationService.CurrentLanguage == "es-ES")
                    SocShared.ModernDialog.AlertAsync(this,"Error", "Error al agregar el número", "OK");
                else
                    SocShared.ModernDialog.AlertAsync(this,"Error", "Error adding the number", "OK");
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is string phone)
                {
                    phones.Remove(phone);
                    SavePhones();
                    _loggingService.LogInfo($"Número eliminado: {phone}");
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al eliminar número", ex);
                if (_localizationService.CurrentLanguage == "es-ES")
                    SocShared.ModernDialog.AlertAsync(this,"Error", "Error al eliminar el número", "OK");
                else
                    SocShared.ModernDialog.AlertAsync(this,"Error", "Error deleting the number", "OK");
            }
        }

        private void SavePhones()
        {
            var phonesJson = JsonSerializer.Serialize(phones);
            Preferences.Default.Set("phones", phonesJson);

            // También guardar en las preferencias compartidas de Android para el BroadcastReceiver
            if (Application.Current?.Handler?.MauiContext?.Context is Android.Content.Context context)
            {
                try
                {
                    var prefs = context.GetSharedPreferences($"{context.PackageName}_preferences", Android.Content.FileCreationMode.Private);
                    var editor = prefs?.Edit();
                    editor?.PutString("phones", phonesJson);
                    editor?.Apply();
                    _loggingService.LogInfo($"Números guardados en preferencias de Android: {phonesJson}");
                }
                catch (Exception ex)
                {
                    _loggingService.LogError($"Error al guardar en preferencias de Android: {ex.Message}");
                }
            }
        }

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            ((CollectionView)sender).SelectedItem = null; // deseleccionar
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Limpiar el número de espacios y caracteres especiales
            var cleanNumber = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            // Expresión regular más flexible para números de teléfono
            // Acepta números con o sin código de país, mínimo 7 dígitos, máximo 15
            var phoneRegex = new Regex(@"^\+?[1-9]\d{6,14}$");

            return phoneRegex.IsMatch(cleanNumber) && cleanNumber.Length >= 7 && cleanNumber.Length <= 15;
        }

        private async void OnSelectFromContactsClicked(object sender, EventArgs e)
        {
            try
            {
                // Selector del sistema: devuelve solo el numero elegido, sin leer la agenda.
                var phoneNumber = await _contactPicker.PickPhoneNumberAsync();

                if (string.IsNullOrWhiteSpace(phoneNumber))
                    return;

                AddPhoneNumberFromContact(phoneNumber);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al abrir contactos", ex);
                if (_localizationService.CurrentLanguage == "es-ES")
                    await SocShared.ModernDialog.AlertAsync(this,"Error", "Error al abrir la lista de contactos", "OK");
                else
                    await SocShared.ModernDialog.AlertAsync(this,"Error", "Error opening contacts list", "OK");
            }
        }

        private void AddPhoneNumberFromContact(string phoneNumber)
        {
            try
            {
                _loggingService.LogInfo("Numero recibido desde el selector de contactos");

                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    var cleanNumber = phoneNumber.Replace(" ", "").Trim();

                    if (IsValidPhoneNumber(cleanNumber))
                    {
                        if (!phones.Contains(cleanNumber))
                        {
                            phones.Add(cleanNumber);
                            SavePhones();
                            _loggingService.LogInfo($"Número agregado desde contactos: {cleanNumber}");

                            // Mostrar confirmación
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                if (_localizationService.CurrentLanguage == "es-ES")
                                    await SocShared.ModernDialog.AlertAsync(this,"Número agregado",
                                        $"El número {cleanNumber} ha sido agregado exitosamente",
                                        "OK");
                                else
                                    await SocShared.ModernDialog.AlertAsync(this,"Number added",
                                        $"The number {cleanNumber} has been successfully added",
                                        "OK");
                            });
                        }
                        else
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                if (_localizationService.CurrentLanguage == "es-ES")
                                    await SocShared.ModernDialog.AlertAsync(this,"Número duplicado",
                                        "Este número ya está en la lista.",
                                        "OK");
                                else
                                    await SocShared.ModernDialog.AlertAsync(this,"Duplicate number",
                                        "This number is already in the list.",
                                        "OK");
                            });
                        }
                    }
                    else
                    {
                        _loggingService.LogWarning($"Número inválido desde contactos: {cleanNumber}");
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            if (_localizationService.CurrentLanguage == "es-ES")
                                await SocShared.ModernDialog.AlertAsync(this,"Número no válido",
                                    "El número seleccionado no es válido.",
                                    "OK");
                            else
                                await SocShared.ModernDialog.AlertAsync(this,"Invalid number",
                                    "The selected number is invalid.",
                                    "OK");
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al procesar contacto seleccionado", ex);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }

}
