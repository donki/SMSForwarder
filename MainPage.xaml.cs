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

        // Métodos para manejo de permisos
        private async void OnCheckPermissionsClicked(object sender, EventArgs e)
        {
            try
            {
                var permissionService = new PermissionService();
                await permissionService.ShowPermissionStatusAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al verificar permisos", ex);
                await DisplayAlert("Error", "Error al verificar el estado de los permisos", "OK");
            }
        }

        private async void OnConfigureAllPermissionsClicked(object sender, EventArgs e)
        {
            try
            {
                var permissionService = new PermissionService();
                var result = await permissionService.CheckAndRequestAllPermissionsAsync();
                
                if (result)
                {
                    await DisplayAlert("Éxito", "Todos los permisos han sido configurados correctamente", "OK");
                }
                else
                {
                    await DisplayAlert("Atención", "Algunos permisos no pudieron ser configurados. Revise la configuración manualmente.", "OK");
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al configurar permisos", ex);
                await DisplayAlert("Error", "Error al configurar los permisos", "OK");
            }
        }

        private async void OnBatteryOptimizationClicked(object sender, EventArgs e)
        {
            try
            {
                var batteryPermission = new SmsPermissions.BatteryOptimizationPermission();
                var status = await batteryPermission.CheckStatusAsync();

                if (status == PermissionStatus.Granted)
                {
                    await DisplayAlert("Estado de Batería", "✅ La optimización de batería está desactivada correctamente", "OK");
                }
                else
                {
                    var result = await DisplayAlert(
                        "Optimización de Batería",
                        "La optimización de batería está activada. Esto puede impedir que la aplicación funcione en segundo plano.\n\n¿Desea abrir la configuración?",
                        "Sí", "No");

                    if (result)
                    {
                        await batteryPermission.RequestAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al gestionar optimización de batería", ex);
                await DisplayAlert("Error", "Error al acceder a la configuración de batería", "OK");
            }
        }

        private async void OnAutostartClicked(object sender, EventArgs e)
        {
            try
            {
                var autostartPermission = new SmsPermissions.AutoStartPermission();
                
                await DisplayAlert(
                    "Configuración de Autostart",
                    "Se abrirá la configuración de autostart. Busque 'SMS Forwarder' en la lista y active el inicio automático para asegurar que la aplicación funcione después de reiniciar el dispositivo.",
                    "Entendido");

                await autostartPermission.RequestAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al gestionar autostart", ex);
                await DisplayAlert("Error", "Error al acceder a la configuración de autostart", "OK");
            }
        }
    }

}
