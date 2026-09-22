using System.Collections.ObjectModel;
using SMSForwarder.Models;

namespace SMSForwarder.Services
{
    /// <summary>
    /// Los destinos de reenvio, en un solo sitio: la lista que ve la interfaz y su guardado. Se
    /// escriben dos claves a la vez, <c>destinations</c> (destinos con sus filtros) y <c>phones</c>
    /// (solo los numeros, formato de hasta la 2026.09.22.0), y ademas en las preferencias nativas de
    /// Android, que es de donde las lee el receptor de SMS (otro proceso, sin MAUI).
    /// </summary>
    public static class DestinationStore
    {
        private static bool _loaded;

        public static ObservableCollection<ForwardDestination> Items { get; } = [];

        public static void Load()
        {
            if (_loaded) return;
            _loaded = true;
            var destinations = ForwardDestinations.Parse(
                Preferences.Default.Get(ForwardDestinations.DestinationsKey, string.Empty),
                Preferences.Default.Get(ForwardDestinations.PhonesKey, "[]"));
            foreach (var d in destinations)
                Items.Add(d);
        }

        public static void Save(ILoggingService? logging = null)
        {
            var destinationsJson = ForwardDestinations.ToJson(Items);
            var phonesJson = ForwardDestinations.ToPhonesJson(Items);
            Preferences.Default.Set(ForwardDestinations.DestinationsKey, destinationsJson);
            Preferences.Default.Set(ForwardDestinations.PhonesKey, phonesJson);
#if ANDROID
            try
            {
                var context = global::Android.App.Application.Context;
                var prefs = context.GetSharedPreferences($"{context.PackageName}_preferences", global::Android.Content.FileCreationMode.Private);
                var editor = prefs?.Edit();
                editor?.PutString(ForwardDestinations.DestinationsKey, destinationsJson);
                editor?.PutString(ForwardDestinations.PhonesKey, phonesJson);
                editor?.Apply();
                logging?.LogInfo($"Destinos guardados: {Items.Count}");
            }
            catch (Exception ex)
            {
                logging?.LogError($"Error al guardar en preferencias de Android: {ex.Message}");
            }
#endif
        }

        /// <summary>Que le llega a este destino, en una frase para la lista y para su pantalla.</summary>
        public static string Describe(ForwardDestination destination, ILocalizationService localization)
        {
            if (destination.ForwardsEverything)
                return localization.GetString("destination.summary_all");
            var parts = new List<string>();
            if (destination.Senders.Count > 0)
                parts.Add(string.Format(localization.GetString("destination.summary_senders"), destination.Senders.Count));
            if (destination.Keywords.Count > 0)
                parts.Add(string.Format(localization.GetString("destination.summary_keywords"), destination.Keywords.Count));
            return string.Join(" · ", parts);
        }
    }
}
