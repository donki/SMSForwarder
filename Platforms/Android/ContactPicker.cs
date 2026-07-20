using Android.App;
using Android.Content;
using Android.Provider;
using SMSForwarder.Services;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Selector de contactos basado en ACTION_PICK sobre la tabla de telefonos.
    ///
    /// La app de contactos del sistema delega permiso de lectura sobre el URI que devuelve,
    /// asi que leer ese URI no requiere READ_CONTACTS.
    ///
    /// Detalle importante: se hace ACTION_PICK sobre Phone.ContentUri (no sobre
    /// Contacts.ContentUri) para que el URI devuelto sea ya la fila del telefono y el numero
    /// se lea de ahi directamente. Consultar despues Phone.ContentUri por contact id -que es
    /// lo que hace Contacts.Default.PickContactAsync de MAUI- queda fuera de esa delegacion
    /// y volveria a exigir el permiso, que es justo lo que esta clase evita.
    /// </summary>
    public class ContactPicker : IContactPicker
    {
        private const int PickPhoneRequestCode = 9001;

        private static TaskCompletionSource<string?>? _pending;

        public Task<string?> PickPhoneNumberAsync()
        {
            var activity = Platform.CurrentActivity;
            if (activity is null)
                return Task.FromResult<string?>(null);

            // Si quedara un selector anterior sin resolver, se cierra su tarea para no colgarla.
            _pending?.TrySetResult(null);

            var tcs = new TaskCompletionSource<string?>();
            _pending = tcs;

            try
            {
                var intent = new Intent(Intent.ActionPick, ContactsContract.CommonDataKinds.Phone.ContentUri);
                activity.StartActivityForResult(intent, PickPhoneRequestCode);
            }
            catch (Exception)
            {
                _pending = null;
                throw;
            }

            return tcs.Task;
        }

        /// <summary>
        /// Enlazado desde MainActivity.OnActivityResult.
        /// Devuelve true si el resultado pertenecia al selector de contactos.
        /// </summary>
        public static bool HandleActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            if (requestCode != PickPhoneRequestCode)
                return false;

            var tcs = _pending;
            _pending = null;

            if (tcs is null)
                return true;

            if (resultCode != Result.Ok || data?.Data is null)
            {
                tcs.TrySetResult(null);
                return true;
            }

            try
            {
                tcs.TrySetResult(ReadNumber(data.Data));
            }
            catch (Exception)
            {
                tcs.TrySetResult(null);
            }

            return true;
        }

        private static string? ReadNumber(global::Android.Net.Uri uri)
        {
            var resolver = global::Android.App.Application.Context.ContentResolver;
            if (resolver is null)
                return null;

            // En una fila de Phone, la columna Data (DATA1) es el numero de telefono.
            var numberColumn = ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Data;

            // Solo se lee el URI devuelto: la delegacion del selector cubre esta lectura.
            using var cursor = resolver.Query(uri, new[] { numberColumn }, null, null, null);

            if (cursor?.MoveToFirst() != true)
                return null;

            var index = cursor.GetColumnIndex(numberColumn);
            return index < 0 ? null : cursor.GetString(index);
        }
    }
}
