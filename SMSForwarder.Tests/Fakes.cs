using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Dispatching;
using SMSForwarder.Models;
using SMSForwarder.Services;

// AppPlatform, DestinationStore y el Shell son estado global: las pruebas van en serie.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SMSForwarder.Tests;

// Dobles de lo que las pantallas piden al dispositivo (Services/AppPlatform.cs) y del buzon y la
// agenda de Android. Registran lo que se les pide y responden lo que diga la prueba.

public sealed class FakeMessageStore : IMessageStore
{
    public bool IsSupported { get; set; } = true;
    public bool IsDefaultSmsApp { get; set; } = true;
    public bool CanBeDefault { get; set; } = true;
    public List<SmsMessageItem> Inbox { get; } = new();
    public List<SmsMessageItem> Sent { get; } = new();
    public List<(string To, string Body)> SentNow { get; } = new();
    public Exception? Throws { get; set; }
    public Func<SmsMessageItem, bool> CanDelete { get; set; } = _ => true;
    public bool SendResult { get; set; } = true;
    public int DefaultRequests { get; private set; }

    public Task<bool> RequestDefaultAsync()
    {
        DefaultRequests++;
        if (Throws is not null) throw Throws;
        IsDefaultSmsApp = true;
        return Task.FromResult(true);
    }

    public Task<List<SmsMessageItem>> GetInboxAsync() => Throws is null ? Task.FromResult(Inbox.ToList()) : throw Throws;
    public Task<List<SmsMessageItem>> GetSentAsync() => Throws is null ? Task.FromResult(Sent.ToList()) : throw Throws;

    public Task<bool> DeleteAsync(SmsMessageItem message)
    {
        if (Throws is not null) throw Throws;
        var ok = CanDelete(message);
        if (ok) { Inbox.Remove(message); Sent.Remove(message); }
        return Task.FromResult(ok);
    }

    public Task<bool> MarkReadAsync(SmsMessageItem message) => Throws is null ? Task.FromResult(true) : throw Throws;

    public Task<bool> SendAsync(string address, string body)
    {
        if (Throws is not null) throw Throws;
        SentNow.Add((address, body));
        return Task.FromResult(SendResult);
    }
}

public sealed class FakeContactPicker : IContactPicker
{
    public string? Number { get; set; }
    public Exception? Throws { get; set; }

    public Task<string?> PickPhoneNumberAsync() => Throws is null ? Task.FromResult(Number) : throw Throws;
}

public sealed class FakeClipboard : IClipboard
{
    public string? Text { get; private set; }
    public bool HasText => Text is not null;
    public event EventHandler<EventArgs>? ClipboardContentChanged { add { } remove { } }
    public Task SetTextAsync(string? text) { Text = text; return Task.CompletedTask; }
    public Task<string?> GetTextAsync() => Task.FromResult(Text);
}

public sealed class FakeLauncher : ILauncher
{
    public Exception? Throws { get; set; }
    public List<Uri> Opened { get; } = new();
    public Task<bool> CanOpenAsync(Uri uri) => Task.FromResult(true);
    public Task<bool> OpenAsync(Uri uri) { if (Throws is not null) throw Throws; Opened.Add(uri); return Task.FromResult(true); }
    public Task<bool> OpenAsync(OpenFileRequest request) => throw new NotSupportedException();
    public Task<bool> TryOpenAsync(Uri uri) => OpenAsync(uri);
}

public sealed class FakeEmail : IEmail
{
    public Exception? Throws { get; set; }
    public EmailMessage? Sent { get; private set; }
    public bool IsComposeSupported => true;
    public Task ComposeAsync(EmailMessage? message) { if (Throws is not null) throw Throws; Sent = message; return Task.CompletedTask; }
}

public sealed class FakeAppInfo : IAppInfo
{
    public string PackageName => "com.socratic.smsforwarder";
    public string Name => "SMS Forwarder";
    public string VersionString => "2026.10.02.0";
    public Version Version => new(2026, 10, 2, 0);
    public string BuildString => "2026100200";
    public void ShowSettingsUI() { }
    public AppTheme RequestedTheme => AppTheme.Light;
    public AppPackagingModel PackagingModel => AppPackagingModel.Packaged;
    public LayoutDirection RequestedLayoutDirection => LayoutDirection.LeftToRight;
}

/// <summary>Un aviso que la app quiso enseñar.</summary>
public sealed record Shown(string Title, string Message, string Accept, string? Cancel);

/// <summary>
/// Estado comun de cada prueba de pantallas: carpeta de datos temporal, preferencias vacias,
/// destinos vacios, idioma elegido, dobles nuevos y la App de verdad (con sus estilos).
/// </summary>
public sealed class AppHarness : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), "sms-tests-" + Guid.NewGuid().ToString("N"));

    public FakeMessageStore Store { get; } = new();
    public FakeContactPicker Contacts { get; } = new();
    public FakeClipboard Clipboard { get; } = new();
    public FakeLauncher Launcher { get; } = new();
    public FakeEmail Email { get; } = new();
    public List<Shown> Alerts { get; } = new();
    public Queue<bool> AlertAnswers { get; } = new();
    public Queue<string?> Answers { get; } = new();
    public List<(string Route, IDictionary<string, object>? Parameters)> Navigations { get; } = new();
    public List<(string Subject, string Body, string Title)> Choosers { get; } = new();
    public Dictionary<Type, PermissionStatus> Permissions { get; } = new();
    public List<Type> Requested { get; } = new();
    public bool? ChooserResult { get; set; }
    public bool MoveToBackResult { get; set; }
    public int MovedToBack { get; private set; }

    public LocalizationService Localization { get; } = new();
    public LoggingService Logging { get; }
    public ServiceProvider Services { get; }
    public App App { get; }

    public AppHarness(string language = "en-US")
    {
        Directory.CreateDirectory(_folder);
        FileSystem.AppDataDirectory = _folder;
        Preferences.Reset();
        DestinationStore.Items.Clear();
        Localization.SetLanguage(language);
        Logging = new LoggingService();

        AppPlatform.Clipboard = Clipboard;
        AppPlatform.Launcher = Launcher;
        AppPlatform.Email = Email;
        AppPlatform.AppInfo = new FakeAppInfo();
        AppPlatform.AlertHandler = (_, title, message, accept, cancel) =>
        {
            Alerts.Add(new Shown(title, message, accept, cancel));
            return Task.FromResult(AlertAnswers.Count > 0 && AlertAnswers.Dequeue());
        };
        AppPlatform.PromptHandler = (_, _, _, _, _, _, _, _) => Task.FromResult(Answers.Count > 0 ? Answers.Dequeue() : null);
        AppPlatform.ActionSheetHandler = (_, _, _, _) => Task.FromResult(Answers.Count > 0 ? Answers.Dequeue() : null);
        AppPlatform.GoToHandler = (route, parameters) => { Navigations.Add((route, parameters)); return Task.CompletedTask; };
        AppPlatform.MainThreadHandler = action => action();
        AppPlatform.CheckPermission = p => Task.FromResult(Permissions.TryGetValue(p.GetType(), out var s) ? s : PermissionStatus.Granted);
        AppPlatform.RequestPermission = p =>
        {
            Requested.Add(p.GetType());
            return Task.FromResult(Permissions.TryGetValue(p.GetType(), out var s) ? s : PermissionStatus.Granted);
        };
        AppPlatform.MoveTaskToBack = () => { MovedToBack++; return MoveToBackResult; };
        AppPlatform.StartEmailChooser = (subject, body, title) =>
        {
            if (ChooserResult is null) return false;
            Choosers.Add((subject, body, title));
            return ChooserResult.Value;
        };

        var services = new ServiceCollection();
        services.AddSingleton<ILocalizationService>(Localization);
        services.AddSingleton<ILoggingService>(Logging);
        services.AddSingleton<IContactPicker>(Contacts);
        services.AddSingleton<IMessageStore>(Store);
        Services = services.BuildServiceProvider();

        App = new App();
        Application.Current = App;
    }

    public void Dispose()
    {
        Application.Current = null;
        AppPlatform.AlertHandler = (_, _, _, _, _) => Task.FromResult(false);
        Services.Dispose();
        DestinationStore.Items.Clear();
        try { Directory.Delete(_folder, recursive: true); } catch (IOException) { }
    }
}

/// <summary>Acceso a lo privado de las paginas (controles con x:Name y manejadores de eventos).</summary>
internal static class Ui
{
    public static T Field<T>(object page, string name) =>
        (T)page.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(page)!;

    public static object? Call(object page, string method, params object?[] args) =>
        page.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name == method && m.GetParameters().Length == args.Length)
            .Invoke(page, args);

    /// <summary>Un cambio de seleccion de CollectionView (su constructor es interno).</summary>
    public static SelectionChangedEventArgs Selection(params object[] current)
    {
        var ctor = typeof(SelectionChangedEventArgs).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .First(c => c.GetParameters().Length == 2 && c.GetParameters()[1].ParameterType != typeof(object)
                        && c.GetParameters()[1].ParameterType.IsAssignableFrom(typeof(List<object>)));
        return (SelectionChangedEventArgs)ctor.Invoke([new List<object>(), current.ToList()]);
    }

    /// <summary>Dispara el gesto de tocar como lo haria el dispositivo.</summary>
    public static void Tap(TapGestureRecognizer tap)
    {
        var handler = (EventHandler<TappedEventArgs>?)typeof(TapGestureRecognizer)
            .GetField("Tapped", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(tap);
        handler?.Invoke(tap, new TappedEventArgs(null));
    }

    public static async Task Until(Func<bool> condition, int timeoutMs = 3000)
    {
        var until = DateTime.UtcNow.AddMilliseconds(timeoutMs);
        while (!condition())
        {
            if (DateTime.UtcNow > until) throw new TimeoutException("La condicion no se cumplio a tiempo.");
            await Task.Delay(10);
        }
    }
}

/// <summary>Hilo principal de mentira: lo que se le manda se ejecuta en el acto.</summary>
public sealed class ImmediateDispatcher : IDispatcher, IDispatcherProvider
{
    [ModuleInitializer]
    internal static void Install() => DispatcherProvider.SetCurrent(new ImmediateDispatcher());

    public IDispatcher? GetForCurrentThread() => this;
    public bool IsDispatchRequired => false;
    public bool Dispatch(Action action) { action(); return true; }
    public bool DispatchDelayed(TimeSpan delay, Action action) => Dispatch(action);
    public IDispatcherTimer CreateTimer() => throw new NotSupportedException();
}
