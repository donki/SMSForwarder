using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Xaml.Internals;
using SMSForwarder.Models;
using SMSForwarder.Services;
using SocShared;

namespace SMSForwarder;

[XamlFilePath("MessagesPage.xaml")]
public class MessagesPage : ContentPage
{
	private readonly IMessageStore _store;

	private readonly IContactPicker _contactPicker;

	private readonly ObservableCollection<SmsMessageItem> _items = new ObservableCollection<SmsMessageItem>();

	private bool _showingInbox = true;

	private bool _loading;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button InboxButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button SentButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private RefreshView Refresher;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private CollectionView MessagesList;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label EmptyLabel;

	public MessagesPage(IMessageStore store, IContactPicker contactPicker)
	{
		InitializeComponent();
		_store = store;
		_contactPicker = contactPicker;
		((ItemsView)MessagesList).ItemsSource = _items;
	}

	protected override async void OnAppearing()
	{
		_003C_003En__0();
		await EnsureReadPermissionAsync();
		await LoadAsync();
	}

	private async Task<bool> EnsureReadPermissionAsync()
	{
		_ = 1;
		try
		{
			PermissionStatus val = await Permissions.CheckStatusAsync<SmsPermissions.ReadSmsPermission>();
			if ((int)val != 3)
			{
				val = await Permissions.RequestAsync<SmsPermissions.ReadSmsPermission>();
			}
			return (int)val == 3;
		}
		catch
		{
			return false;
		}
	}

	private async Task LoadAsync()
	{
		if (_loading)
		{
			return;
		}
		_loading = true;
		try
		{
			Refresher.IsRefreshing = true;
			EmptyLabel.Text = (_showingInbox ? "No hay mensajes recibidos" : "No hay mensajes enviados");
			List<SmsMessageItem> list = ((!_showingInbox) ? (await _store.GetSentAsync()) : (await _store.GetInboxAsync()));
			_items.Clear();
			foreach (SmsMessageItem item in list)
			{
				_items.Add(item);
			}
		}
		catch (Exception ex)
		{
			await ModernDialog.AlertAsync((Page)(object)this, "Mensajes", "No se pudieron cargar los mensajes: " + ex.Message, "OK");
		}
		finally
		{
			Refresher.IsRefreshing = false;
			_loading = false;
		}
	}

	private void SetSegment(bool inbox)
	{
		_showingInbox = inbox;
		((StyleableElement)InboxButton).Style = (inbox ? GetStyle("PrimaryButton") : GetStyle("OutlineButton"));
		((StyleableElement)SentButton).Style = (inbox ? GetStyle("OutlineButton") : GetStyle("PrimaryButton"));
	}

	private Style GetStyle(string key)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		return (Style)Application.Current.Resources[key];
	}

	private async void OnInboxClicked(object sender, EventArgs e)
	{
		if (_showingInbox)
		{
			await LoadAsync();
			return;
		}
		SetSegment(inbox: true);
		await LoadAsync();
	}

	private async void OnSentClicked(object sender, EventArgs e)
	{
		if (!_showingInbox)
		{
			await LoadAsync();
			return;
		}
		SetSegment(inbox: false);
		await LoadAsync();
	}

	private async void OnRefreshClicked(object sender, EventArgs e)
	{
		await LoadAsync();
	}

	private async void OnPullRefresh(object sender, EventArgs e)
	{
		await LoadAsync();
	}

	private async void OnMessageTapped(object sender, EventArgs e)
	{
		object obj = ((sender is Element) ? sender : null);
		object obj2 = ((obj != null) ? ((BindableObject)obj).BindingContext : null);
		if (obj2 is SmsMessageItem m)
		{
			if (m.IsInbox && !m.IsRead && _store.IsDefaultSmsApp)
			{
				await _store.MarkReadAsync(m);
				m.IsRead = true;
			}
			string title = m.DirectionIcon + " " + m.DisplayAddress;
			string message = (string.IsNullOrEmpty(m.DateText) ? "" : (m.DateText + "\n\n")) + m.Body;
			if (await ModernDialog.AlertAsync((Page)(object)this, title, message, "Borrar", "Cerrar"))
			{
				await DeleteAsync(m);
			}
		}
	}

	private async void OnDeleteInvoked(object sender, EventArgs e)
	{
		object obj = ((sender is MenuItem) ? sender : null);
		if (((obj != null) ? ((MenuItem)obj).CommandParameter : null) is SmsMessageItem m)
		{
			await DeleteAsync(m);
		}
	}

	private async Task DeleteAsync(SmsMessageItem m)
	{
		if (!_store.IsDefaultSmsApp)
		{
			await ModernDialog.AlertAsync((Page)(object)this, "Borrar", "Para borrar mensajes, ve a Configuración y pon SMS Forwarder como app de SMS predeterminada.", "OK");
		}
		else if (await _store.DeleteAsync(m))
		{
			_items.Remove(m);
		}
		else
		{
			await ModernDialog.AlertAsync((Page)(object)this, "Borrar", "No se pudo borrar el mensaje.", "OK");
		}
	}

	private async void OnComposeClicked(object sender, EventArgs e)
	{
		string text = await ModernDialog.ActionSheetAsync((Page)(object)this, "Nuevo mensaje", "Cancelar", "✍\ufe0f Escribir número", "\ud83d\udc65 Elegir contacto");
		string number;
		if (text == "\ud83d\udc65 Elegir contacto")
		{
			try
			{
				number = await _contactPicker.PickPhoneNumberAsync();
			}
			catch (Exception ex)
			{
				await ModernDialog.AlertAsync((Page)(object)this, "Contactos", "No se pudo abrir la agenda: " + ex.Message, "OK");
				return;
			}
		}
		else
		{
			if (!(text == "✍\ufe0f Escribir número"))
			{
				return;
			}
			number = await ModernDialog.PromptAsync((Page)(object)this, "Nuevo mensaje", "Número de destino:", "Siguiente", "Cancelar");
		}
		if (string.IsNullOrWhiteSpace(number))
		{
			return;
		}
		string text2 = await ModernDialog.PromptAsync((Page)(object)this, "Nuevo mensaje", "Texto:", "Enviar", "Cancelar");
		if (!string.IsNullOrEmpty(text2))
		{
			bool ok = await _store.SendAsync(number.Trim(), text2);
			await ModernDialog.AlertAsync((Page)(object)this, "Enviar", ok ? "Mensaje enviado." : "No se pudo enviar el mensaje.", "OK");
			if (ok && !_showingInbox)
			{
				await LoadAsync();
			}
		}
	}

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	[MemberNotNull("InboxButton")]
	[MemberNotNull("SentButton")]
	[MemberNotNull("Refresher")]
	[MemberNotNull("MessagesList")]
	[MemberNotNull("EmptyLabel")]
	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Expected O, but got Unknown
		//IL_03aa: Expected O, but got Unknown
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03f1: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Expected O, but got Unknown
		//IL_04fa: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_0541: Expected O, but got Unknown
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Expected O, but got Unknown
		//IL_0735: Expected O, but got Unknown
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_077c: Expected O, but got Unknown
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		ToolbarItem val = new ToolbarItem();
		ToolbarItem val2 = new ToolbarItem();
		StaticResourceExtension val3 = new StaticResourceExtension();
		Button val4 = new Button();
		StaticResourceExtension val5 = new StaticResourceExtension();
		Button val6 = new Button();
		Grid val7 = new Grid();
		Label val8 = new Label();
		StaticResourceExtension val9 = new StaticResourceExtension();
		Label val10 = new Label();
		VerticalStackLayout val11 = new VerticalStackLayout();
		DataTemplate val12 = new DataTemplate();
		CollectionView val13 = new CollectionView();
		RefreshView val14 = new RefreshView();
		Grid val15 = new Grid();
		MessagesPage messagesPage;
		NameScope val16 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(messagesPage = this))) ?? ((object)new NameScope()));
		NameScope.SetNameScope((BindableObject)(object)messagesPage, (INameScope)(object)val16);
		((Element)val).transientNamescope = (INameScope)(object)val16;
		((Element)val2).transientNamescope = (INameScope)(object)val16;
		((Element)val15).transientNamescope = (INameScope)(object)val16;
		((Element)val7).transientNamescope = (INameScope)(object)val16;
		((Element)val4).transientNamescope = (INameScope)(object)val16;
		((INameScope)val16).RegisterName("InboxButton", (object)val4);
		if (((Element)val4).StyleId == null)
		{
			((Element)val4).StyleId = "InboxButton";
		}
		((Element)val6).transientNamescope = (INameScope)(object)val16;
		((INameScope)val16).RegisterName("SentButton", (object)val6);
		if (((Element)val6).StyleId == null)
		{
			((Element)val6).StyleId = "SentButton";
		}
		((Element)val14).transientNamescope = (INameScope)(object)val16;
		((INameScope)val16).RegisterName("Refresher", (object)val14);
		if (((Element)val14).StyleId == null)
		{
			((Element)val14).StyleId = "Refresher";
		}
		((Element)val13).transientNamescope = (INameScope)(object)val16;
		((INameScope)val16).RegisterName("MessagesList", (object)val13);
		if (((Element)val13).StyleId == null)
		{
			((Element)val13).StyleId = "MessagesList";
		}
		((Element)val11).transientNamescope = (INameScope)(object)val16;
		((Element)val8).transientNamescope = (INameScope)(object)val16;
		((Element)val10).transientNamescope = (INameScope)(object)val16;
		((INameScope)val16).RegisterName("EmptyLabel", (object)val10);
		if (((Element)val10).StyleId == null)
		{
			((Element)val10).StyleId = "EmptyLabel";
		}
		InboxButton = val4;
		SentButton = val6;
		Refresher = val14;
		MessagesList = val13;
		EmptyLabel = val10;
		((BindableObject)messagesPage).SetValue(Page.TitleProperty, (object)"Mensajes");
		((BindableObject)val).SetValue(MenuItem.IconImageSourceProperty, (object)ImageSource.FromFile("refresh.svg"));
		((MenuItem)val).Clicked += messagesPage.OnRefreshClicked;
		((Page)messagesPage).ToolbarItems.Add(val);
		((BindableObject)val2).SetValue(MenuItem.IconImageSourceProperty, (object)ImageSource.FromFile("add.svg"));
		((MenuItem)val2).Clicked += messagesPage.OnComposeClicked;
		((Page)messagesPage).ToolbarItems.Add(val2);
		((BindableObject)val15).SetValue(Grid.RowDefinitionsProperty, (object)new RowDefinitionCollection((RowDefinition[])(object)new RowDefinition[2]
		{
			new RowDefinition(GridLength.Auto),
			new RowDefinition(GridLength.Star)
		}));
		((BindableObject)val15).SetValue(Layout.PaddingProperty, (object)new Thickness(16.0));
		((BindableObject)val15).SetValue(Grid.RowSpacingProperty, (object)12.0);
		((BindableObject)val7).SetValue(Grid.RowProperty, (object)0);
		((BindableObject)val7).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
		{
			new ColumnDefinition(GridLength.Star),
			new ColumnDefinition(GridLength.Star)
		}));
		((BindableObject)val7).SetValue(Grid.ColumnSpacingProperty, (object)8.0);
		((BindableObject)val4).SetValue(Grid.ColumnProperty, (object)0);
		((BindableObject)val4).SetValue(Button.TextProperty, (object)"\ud83d\udce5 Recibidos");
		val4.Clicked += messagesPage.OnInboxClicked;
		val3.Key = "PrimaryButton";
		StaticResourceExtension val17 = new StaticResourceExtension
		{
			Key = "PrimaryButton"
		};
		XamlServiceProvider val18 = new XamlServiceProvider();
		Type? typeFromHandle = typeof(IProvideValueTarget);
		object[] array = new object[0 + 4];
		array[0] = val4;
		array[1] = val7;
		array[2] = val15;
		array[3] = messagesPage;
		SimpleValueTargetProvider val19 = new SimpleValueTargetProvider(array, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val16, val16, val16, val16, val16 }, (object)messagesPage);
		object obj = (object)val19;
		val18.Add(typeFromHandle, (object)val19);
		val18.Add(typeof(IReferenceProvider), obj);
		val18.Add(typeof(IRootObjectProvider), obj);
		val18.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(22, 21)));
		object obj2 = val17.ProvideValue((IServiceProvider)val18);
		((BindableObject)val4).SetValue(VisualElement.StyleProperty, (obj2 == null || !typeof(BindingBase).IsAssignableFrom(obj2.GetType())) ? obj2 : obj2);
		((BindableObject)val4).SetValue(Button.FontSizeProperty, (object)14.0);
		((Layout)val7).Children.Add((IView)(object)val4);
		((BindableObject)val6).SetValue(Grid.ColumnProperty, (object)1);
		((BindableObject)val6).SetValue(Button.TextProperty, (object)"\ud83d\udce4 Enviados");
		val6.Clicked += messagesPage.OnSentClicked;
		val5.Key = "OutlineButton";
		StaticResourceExtension val20 = new StaticResourceExtension
		{
			Key = "OutlineButton"
		};
		XamlServiceProvider val21 = new XamlServiceProvider();
		Type? typeFromHandle2 = typeof(IProvideValueTarget);
		object[] array2 = new object[0 + 4];
		array2[0] = val6;
		array2[1] = val7;
		array2[2] = val15;
		array2[3] = messagesPage;
		SimpleValueTargetProvider val22 = new SimpleValueTargetProvider(array2, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val16, val16, val16, val16, val16 }, (object)messagesPage);
		object obj3 = (object)val22;
		val21.Add(typeFromHandle2, (object)val22);
		val21.Add(typeof(IReferenceProvider), obj3);
		val21.Add(typeof(IRootObjectProvider), obj3);
		val21.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(28, 21)));
		object obj4 = val20.ProvideValue((IServiceProvider)val21);
		((BindableObject)val6).SetValue(VisualElement.StyleProperty, (obj4 == null || !typeof(BindingBase).IsAssignableFrom(obj4.GetType())) ? obj4 : obj4);
		((BindableObject)val6).SetValue(Button.FontSizeProperty, (object)14.0);
		((Layout)val7).Children.Add((IView)(object)val6);
		((Layout)val15).Children.Add((IView)(object)val7);
		((BindableObject)val14).SetValue(Grid.RowProperty, (object)1);
		val14.Refreshing += messagesPage.OnPullRefresh;
		((BindableObject)val13).SetValue(SelectableItemsView.SelectionModeProperty, (object)(SelectionMode)0);
		((BindableObject)val11).SetValue(Layout.PaddingProperty, (object)new Thickness(24.0));
		((BindableObject)val11).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val11).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val11).SetValue(StackBase.SpacingProperty, (object)8.0);
		((BindableObject)val8).SetValue(Label.TextProperty, (object)"\ud83d\udced");
		((BindableObject)val8).SetValue(Label.FontSizeProperty, (object)40.0);
		((BindableObject)val8).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val11).Children.Add((IView)(object)val8);
		((BindableObject)val10).SetValue(Label.TextProperty, (object)"No hay mensajes");
		val9.Key = "HintText";
		StaticResourceExtension val23 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val24 = new XamlServiceProvider();
		Type? typeFromHandle3 = typeof(IProvideValueTarget);
		object[] array3 = new object[0 + 6];
		array3[0] = val10;
		array3[1] = val11;
		array3[2] = val13;
		array3[3] = val14;
		array3[4] = val15;
		array3[5] = messagesPage;
		SimpleValueTargetProvider val25 = new SimpleValueTargetProvider(array3, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val16, val16, val16, val16, val16, val16, val16 }, (object)messagesPage);
		object obj5 = (object)val25;
		val24.Add(typeFromHandle3, (object)val25);
		val24.Add(typeof(IReferenceProvider), obj5);
		val24.Add(typeof(IRootObjectProvider), obj5);
		val24.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(40, 32)));
		object obj6 = val23.ProvideValue((IServiceProvider)val24);
		((BindableObject)val10).SetValue(VisualElement.StyleProperty, (obj6 == null || !typeof(BindingBase).IsAssignableFrom(obj6.GetType())) ? obj6 : obj6);
		((BindableObject)val10).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val11).Children.Add((IView)(object)val10);
		((BindableObject)val13).SetValue(ItemsView.EmptyViewProperty, (object)val11);
		NameScope _scope0 = val16;
		NameScope _scope1 = val16;
		NameScope _scope2 = val16;
		NameScope _scope3 = val16;
		NameScope _scope4 = val16;
		NameScope _scope5 = val16;
		NameScope _scope6 = val16;
		NameScope _scope7 = val16;
		NameScope _scope8 = val16;
		NameScope _scope9 = val16;
		NameScope _scope10 = val16;
		NameScope _scope11 = val16;
		NameScope _scope12 = val16;
		NameScope _scope13 = val16;
		NameScope _scope14 = val16;
		NameScope _scope15 = val16;
		NameScope _scope16 = val16;
		NameScope _scope17 = val16;
		NameScope _scope18 = val16;
		NameScope _scope19 = val16;
		NameScope _scope20 = val16;
		NameScope _scope21 = val16;
		NameScope _scope22 = val16;
		NameScope _scope23 = val16;
		NameScope _scope24 = val16;
		NameScope _scope25 = val16;
		NameScope _scope26 = val16;
		NameScope _scope27 = val16;
		NameScope _scope28 = val16;
		NameScope _scope29 = val16;
		NameScope _scope30 = val16;
		NameScope _scope31 = val16;
		NameScope _scope32 = val16;
		NameScope _scope33 = val16;
		NameScope _scope34 = val16;
		NameScope _scope35 = val16;
		NameScope _scope36 = val16;
		NameScope _scope37 = val16;
		NameScope _scope38 = val16;
		NameScope _scope39 = val16;
		NameScope _scope40 = val16;
		NameScope _scope41 = val16;
		NameScope _scope42 = val16;
		NameScope _scope43 = val16;
		NameScope _scope44 = val16;
		NameScope _scope45 = val16;
		NameScope _scope46 = val16;
		NameScope _scope47 = val16;
		NameScope _scope48 = val16;
		NameScope _scope49 = val16;
		NameScope _scope50 = val16;
		NameScope _scope51 = val16;
		NameScope _scope52 = val16;
		NameScope _scope53 = val16;
		NameScope _scope54 = val16;
		NameScope _scope55 = val16;
		NameScope _scope56 = val16;
		NameScope _scope57 = val16;
		object[] array4 = new object[0 + 5];
		array4[0] = val12;
		array4[1] = val13;
		array4[2] = val14;
		array4[3] = val15;
		array4[4] = messagesPage;
		object[] parentValues = array4;
		MessagesPage root = messagesPage;
		((ElementTemplate)val12).LoadTemplate = delegate
		{
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Expected O, but got Unknown
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Expected O, but got Unknown
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Expected O, but got Unknown
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Expected O, but got Unknown
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Expected O, but got Unknown
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Expected O, but got Unknown
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Expected O, but got Unknown
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Expected O, but got Unknown
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Expected O, but got Unknown
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Expected O, but got Unknown
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Expected O, but got Unknown
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Expected O, but got Unknown
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Expected O, but got Unknown
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Expected O, but got Unknown
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Expected O, but got Unknown
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Expected O, but got Unknown
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Expected O, but got Unknown
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Expected O, but got Unknown
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Expected O, but got Unknown
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Expected O, but got Unknown
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Expected O, but got Unknown
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Expected O, but got Unknown
			//IL_0369: Expected O, but got Unknown
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Expected O, but got Unknown
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Expected O, but got Unknown
			//IL_03b0: Expected O, but got Unknown
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Expected O, but got Unknown
			//IL_04d3: Expected O, but got Unknown
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Expected O, but got Unknown
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Expected O, but got Unknown
			//IL_051a: Expected O, but got Unknown
			//IL_0593: Unknown result type (might be due to invalid IL or missing references)
			//IL_0669: Unknown result type (might be due to invalid IL or missing references)
			//IL_0687: Unknown result type (might be due to invalid IL or missing references)
			//IL_068c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0692: Expected O, but got Unknown
			//IL_0694: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_069f: Expected O, but got Unknown
			//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ac: Expected O, but got Unknown
			//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b6: Expected O, but got Unknown
			//IL_0717: Unknown result type (might be due to invalid IL or missing references)
			//IL_082a: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0987: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aeb: Expected O, but got Unknown
			//IL_0af0: Expected O, but got Unknown
			//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2d: Expected O, but got Unknown
			//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b32: Expected O, but got Unknown
			//IL_0b37: Expected O, but got Unknown
			//IL_0bf0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ca2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ca7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d54: Expected O, but got Unknown
			//IL_0d59: Expected O, but got Unknown
			//IL_0d59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d96: Expected O, but got Unknown
			//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d9b: Expected O, but got Unknown
			//IL_0da0: Expected O, but got Unknown
			//IL_0e8b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f42: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f52: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe4: Expected O, but got Unknown
			//IL_0fe9: Expected O, but got Unknown
			//IL_0fe9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
			//IL_100d: Unknown result type (might be due to invalid IL or missing references)
			//IL_101c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1026: Expected O, but got Unknown
			//IL_1021: Unknown result type (might be due to invalid IL or missing references)
			//IL_102b: Expected O, but got Unknown
			//IL_1030: Expected O, but got Unknown
			//IL_1083: Unknown result type (might be due to invalid IL or missing references)
			NameScope val26 = _scope0;
			NameScope val27 = _scope1;
			NameScope val28 = _scope2;
			NameScope val29 = _scope3;
			NameScope val30 = _scope4;
			NameScope val31 = _scope5;
			NameScope val32 = _scope6;
			NameScope val33 = _scope7;
			NameScope val34 = _scope8;
			NameScope val35 = _scope9;
			NameScope val36 = _scope10;
			NameScope val37 = _scope11;
			NameScope val38 = _scope12;
			NameScope val39 = _scope13;
			NameScope val40 = _scope14;
			NameScope val41 = _scope15;
			NameScope val42 = _scope16;
			NameScope val43 = _scope17;
			NameScope val44 = _scope18;
			NameScope val45 = _scope19;
			NameScope val46 = _scope20;
			NameScope val47 = _scope21;
			NameScope val48 = _scope22;
			NameScope val49 = _scope23;
			NameScope val50 = _scope24;
			NameScope val51 = _scope25;
			NameScope val52 = _scope26;
			NameScope val53 = _scope27;
			NameScope val54 = _scope28;
			NameScope val55 = _scope29;
			NameScope val56 = _scope30;
			NameScope val57 = _scope31;
			NameScope val58 = _scope32;
			NameScope val59 = _scope33;
			NameScope val60 = _scope34;
			NameScope val61 = _scope35;
			NameScope val62 = _scope36;
			NameScope val63 = _scope37;
			NameScope val64 = _scope38;
			NameScope val65 = _scope39;
			NameScope val66 = _scope40;
			NameScope val67 = _scope41;
			NameScope val68 = _scope42;
			NameScope val69 = _scope43;
			NameScope val70 = _scope44;
			NameScope val71 = _scope45;
			NameScope val72 = _scope46;
			NameScope val73 = _scope47;
			NameScope val74 = _scope48;
			NameScope val75 = _scope49;
			NameScope val76 = _scope50;
			NameScope val77 = _scope51;
			NameScope val78 = _scope52;
			NameScope val79 = _scope53;
			NameScope val80 = _scope54;
			NameScope val81 = _scope55;
			NameScope val82 = _scope56;
			NameScope val83 = _scope57;
			StaticResourceExtension val84 = new StaticResourceExtension();
			StaticResourceExtension val85 = new StaticResourceExtension();
			BindingExtension val86 = new BindingExtension();
			SwipeItem val87 = new SwipeItem();
			SwipeItems val88 = new SwipeItems();
			BindingExtension val89 = new BindingExtension();
			TapGestureRecognizer val90 = new TapGestureRecognizer();
			BindingExtension val91 = new BindingExtension();
			Label val92 = new Label();
			BindingExtension val93 = new BindingExtension();
			StaticResourceExtension val94 = new StaticResourceExtension();
			Label val95 = new Label();
			BindingExtension val96 = new BindingExtension();
			StaticResourceExtension val97 = new StaticResourceExtension();
			Label val98 = new Label();
			VerticalStackLayout val99 = new VerticalStackLayout();
			BindingExtension val100 = new BindingExtension();
			StaticResourceExtension val101 = new StaticResourceExtension();
			Label val102 = new Label();
			Grid val103 = new Grid();
			SwipeView val104 = new SwipeView();
			Border val105 = new Border();
			NameScope val106 = new NameScope();
			NameScope.SetNameScope((BindableObject)(object)val105, (INameScope)(object)val106);
			((Element)val104).transientNamescope = (INameScope)(object)val106;
			((Element)val88).transientNamescope = (INameScope)(object)val106;
			((Element)val87).transientNamescope = (INameScope)(object)val106;
			((Element)val103).transientNamescope = (INameScope)(object)val106;
			((Element)val90).transientNamescope = (INameScope)(object)val106;
			((Element)val92).transientNamescope = (INameScope)(object)val106;
			((Element)val99).transientNamescope = (INameScope)(object)val106;
			((Element)val95).transientNamescope = (INameScope)(object)val106;
			((Element)val98).transientNamescope = (INameScope)(object)val106;
			((Element)val102).transientNamescope = (INameScope)(object)val106;
			val84.Key = "Card";
			StaticResourceExtension val107 = new StaticResourceExtension
			{
				Key = "Card"
			};
			XamlServiceProvider val108 = new XamlServiceProvider();
			Type? typeFromHandle4 = typeof(IProvideValueTarget);
			int length;
			object[] array5 = new object[(length = parentValues.Length) + 1];
			Array.Copy(parentValues, 0, array5, 1, length);
			array5[0] = val105;
			SimpleValueTargetProvider val109 = new SimpleValueTargetProvider(array5, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val106, val106, val82, val64, val60, val36, val26 }, (object)root);
			object obj7 = (object)val109;
			val108.Add(typeFromHandle4, (object)val109);
			val108.Add(typeof(IReferenceProvider), obj7);
			val108.Add(typeof(IRootObjectProvider), obj7);
			val108.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(46, 33)));
			object obj8 = val107.ProvideValue((IServiceProvider)val108);
			((BindableObject)val105).SetValue(VisualElement.StyleProperty, (obj8 == null || !typeof(BindingBase).IsAssignableFrom(obj8.GetType())) ? obj8 : obj8);
			((BindableObject)val105).SetValue(View.MarginProperty, (object)new Thickness(0.0, 4.0));
			((BindableObject)val87).SetValue(MenuItem.TextProperty, (object)"Borrar");
			val85.Key = "Danger";
			StaticResourceExtension val110 = new StaticResourceExtension
			{
				Key = "Danger"
			};
			XamlServiceProvider val111 = new XamlServiceProvider();
			Type? typeFromHandle5 = typeof(IProvideValueTarget);
			int length2;
			object[] array6 = new object[(length2 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array6, 4, length2);
			array6[0] = val87;
			array6[1] = val88;
			array6[2] = val104;
			array6[3] = val105;
			SimpleValueTargetProvider val112 = new SimpleValueTargetProvider(array6, (object)SwipeItem.BackgroundColorProperty, (INameScope[])(object)new NameScope[10] { val106, val106, val106, val106, val106, val82, val64, val60, val36, val26 }, (object)root);
			object obj9 = (object)val112;
			val111.Add(typeFromHandle5, (object)val112);
			val111.Add(typeof(IReferenceProvider), obj9);
			val111.Add(typeof(IRootObjectProvider), obj9);
			val111.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(51, 52)));
			object obj10 = val110.ProvideValue((IServiceProvider)val111);
			((BindableObject)val87).SetValue(SwipeItem.BackgroundColorProperty, (obj10 == null || !typeof(BindingBase).IsAssignableFrom(obj10.GetType())) ? obj10 : obj10);
			val87.Invoked += root.OnDeleteInvoked;
			val86.Path = ".";
			val86.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SmsMessageItem, SmsMessageItem>((Func<SmsMessageItem, ValueTuple<SmsMessageItem, bool>>)((SmsMessageItem P_0) => (P_0, true)), (Action<SmsMessageItem, SmsMessageItem>)null, (Tuple<Func<SmsMessageItem, object>, string>[])null);
			((BindingBase)val86.TypedBinding).Mode = val86.Mode;
			val86.TypedBinding.Converter = val86.Converter;
			val86.TypedBinding.ConverterParameter = val86.ConverterParameter;
			((BindingBase)val86.TypedBinding).StringFormat = val86.StringFormat;
			val86.TypedBinding.Source = val86.Source;
			val86.TypedBinding.UpdateSourceEventName = val86.UpdateSourceEventName;
			((BindingBase)val86.TypedBinding).FallbackValue = val86.FallbackValue;
			((BindingBase)val86.TypedBinding).TargetNullValue = val86.TargetNullValue;
			BindingBase typedBinding = (BindingBase)(object)val86.TypedBinding;
			((BindableObject)val87).SetBinding(MenuItem.CommandParameterProperty, typedBinding);
			val88.Add((ISwipeItem)(object)val87);
			((BindableObject)val104).SetValue(SwipeView.RightItemsProperty, (object)val88);
			((BindableObject)val103).SetValue(Layout.PaddingProperty, (object)new Thickness(14.0, 12.0));
			((BindableObject)val103).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[3]
			{
				new ColumnDefinition(GridLength.Auto),
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Auto)
			}));
			((BindableObject)val103).SetValue(Grid.ColumnSpacingProperty, (object)10.0);
			val90.Tapped += root.OnMessageTapped;
			val89.Path = ".";
			val89.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SmsMessageItem, SmsMessageItem>((Func<SmsMessageItem, ValueTuple<SmsMessageItem, bool>>)((SmsMessageItem P_0) => (P_0, true)), (Action<SmsMessageItem, SmsMessageItem>)null, (Tuple<Func<SmsMessageItem, object>, string>[])null);
			((BindingBase)val89.TypedBinding).Mode = val89.Mode;
			val89.TypedBinding.Converter = val89.Converter;
			val89.TypedBinding.ConverterParameter = val89.ConverterParameter;
			((BindingBase)val89.TypedBinding).StringFormat = val89.StringFormat;
			val89.TypedBinding.Source = val89.Source;
			val89.TypedBinding.UpdateSourceEventName = val89.UpdateSourceEventName;
			((BindingBase)val89.TypedBinding).FallbackValue = val89.FallbackValue;
			((BindingBase)val89.TypedBinding).TargetNullValue = val89.TargetNullValue;
			BindingBase typedBinding2 = (BindingBase)(object)val89.TypedBinding;
			((BindableObject)val90).SetBinding(TapGestureRecognizer.CommandParameterProperty, typedBinding2);
			((View)val103).GestureRecognizers.Add((IGestureRecognizer)(object)val90);
			((BindableObject)val92).SetValue(Grid.ColumnProperty, (object)0);
			val91.Path = "DirectionIcon";
			val91.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SmsMessageItem, string>((Func<SmsMessageItem, ValueTuple<string, bool>>)((SmsMessageItem P_0) => (P_0 != null) ? (P_0.DirectionIcon, true) : default((string, bool))), (Action<SmsMessageItem, string>)null, new Tuple<Func<SmsMessageItem, object>, string>[1]
			{
				new Tuple<Func<SmsMessageItem, object>, string>((SmsMessageItem P_0) => P_0, "DirectionIcon")
			});
			((BindingBase)val91.TypedBinding).Mode = val91.Mode;
			val91.TypedBinding.Converter = val91.Converter;
			val91.TypedBinding.ConverterParameter = val91.ConverterParameter;
			((BindingBase)val91.TypedBinding).StringFormat = val91.StringFormat;
			val91.TypedBinding.Source = val91.Source;
			val91.TypedBinding.UpdateSourceEventName = val91.UpdateSourceEventName;
			((BindingBase)val91.TypedBinding).FallbackValue = val91.FallbackValue;
			((BindingBase)val91.TypedBinding).TargetNullValue = val91.TargetNullValue;
			BindingBase typedBinding3 = (BindingBase)(object)val91.TypedBinding;
			((BindableObject)val92).SetBinding(Label.TextProperty, typedBinding3);
			((BindableObject)val92).SetValue(Label.FontSizeProperty, (object)18.0);
			((BindableObject)val92).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Start);
			((Layout)val103).Children.Add((IView)(object)val92);
			((BindableObject)val99).SetValue(Grid.ColumnProperty, (object)1);
			((BindableObject)val99).SetValue(StackBase.SpacingProperty, (object)2.0);
			val93.Path = "DisplayAddress";
			val93.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SmsMessageItem, string>((Func<SmsMessageItem, ValueTuple<string, bool>>)((SmsMessageItem P_0) => (P_0 != null) ? (P_0.DisplayAddress, true) : default((string, bool))), (Action<SmsMessageItem, string>)null, new Tuple<Func<SmsMessageItem, object>, string>[1]
			{
				new Tuple<Func<SmsMessageItem, object>, string>((SmsMessageItem P_0) => P_0, "DisplayAddress")
			});
			((BindingBase)val93.TypedBinding).Mode = val93.Mode;
			val93.TypedBinding.Converter = val93.Converter;
			val93.TypedBinding.ConverterParameter = val93.ConverterParameter;
			((BindingBase)val93.TypedBinding).StringFormat = val93.StringFormat;
			val93.TypedBinding.Source = val93.Source;
			val93.TypedBinding.UpdateSourceEventName = val93.UpdateSourceEventName;
			((BindingBase)val93.TypedBinding).FallbackValue = val93.FallbackValue;
			((BindingBase)val93.TypedBinding).TargetNullValue = val93.TargetNullValue;
			BindingBase typedBinding4 = (BindingBase)(object)val93.TypedBinding;
			((BindableObject)val95).SetBinding(Label.TextProperty, typedBinding4);
			val94.Key = "BodyText";
			StaticResourceExtension val113 = new StaticResourceExtension
			{
				Key = "BodyText"
			};
			XamlServiceProvider val114 = new XamlServiceProvider();
			Type? typeFromHandle6 = typeof(IProvideValueTarget);
			int length3;
			object[] array7 = new object[(length3 = parentValues.Length) + 5];
			Array.Copy(parentValues, 0, array7, 5, length3);
			array7[0] = val95;
			array7[1] = val99;
			array7[2] = val103;
			array7[3] = val104;
			array7[4] = val105;
			SimpleValueTargetProvider val115 = new SimpleValueTargetProvider(array7, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[11]
			{
				val106, val106, val106, val106, val106, val106, val82, val64, val60, val36,
				val26
			}, (object)root);
			object obj11 = (object)val115;
			val114.Add(typeFromHandle6, (object)val115);
			val114.Add(typeof(IReferenceProvider), obj11);
			val114.Add(typeof(IRootObjectProvider), obj11);
			val114.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(63, 48)));
			object obj12 = val113.ProvideValue((IServiceProvider)val114);
			((BindableObject)val95).SetValue(VisualElement.StyleProperty, (obj12 == null || !typeof(BindingBase).IsAssignableFrom(obj12.GetType())) ? obj12 : obj12);
			((BindableObject)val95).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
			((BindableObject)val95).SetValue(Label.FontSizeProperty, (object)15.0);
			((Layout)val99).Children.Add((IView)(object)val95);
			val96.Path = "Snippet";
			val96.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SmsMessageItem, string>((Func<SmsMessageItem, ValueTuple<string, bool>>)((SmsMessageItem P_0) => (P_0 != null) ? (P_0.Snippet, true) : default((string, bool))), (Action<SmsMessageItem, string>)null, new Tuple<Func<SmsMessageItem, object>, string>[1]
			{
				new Tuple<Func<SmsMessageItem, object>, string>((SmsMessageItem P_0) => P_0, "Snippet")
			});
			((BindingBase)val96.TypedBinding).Mode = val96.Mode;
			val96.TypedBinding.Converter = val96.Converter;
			val96.TypedBinding.ConverterParameter = val96.ConverterParameter;
			((BindingBase)val96.TypedBinding).StringFormat = val96.StringFormat;
			val96.TypedBinding.Source = val96.Source;
			val96.TypedBinding.UpdateSourceEventName = val96.UpdateSourceEventName;
			((BindingBase)val96.TypedBinding).FallbackValue = val96.FallbackValue;
			((BindingBase)val96.TypedBinding).TargetNullValue = val96.TargetNullValue;
			BindingBase typedBinding5 = (BindingBase)(object)val96.TypedBinding;
			((BindableObject)val98).SetBinding(Label.TextProperty, typedBinding5);
			val97.Key = "HintText";
			StaticResourceExtension val116 = new StaticResourceExtension
			{
				Key = "HintText"
			};
			XamlServiceProvider val117 = new XamlServiceProvider();
			Type? typeFromHandle7 = typeof(IProvideValueTarget);
			int length4;
			object[] array8 = new object[(length4 = parentValues.Length) + 5];
			Array.Copy(parentValues, 0, array8, 5, length4);
			array8[0] = val98;
			array8[1] = val99;
			array8[2] = val103;
			array8[3] = val104;
			array8[4] = val105;
			SimpleValueTargetProvider val118 = new SimpleValueTargetProvider(array8, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[11]
			{
				val106, val106, val106, val106, val106, val106, val82, val64, val60, val36,
				val26
			}, (object)root);
			object obj13 = (object)val118;
			val117.Add(typeFromHandle7, (object)val118);
			val117.Add(typeof(IReferenceProvider), obj13);
			val117.Add(typeof(IRootObjectProvider), obj13);
			val117.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(67, 48)));
			object obj14 = val116.ProvideValue((IServiceProvider)val117);
			((BindableObject)val98).SetValue(VisualElement.StyleProperty, (obj14 == null || !typeof(BindingBase).IsAssignableFrom(obj14.GetType())) ? obj14 : obj14);
			((BindableObject)val98).SetValue(Label.FontSizeProperty, (object)14.0);
			((BindableObject)val98).SetValue(Label.LineBreakModeProperty, (object)(LineBreakMode)4);
			((BindableObject)val98).SetValue(Label.MaxLinesProperty, (object)2);
			((Layout)val99).Children.Add((IView)(object)val98);
			((Layout)val103).Children.Add((IView)(object)val99);
			((BindableObject)val102).SetValue(Grid.ColumnProperty, (object)2);
			val100.Path = "DateText";
			val100.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SmsMessageItem, string>((Func<SmsMessageItem, ValueTuple<string, bool>>)((SmsMessageItem P_0) => (P_0 != null) ? (P_0.DateText, true) : default((string, bool))), (Action<SmsMessageItem, string>)null, new Tuple<Func<SmsMessageItem, object>, string>[1]
			{
				new Tuple<Func<SmsMessageItem, object>, string>((SmsMessageItem P_0) => P_0, "DateText")
			});
			((BindingBase)val100.TypedBinding).Mode = val100.Mode;
			val100.TypedBinding.Converter = val100.Converter;
			val100.TypedBinding.ConverterParameter = val100.ConverterParameter;
			((BindingBase)val100.TypedBinding).StringFormat = val100.StringFormat;
			val100.TypedBinding.Source = val100.Source;
			val100.TypedBinding.UpdateSourceEventName = val100.UpdateSourceEventName;
			((BindingBase)val100.TypedBinding).FallbackValue = val100.FallbackValue;
			((BindingBase)val100.TypedBinding).TargetNullValue = val100.TargetNullValue;
			BindingBase typedBinding6 = (BindingBase)(object)val100.TypedBinding;
			((BindableObject)val102).SetBinding(Label.TextProperty, typedBinding6);
			val101.Key = "HintText";
			StaticResourceExtension val119 = new StaticResourceExtension
			{
				Key = "HintText"
			};
			XamlServiceProvider val120 = new XamlServiceProvider();
			Type? typeFromHandle8 = typeof(IProvideValueTarget);
			int length5;
			object[] array9 = new object[(length5 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array9, 4, length5);
			array9[0] = val102;
			array9[1] = val103;
			array9[2] = val104;
			array9[3] = val105;
			SimpleValueTargetProvider val121 = new SimpleValueTargetProvider(array9, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[10] { val106, val106, val106, val106, val106, val82, val64, val60, val36, val26 }, (object)root);
			object obj15 = (object)val121;
			val120.Add(typeFromHandle8, (object)val121);
			val120.Add(typeof(IReferenceProvider), obj15);
			val120.Add(typeof(IRootObjectProvider), obj15);
			val120.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(74, 44)));
			object obj16 = val119.ProvideValue((IServiceProvider)val120);
			((BindableObject)val102).SetValue(VisualElement.StyleProperty, (obj16 == null || !typeof(BindingBase).IsAssignableFrom(obj16.GetType())) ? obj16 : obj16);
			((BindableObject)val102).SetValue(Label.FontSizeProperty, (object)11.0);
			((BindableObject)val102).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Start);
			((Layout)val103).Children.Add((IView)(object)val102);
			((BindableObject)val104).SetValue(ContentView.ContentProperty, (object)val103);
			((BindableObject)val105).SetValue(Border.ContentProperty, (object)val104);
			return val105;
		};
		((BindableObject)val13).SetValue(ItemsView.ItemTemplateProperty, (object)val12);
		((BindableObject)val14).SetValue(ContentView.ContentProperty, (object)val13);
		((Layout)val15).Children.Add((IView)(object)val14);
		((BindableObject)messagesPage).SetValue(ContentPage.ContentProperty, (object)val15);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0()
	{
		((Page)this).OnAppearing();
	}
}
