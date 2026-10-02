using SMSForwarder.Models;
using SMSForwarder.Pages;
using SMSForwarder.Services;

namespace SMSForwarder.Tests;

/// <summary>Las pantallas con su XAML real, sin dispositivo: textos, botones, navegacion y avisos.</summary>
public sealed class SettingsPageTests : IDisposable
{
    private readonly AppHarness _h = new();

    public void Dispose() => _h.Dispose();

    private MainPage NewPage() => new(_h.Logging, _h.Contacts, _h.Localization);

    private static void Add(MainPage page, string number)
    {
        Ui.Field<Entry>(page, "PhoneEntry").Text = number;
        Ui.Call(page, "OnAddClicked", null, EventArgs.Empty);
    }

    [Fact]
    public void Texts_AndLanguageButtons_FollowTheLanguage()
    {
        var page = NewPage();
        Assert.Equal(_h.Localization.GetString("main.title"), Ui.Field<Label>(page, "TitleLabel").Text);
        Assert.StartsWith("• Received SMS", Ui.Field<Label>(page, "InfoText").Text);

        Ui.Field<Button>(page, "SpanishButton").SendClicked();

        Assert.Equal("es-ES", _h.Localization.CurrentLanguage);
        Assert.Equal("Configuración", Ui.Field<Label>(page, "TitleLabel").Text);
        Assert.StartsWith("• Los SMS recibidos", Ui.Field<Label>(page, "InfoText").Text);
        Ui.Field<Button>(page, "SpanishButton").SendClicked();   // ya esta: nada
        Ui.Field<Button>(page, "EnglishButton").SendClicked();
        Assert.Equal("en-US", _h.Localization.CurrentLanguage);
    }

    [Fact]
    public void AddingNumbers_ValidatesThemAndSkipsDuplicates()
    {
        var page = NewPage();

        Add(page, "600 111 222");
        Add(page, "600111222");
        Add(page, "123");
        Add(page, "   ");

        Assert.Equal(new[] { "600111222" }, DestinationStore.Items.Select(d => d.Phone));
        Assert.Equal(new[] { "Duplicate number", "Invalid number" }, _h.Alerts.Select(a => a.Title));
        Assert.False(string.IsNullOrEmpty(DestinationStore.Items[0].Summary));

        Assert.Contains("600111222", Preferences.Default.Get(ForwardDestinations.PhonesKey, ""));   // guardado
    }

    [Fact]
    public void Spanish_Alerts_AreInSpanish()
    {
        _h.Localization.SetLanguage("es-ES");
        var page = NewPage();

        Add(page, "600111222");
        Add(page, "600111222");
        Add(page, "1");

        Assert.Equal(new[] { "Número duplicado", "Número no válido" }, _h.Alerts.Select(a => a.Title));
    }

    [Fact]
    public async Task Delete_AsksFirst()
    {
        var page = NewPage();
        Add(page, "600111222");
        var button = new ImageButton { CommandParameter = DestinationStore.Items[0] };

        Ui.Call(page, "OnDeleteClicked", button, EventArgs.Empty);   // dice que no
        await Task.Delay(10);
        Assert.Single(DestinationStore.Items);

        _h.AlertAnswers.Enqueue(true);
        Ui.Call(page, "OnDeleteClicked", button, EventArgs.Empty);
        await Ui.Until(() => DestinationStore.Items.Count == 0);
        Ui.Call(page, "OnDeleteClicked", new ImageButton(), EventArgs.Empty);   // sin destino: nada
        Assert.Equal(2, _h.Alerts.Count);
    }

    [Fact]
    public async Task Contacts_AddTheChosenNumber_OrExplain()
    {
        var page = NewPage();

        _h.Contacts.Number = "+34 600 111 222";
        Ui.Call(page, "OnSelectFromContactsClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 1);
        Assert.Equal("Number added", _h.Alerts[0].Title);
        Assert.Equal("+34600111222", DestinationStore.Items[0].Phone);

        Ui.Call(page, "OnSelectFromContactsClicked", null, EventArgs.Empty);   // otra vez: duplicado
        _h.Contacts.Number = "12";
        Ui.Call(page, "OnSelectFromContactsClicked", null, EventArgs.Empty);
        _h.Contacts.Number = null;                                              // cancelado
        Ui.Call(page, "OnSelectFromContactsClicked", null, EventArgs.Empty);
        _h.Contacts.Throws = new InvalidOperationException("sin agenda");
        Ui.Call(page, "OnSelectFromContactsClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 4);

        Assert.Equal(new[] { "Number added", "Duplicate number", "Invalid number", "Error" }, _h.Alerts.Select(a => a.Title));
    }

    [Fact]
    public async Task Contacts_InSpanish()
    {
        _h.Localization.SetLanguage("es-ES");
        var page = NewPage();

        foreach (var number in new[] { "600111222", "600111222", "12" })
        {
            _h.Contacts.Number = number;
            Ui.Call(page, "OnSelectFromContactsClicked", null, EventArgs.Empty);
        }
        _h.Contacts.Throws = new InvalidOperationException();
        Ui.Call(page, "OnSelectFromContactsClicked", null, EventArgs.Empty);

        await Ui.Until(() => _h.Alerts.Count == 4);
        Assert.Equal(new[] { "Número agregado", "Número duplicado", "Número no válido", "Error" }, _h.Alerts.Select(a => a.Title));
    }

    [Fact]
    public async Task TappingANumber_OpensItsFilters()
    {
        var page = NewPage();
        Add(page, "600111222");

        Ui.Call(page, "OnDestinationTapped", new Grid { BindingContext = DestinationStore.Items[0] }, new TappedEventArgs(null));
        Ui.Call(page, "OnDestinationTapped", new Grid(), new TappedEventArgs(null));
        await Task.Delay(10);

        var (route, parameters) = Assert.Single(_h.Navigations);
        Assert.Equal(nameof(DestinationPage), route);
        Assert.Same(DestinationStore.Items[0], parameters!["destination"]);
    }

    [Fact]
    public void Appearing_RefreshesTheSummaries_AndSelectionIsCleared()
    {
        var page = NewPage();
        Add(page, "600111222");
        DestinationStore.Items[0].Summary = "viejo";

        Ui.Call(page, "OnAppearing");
        Ui.Call(page, "OnDisappearing");
        var list = Ui.Field<CollectionView>(page, "PhoneList");
        list.SelectedItem = DestinationStore.Items[0];
        Ui.Call(page, "OnItemSelected", list, Ui.Selection());

        Assert.NotEqual("viejo", DestinationStore.Items[0].Summary);
        Assert.Null(list.SelectedItem);
    }
}

public sealed class MessagesPageTests : IDisposable
{
    private readonly AppHarness _h = new();

    public void Dispose() => _h.Dispose();

    private static SmsMessageItem Sms(long id, string from = "600111222", bool read = true, bool inbox = true) =>
        new() { Id = id, Address = from, Body = $"texto {id}", Date = DateTime.Today, IsRead = read, IsInbox = inbox };

    private async Task<MessagesPage> OpenAsync()
    {
        var page = new MessagesPage(_h.Store, _h.Localization);
        Ui.Call(page, "OnAppearing");
        await Task.Delay(20);
        return page;
    }

    private static System.Collections.ObjectModel.ObservableCollection<SmsMessageItem> Messages(MessagesPage page) =>
        Ui.Field<System.Collections.ObjectModel.ObservableCollection<SmsMessageItem>>(page, "_messages");

    [Fact]
    public async Task Inbox_AndSent_AreListed()
    {
        _h.Store.Inbox.AddRange([Sms(1), Sms(2)]);
        _h.Store.Sent.Add(Sms(3, inbox: false));
        var page = await OpenAsync();

        Assert.Equal(2, Messages(page).Count);
        Assert.Equal(_h.Localization.GetString("messages.title"), page.Title);

        Ui.Call(page, "OnSentClicked", null, EventArgs.Empty);
        await Ui.Until(() => Messages(page).Count == 1);
        Assert.Equal(_h.Localization.GetString("messages.empty_sent_hint"), Ui.Field<Label>(page, "EmptyHintLabel").Text);
        Ui.Call(page, "OnSentClicked", null, EventArgs.Empty);   // ya esta
        Ui.Call(page, "OnInboxClicked", null, EventArgs.Empty);
        await Ui.Until(() => Messages(page).Count == 2);
        Ui.Call(page, "OnRefreshClicked", null, EventArgs.Empty);

        _h.Localization.SetLanguage("es-ES");
        Assert.Equal("Mensajes", page.Title);
    }

    [Fact]
    public async Task WithoutPermission_TheListIsEmpty_AndErrorsAreShown()
    {
        _h.Store.Inbox.Add(Sms(1));
        _h.Permissions[typeof(Permissions.Sms)] = PermissionStatus.Denied;
        var page = await OpenAsync();
        Assert.Empty(Messages(page));
        Assert.Contains(typeof(Permissions.Sms), _h.Requested);

        _h.Permissions.Clear();
        _h.Store.Throws = new InvalidOperationException("proveedor roto");
        Ui.Call(page, "OnRefreshClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 1);
        Assert.Contains("proveedor roto", _h.Alerts[0].Message);
    }

    [Fact]
    public async Task DefaultAppBanner_ShowsUntilTheAppIsTheDefault()
    {
        _h.Store.IsDefaultSmsApp = false;
        var page = await OpenAsync();
        Assert.True(Ui.Field<VisualElement>(page, "DefaultAppBanner").IsVisible);

        Ui.Call(page, "OnMakeDefaultClicked", null, EventArgs.Empty);
        await Ui.Until(() => !Ui.Field<VisualElement>(page, "DefaultAppBanner").IsVisible);

        _h.Store.Throws = new InvalidOperationException();
        Ui.Call(page, "OnMakeDefaultClicked", null, EventArgs.Empty);
        Assert.Equal(2, _h.Store.DefaultRequests);
    }

    [Fact]
    public async Task OpeningAMessage_MarksItRead_AndShowsTheDetail()
    {
        var unread = Sms(1, read: false);
        _h.Store.Inbox.Add(unread);
        var page = await OpenAsync();

        Ui.Call(page, "OnMessageSelected", null, Ui.Selection(unread));
        Ui.Call(page, "OnMessageSelected", null, Ui.Selection());
        await Ui.Until(() => _h.Navigations.Count == 1);

        Assert.True(unread.IsRead);
        Assert.Equal(nameof(MessageDetailPage), _h.Navigations[0].Route);
        Assert.Same(unread, _h.Navigations[0].Parameters!["message"]);
    }

    [Fact]
    public async Task Compose_OpensAnEmptyMessage()
    {
        var page = await OpenAsync();

        Ui.Call(page, "OnComposeClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Navigations.Count == 1);

        Assert.Equal(nameof(ComposePage), _h.Navigations[0].Route);
        Assert.Empty(_h.Navigations[0].Parameters!);
    }

    [Fact]
    public async Task SelectionMode_SelectsAndDeletesSeveral()
    {
        _h.Store.Inbox.AddRange([Sms(1), Sms(2), Sms(3)]);
        var page = await OpenAsync();
        var messages = Messages(page);

        Ui.Call(page, "OnSelectModeClicked", null, EventArgs.Empty);
        Assert.True(page.IsSelecting);
        Assert.False(page.IsNotSelecting);
        Ui.Call(page, "OnMessageSelected", null, Ui.Selection(messages[0]));
        Ui.Call(page, "OnItemCheckedChanged", null, new CheckedChangedEventArgs(true));
        Assert.Equal(string.Format(_h.Localization.GetString("messages.delete_selected_count"), 1), Ui.Field<Button>(page, "DeleteSelectedButton").Text);

        Ui.Call(page, "OnSelectAllClicked", null, EventArgs.Empty);
        Assert.All(messages, m => Assert.True(m.IsSelected));
        Ui.Call(page, "OnSelectAllClicked", null, EventArgs.Empty);   // todo marcado: desmarca
        Assert.All(messages, m => Assert.False(m.IsSelected));
        Ui.Call(page, "OnDeleteSelectedClicked", null, EventArgs.Empty);   // nada marcado: nada

        Ui.Call(page, "OnSelectAllClicked", null, EventArgs.Empty);
        _h.Store.CanDelete = m => m.Id != 2;
        _h.AlertAnswers.Enqueue(true);
        Ui.Call(page, "OnDeleteSelectedClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 2);

        Assert.Single(messages);
        Assert.False(page.IsSelecting);
        Assert.Equal(string.Format(_h.Localization.GetString("messages.delete_partial"), 2, 3), _h.Alerts[1].Message);
    }

    [Fact]
    public async Task DeleteSelected_ErrorsAreCounted()
    {
        _h.Store.Inbox.AddRange([Sms(1), Sms(2)]);
        var page = await OpenAsync();
        Ui.Call(page, "OnSelectModeClicked", null, EventArgs.Empty);
        Ui.Call(page, "OnSelectAllClicked", null, EventArgs.Empty);
        _h.Store.Throws = new InvalidOperationException("bloqueado");
        _h.AlertAnswers.Enqueue(true);

        Ui.Call(page, "OnDeleteSelectedClicked", null, EventArgs.Empty);

        await Ui.Until(() => _h.Alerts.Count == 2);
        Assert.EndsWith("(bloqueado)", _h.Alerts[1].Message);
        Ui.Call(page, "OnSelectModeClicked", null, EventArgs.Empty);
        Ui.Call(page, "OnCancelSelectionClicked", null, EventArgs.Empty);
        page.CancelSelection();
        Assert.False(page.IsSelecting);
    }

    [Fact]
    public async Task DeleteOne_NeedsTheDefaultApp_AndConfirmation()
    {
        var message = Sms(1);
        _h.Store.Inbox.Add(message);
        var page = await OpenAsync();
        var button = new ImageButton { CommandParameter = message };

        _h.Store.IsDefaultSmsApp = false;
        Ui.Call(page, "OnDeleteMessageClicked", button, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 1);
        Assert.Equal(_h.Localization.GetString("messages.delete_needs_default"), _h.Alerts[0].Message);

        _h.Store.IsDefaultSmsApp = true;
        Ui.Call(page, "OnDeleteMessageClicked", button, EventArgs.Empty);   // no confirma
        _h.Store.CanDelete = _ => false;
        _h.AlertAnswers.Enqueue(true);
        Ui.Call(page, "OnDeleteMessageClicked", button, EventArgs.Empty);   // el proveedor no deja
        _h.Store.Throws = new InvalidOperationException("roto");
        _h.AlertAnswers.Enqueue(true);
        Ui.Call(page, "OnDeleteMessageClicked", button, EventArgs.Empty);
        _h.Store.Throws = null;
        _h.Store.CanDelete = _ => true;
        _h.AlertAnswers.Enqueue(true);
        Ui.Call(page, "OnDeleteMessageClicked", button, EventArgs.Empty);
        Ui.Call(page, "OnDeleteMessageClicked", new ImageButton(), EventArgs.Empty);

        await Ui.Until(() => Messages(page).Count == 0);
        Assert.Contains(_h.Alerts, a => a.Message == _h.Localization.GetString("messages.delete_error"));
        Assert.Contains(_h.Alerts, a => a.Message.EndsWith(": roto"));
    }
}

public sealed class MessageDetailPageTests : IDisposable
{
    private readonly AppHarness _h = new();

    public void Dispose() => _h.Dispose();

    private MessageDetailPage Open(string address = "600111222", string body = "Mira www.example.com y https://a.b/c ya")
    {
        var page = new MessageDetailPage(_h.Localization, _h.Logging);
        page.ApplyQueryAttributes(new Dictionary<string, object> { ["message"] = new SmsMessageItem { Address = address, Body = body, Date = new DateTime(2026, 10, 2, 9, 30, 0) } });
        return page;
    }

    [Fact]
    public async Task Shows_TheMessage_WithItsLinks()
    {
        var page = Open();

        Assert.Equal("600111222", Ui.Field<Label>(page, "AddressLabel").Text);
        Assert.Equal("02/10/2026 09:30", Ui.Field<Label>(page, "DateLabel").Text);
        var spans = Ui.Field<Label>(page, "BodyLabel").FormattedText.Spans;
        Assert.Equal(new[] { "Mira ", "www.example.com", " y ", "https://a.b/c", " ya" }, spans.Select(s => s.Text));

        // Tocar un enlace lo abre; un «www.» suelto, con https.
        Ui.Tap((TapGestureRecognizer)spans[1].GestureRecognizers[0]);
        await Ui.Until(() => _h.Launcher.Opened.Count == 1);
        Assert.Equal("https://www.example.com/", _h.Launcher.Opened[0].AbsoluteUri);

        _h.Launcher.Throws = new InvalidOperationException();
        Ui.Tap((TapGestureRecognizer)spans[3].GestureRecognizers[0]);
        await Ui.Until(() => _h.Alerts.Count == 1);
        Assert.Equal(_h.Localization.GetString("messages.link_error"), _h.Alerts[0].Message);
    }

    [Fact]
    public void WithoutSender_OnlyForwardingTheTextIsPossible()
    {
        var page = Open(address: "", body: "codigo 1234");

        Assert.False(Ui.Field<Button>(page, "ReplyButton").IsEnabled);
        Assert.False(Ui.Field<Button>(page, "ForwardRuleButton").IsEnabled);
        Assert.True(Ui.Field<Button>(page, "ForwardButton").IsEnabled);

        page.ApplyQueryAttributes(new Dictionary<string, object>());   // sin mensaje: se queda como estaba
        Assert.Equal("(desconocido)", Ui.Field<Label>(page, "AddressLabel").Text);
    }

    [Fact]
    public async Task Copy_AndReply()
    {
        var page = Open();

        Ui.Call(page, "OnCopyAddressClicked", null, EventArgs.Empty);
        Assert.Equal("600111222", _h.Clipboard.Text);
        Ui.Call(page, "OnCopyBodyClicked", null, EventArgs.Empty);
        Assert.StartsWith("Mira", _h.Clipboard.Text);
        Ui.Call(page, "OnReplyClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Navigations.Count == 1);

        Assert.Equal(2, _h.Alerts.Count);
        Assert.Equal("600111222", _h.Navigations[0].Parameters!["to"]);

        var empty = new MessageDetailPage(_h.Localization, _h.Logging);   // sin mensaje: nada
        Ui.Call(empty, "OnCopyAddressClicked", null, EventArgs.Empty);
        Ui.Call(empty, "OnCopyBodyClicked", null, EventArgs.Empty);
        Ui.Call(empty, "OnReplyClicked", null, EventArgs.Empty);
        Ui.Call(empty, "OnForwardClicked", null, EventArgs.Empty);
        Ui.Call(empty, "OnForwardRuleClicked", null, EventArgs.Empty);
        Assert.Single(_h.Navigations);
    }

    [Fact]
    public async Task ForwardNow_ToAConfiguredNumber_OrAnother()
    {
        DestinationStore.Items.Add(new ForwardDestination { Phone = "699000000" });
        DestinationStore.Save();
        var page = Open(body: "hola");

        _h.Answers.Enqueue("699000000");
        Ui.Call(page, "OnForwardClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Navigations.Count == 1);
        Assert.Equal("699000000", _h.Navigations[0].Parameters!["to"]);
        Assert.Equal(string.Format(_h.Localization.GetString("messages.forward_body"), "600111222", "hola"), _h.Navigations[0].Parameters!["body"]);

        _h.Answers.Enqueue(_h.Localization.GetString("messages.forward_other_recipient"));
        Ui.Call(page, "OnForwardClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Navigations.Count == 2);
        Assert.False(_h.Navigations[1].Parameters!.ContainsKey("to"));

        _h.Answers.Enqueue(null);   // cancelado
        Ui.Call(page, "OnForwardClicked", null, EventArgs.Empty);
        await Task.Delay(10);
        Assert.Equal(2, _h.Navigations.Count);
    }

    [Fact]
    public async Task ForwardNow_WithoutDestinations_GoesStraightToCompose()
    {
        var page = Open(address: "", body: "codigo");

        Ui.Call(page, "OnForwardClicked", null, EventArgs.Empty);

        await Ui.Until(() => _h.Navigations.Count == 1);
        Assert.Equal("codigo", _h.Navigations[0].Parameters!["body"]);
    }

    [Fact]
    public async Task ForwardRule_ToANewNumber_WithAWord()
    {
        var page = Open(address: "BANCO");

        _h.Answers.Enqueue(_h.Localization.GetString("messages.forward_new_number"));
        _h.Answers.Enqueue("699 000 000");
        _h.Answers.Enqueue(_h.Localization.GetString("messages.forward_with_word"));
        _h.Answers.Enqueue(" clave ");
        _h.AlertAnswers.Enqueue(true);   // un numero nuevo lo recibia todo: se avisa de que deja de ser asi
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 2);

        var destination = Assert.Single(DestinationStore.Items);
        Assert.Equal("699000000", destination.Phone);
        Assert.Equal(new[] { "BANCO" }, destination.Senders);
        Assert.Equal(new[] { "clave" }, destination.Keywords);
        Assert.Equal(string.Format(_h.Localization.GetString("messages.forward_done_word"), "699000000", "BANCO", "clave"), _h.Alerts[1].Message);
    }

    [Fact]
    public async Task ForwardRule_ToAnExistingNumber_WarnsThatItStopsGettingEverything()
    {
        DestinationStore.Items.Add(new ForwardDestination { Phone = "699000000" });
        var page = Open(address: "BANCO");

        // Primero dice que no al aviso; luego que si, con todo lo del remitente.
        _h.Answers.Enqueue("699000000");
        _h.Answers.Enqueue(string.Format(_h.Localization.GetString("messages.forward_all_from"), "BANCO"));
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 1);
        Assert.Empty(DestinationStore.Items[0].Senders);

        _h.Answers.Enqueue("699000000");
        _h.Answers.Enqueue(string.Format(_h.Localization.GetString("messages.forward_all_from"), "BANCO"));
        _h.AlertAnswers.Enqueue(true);
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 3);
        Assert.Equal(new[] { "BANCO" }, DestinationStore.Items[0].Senders);
        Assert.Equal(string.Format(_h.Localization.GetString("messages.forward_done"), "699000000", "BANCO"), _h.Alerts[2].Message);

        // Con palabras ya puestas, se dice que tambien se exigen.
        DestinationStore.Items[0].Keywords.Add("pin");
        _h.Answers.Enqueue("699000000");
        _h.Answers.Enqueue(string.Format(_h.Localization.GetString("messages.forward_all_from"), "BANCO"));
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 4);
        Assert.Equal(string.Format(_h.Localization.GetString("messages.forward_done_words"), "699000000", "BANCO", "pin"), _h.Alerts[3].Message);
    }

    [Fact]
    public async Task ForwardRule_CancelledOrInvalid_ChangesNothing()
    {
        var page = Open(address: "BANCO");
        var newNumber = _h.Localization.GetString("messages.forward_new_number");

        _h.Answers.Enqueue(null);                                     // cancela a quien
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        _h.Answers.Enqueue(newNumber); _h.Answers.Enqueue("");        // numero vacio
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        _h.Answers.Enqueue(newNumber); _h.Answers.Enqueue("12");      // numero no valido
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        _h.Answers.Enqueue(newNumber); _h.Answers.Enqueue("699000000"); _h.Answers.Enqueue(null);   // cancela que
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        _h.Answers.Enqueue("699000000"); _h.Answers.Enqueue(_h.Localization.GetString("messages.forward_with_word")); _h.Answers.Enqueue("  ");
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        _h.Answers.Enqueue("otro");                                   // un numero que ya no esta
        Ui.Call(page, "OnForwardRuleClicked", null, EventArgs.Empty);
        await Task.Delay(20);

        Assert.Equal(_h.Localization.GetString("messages.forward_invalid_number"), Assert.Single(_h.Alerts).Message);
        Assert.Empty(DestinationStore.Items.SelectMany(d => d.Senders));
    }
}

public sealed class ComposePageTests : IDisposable
{
    private readonly AppHarness _h = new();

    public void Dispose() => _h.Dispose();

    private ComposePage NewPage() => new(_h.Store, _h.Contacts, _h.Localization);

    [Fact]
    public void Prefilled_ByWhoeverOpensIt_WithTheCounter()
    {
        var page = NewPage();
        Assert.Equal("0/160", Ui.Field<Label>(page, "CounterLabel").Text);

        page.ApplyQueryAttributes(new Dictionary<string, object> { ["to"] = "600111222", ["body"] = new string('x', 200) });

        Assert.Equal("600111222", Ui.Field<Entry>(page, "ToEntry").Text);
        Assert.Equal("200/160 · 2 SMS", Ui.Field<Label>(page, "CounterLabel").Text);
        page.ApplyQueryAttributes(new Dictionary<string, object>());
        _h.Localization.SetLanguage("es-ES");
        Assert.Equal(_h.Localization.GetString("compose.title"), page.Title);
    }

    [Fact]
    public async Task Contact_FillsTheRecipient()
    {
        var page = NewPage();
        _h.Contacts.Number = "600111222";
        Ui.Call(page, "OnPickContactClicked", null, EventArgs.Empty);
        _h.Contacts.Throws = new InvalidOperationException();
        Ui.Call(page, "OnPickContactClicked", null, EventArgs.Empty);
        await Task.Delay(10);

        Assert.Equal("600111222", Ui.Field<Entry>(page, "ToEntry").Text);
    }

    [Fact]
    public async Task Send_ChecksNumberTextAndPermission_ThenSends()
    {
        var page = NewPage();
        var to = Ui.Field<Entry>(page, "ToEntry");
        var body = Ui.Field<Editor>(page, "BodyEditor");

        to.Text = "12";
        Ui.Call(page, "OnSendClicked", null, EventArgs.Empty);
        to.Text = "600 111-222";
        Ui.Call(page, "OnSendClicked", null, EventArgs.Empty);
        body.Text = "hola";
        _h.Permissions[typeof(Permissions.Sms)] = PermissionStatus.Denied;
        Ui.Call(page, "OnSendClicked", null, EventArgs.Empty);
        _h.Permissions.Clear();
        Ui.Call(page, "OnSendClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Navigations.Count == 1);

        Assert.Equal(new[] { "compose.invalid_number", "compose.empty_body", "compose.no_permission", "compose.sent" }
            .Select(_h.Localization.GetString), _h.Alerts.Select(a => a.Message));
        Assert.Equal(("600 111-222", "hola"), _h.Store.SentNow[0]);
        Assert.Equal("..", _h.Navigations[0].Route);
        Assert.Equal(string.Empty, body.Text);
        Assert.True(Ui.Field<Button>(page, "SendButton").IsEnabled);
    }

    [Fact]
    public async Task SendFailures_AreShown()
    {
        var page = NewPage();
        Ui.Field<Entry>(page, "ToEntry").Text = "600111222";
        Ui.Field<Editor>(page, "BodyEditor").Text = "hola";

        _h.Store.SendResult = false;
        Ui.Call(page, "OnSendClicked", null, EventArgs.Empty);
        _h.Store.Throws = new InvalidOperationException("sin red");
        Ui.Call(page, "OnSendClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 2);

        Assert.Equal(_h.Localization.GetString("compose.send_error"), _h.Alerts[0].Message);
        Assert.EndsWith(": sin red", _h.Alerts[1].Message);
    }
}

public sealed class DestinationPageTests : IDisposable
{
    private readonly AppHarness _h = new();

    public void Dispose() => _h.Dispose();

    private (DestinationPage Page, ForwardDestination Destination) Open()
    {
        var destination = new ForwardDestination { Phone = "699000000" };
        DestinationStore.Items.Add(destination);
        var page = new DestinationPage(_h.Localization, _h.Contacts, _h.Logging);
        page.ApplyQueryAttributes(new Dictionary<string, object> { ["destination"] = destination });
        return (page, destination);
    }

    [Fact]
    public void Senders_AndWords_AreAddedRemovedAndSaved()
    {
        var (page, destination) = Open();
        Assert.True(Ui.Field<Switch>(page, "AllSwitch").IsToggled);
        Assert.Equal("699000000", Ui.Field<Label>(page, "PhoneLabel").Text);

        Ui.Field<Switch>(page, "AllSwitch").IsToggled = false;
        Assert.True(Ui.Field<VisualElement>(page, "FiltersPanel").IsVisible);
        Ui.Field<Entry>(page, "SenderEntry").Text = "600 111 222";
        Ui.Call(page, "OnAddSenderClicked", null, EventArgs.Empty);
        Ui.Field<Entry>(page, "SenderEntry").Text = "600111222";
        Ui.Call(page, "OnAddSenderClicked", null, EventArgs.Empty);   // repetido
        Ui.Call(page, "OnAddSenderClicked", null, EventArgs.Empty);   // vacio tras el aviso? no: sigue escrito
        Ui.Field<Entry>(page, "KeywordEntry").Text = "Clave";
        Ui.Call(page, "OnAddKeywordClicked", null, EventArgs.Empty);
        Ui.Field<Entry>(page, "KeywordEntry").Text = "clave";
        Ui.Call(page, "OnAddKeywordClicked", null, EventArgs.Empty);   // repetida sin mayusculas
        Ui.Field<Entry>(page, "KeywordEntry").Text = "  ";
        Ui.Call(page, "OnAddKeywordClicked", null, EventArgs.Empty);

        Assert.Equal(new[] { "600111222" }, destination.Senders);
        Assert.Equal(new[] { "Clave" }, destination.Keywords);
        Assert.Equal(DestinationStore.Describe(destination, _h.Localization), Ui.Field<Label>(page, "SummaryLabel").Text);
        Assert.All(_h.Alerts, a => Assert.Equal(_h.Localization.GetString("destination.duplicate"), a.Message));

        Ui.Call(page, "OnRemoveSenderClicked", new ImageButton { CommandParameter = "600111222" }, EventArgs.Empty);
        Ui.Call(page, "OnRemoveKeywordClicked", new ImageButton { CommandParameter = "Clave" }, EventArgs.Empty);
        Ui.Call(page, "OnRemoveSenderClicked", new ImageButton(), EventArgs.Empty);
        Ui.Call(page, "OnRemoveKeywordClicked", new ImageButton(), EventArgs.Empty);
        Assert.Empty(destination.Senders);
        Assert.Empty(destination.Keywords);
    }

    [Fact]
    public void AllMessages_ClearsTheFilters()
    {
        var (page, destination) = Open();
        Ui.Field<Switch>(page, "AllSwitch").IsToggled = false;
        Ui.Field<Entry>(page, "KeywordEntry").Text = "pin";
        Ui.Call(page, "OnAddKeywordClicked", null, EventArgs.Empty);

        Ui.Field<Switch>(page, "AllSwitch").IsToggled = true;

        Assert.Empty(destination.Keywords);
        Assert.False(Ui.Field<VisualElement>(page, "FiltersPanel").IsVisible);
        page.ApplyQueryAttributes(new Dictionary<string, object>());
    }

    [Fact]
    public async Task Contacts_AddASender()
    {
        var (page, destination) = Open();
        _h.Contacts.Number = "+34 600 111 222";
        Ui.Call(page, "OnPickSenderClicked", null, EventArgs.Empty);
        _h.Contacts.Number = null;
        Ui.Call(page, "OnPickSenderClicked", null, EventArgs.Empty);
        _h.Contacts.Throws = new InvalidOperationException();
        Ui.Call(page, "OnPickSenderClicked", null, EventArgs.Empty);
        await Task.Delay(10);

        Assert.Equal(new[] { "+34600111222" }, destination.Senders);
    }

    [Fact]
    public void WithoutDestination_NothingIsSaved()
    {
        var page = new DestinationPage(_h.Localization, _h.Contacts, _h.Logging);

        Ui.Field<Switch>(page, "AllSwitch").IsToggled = false;
        Ui.Field<Entry>(page, "KeywordEntry").Text = "x";
        Ui.Call(page, "OnAddKeywordClicked", null, EventArgs.Empty);

        Assert.Empty(DestinationStore.Items);
    }
}

public sealed class DiagnosticsPageTests : IDisposable
{
    private readonly AppHarness _h = new();

    public void Dispose() => _h.Dispose();

    private async Task<DiagnosticsPage> OpenAsync()
    {
        var page = new DiagnosticsPage(_h.Logging, _h.Localization);
        await Task.Delay(20);
        return page;
    }

    [Fact]
    public async Task Status_PermissionsNumbersAndLog()
    {
        Preferences.Default.Set("phones", "[\"1\",\"2\"]");
        _h.Permissions[typeof(SmsPermissions.SendSms)] = PermissionStatus.Denied;
        _h.Logging.LogInfo("algo paso");

        var page = await OpenAsync();

        var status = Ui.Field<Label>(page, "PermissionsStatus").Text;
        Assert.Contains(_h.Localization.GetString("diagnostics.granted"), status);
        Assert.Contains(_h.Localization.GetString("diagnostics.denied"), status);
        Assert.Equal(string.Format(_h.Localization.GetString("diagnostics.numbers_count"), 2), Ui.Field<Label>(page, "PhonesCount").Text);
        Assert.Contains("algo paso", Ui.Field<Label>(page, "LogsLabel").Text);

        Preferences.Default.Set("phones", "[\"1\"]");
        _h.Permissions[typeof(SmsPermissions.SendSms)] = PermissionStatus.Unknown;
        _h.Localization.SetLanguage("es-ES");
        await Task.Delay(20);
        Assert.Equal(_h.Localization.GetString("diagnostics.numbers_count_one"), Ui.Field<Label>(page, "PhonesCount").Text);
        Assert.Contains(_h.Localization.GetString("diagnostics.not_decided"), Ui.Field<Label>(page, "PermissionsStatus").Text);
    }

    [Fact]
    public async Task BadData_IsReported()
    {
        Preferences.Default.Set("phones", "no es json");

        await OpenAsync();

        Assert.Equal(_h.Localization.GetString("diagnostics.refresh_error"), Assert.Single(_h.Alerts).Message);
    }

    [Fact]
    public async Task ClearLog_EmptiesIt()
    {
        _h.Logging.LogInfo("algo");
        var page = await OpenAsync();

        Ui.Call(page, "OnClearLogsClicked", null, EventArgs.Empty);
        Ui.Call(page, "OnRefreshClicked", null, EventArgs.Empty);
        await Task.Delay(20);

        Assert.DoesNotContain("] algo", Ui.Field<Label>(page, "LogsLabel").Text);
    }

    [Fact]
    public async Task PermissionButtons_ExplainAndAsk()
    {
        var shell = (AppShell)((Window)((IApplication)_h.App).CreateWindow(null)).Page!;
        var page = await OpenAsync();
        await shell.Navigation.PushAsync(page);

        Ui.Call(page, "OnCheckPermissionsClicked", null, EventArgs.Empty);
        Ui.Call(page, "OnBatteryOptimizationClicked", null, EventArgs.Empty);
        Ui.Call(page, "OnAutostartClicked", null, EventArgs.Empty);
        Ui.Call(page, "OnConfigureAllPermissionsClicked", null, EventArgs.Empty);
        await Task.Delay(50);

        Assert.Contains(_h.Alerts, a => a.Title == _h.Localization.GetString("diagnostics.autostart_title"));
        Assert.Contains(_h.Alerts, a => a.Message == _h.Localization.GetString("diagnostics.all_configured")
                                     || a.Message == _h.Localization.GetString("diagnostics.some_not_configured"));
    }
}

public sealed class AboutAndShellTests : IDisposable
{
    private readonly AppHarness _h = new();

    public void Dispose() => _h.Dispose();

    [Fact]
    public void About_LanguageButtons_ChangeTheWholeApp()
    {
        // Hasta la 2026.10.02.0 estos botones solo cambiaban los textos de Acerca de.
        var page = new AboutPage(_h.Localization);
        Assert.Equal("About", page.Title);
        Assert.Equal("Version 2026.10.02.0", Ui.Field<Label>(page, "VersionLabel").Text);

        Ui.Call(page, "OnSpanishClicked", null, EventArgs.Empty);

        Assert.Equal("es-ES", _h.Localization.CurrentLanguage);
        Assert.Equal("Acerca de", page.Title);
        Ui.Call(page, "OnEnglishClicked", null, EventArgs.Empty);
        Assert.Equal("en-US", _h.Localization.CurrentLanguage);

        _h.Localization.SetLanguage("es-ES");   // cambiado desde otra pantalla
        Assert.Equal("Acerca de", page.Title);
        Assert.NotNull(new AboutPage());
    }

    [Fact]
    public async Task About_Contact_ChooserEmailOrExplains()
    {
        var page = new AboutPage(_h.Localization);

        _h.ChooserResult = true;
        Ui.Call(page, "OnContactEmailClicked", null, EventArgs.Empty);
        _h.ChooserResult = null;
        Ui.Call(page, "OnContactEmailClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Email.Sent is not null);
        Assert.Single(_h.Choosers);
        Assert.Contains("2026.10.02.0", _h.Email.Sent!.Body);

        _h.Email.Throws = new FeatureNotSupportedException();
        Ui.Call(page, "OnContactEmailClicked", null, EventArgs.Empty);
        _h.Email.Throws = new InvalidOperationException("roto");
        Ui.Call(page, "OnContactEmailClicked", null, EventArgs.Empty);
        _h.Localization.SetLanguage("es-ES");
        Ui.Call(page, "OnContactEmailClicked", null, EventArgs.Empty);
        _h.Email.Throws = new FeatureNotSupportedException();
        Ui.Call(page, "OnContactEmailClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Count == 4);

        Assert.Equal("Email client not available on this device", _h.Alerts[0].Message);
        Assert.Contains("roto", _h.Alerts[1].Message);
        Assert.StartsWith("No se pudo abrir", _h.Alerts[2].Message);
        Assert.Equal("Cliente de correo no disponible en este dispositivo", _h.Alerts[3].Message);
    }

    [Fact]
    public void Shell_Back_ClosesTheMenu_LeavesSelection_GoesHome_ThenHides()
    {
        var shell = (AppShell)((Window)((IApplication)_h.App).CreateWindow(null)).Page!;
        Assert.Equal("v2026.10.02.0", Ui.Field<Label>(shell, "FooterVersion").Text);

        shell.FlyoutIsPresented = true;
        Assert.True(shell.SendBackButtonPressed());
        Assert.False(shell.FlyoutIsPresented);

        shell.CurrentItem = Ui.Field<FlyoutItem>(shell, "AboutItem");
        Assert.True(shell.SendBackButtonPressed());
        Assert.Same(Ui.Field<FlyoutItem>(shell, "MessagesItem"), shell.CurrentItem);

        _h.MoveToBackResult = true;
        Assert.True(shell.SendBackButtonPressed());
        Assert.Equal(1, _h.MovedToBack);
    }

    [Fact]
    public void App_And_MauiProgram()
    {
        var window = (Window)((IApplication)_h.App).CreateWindow(null);
        Assert.IsType<AppShell>(window.Page);

        var app = MauiProgram.CreateMauiApp();
        Assert.NotNull(app.Services.GetService(typeof(ILocalizationService)));
    }
}

/// <summary>Los avisos y peticiones de permisos de Diagnosticos (lo que antes se probaba a mano).</summary>
public sealed class PermissionServiceTests : IDisposable
{
    private readonly AppHarness _h = new();

    public PermissionServiceTests() => ((IApplication)_h.App).CreateWindow(null);   // un Shell donde avisar

    public void Dispose() => _h.Dispose();

    private PermissionService NewService() => new(_h.Localization);

    [Fact]
    public async Task AllGranted_IsSaid()
    {
        Assert.True(await NewService().CheckAndRequestAllPermissionsAsync());
        Assert.True(await NewService().CheckBatteryOptimizationStatusAsync());
        await NewService().ShowPermissionStatusAsync();

        Assert.Contains(_h.Alerts, a => a.Title == _h.Localization.GetString("perm.status_title"));
        Assert.DoesNotContain(typeof(SmsPermissions.ReceiveSms), _h.Requested);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Denied_AsksForEach_AndReportsWhatIsMissing(bool accept)
    {
        _h.Permissions[typeof(SmsPermissions.ReceiveSms)] = PermissionStatus.Denied;
        _h.Permissions[typeof(SmsPermissions.SendSms)] = PermissionStatus.Denied;
        _h.Permissions[typeof(SmsPermissions.BatteryOptimizationPermission)] = PermissionStatus.Denied;
        for (var i = 0; i < 5; i++) _h.AlertAnswers.Enqueue(accept);

        var allOk = await NewService().CheckAndRequestAllPermissionsAsync();
        await NewService().ShowPermissionStatusAsync();

        Assert.False(allOk);
        Assert.False(await NewService().CheckBatteryOptimizationStatusAsync());
        Assert.Equal(accept, _h.Requested.Contains(typeof(SmsPermissions.ReceiveSms)));
        Assert.Equal(accept, _h.Requested.Contains(typeof(SmsPermissions.BatteryOptimizationPermission)));
        Assert.Contains(_h.Alerts, a => a.Title == _h.Localization.GetString("perm.sms_title"));
    }

    [Fact]
    public async Task Failures_AreShown_NotThrown()
    {
        AppPlatform.CheckPermission = _ => throw new InvalidOperationException("roto");
        AppPlatform.RequestPermission = _ => throw new InvalidOperationException("roto");
        _h.AlertAnswers.Enqueue(true);

        Assert.False(await NewService().CheckAndRequestAllPermissionsAsync());
        Assert.False(await NewService().CheckBatteryOptimizationStatusAsync());
        await NewService().ShowPermissionStatusAsync();

        Assert.True(_h.Alerts.Count(a => a.Message.Contains("roto")) >= 3);
    }

    [Fact]
    public async Task Diagnostics_BatteryAndAutostart_AskWhenNeeded()
    {
        _h.Permissions[typeof(SmsPermissions.BatteryOptimizationPermission)] = PermissionStatus.Denied;
        _h.AlertAnswers.Enqueue(true);
        var page = new DiagnosticsPage(_h.Logging, _h.Localization);

        Ui.Call(page, "OnBatteryOptimizationClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Requested.Contains(typeof(SmsPermissions.BatteryOptimizationPermission)));

        AppPlatform.RequestPermission = _ => throw new InvalidOperationException("roto");
        Ui.Call(page, "OnAutostartClicked", null, EventArgs.Empty);
        Ui.Call(page, "OnBatteryOptimizationClicked", null, EventArgs.Empty);
        await Ui.Until(() => _h.Alerts.Any(a => a.Message == _h.Localization.GetString("diagnostics.autostart_error")));
        Assert.Contains(_h.Alerts, a => a.Message == _h.Localization.GetString("diagnostics.battery_on"));
    }
}
