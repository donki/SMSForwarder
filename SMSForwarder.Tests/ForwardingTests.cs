using SMSForwarder.Models;
using SMSForwarder.Services;

namespace SMSForwarder.Tests;

public class PhoneNumbersTests
{
    [Theory]
    [InlineData(" +34 (600) 11-22.33 ", "34600112233")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData(null, "")]
    [InlineData("BBVA", "BBVA")]
    public void Clean_quita_separadores(string? input, string expected) =>
        Assert.Equal(expected, PhoneNumbers.Clean(input!));

    [Theory]
    [InlineData("+34600112233", "600 112 233", true)]       // con y sin prefijo de pais
    [InlineData("0034600112233", "600112233", true)]
    [InlineData("600112233", "600112234", false)]
    [InlineData("12345", "12345", true)]                    // cortos: solo enteros
    [InlineData("12345", "912345", false)]
    [InlineData("12345678", "912345678", false)]            // menos de 9 digitos en comun
    [InlineData("BBVA", "BBVA", true)]
    [InlineData("bbva", "BBVA", true)]                      // fallo arreglado: los remitentes con nombre
    [InlineData("Amazon", "AMAZON", true)]
    [InlineData("", "600112233", false)]
    [InlineData("600112233", " ", false)]
    public void AreEqual(string a, string b, bool expected)
    {
        Assert.Equal(expected, PhoneNumbers.AreEqual(a, b));
        Assert.Equal(expected, PhoneNumbers.AreEqual(b, a));
    }

    [Theory]
    [InlineData("600112233", true)]
    [InlineData("+34 600-11-22-33", true)]
    [InlineData("(600) 112233", true)]
    [InlineData("1234567", true)]              // 7 digitos
    [InlineData("123456789012345", true)]      // 15 digitos
    [InlineData("123456", false)]              // 6
    [InlineData("1234567890123456", false)]    // 16
    [InlineData("0600112233", false)]          // empieza por 0
    [InlineData("60011a233", false)]
    [InlineData("++34600112233", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValid(string? number, bool expected) => Assert.Equal(expected, PhoneNumbers.IsValid(number!));
}

public class ForwardDestinationTests
{
    [Fact]
    public void Sin_filtros_le_llega_todo()
    {
        var destination = new ForwardDestination { Phone = "600112233" };
        Assert.True(destination.ForwardsEverything);
        Assert.True(destination.Matches("cualquiera", "lo que sea"));
        Assert.True(destination.Matches("", ""));
    }

    [Fact]
    public void Solo_de_ciertos_remitentes()
    {
        var destination = new ForwardDestination { Senders = ["+34 611 000 111", "BBVA"] };
        Assert.False(destination.ForwardsEverything);
        Assert.True(destination.Matches("611000111", "hola"));
        Assert.True(destination.Matches("bbva", "hola"));
        Assert.False(destination.Matches("622000111", "hola"));
    }

    [Fact]
    public void Solo_con_ciertas_palabras_sin_mirar_mayusculas_ni_acentos()
    {
        var destination = new ForwardDestination { Keywords = ["PAGO", "código de acceso"] };
        Assert.False(destination.ForwardsEverything);
        Assert.True(destination.Matches("x", "Se pagó la factura"));
        Assert.True(destination.Matches("x", "Tu CODIGO DE ACCESO es 1234"));
        Assert.False(destination.Matches("x", "Tu codigo es 1234"));
        Assert.False(destination.Matches("x", ""));
    }

    [Fact]
    public void Con_las_dos_listas_hay_que_cumplir_las_dos()
    {
        var destination = new ForwardDestination { Senders = ["BBVA"], Keywords = ["compra"] };
        Assert.True(destination.Matches("BBVA", "Compra de 20 EUR"));
        Assert.False(destination.Matches("BBVA", "Transferencia"));
        Assert.False(destination.Matches("Otro", "Compra de 20 EUR"));
    }

    [Theory]
    [InlineData("texto", "", true)]
    [InlineData("texto", "   ", true)]
    [InlineData("", "a", false)]
    [InlineData("Ñandú feliz", "nandu", true)]
    [InlineData("  camión  ", "CAMION", true)]
    public void ContainsText(string body, string needle, bool expected) =>
        Assert.Equal(expected, ForwardDestination.ContainsText(body, needle));

    [Fact]
    public void Summary_avisa_solo_si_cambia()
    {
        var destination = new ForwardDestination();
        var changes = new List<string?>();
        destination.PropertyChanged += (_, e) => changes.Add(e.PropertyName);

        destination.Summary = "a";
        destination.Summary = "a";
        destination.Summary = "b";

        Assert.Equal("b", destination.Summary);
        Assert.Equal([nameof(ForwardDestination.Summary), nameof(ForwardDestination.Summary)], changes);
    }
}

public class ForwardDestinationsTests
{
    [Fact]
    public void Ida_y_vuelta_con_filtros()
    {
        var original = new[]
        {
            new ForwardDestination { Phone = "600112233", Senders = ["BBVA"], Keywords = ["pago"] },
            new ForwardDestination { Phone = "611000111" },
        };

        var json = ForwardDestinations.ToJson(original);
        Assert.DoesNotContain("Summary", json);
        Assert.DoesNotContain("ForwardsEverything", json);

        var parsed = ForwardDestinations.Parse(json, null);

        Assert.Equal(["600112233", "611000111"], parsed.Select(d => d.Phone));
        Assert.Equal(["BBVA"], parsed[0].Senders);
        Assert.Equal(["pago"], parsed[0].Keywords);
        Assert.True(parsed[1].ForwardsEverything);
        Assert.Equal("""["600112233","611000111"]""", ForwardDestinations.ToPhonesJson(original));
    }

    [Fact]
    public void Descarta_destinos_sin_numero_y_acepta_mayusculas_distintas()
    {
        var parsed = ForwardDestinations.Parse("""[{"phone":"600112233"},{"Phone":"  "},{"PHONE":"611"}]""", null);
        Assert.Equal(["600112233", "611"], parsed.Select(d => d.Phone));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("roto")]
    [InlineData("null")]
    public void Formato_viejo_cuando_no_hay_destinos_legibles(string? destinations)
    {
        var parsed = ForwardDestinations.Parse(destinations, """["600112233", " ", "611000111"]""");
        Assert.Equal(["600112233", "611000111"], parsed.Select(d => d.Phone));
        Assert.All(parsed, d => Assert.True(d.ForwardsEverything));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("roto", "roto")]
    [InlineData("null", "null")]
    public void Nada_legible_es_lista_vacia(string? destinations, string? phones) =>
        Assert.Empty(ForwardDestinations.Parse(destinations, phones));
}

public class ForwardingRulesTests
{
    private static readonly List<ForwardDestination> Destinations =
    [
        new() { Phone = "600112233" },
        new() { Phone = "611000111", Senders = ["BBVA"] },
        new() { Phone = "622000111", Keywords = ["urgente"] },
    ];

    [Fact]
    public void Sin_filtros_va_a_todos_los_que_casan()
    {
        var (decision, targets) = ForwardingRules.Decide(Destinations, "BBVA", "Compra de 20 EUR");
        Assert.Equal(ForwardDecision.Forward, decision);
        Assert.Equal(["600112233", "611000111"], targets.Select(d => d.Phone));

        (decision, targets) = ForwardingRules.Decide(Destinations, "Amazon", "Pedido URGENTE");
        Assert.Equal(["600112233", "622000111"], targets.Select(d => d.Phone));
    }

    [Fact]
    public void Mensaje_vacio_o_sin_destinos()
    {
        Assert.Equal(ForwardDecision.EmptyMessage, ForwardingRules.Decide(Destinations, "x", "").Decision);
        Assert.Equal(ForwardDecision.EmptyMessage, ForwardingRules.Decide(Destinations, "x", null!).Decision);
        Assert.Equal(ForwardDecision.NoDestinations, ForwardingRules.Decide([], "x", "hola").Decision);
    }

    [Fact]
    public void No_reenvia_lo_que_manda_un_numero_de_reenvio()
    {
        var (decision, targets) = ForwardingRules.Decide(Destinations, "+34 600 11 22 33", "hola");
        Assert.Equal(ForwardDecision.FromForwardingNumber, decision);
        Assert.Empty(targets);
    }

    [Fact]
    public void No_reenvia_un_reenvio()
    {
        Assert.Equal(ForwardDecision.AlreadyForwarded,
            ForwardingRules.Decide(Destinations, "699", "[SMSForwarder] De: 655\nhola").Decision);
    }

    [Fact]
    public void Ningun_destino_casa()
    {
        List<ForwardDestination> onlyBank = [new() { Phone = "611000111", Senders = ["BBVA"] }, new() { Phone = " " }];
        var (decision, targets) = ForwardingRules.Decide(onlyBank, "Amazon", "Pedido");
        Assert.Equal(ForwardDecision.NoDestinationMatches, decision);
        Assert.Empty(targets);
    }

    [Theory]
    [InlineData("[SMSForwarder] De: 600\nhola", true)]
    [InlineData("  [smsforwarder] algo", true)]
    [InlineData("De: 600112233 hola", true)]
    [InlineData("from: someone", true)]
    [InlineData("Reenviado: algo", true)]
    [InlineData("Forwarded: x", true)]
    [InlineData("SMS de: 600", true)]
    [InlineData("Compra de: 23,00 EUR en MERCADONA", false)]   // fallo arreglado: se tragaba avisos del banco
    [InlineData("Codigo de: acceso 1234", false)]
    [InlineData("Message from: Google", false)]
    [InlineData("Hola, que tal", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsForwardedMessage(string body, bool expected) =>
        Assert.Equal(expected, ForwardingRules.IsForwardedMessage(body));

    [Fact]
    public void Mensaje_corto_se_reenvia_entero()
    {
        Assert.Equal("[SMSForwarder] De: BBVA\nCompra de 20 EUR", ForwardingRules.BuildMessage("BBVA", "Compra de 20 EUR"));
        var exact = new string('x', ForwardingRules.SmsLength - ForwardingRules.Prefix.Length - "600".Length - 1);
        Assert.Equal(ForwardingRules.SmsLength, ForwardingRules.BuildMessage("600", exact).Length);
        Assert.EndsWith(exact, ForwardingRules.BuildMessage("600", exact));
    }

    [Fact]
    public void Mensaje_largo_se_recorta_para_caber_en_un_sms()
    {
        var body = new string('a', 300);
        var message = ForwardingRules.BuildMessage("+34600112233", body);

        Assert.True(message.Length <= ForwardingRules.SmsLength);
        Assert.StartsWith("[SMSForwarder] De: +34600112233\naaa", message);
        Assert.EndsWith("...", message);
    }

    [Fact]
    public void Remitente_enorme_no_revienta()
    {
        var sender = new string('9', 200);
        var message = ForwardingRules.BuildMessage(sender, "hola");
        Assert.Equal($"[SMSForwarder] De: {sender}\n...", message);
    }

    [Fact]
    public void Un_caracter_de_mas_ya_se_recorta()
    {
        var sender = "600";
        var fits = ForwardingRules.SmsLength - ForwardingRules.Prefix.Length - sender.Length - 1;
        var message = ForwardingRules.BuildMessage(sender, new string('b', fits + 1));
        Assert.Equal($"{ForwardingRules.Prefix}{sender}\n{new string('b', fits - 3)}...", message);
        Assert.Equal(ForwardingRules.SmsLength, message.Length);
    }
}

public class DuplicateFilterTests
{
    [Fact]
    public void El_mismo_sms_seguido_es_duplicado()
    {
        var filter = new DuplicateFilter(TimeSpan.FromSeconds(5));
        var t0 = new DateTime(2026, 9, 30, 10, 0, 0);

        Assert.False(filter.IsDuplicate("600", "hola", t0));
        Assert.True(filter.IsDuplicate("600", "hola", t0.AddSeconds(4.9)));
        Assert.False(filter.IsDuplicate("600", "hola", t0.AddSeconds(5)));   // pasado el plazo
        Assert.False(filter.IsDuplicate("601", "hola", t0.AddSeconds(6)));   // otro remitente
        Assert.False(filter.IsDuplicate("601", "adios", t0.AddSeconds(7)));  // otro texto
        Assert.True(filter.IsDuplicate("601", "adios", t0.AddSeconds(8)));
    }
}
