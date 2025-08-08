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
        private readonly IContactService _contactService;
        private ObservableCollection<string> phones = new();

        public MainPage(ILoggingService loggingService, IContactService contactService)
        {
            InitializeComponent();
            _loggingService = loggingService;
            _contactService = contactService;
            
            var json = Preferences.Default.Get("phones", "[]");
            var list = JsonSerializer.Deserialize<List<string>>(json);
            if (list != null)
            {
                foreach (var phone in list)
                    phones.Add(phone);
            }
            PhoneList.ItemsSource = phones;
            
            // Suscribirse al mensaje de contacto seleccionado
            MessagingCenter.Subscribe<ContactsPage, string>(this, "ContactSelected", OnContactSelected);
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
                var contactsPage = new ContactsPage(_contactService);
                
                // Suscribirse al evento de contacto seleccionado
                contactsPage.ContactSelected += (phoneNumber) =>
                {
                    // Llamar al método de procesamiento en el hilo principal
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        OnContactSelectedFromEvent(phoneNumber);
                    });
                };
                
                await Navigation.PushAsync(contactsPage);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al abrir contactos", ex);
                await DisplayAlert("Error", "Error al abrir la lista de contactos", "OK");
            }
        }

        private void OnContactSelectedFromEvent(string phoneNumber)
        {
            try
            {
                // Debug: Agregar log para verificar que se está recibiendo el mensaje
                System.Diagnostics.Debug.WriteLine($"Recibido contacto seleccionado por evento: {phoneNumber}");
                _loggingService.LogInfo($"Recibido contacto seleccionado por evento: {phoneNumber}");
                
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
                                await DisplayAlert("Número agregado", 
                                    $"El número {cleanNumber} ha sido agregado exitosamente", 
                                    "OK");
                            });
                        }
                        else
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                await DisplayAlert("Número duplicado", 
                                    "Este número ya está en la lista.", 
                                    "OK");
                            });
                        }
                    }
                    else
                    {
                        _loggingService.LogWarning($"Número inválido desde contactos: {cleanNumber}");
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("Número no válido", 
                                "El número seleccionado no es válido.", 
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

        private void OnContactSelected(ContactsPage sender, string phoneNumber)
        {
            try
            {
                // Debug: Agregar log para verificar que se está recibiendo el mensaje
                System.Diagnostics.Debug.WriteLine($"Recibido contacto seleccionado por MessagingCenter: {phoneNumber}");
                _loggingService.LogInfo($"Recibido contacto seleccionado por MessagingCenter: {phoneNumber}");
                
                // Llamar al mismo método que maneja el evento
                OnContactSelectedFromEvent(phoneNumber);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error al procesar contacto seleccionado", ex);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Desuscribirse del mensaje para evitar memory leaks
            MessagingCenter.Unsubscribe<ContactsPage, string>(this, "ContactSelected");
        }
    }

}
