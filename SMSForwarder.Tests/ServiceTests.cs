using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using SMSForwarder.Models;
using SMSForwarder.Services;

namespace SMSForwarder.Tests;

/// <summary>Las pruebas que tocan estado estatico de la app (DestinationStore, cultura) van en fila.</summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class AppStateCollection
{
    public const string Name = "Estado de la app";
}

[Collection(AppStateCollection.Name)]
public sealed partial class LocalizationServiceTests : IDisposable
{
    private readonly CultureInfo _culture = CultureInfo.CurrentUICulture;

    public LocalizationServiceTests() => Preferences.Reset();

    public void Dispose() => CultureInfo.CurrentUICulture = _culture;

    private static Dictionary<string, Dictionary<string, string>> Tables(LocalizationService service) =>
        (Dictionary<string, Dictionary<string, string>>)typeof(LocalizationService)
            .GetField("_strings", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(service)!;

    [Fact]
    public void Castellano_e_ingles_tienen_las_mismas_claves_y_los_mismos_huecos()
    {
        var tables = Tables(new LocalizationService());
        var es = tables["es-ES"];
        var en = tables["en-US"];

        Assert.Equal(2, tables.Count);
        Assert.Empty(es.Keys.Except(en.Keys));
        Assert.Empty(en.Keys.Except(es.Keys));
        foreach (var (key, value) in en)
        {
            Assert.False(string.IsNullOrWhiteSpace(value), $"en:{key} vacio");
            Assert.False(string.IsNullOrWhiteSpace(es[key]), $"es:{key} vacio");
            Assert.True(Holes(value).SetEquals(Holes(es[key])), $"{key}: huecos distintos");
        }
    }

    private static HashSet<string> Holes(string text) => Hole().Matches(text).Select(m => m.Value).ToHashSet();

    [GeneratedRegex(@"\{\d+[^}]*\}")]
    private static partial Regex Hole();

    [Theory]
    [InlineData("es-ES", "es-ES")]
    [InlineData("es-MX", "es-ES")]
    [InlineData("en-GB", "en-US")]
    [InlineData("fr-FR", "en-US")]
    public void Sin_idioma_guardado_sigue_al_sistema(string system, string expected)
    {
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(system);
        var service = new LocalizationService();
        service.Initialize();
        Assert.Equal(expected, service.CurrentLanguage);
    }

    [Fact]
    public void El_idioma_guardado_manda_y_uno_desconocido_se_ignora()
    {
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        Preferences.Default.Set("app_language", "es-ES");
        var service = new LocalizationService();
        service.Initialize();
        Assert.Equal("es-ES", service.CurrentLanguage);

        Preferences.Default.Set("app_language", "xx-XX");
        var other = new LocalizationService();
        other.Initialize();
        Assert.Equal("en-US", other.CurrentLanguage);
    }

    [Fact]
    public void Cambiar_de_idioma_avisa_y_se_guarda()
    {
        var service = new LocalizationService();
        var changes = 0;
        service.LanguageChanged += (_, _) => changes++;

        service.SetLanguage("es-ES");
        service.SetLanguage("es-ES");   // el mismo: no avisa
        service.SetLanguage("xx-XX");   // desconocido: no cambia
        Assert.Equal(1, changes);
        Assert.Equal("es-ES", service.CurrentLanguage);
        Assert.Equal("es-ES", Preferences.Default.Get("app_language", ""));

        var es = Tables(service)["es-ES"];
        var key = es.Keys.First();
        Assert.Equal(es[key], service.GetString(key));
    }

    [Fact]
    public void Clave_sin_traducir_cae_al_ingles_y_desconocida_devuelve_la_clave()
    {
        var service = new LocalizationService();
        service.SetLanguage("es-ES");
        var tables = Tables(service);
        var key = tables["en-US"].Keys.First();
        var saved = tables["es-ES"][key];
        tables["es-ES"].Remove(key);

        Assert.Equal(tables["en-US"][key], service.GetString(key));
        Assert.Equal("no.existe", service.GetString("no.existe"));
        tables["es-ES"][key] = saved;
    }
}

public sealed class LoggingServiceTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), "smsf-tests-" + Guid.NewGuid().ToString("N"));

    public LoggingServiceTests()
    {
        Directory.CreateDirectory(_folder);
        FileSystem.AppDataDirectory = _folder;
    }

    public void Dispose()
    {
        try { Directory.Delete(_folder, true); } catch (IOException) { }
    }

    [Fact]
    public void Sin_registro_lo_dice()
    {
        Assert.Equal("No hay logs disponibles.", new LoggingService().GetLogContents());
    }

    [Fact]
    public void Apunta_niveles_y_excepciones()
    {
        var log = new LoggingService();
        log.LogInfo("uno");
        log.LogWarning("dos");
        log.LogError("tres");
        log.LogError("cuatro", new InvalidOperationException("roto"));

        var text = log.GetLogContents();
        Assert.Contains("[INFO] uno", text);
        Assert.Contains("[WARNING] dos", text);
        Assert.Contains("[ERROR] tres", text);
        Assert.Contains("[ERROR] cuatro - Exception: System.InvalidOperationException: roto", text);
    }

    [Fact]
    public void Se_recorta_al_pasar_de_mil_lineas()
    {
        var path = Path.Combine(_folder, "sms_forwarder.log");
        File.WriteAllLines(path, Enumerable.Range(0, 1000).Select(i => $"linea {i}"));

        new LoggingService().LogInfo("nueva");

        var lines = File.ReadAllLines(path);
        Assert.Equal(800, lines.Length);
        Assert.EndsWith("[INFO] nueva", lines[^1]);
        Assert.Equal("linea 201", lines[0]);
    }

    [Fact]
    public void Si_no_puede_escribir_ni_leer_no_revienta()
    {
        var log = new LoggingService();
        log.LogInfo("antes");
        using (File.Open(Path.Combine(_folder, "sms_forwarder.log"), FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            log.LogInfo("se pierde");   // el fichero esta bloqueado
            Assert.Equal("Error al leer los logs.", log.GetLogContents());
        }
        Assert.DoesNotContain("se pierde", log.GetLogContents());
    }
}

[Collection(AppStateCollection.Name)]
public sealed class DestinationStoreTests : IDisposable
{
    public DestinationStoreTests() => Reset();

    public void Dispose() => Reset();

    private static void Reset()
    {
        Preferences.Reset();
        DestinationStore.Items.Clear();
        typeof(DestinationStore).GetField("_loaded", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, false);
    }

    [Fact]
    public void Carga_una_sola_vez_y_guarda_los_dos_formatos()
    {
        Preferences.Default.Set(ForwardDestinations.PhonesKey, """["600112233"]""");
        DestinationStore.Load();
        DestinationStore.Load();   // la segunda no duplica
        Assert.Equal(["600112233"], DestinationStore.Items.Select(d => d.Phone));

        DestinationStore.Items.Add(new ForwardDestination { Phone = "611000111", Keywords = ["pago"] });
        DestinationStore.Save(new FakeLog());

        Assert.Equal("""["600112233","611000111"]""", Preferences.Default.Get(ForwardDestinations.PhonesKey, ""));
        var reloaded = ForwardDestinations.Parse(Preferences.Default.Get(ForwardDestinations.DestinationsKey, ""), null);
        Assert.Equal(["pago"], reloaded[1].Keywords);
    }

    [Fact]
    public void Sin_nada_guardado_empieza_vacio()
    {
        DestinationStore.Load();
        Assert.Empty(DestinationStore.Items);
    }

    [Fact]
    public void Describe_que_le_llega()
    {
        var loc = new LocalizationService();
        loc.SetLanguage("en-US");

        Assert.Equal(loc.GetString("destination.summary_all"),
            DestinationStore.Describe(new ForwardDestination { Phone = "1" }, loc));

        var senders = string.Format(loc.GetString("destination.summary_senders"), 2);
        var keywords = string.Format(loc.GetString("destination.summary_keywords"), 1);
        Assert.Equal(senders, DestinationStore.Describe(new ForwardDestination { Senders = ["a", "b"] }, loc));
        Assert.Equal(keywords, DestinationStore.Describe(new ForwardDestination { Keywords = ["x"] }, loc));
        Assert.Equal($"{senders} · {keywords}",
            DestinationStore.Describe(new ForwardDestination { Senders = ["a", "b"], Keywords = ["x"] }, loc));
    }

    private sealed class FakeLog : ILoggingService
    {
        public void LogInfo(string message) { }
        public void LogError(string message, Exception ex = null!) { }
        public void LogWarning(string message) { }
        public string GetLogContents() => "";
    }
}

public class SmsMessageItemTests
{
    [Fact]
    public void Textos_de_la_fila()
    {
        var item = new SmsMessageItem { Address = " ", Body = new string('x', 120), IsInbox = true };
        Assert.Equal("(desconocido)", item.DisplayAddress);
        Assert.Equal(new string('x', 100) + "…", item.Snippet);
        Assert.Equal("ic_inbox.png", item.DirectionIcon);
        Assert.Equal("", item.DateText);
        Assert.Equal("", item.ListDateText);

        var sent = new SmsMessageItem { Address = "600", Body = "hola", IsInbox = false };
        Assert.Equal("600", sent.DisplayAddress);
        Assert.Equal("hola", sent.Snippet);
        Assert.Equal("ic_outbox.png", sent.DirectionIcon);
    }

    [Fact]
    public void Fechas_cortas_segun_lo_reciente()
    {
        var today = DateTime.Today.AddHours(9).AddMinutes(5);
        Assert.Equal("09:05", new SmsMessageItem { Date = today }.ListDateText);
        Assert.Equal(today.ToString("dd/MM/yyyy HH:mm"), new SmsMessageItem { Date = today }.DateText);

        // Otro dia del mismo año (si hoy es 1 de enero, se usa el 2; la rama es la misma).
        var sameYear = DateTime.Today.DayOfYear > 1 ? DateTime.Today.AddDays(-1) : DateTime.Today.AddDays(1);
        Assert.Equal(sameYear.ToString("dd/MM"), new SmsMessageItem { Date = sameYear }.ListDateText);

        var old = new DateTime(2020, 3, 4, 10, 0, 0);
        Assert.Equal(old.ToString("dd/MM/yy"), new SmsMessageItem { Date = old }.ListDateText);
    }

    [Fact]
    public void Marcar_avisa_solo_si_cambia()
    {
        var item = new SmsMessageItem();
        var changes = new List<string?>();
        item.PropertyChanged += (_, e) => changes.Add(e.PropertyName);

        item.IsSelected = true;
        item.IsSelected = true;
        item.IsSelected = false;

        Assert.Equal([nameof(SmsMessageItem.IsSelected), nameof(SmsMessageItem.IsSelected)], changes);
        Assert.False(item.IsSelected);
        Assert.Equal(0, item.Id);
        Assert.False(item.IsRead);
    }
}

public class DefaultRolePromptTests
{
    [Fact]
    public async Task Fuera_de_Android_no_hay_nada_que_esperar()
    {
        Assert.True(DefaultRolePrompt.Done.IsCompleted);
        DefaultRolePrompt.MarkDone();   // repetir no revienta
        await DefaultRolePrompt.Done;
    }
}
