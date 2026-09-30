// Cuñas de lo que la app toma de Microsoft.Maui.Storage. En las pruebas no hay MAUI: la carpeta de
// datos es una temporal por prueba y las preferencias viven en memoria. Van por flujo asincrono
// (AsyncLocal) para que las pruebas puedan correr en paralelo sin pisarse.
namespace Microsoft.Maui.Storage;

public static class FileSystem
{
    private static readonly AsyncLocal<string?> Current = new();

    public static string AppDataDirectory
    {
        get => Current.Value ?? throw new InvalidOperationException("La prueba no ha preparado su carpeta de datos.");
        set => Current.Value = value;
    }
}

public sealed class Preferences
{
    private static readonly AsyncLocal<Preferences?> Current = new();

    private readonly Dictionary<string, object> _values = new();

    public static Preferences Default => Current.Value ??= new Preferences();

    public static void Reset() => Current.Value = new Preferences();

    public T Get<T>(string key, T defaultValue) =>
        _values.TryGetValue(key, out var value) ? (T)value : defaultValue;

    public void Set<T>(string key, T value) => _values[key] = value!;
}
