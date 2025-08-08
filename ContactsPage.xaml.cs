using SMSForwarder.Models;
using SMSForwarder.Services;
using System.Collections.ObjectModel;
using AppContact = SMSForwarder.Models.Contact;

namespace SMSForwarder
{
    public partial class ContactsPage : ContentPage
    {
        private readonly IContactService _contactService;
        private ObservableCollection<AppContact> _allContacts = new();
        private ObservableCollection<AppContact> _filteredContacts = new();
        
        // Evento para notificar cuando se selecciona un contacto
        public event Action<string>? ContactSelected;

        public ContactsPage(IContactService contactService)
        {
            InitializeComponent();
            _contactService = contactService;
            ContactsList.ItemsSource = _filteredContacts;
            LoadContacts();
        }

        private async void LoadContacts()
        {
            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                ContactsList.IsVisible = false;

                var contacts = await _contactService.GetContactsAsync();
                
                _allContacts.Clear();
                _filteredContacts.Clear();

                foreach (var contact in contacts)
                {
                    _allContacts.Add(contact);
                    _filteredContacts.Add(contact);
                }

                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                ContactsList.IsVisible = true;

                if (!contacts.Any())
                {
                    await DisplayAlert("Sin contactos", 
                        "No se encontraron contactos con números de teléfono. Verifica que tienes contactos guardados y que has concedido los permisos necesarios.", 
                        "OK");
                }
            }
            catch (Exception ex)
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                ContactsList.IsVisible = true;

                await DisplayAlert("Error", 
                    $"Error al cargar contactos: {ex.Message}", 
                    "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchText = e.NewTextValue?.ToLower() ?? "";
                
                _filteredContacts.Clear();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (var contact in _allContacts)
                    {
                        _filteredContacts.Add(contact);
                    }
                }
                else
                {
                    var filtered = _allContacts.Where(c => 
                        c.Name.ToLower().Contains(searchText) || 
                        c.PhoneNumber.Contains(searchText)).ToList();

                    foreach (var contact in filtered)
                    {
                        _filteredContacts.Add(contact);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error filtering contacts: {ex.Message}");
            }
        }

        private async void OnContactSelected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.FirstOrDefault() is AppContact selectedContact)
                {
                    var result = await DisplayAlert("Confirmar selección", 
                        $"¿Deseas añadir el número de {selectedContact.Name}?\n\n{selectedContact.PhoneNumber}", 
                        "Sí", "No");

                    if (result)
                    {
                        // Debug: Agregar log para verificar que se está enviando el mensaje
                        System.Diagnostics.Debug.WriteLine($"Enviando contacto seleccionado: {selectedContact.PhoneNumber}");
                        
                        // Notificar a través del evento
                        ContactSelected?.Invoke(selectedContact.PhoneNumber);
                        
                        // También mantener el MessagingCenter como respaldo
                        MessagingCenter.Send(this, "ContactSelected", selectedContact.PhoneNumber);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        // Deseleccionar si el usuario cancela
                        ContactsList.SelectedItem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al seleccionar contacto: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            SearchEntry.Text = "";
            LoadContacts();
        }
    }
}