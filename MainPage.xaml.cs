using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;
using SMSForwarder.Services;

namespace SMSForwarder
{
    // MainPage.xaml.cs


    public partial class MainPage : ContentPage
    {
        private readonly ILoggingService _loggingService;
        private ObservableCollection<string> phones = new();

        public MainPage(ILoggingService loggingService)
        {
            InitializeComponent();
            _loggingService = loggingService;
            var json = Preferences.Default.Get("phones", "[]");
            var list = JsonSerializer.Deserialize<List<string>>(json);
            if (list != null)
            {
                foreach (var phone in list)
                    phones.Add(phone);
            }
            PhoneList.ItemsSource = phones;
        }


        private void OnAddClicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(PhoneEntry.Text))
                {
                    var cleanNumber = PhoneEntry.Text.Trim();
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
                            DisplayAlert("Número duplicado", "Este número ya está en la lista.", "OK");
                        }
                    }
                    else
                    {
                        DisplayAlert("Número no válido", "Por favor, introduce un número de teléfono válido (7-15 dígitos).", "OK");
                        _loggingService.LogWarning($"Intento de agregar número inválido: {cleanNumber}");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al agregar número", ex);
                DisplayAlert("Error", "Error al agregar el número", "OK");
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
                DisplayAlert("Error", "Error al eliminar el número", "OK");
            }
        }

        private void SavePhones()
        {
            Preferences.Default.Set("phones", JsonSerializer.Serialize(phones));
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
    }

}
