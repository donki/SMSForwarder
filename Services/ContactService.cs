using SMSForwarder.Models;
using AppContact = SMSForwarder.Models.Contact;

namespace SMSForwarder.Services
{
    public interface IContactService
    {
        Task<List<AppContact>> GetContactsAsync();
        Task<bool> RequestContactPermissionAsync();
    }

    public class ContactService : IContactService
    {
        public async Task<bool> RequestContactPermissionAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
                
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.ContactsRead>();
                }
                
                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error requesting contact permission: {ex.Message}");
                return false;
            }
        }

        public async Task<List<AppContact>> GetContactsAsync()
        {
            var contacts = new List<AppContact>();
            
            try
            {
                var hasPermission = await RequestContactPermissionAsync();
                if (!hasPermission)
                {
                    return contacts;
                }

#if ANDROID
                await GetAndroidContactsAsync(contacts);
#endif
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting contacts: {ex.Message}");
            }
            
            return contacts.OrderBy(c => c.Name).ToList();
        }

#if ANDROID
        private async Task GetAndroidContactsAsync(List<AppContact> contacts)
        {
            try
            {
                var context = Platform.CurrentActivity ?? Android.App.Application.Context;
                if (context == null) return;

                var resolver = context.ContentResolver;
                if (resolver == null) return;

                var cursor = resolver.Query(
                    Android.Provider.ContactsContract.CommonDataKinds.Phone.ContentUri,
                    new string[] {
                        Android.Provider.ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId,
                        Android.Provider.ContactsContract.CommonDataKinds.Phone.InterfaceConsts.DisplayName,
                        Android.Provider.ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Data
                    },
                    null, null, 
                    Android.Provider.ContactsContract.CommonDataKinds.Phone.InterfaceConsts.DisplayName + " ASC");

                if (cursor != null && cursor.MoveToFirst())
                {
                    var contactIdIndex = cursor.GetColumnIndex(Android.Provider.ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId);
                    var nameIndex = cursor.GetColumnIndex(Android.Provider.ContactsContract.CommonDataKinds.Phone.InterfaceConsts.DisplayName);
                    var numberIndex = cursor.GetColumnIndex(Android.Provider.ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Data);

                    do
                    {
                        var contactId = cursor.GetString(contactIdIndex) ?? "";
                        var name = cursor.GetString(nameIndex) ?? "Sin nombre";
                        var phoneNumber = cursor.GetString(numberIndex) ?? "";

                        // Limpiar el número de teléfono
                        phoneNumber = CleanPhoneNumber(phoneNumber);

                        if (!string.IsNullOrWhiteSpace(phoneNumber) && IsValidPhoneNumber(phoneNumber))
                        {
                            // Evitar duplicados
                            if (!contacts.Any(c => c.PhoneNumber == phoneNumber))
                            {
                                contacts.Add(new AppContact
                                {
                                    Id = contactId,
                                    Name = name,
                                    PhoneNumber = phoneNumber
                                });
                            }
                        }
                    }
                    while (cursor.MoveToNext());
                }

                cursor?.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting Android contacts: {ex.Message}");
            }
        }
#endif

        private string CleanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return "";

            // Remover espacios, guiones, paréntesis y otros caracteres
            var cleaned = phoneNumber.Replace(" ", "")
                                   .Replace("-", "")
                                   .Replace("(", "")
                                   .Replace(")", "")
                                   .Replace(".", "")
                                   .Trim();

            return cleaned;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Verificar que tenga al menos 7 dígitos y máximo 15
            var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
            
            // Permitir números que empiecen con + seguido de dígitos
            if (phoneNumber.StartsWith("+"))
            {
                return digitsOnly.Length >= 7 && digitsOnly.Length <= 15;
            }
            
            return digitsOnly.Length >= 7 && digitsOnly.Length <= 15;
        }
    }
}