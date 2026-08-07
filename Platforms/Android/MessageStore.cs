using Android.App;
using Android.App.Roles;
using Android.Content;
using Android.OS;
using AndroidTelephony = Android.Provider.Telephony;
using AndroidSmsManager = Android.Telephony.SmsManager;
using SMSForwarder.Models;
using SMSForwarder.Services;

namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Implementacion Android de <see cref="IMessageStore"/>: acceso al proveedor de SMS del
    /// sistema (content://sms) y peticion del rol de app de SMS por defecto.
    /// </summary>
    public class MessageStore : IMessageStore
    {
        public const int RequestDefaultCode = 8123;
        private static TaskCompletionSource<bool>? _pendingDefault;

        // Columnas del proveedor (nombres estables).
        private const string ColId = "_id";
        private const string ColAddress = "address";
        private const string ColBody = "body";
        private const string ColDate = "date";
        private const string ColRead = "read";
        private const string ColType = "type";

        public bool IsSupported => true;

        public bool IsDefaultSmsApp => IsAppDefault(global::Android.App.Application.Context);

        public bool CanBeDefault
        {
            get
            {
                try
                {
                    var ctx = Platform.CurrentActivity ?? global::Android.App.Application.Context;
                    var pm = ctx.PackageManager;
                    bool telephony = pm != null &&
                        pm.HasSystemFeature(global::Android.Content.PM.PackageManager.FeatureTelephony);
                    if (!telephony) return false;
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                    {
                        var rm = (RoleManager?)ctx.GetSystemService(Context.RoleService);
                        return rm != null && rm.IsRoleAvailable(RoleManager.RoleSms);
                    }
                    return true;
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// True si esta app es el gestor de SMS por defecto del sistema.
        ///
        /// Desde Android 10 la fuente de verdad es RoleManager: hay dispositivos (Xiaomi/HyperOS,
        /// por ejemplo) que conceden el rol dejando <c>Settings.Secure.sms_default_application</c>
        /// a null, y entonces GetDefaultSmsPackage devuelve null aunque el rol si este concedido.
        /// Por eso se consulta primero el rol y solo se cae al ajuste antiguo en API menores.
        /// </summary>
        public static bool IsAppDefault(Context context)
        {
            try
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    var rm = (RoleManager?)context.GetSystemService(Context.RoleService);
                    if (rm != null && rm.IsRoleAvailable(RoleManager.RoleSms))
                        return rm.IsRoleHeld(RoleManager.RoleSms);
                }

                var def = AndroidTelephony.Sms.GetDefaultSmsPackage(context);
                return def != null && def == context.PackageName;
            }
            catch { return false; }
        }

        public Task<bool> RequestDefaultAsync()
        {
            var activity = Platform.CurrentActivity;
            if (activity == null) return Task.FromResult(false);

            if (IsAppDefault(activity)) return Task.FromResult(true);

            _pendingDefault?.TrySetResult(false);
            var tcs = new TaskCompletionSource<bool>();
            _pendingDefault = tcs;

            try
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    var rm = (RoleManager?)activity.GetSystemService(Context.RoleService);
                    if (rm != null && rm.IsRoleAvailable(RoleManager.RoleSms))
                    {
                        if (rm.IsRoleHeld(RoleManager.RoleSms))
                        {
                            _pendingDefault = null;
                            return Task.FromResult(true);
                        }
                        var intent = rm.CreateRequestRoleIntent(RoleManager.RoleSms);
                        activity.StartActivityForResult(intent, RequestDefaultCode);
                        return tcs.Task;
                    }
                }

                // Ruta clasica (< Android 10).
                var change = new Intent(AndroidTelephony.Sms.Intents.ActionChangeDefault);
                change.PutExtra(AndroidTelephony.Sms.Intents.ExtraPackageName, activity.PackageName);
                activity.StartActivityForResult(change, RequestDefaultCode);
                return tcs.Task;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MessageStore] RequestDefault: {ex.Message}");
                _pendingDefault = null;
                return Task.FromResult(false);
            }
        }

        /// <summary>Enlazado desde MainActivity.OnActivityResult. Devuelve true si el resultado era nuestro.</summary>
        public static bool HandleActivityResult(int requestCode, Context context)
        {
            if (requestCode != RequestDefaultCode) return false;
            var tcs = _pendingDefault;
            _pendingDefault = null;
            tcs?.TrySetResult(IsAppDefault(context));
            return true;
        }

        public Task<List<SmsMessageItem>> GetInboxAsync()
            => Task.Run(() => Query(AndroidTelephony.Sms.Inbox.ContentUri, isInbox: true));

        public Task<List<SmsMessageItem>> GetSentAsync()
            => Task.Run(() => Query(AndroidTelephony.Sms.Sent.ContentUri, isInbox: false));

        private static List<SmsMessageItem> Query(global::Android.Net.Uri? uri, bool isInbox)
        {
            var list = new List<SmsMessageItem>();
            if (uri == null) return list;
            var resolver = global::Android.App.Application.Context.ContentResolver;
            if (resolver == null) return list;

            try
            {
                using var c = resolver.Query(uri,
                    new[] { ColId, ColAddress, ColBody, ColDate, ColRead },
                    null, null, ColDate + " DESC");
                if (c == null) return list;

                int iId = c.GetColumnIndex(ColId);
                int iAddr = c.GetColumnIndex(ColAddress);
                int iBody = c.GetColumnIndex(ColBody);
                int iDate = c.GetColumnIndex(ColDate);
                int iRead = c.GetColumnIndex(ColRead);

                while (c.MoveToNext() && list.Count < 500)
                {
                    long epoch = iDate >= 0 ? c.GetLong(iDate) : 0;
                    list.Add(new SmsMessageItem
                    {
                        Id = iId >= 0 ? c.GetLong(iId) : 0,
                        Address = iAddr >= 0 ? (c.GetString(iAddr) ?? "") : "",
                        Body = iBody >= 0 ? (c.GetString(iBody) ?? "") : "",
                        Date = FromEpoch(epoch),
                        IsRead = iRead >= 0 && c.GetInt(iRead) == 1,
                        IsInbox = isInbox,
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MessageStore] Query: {ex.Message}");
            }
            return list;
        }

        public Task<bool> DeleteAsync(SmsMessageItem message) => Task.Run(() =>
        {
            var resolver = global::Android.App.Application.Context.ContentResolver;
            if (resolver == null || message.Id <= 0) return false;
            try
            {
                var uri = ContentUris.WithAppendedId(AndroidTelephony.Sms.ContentUri!, message.Id);
                return resolver.Delete(uri, null, null) > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MessageStore] Delete: {ex.Message}");
                return false;
            }
        });

        public Task<bool> MarkReadAsync(SmsMessageItem message) => Task.Run(() =>
        {
            var resolver = global::Android.App.Application.Context.ContentResolver;
            if (resolver == null || message.Id <= 0) return false;
            try
            {
                var values = new ContentValues();
                values.Put(ColRead, 1);
                var uri = ContentUris.WithAppendedId(AndroidTelephony.Sms.ContentUri!, message.Id);
                return resolver.Update(uri, values, null, null) > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MessageStore] MarkRead: {ex.Message}");
                return false;
            }
        });

        public Task<bool> SendAsync(string address, string body) => Task.Run(() =>
        {
            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrEmpty(body)) return false;
            try
            {
#pragma warning disable CS0618
                using var sms = AndroidSmsManager.Default;
#pragma warning restore CS0618
                if (sms == null) return false;
                if (body.Length > 160)
                {
                    var parts = sms.DivideMessage(body);
                    sms.SendMultipartTextMessage(address, null, parts, null, null);
                }
                else
                {
                    sms.SendTextMessage(address, null, body, null, null);
                }

                // Guardar en Enviados (solo funciona si somos la app por defecto; si no, se ignora).
                try
                {
                    var resolver = global::Android.App.Application.Context.ContentResolver;
                    var values = new ContentValues();
                    values.Put(ColAddress, address);
                    values.Put(ColBody, body);
                    values.Put(ColDate, Java.Lang.JavaSystem.CurrentTimeMillis());
                    values.Put(ColRead, 1);
                    values.Put(ColType, 2); // MESSAGE_TYPE_SENT
                    resolver?.Insert(AndroidTelephony.Sms.Sent.ContentUri!, values);
                }
                catch { }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MessageStore] Send: {ex.Message}");
                return false;
            }
        });

        /// <summary>
        /// Guarda un SMS entrante en el buzon del sistema. La llama <see cref="SmsDeliverReceiver"/>
        /// cuando la app es la predeterminada (responsable de la persistencia).
        /// </summary>
        public static void PersistInbox(Context context, string sender, string body, long timestampMillis)
        {
            try
            {
                var resolver = context.ContentResolver;
                if (resolver == null) return;
                var values = new ContentValues();
                values.Put(ColAddress, sender);
                values.Put(ColBody, body);
                values.Put(ColDate, timestampMillis > 0 ? timestampMillis : Java.Lang.JavaSystem.CurrentTimeMillis());
                values.Put(ColRead, 0);
                values.Put(ColType, 1); // MESSAGE_TYPE_INBOX
                resolver.Insert(AndroidTelephony.Sms.Inbox.ContentUri!, values);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MessageStore] PersistInbox: {ex.Message}");
            }
        }

        private static DateTime FromEpoch(long millis)
        {
            if (millis <= 0) return DateTime.MinValue;
            try { return DateTimeOffset.FromUnixTimeMilliseconds(millis).LocalDateTime; }
            catch { return DateTime.MinValue; }
        }
    }
}
