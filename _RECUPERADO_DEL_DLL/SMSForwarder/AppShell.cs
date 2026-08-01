using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Xaml.Internals;
using SMSForwarder.Pages;
using SMSForwarder.Services;

namespace SMSForwarder;

[XamlFilePath("AppShell.xaml")]
public class AppShell : Shell
{
	private readonly ILocalizationService _localizationService;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label HeaderTitle;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private FlyoutItem SettingsItem;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private FlyoutItem MessagesItem;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private FlyoutItem DiagnosticsItem;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private FlyoutItem AboutItem;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label FooterVersion;

	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("DiagnosticsPage", typeof(DiagnosticsPage));
		Routing.RegisterRoute("AboutPage", typeof(AboutPage));
		MauiApplication current = MauiApplication.Current;
		_localizationService = ((current != null) ? current.Services.GetRequiredService<ILocalizationService>() : null) ?? new LocalizationService();
		_localizationService.LanguageChanged += OnLanguageChanged;
		UpdateLocalizedStrings();
	}

	private void OnLanguageChanged(object? sender, EventArgs e)
	{
		MainThread.BeginInvokeOnMainThread((Action)UpdateLocalizedStrings);
	}

	private void UpdateLocalizedStrings()
	{
		HeaderTitle.Text = "SMS Forwarder";
		((BaseShellItem)SettingsItem).Title = _localizationService.GetString("menu.settings");
		((BaseShellItem)DiagnosticsItem).Title = _localizationService.GetString("menu.diagnostics");
		((BaseShellItem)AboutItem).Title = _localizationService.GetString("menu.about");
		FooterVersion.Text = "v2026.07.28.0";
	}

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	[MemberNotNull("HeaderTitle")]
	[MemberNotNull("SettingsItem")]
	[MemberNotNull("MessagesItem")]
	[MemberNotNull("DiagnosticsItem")]
	[MemberNotNull("AboutItem")]
	[MemberNotNull("FooterVersion")]
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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_035e: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_03a4: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Expected O, but got Unknown
		//IL_0490: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Expected O, but got Unknown
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Expected O, but got Unknown
		//IL_0554: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Expected O, but got Unknown
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Expected O, but got Unknown
		//IL_05a2: Expected O, but got Unknown
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0628: Expected O, but got Unknown
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Expected O, but got Unknown
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_066e: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ef: Expected O, but got Unknown
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Expected O, but got Unknown
		//IL_0735: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_077b: Expected O, but got Unknown
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Expected O, but got Unknown
		//IL_0785: Expected O, but got Unknown
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Expected O, but got Unknown
		//IL_07f2: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Expected O, but got Unknown
		//IL_0838: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Expected O, but got Unknown
		//IL_08e0: Expected O, but got Unknown
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Expected O, but got Unknown
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Expected O, but got Unknown
		//IL_0926: Expected O, but got Unknown
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a2: Expected O, but got Unknown
		//IL_09a7: Expected O, but got Unknown
		//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Expected O, but got Unknown
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Expected O, but got Unknown
		//IL_09ed: Expected O, but got Unknown
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Expected O, but got Unknown
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Expected O, but got Unknown
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a38: Expected O, but got Unknown
		//IL_0a3d: Expected O, but got Unknown
		//IL_0a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a81: Expected O, but got Unknown
		//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Expected O, but got Unknown
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Expected O, but got Unknown
		//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9d: Expected O, but got Unknown
		//IL_0fa2: Expected O, but got Unknown
		//IL_0fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Expected O, but got Unknown
		//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe4: Expected O, but got Unknown
		//IL_0fe9: Expected O, but got Unknown
		//IL_1060: Unknown result type (might be due to invalid IL or missing references)
		//IL_1076: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1156: Unknown result type (might be due to invalid IL or missing references)
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_116b: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c0: Expected O, but got Unknown
		//IL_11c5: Expected O, but got Unknown
		//IL_11c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1202: Expected O, but got Unknown
		//IL_11fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1207: Expected O, but got Unknown
		//IL_120c: Expected O, but got Unknown
		//IL_1245: Unknown result type (might be due to invalid IL or missing references)
		//IL_1297: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1335: Unknown result type (might be due to invalid IL or missing references)
		//IL_133a: Unknown result type (might be due to invalid IL or missing references)
		//IL_133d: Expected O, but got Unknown
		//IL_1342: Expected O, but got Unknown
		//IL_1342: Unknown result type (might be due to invalid IL or missing references)
		//IL_1354: Unknown result type (might be due to invalid IL or missing references)
		//IL_1366: Unknown result type (might be due to invalid IL or missing references)
		//IL_1375: Unknown result type (might be due to invalid IL or missing references)
		//IL_137f: Expected O, but got Unknown
		//IL_137a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1384: Expected O, but got Unknown
		//IL_1389: Expected O, but got Unknown
		//IL_13c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d8: Unknown result type (might be due to invalid IL or missing references)
		StaticResourceExtension val = new StaticResourceExtension();
		StaticResourceExtension val2 = new StaticResourceExtension();
		StaticResourceExtension val3 = new StaticResourceExtension();
		AppThemeBindingExtension val4 = new AppThemeBindingExtension();
		StaticResourceExtension val5 = new StaticResourceExtension();
		StaticResourceExtension val6 = new StaticResourceExtension();
		AppThemeBindingExtension val7 = new AppThemeBindingExtension();
		StaticResourceExtension val8 = new StaticResourceExtension();
		StaticResourceExtension val9 = new StaticResourceExtension();
		StaticResourceExtension val10 = new StaticResourceExtension();
		AppThemeBindingExtension val11 = new AppThemeBindingExtension();
		Image val12 = new Image();
		Label val13 = new Label();
		Grid val14 = new Grid();
		DataTemplate val15 = new DataTemplate();
		StaticResourceExtension val16 = new StaticResourceExtension();
		Image val17 = new Image();
		StaticResourceExtension val18 = new StaticResourceExtension();
		Label val19 = new Label();
		VerticalStackLayout val20 = new VerticalStackLayout();
		Grid val21 = new Grid();
		StaticResourceExtension val22 = new StaticResourceExtension();
		Label val23 = new Label();
		Grid val24 = new Grid();
		DataTemplate val25 = new DataTemplate(typeof(MainPage));
		ShellContent val26 = new ShellContent();
		FlyoutItem val27 = new FlyoutItem();
		DataTemplate val28 = new DataTemplate(typeof(MessagesPage));
		ShellContent val29 = new ShellContent();
		FlyoutItem val30 = new FlyoutItem();
		DataTemplate val31 = new DataTemplate(typeof(DiagnosticsPage));
		ShellContent val32 = new ShellContent();
		FlyoutItem val33 = new FlyoutItem();
		DataTemplate val34 = new DataTemplate(typeof(AboutPage));
		ShellContent val35 = new ShellContent();
		FlyoutItem val36 = new FlyoutItem();
		AppShell appShell;
		NameScope val37 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(appShell = this))) ?? ((object)new NameScope()));
		NameScope.SetNameScope((BindableObject)(object)appShell, (INameScope)(object)val37);
		((Element)val14).transientNamescope = (INameScope)(object)val37;
		((Element)val12).transientNamescope = (INameScope)(object)val37;
		((Element)val13).transientNamescope = (INameScope)(object)val37;
		((Element)val21).transientNamescope = (INameScope)(object)val37;
		((Element)val20).transientNamescope = (INameScope)(object)val37;
		((Element)val17).transientNamescope = (INameScope)(object)val37;
		((Element)val19).transientNamescope = (INameScope)(object)val37;
		((INameScope)val37).RegisterName("HeaderTitle", (object)val19);
		if (((Element)val19).StyleId == null)
		{
			((Element)val19).StyleId = "HeaderTitle";
		}
		((Element)val24).transientNamescope = (INameScope)(object)val37;
		((Element)val23).transientNamescope = (INameScope)(object)val37;
		((INameScope)val37).RegisterName("FooterVersion", (object)val23);
		if (((Element)val23).StyleId == null)
		{
			((Element)val23).StyleId = "FooterVersion";
		}
		((Element)val27).transientNamescope = (INameScope)(object)val37;
		((INameScope)val37).RegisterName("SettingsItem", (object)val27);
		if (((Element)val27).StyleId == null)
		{
			((Element)val27).StyleId = "SettingsItem";
		}
		((Element)val26).transientNamescope = (INameScope)(object)val37;
		((Element)val30).transientNamescope = (INameScope)(object)val37;
		((INameScope)val37).RegisterName("MessagesItem", (object)val30);
		if (((Element)val30).StyleId == null)
		{
			((Element)val30).StyleId = "MessagesItem";
		}
		((Element)val29).transientNamescope = (INameScope)(object)val37;
		((Element)val33).transientNamescope = (INameScope)(object)val37;
		((INameScope)val37).RegisterName("DiagnosticsItem", (object)val33);
		if (((Element)val33).StyleId == null)
		{
			((Element)val33).StyleId = "DiagnosticsItem";
		}
		((Element)val32).transientNamescope = (INameScope)(object)val37;
		((Element)val36).transientNamescope = (INameScope)(object)val37;
		((INameScope)val37).RegisterName("AboutItem", (object)val36);
		if (((Element)val36).StyleId == null)
		{
			((Element)val36).StyleId = "AboutItem";
		}
		((Element)val35).transientNamescope = (INameScope)(object)val37;
		HeaderTitle = val19;
		FooterVersion = val23;
		SettingsItem = val27;
		MessagesItem = val30;
		DiagnosticsItem = val33;
		AboutItem = val36;
		((BindableObject)appShell).SetValue(Shell.FlyoutBehaviorProperty, (object)(FlyoutBehavior)1);
		((BindableObject)appShell).SetValue(Page.TitleProperty, (object)"SMS Forwarder");
		val.Key = "Primary";
		StaticResourceExtension val38 = new StaticResourceExtension
		{
			Key = "Primary"
		};
		XamlServiceProvider val39 = new XamlServiceProvider();
		Type? typeFromHandle = typeof(IProvideValueTarget);
		object[] array = new object[0 + 1];
		array[0] = appShell;
		SimpleValueTargetProvider val40 = new SimpleValueTargetProvider(array, (object)Shell.ForegroundColorProperty, (INameScope[])(object)new NameScope[2] { val37, val37 }, (object)appShell);
		object obj = (object)val40;
		val39.Add(typeFromHandle, (object)val40);
		val39.Add(typeof(IReferenceProvider), obj);
		val39.Add(typeof(IRootObjectProvider), obj);
		val39.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(11, 5)));
		object obj2 = val38.ProvideValue((IServiceProvider)val39);
		((BindableObject)appShell).SetValue(Shell.ForegroundColorProperty, (obj2 == null || !typeof(BindingBase).IsAssignableFrom(obj2.GetType())) ? obj2 : obj2);
		val2.Key = "TextPrimaryLight";
		StaticResourceExtension val41 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val42 = new XamlServiceProvider();
		Type? typeFromHandle2 = typeof(IProvideValueTarget);
		object[] array2 = new object[0 + 2];
		array2[0] = val4;
		array2[1] = appShell;
		SimpleValueTargetProvider val43 = new SimpleValueTargetProvider(array2, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[3] { val37, val37, val37 }, (object)appShell);
		object obj3 = (object)val43;
		val42.Add(typeFromHandle2, (object)val43);
		val42.Add(typeof(IReferenceProvider), obj3);
		val42.Add(typeof(IRootObjectProvider), obj3);
		val42.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(12, 5)));
		object light = val41.ProvideValue((IServiceProvider)val42);
		val4.Light = light;
		val3.Key = "TextPrimaryDark";
		StaticResourceExtension val44 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val45 = new XamlServiceProvider();
		Type? typeFromHandle3 = typeof(IProvideValueTarget);
		object[] array3 = new object[0 + 2];
		array3[0] = val4;
		array3[1] = appShell;
		SimpleValueTargetProvider val46 = new SimpleValueTargetProvider(array3, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[3] { val37, val37, val37 }, (object)appShell);
		object obj4 = (object)val46;
		val45.Add(typeFromHandle3, (object)val46);
		val45.Add(typeof(IReferenceProvider), obj4);
		val45.Add(typeof(IRootObjectProvider), obj4);
		val45.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(12, 5)));
		object dark = val44.ProvideValue((IServiceProvider)val45);
		val4.Dark = dark;
		XamlServiceProvider val47 = new XamlServiceProvider();
		val47.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)appShell, (object)Shell.TitleColorProperty));
		val47.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(12, 5)));
		BindingBase val48 = ((IMarkupExtension<BindingBase>)(object)val4).ProvideValue((IServiceProvider)val47);
		((BindableObject)appShell).SetBinding(Shell.TitleColorProperty, val48);
		val5.Key = "TextSecondaryLight";
		StaticResourceExtension val49 = new StaticResourceExtension
		{
			Key = "TextSecondaryLight"
		};
		XamlServiceProvider val50 = new XamlServiceProvider();
		Type? typeFromHandle4 = typeof(IProvideValueTarget);
		object[] array4 = new object[0 + 2];
		array4[0] = val7;
		array4[1] = appShell;
		SimpleValueTargetProvider val51 = new SimpleValueTargetProvider(array4, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[3] { val37, val37, val37 }, (object)appShell);
		object obj5 = (object)val51;
		val50.Add(typeFromHandle4, (object)val51);
		val50.Add(typeof(IReferenceProvider), obj5);
		val50.Add(typeof(IRootObjectProvider), obj5);
		val50.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(13, 5)));
		object light2 = val49.ProvideValue((IServiceProvider)val50);
		val7.Light = light2;
		val6.Key = "TextSecondaryDark";
		StaticResourceExtension val52 = new StaticResourceExtension
		{
			Key = "TextSecondaryDark"
		};
		XamlServiceProvider val53 = new XamlServiceProvider();
		Type? typeFromHandle5 = typeof(IProvideValueTarget);
		object[] array5 = new object[0 + 2];
		array5[0] = val7;
		array5[1] = appShell;
		SimpleValueTargetProvider val54 = new SimpleValueTargetProvider(array5, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[3] { val37, val37, val37 }, (object)appShell);
		object obj6 = (object)val54;
		val53.Add(typeFromHandle5, (object)val54);
		val53.Add(typeof(IReferenceProvider), obj6);
		val53.Add(typeof(IRootObjectProvider), obj6);
		val53.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(13, 5)));
		object dark2 = val52.ProvideValue((IServiceProvider)val53);
		val7.Dark = dark2;
		XamlServiceProvider val55 = new XamlServiceProvider();
		val55.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)appShell, (object)Shell.UnselectedColorProperty));
		val55.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(13, 5)));
		BindingBase val56 = ((IMarkupExtension<BindingBase>)(object)val7).ProvideValue((IServiceProvider)val55);
		((BindableObject)appShell).SetBinding(Shell.UnselectedColorProperty, val56);
		val8.Key = "SeparatorLight";
		StaticResourceExtension val57 = new StaticResourceExtension
		{
			Key = "SeparatorLight"
		};
		XamlServiceProvider val58 = new XamlServiceProvider();
		Type? typeFromHandle6 = typeof(IProvideValueTarget);
		object[] array6 = new object[0 + 1];
		array6[0] = appShell;
		SimpleValueTargetProvider val59 = new SimpleValueTargetProvider(array6, (object)Shell.DisabledColorProperty, (INameScope[])(object)new NameScope[2] { val37, val37 }, (object)appShell);
		object obj7 = (object)val59;
		val58.Add(typeFromHandle6, (object)val59);
		val58.Add(typeof(IReferenceProvider), obj7);
		val58.Add(typeof(IRootObjectProvider), obj7);
		val58.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(14, 5)));
		object obj8 = val57.ProvideValue((IServiceProvider)val58);
		((BindableObject)appShell).SetValue(Shell.DisabledColorProperty, (obj8 == null || !typeof(BindingBase).IsAssignableFrom(obj8.GetType())) ? obj8 : obj8);
		val9.Key = "PageBackgroundLight";
		StaticResourceExtension val60 = new StaticResourceExtension
		{
			Key = "PageBackgroundLight"
		};
		XamlServiceProvider val61 = new XamlServiceProvider();
		Type? typeFromHandle7 = typeof(IProvideValueTarget);
		object[] array7 = new object[0 + 2];
		array7[0] = val11;
		array7[1] = appShell;
		SimpleValueTargetProvider val62 = new SimpleValueTargetProvider(array7, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[3] { val37, val37, val37 }, (object)appShell);
		object obj9 = (object)val62;
		val61.Add(typeFromHandle7, (object)val62);
		val61.Add(typeof(IReferenceProvider), obj9);
		val61.Add(typeof(IRootObjectProvider), obj9);
		val61.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(15, 5)));
		object light3 = val60.ProvideValue((IServiceProvider)val61);
		val11.Light = light3;
		val10.Key = "PageBackgroundDark";
		StaticResourceExtension val63 = new StaticResourceExtension
		{
			Key = "PageBackgroundDark"
		};
		XamlServiceProvider val64 = new XamlServiceProvider();
		Type? typeFromHandle8 = typeof(IProvideValueTarget);
		object[] array8 = new object[0 + 2];
		array8[0] = val11;
		array8[1] = appShell;
		SimpleValueTargetProvider val65 = new SimpleValueTargetProvider(array8, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[3] { val37, val37, val37 }, (object)appShell);
		object obj10 = (object)val65;
		val64.Add(typeFromHandle8, (object)val65);
		val64.Add(typeof(IReferenceProvider), obj10);
		val64.Add(typeof(IRootObjectProvider), obj10);
		val64.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(15, 5)));
		object dark3 = val63.ProvideValue((IServiceProvider)val64);
		val11.Dark = dark3;
		XamlServiceProvider val66 = new XamlServiceProvider();
		val66.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)appShell, (object)Shell.FlyoutBackgroundColorProperty));
		val66.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(15, 5)));
		BindingBase val67 = ((IMarkupExtension<BindingBase>)(object)val11).ProvideValue((IServiceProvider)val66);
		((BindableObject)appShell).SetBinding(Shell.FlyoutBackgroundColorProperty, val67);
		((BindableObject)appShell).SetValue(Shell.FlyoutWidthProperty, (object)260.0);
		((BindableObject)val14).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
		{
			new ColumnDefinition(GridLength.Auto),
			new ColumnDefinition(GridLength.Star)
		}));
		((BindableObject)val14).SetValue(Layout.PaddingProperty, (object)new Thickness(0.0));
		((BindableObject)val12).SetValue(Image.SourceProperty, (object)ImageSource.FromFile("app_logo.svg"));
		((BindableObject)val12).SetValue(VisualElement.HeightRequestProperty, (object)24.0);
		((BindableObject)val12).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val14).Children.Add((IView)(object)val12);
		((BindableObject)val13).SetValue(Grid.ColumnProperty, (object)1);
		((BindableObject)val13).SetValue(Label.TextProperty, (object)"SMS Forwarder");
		((BindableObject)val13).SetValue(Label.FontSizeProperty, (object)18.0);
		((BindableObject)val13).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		((BindableObject)val13).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val13).SetValue(View.MarginProperty, (object)new Thickness(8.0, 0.0, 0.0, 0.0));
		((Layout)val14).Children.Add((IView)(object)val13);
		((BindableObject)appShell).SetValue(Shell.TitleViewProperty, (object)val14);
		NameScope _scope0 = val37;
		NameScope _scope1 = val37;
		NameScope _scope2 = val37;
		NameScope _scope3 = val37;
		NameScope _scope4 = val37;
		NameScope _scope5 = val37;
		NameScope _scope6 = val37;
		NameScope _scope7 = val37;
		NameScope _scope8 = val37;
		NameScope _scope9 = val37;
		NameScope _scope10 = val37;
		NameScope _scope11 = val37;
		NameScope _scope12 = val37;
		NameScope _scope13 = val37;
		NameScope _scope14 = val37;
		NameScope _scope15 = val37;
		NameScope _scope16 = val37;
		NameScope _scope17 = val37;
		NameScope _scope18 = val37;
		NameScope _scope19 = val37;
		NameScope _scope20 = val37;
		NameScope _scope21 = val37;
		NameScope _scope22 = val37;
		NameScope _scope23 = val37;
		NameScope _scope24 = val37;
		NameScope _scope25 = val37;
		NameScope _scope26 = val37;
		NameScope _scope27 = val37;
		NameScope _scope28 = val37;
		NameScope _scope29 = val37;
		NameScope _scope30 = val37;
		NameScope _scope31 = val37;
		NameScope _scope32 = val37;
		NameScope _scope33 = val37;
		NameScope _scope34 = val37;
		NameScope _scope35 = val37;
		NameScope _scope36 = val37;
		NameScope _scope37 = val37;
		NameScope _scope38 = val37;
		NameScope _scope39 = val37;
		NameScope _scope40 = val37;
		NameScope _scope41 = val37;
		NameScope _scope42 = val37;
		NameScope _scope43 = val37;
		NameScope _scope44 = val37;
		NameScope _scope45 = val37;
		NameScope _scope46 = val37;
		NameScope _scope47 = val37;
		NameScope _scope48 = val37;
		NameScope _scope49 = val37;
		NameScope _scope50 = val37;
		NameScope _scope51 = val37;
		NameScope _scope52 = val37;
		NameScope _scope53 = val37;
		NameScope _scope54 = val37;
		NameScope _scope55 = val37;
		NameScope _scope56 = val37;
		NameScope _scope57 = val37;
		NameScope _scope58 = val37;
		NameScope _scope59 = val37;
		NameScope _scope60 = val37;
		NameScope _scope61 = val37;
		NameScope _scope62 = val37;
		NameScope _scope63 = val37;
		NameScope _scope64 = val37;
		NameScope _scope65 = val37;
		NameScope _scope66 = val37;
		NameScope _scope67 = val37;
		NameScope _scope68 = val37;
		NameScope _scope69 = val37;
		NameScope _scope70 = val37;
		NameScope _scope71 = val37;
		NameScope _scope72 = val37;
		NameScope _scope73 = val37;
		NameScope _scope74 = val37;
		NameScope _scope75 = val37;
		NameScope _scope76 = val37;
		NameScope _scope77 = val37;
		NameScope _scope78 = val37;
		NameScope _scope79 = val37;
		NameScope _scope80 = val37;
		NameScope _scope81 = val37;
		NameScope _scope82 = val37;
		NameScope _scope83 = val37;
		NameScope _scope84 = val37;
		NameScope _scope85 = val37;
		NameScope _scope86 = val37;
		NameScope _scope87 = val37;
		NameScope _scope88 = val37;
		NameScope _scope89 = val37;
		NameScope _scope90 = val37;
		NameScope _scope91 = val37;
		NameScope _scope92 = val37;
		NameScope _scope93 = val37;
		NameScope _scope94 = val37;
		NameScope _scope95 = val37;
		NameScope _scope96 = val37;
		NameScope _scope97 = val37;
		NameScope _scope98 = val37;
		NameScope _scope99 = val37;
		NameScope _scope100 = val37;
		NameScope _scope101 = val37;
		object[] array9 = new object[0 + 2];
		array9[0] = val15;
		array9[1] = appShell;
		object[] parentValues = array9;
		AppShell root = appShell;
		((ElementTemplate)val15).LoadTemplate = delegate
		{
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Expected O, but got Unknown
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Expected O, but got Unknown
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Expected O, but got Unknown
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Expected O, but got Unknown
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Expected O, but got Unknown
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Expected O, but got Unknown
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Expected O, but got Unknown
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Expected O, but got Unknown
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Expected O, but got Unknown
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Expected O, but got Unknown
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Expected O, but got Unknown
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Expected O, but got Unknown
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0601: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0776: Unknown result type (might be due to invalid IL or missing references)
			//IL_077b: Unknown result type (might be due to invalid IL or missing references)
			//IL_077e: Expected O, but got Unknown
			//IL_0783: Expected O, but got Unknown
			//IL_0783: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Expected O, but got Unknown
			//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c5: Expected O, but got Unknown
			//IL_07ca: Expected O, but got Unknown
			//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0874: Unknown result type (might be due to invalid IL or missing references)
			//IL_0879: Unknown result type (might be due to invalid IL or missing references)
			//IL_087c: Expected O, but got Unknown
			//IL_0881: Expected O, but got Unknown
			//IL_0881: Unknown result type (might be due to invalid IL or missing references)
			//IL_0893: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08be: Expected O, but got Unknown
			//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c3: Expected O, but got Unknown
			//IL_08c8: Expected O, but got Unknown
			//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08da: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f6: Expected O, but got Unknown
			//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0905: Unknown result type (might be due to invalid IL or missing references)
			//IL_090f: Expected O, but got Unknown
			//IL_090a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0914: Expected O, but got Unknown
			//IL_0919: Expected O, but got Unknown
			NameScope val77 = _scope0;
			NameScope val78 = _scope1;
			NameScope val79 = _scope2;
			NameScope val80 = _scope3;
			NameScope val81 = _scope4;
			NameScope val82 = _scope5;
			NameScope val83 = _scope6;
			NameScope val84 = _scope7;
			NameScope val85 = _scope8;
			NameScope val86 = _scope9;
			NameScope val87 = _scope10;
			NameScope val88 = _scope11;
			NameScope val89 = _scope12;
			NameScope val90 = _scope13;
			NameScope val91 = _scope14;
			NameScope val92 = _scope15;
			NameScope val93 = _scope16;
			NameScope val94 = _scope17;
			NameScope val95 = _scope18;
			NameScope val96 = _scope19;
			NameScope val97 = _scope20;
			NameScope val98 = _scope21;
			NameScope val99 = _scope22;
			NameScope val100 = _scope23;
			NameScope val101 = _scope24;
			NameScope val102 = _scope25;
			NameScope val103 = _scope26;
			NameScope val104 = _scope27;
			NameScope val105 = _scope28;
			NameScope val106 = _scope29;
			NameScope val107 = _scope30;
			NameScope val108 = _scope31;
			NameScope val109 = _scope32;
			NameScope val110 = _scope33;
			NameScope val111 = _scope34;
			NameScope val112 = _scope35;
			NameScope val113 = _scope36;
			NameScope val114 = _scope37;
			NameScope val115 = _scope38;
			NameScope val116 = _scope39;
			NameScope val117 = _scope40;
			NameScope val118 = _scope41;
			NameScope val119 = _scope42;
			NameScope val120 = _scope43;
			NameScope val121 = _scope44;
			NameScope val122 = _scope45;
			NameScope val123 = _scope46;
			NameScope val124 = _scope47;
			NameScope val125 = _scope48;
			NameScope val126 = _scope49;
			NameScope val127 = _scope50;
			NameScope val128 = _scope51;
			NameScope val129 = _scope52;
			NameScope val130 = _scope53;
			NameScope val131 = _scope54;
			NameScope val132 = _scope55;
			NameScope val133 = _scope56;
			NameScope val134 = _scope57;
			NameScope val135 = _scope58;
			NameScope val136 = _scope59;
			NameScope val137 = _scope60;
			NameScope val138 = _scope61;
			NameScope val139 = _scope62;
			NameScope val140 = _scope63;
			NameScope val141 = _scope64;
			NameScope val142 = _scope65;
			NameScope val143 = _scope66;
			NameScope val144 = _scope67;
			NameScope val145 = _scope68;
			NameScope val146 = _scope69;
			NameScope val147 = _scope70;
			NameScope val148 = _scope71;
			NameScope val149 = _scope72;
			NameScope val150 = _scope73;
			NameScope val151 = _scope74;
			NameScope val152 = _scope75;
			NameScope val153 = _scope76;
			NameScope val154 = _scope77;
			NameScope val155 = _scope78;
			NameScope val156 = _scope79;
			NameScope val157 = _scope80;
			NameScope val158 = _scope81;
			NameScope val159 = _scope82;
			NameScope val160 = _scope83;
			NameScope val161 = _scope84;
			NameScope val162 = _scope85;
			NameScope val163 = _scope86;
			NameScope val164 = _scope87;
			NameScope val165 = _scope88;
			NameScope val166 = _scope89;
			NameScope val167 = _scope90;
			NameScope val168 = _scope91;
			NameScope val169 = _scope92;
			NameScope val170 = _scope93;
			NameScope val171 = _scope94;
			NameScope val172 = _scope95;
			NameScope val173 = _scope96;
			NameScope val174 = _scope97;
			NameScope val175 = _scope98;
			NameScope val176 = _scope99;
			NameScope val177 = _scope100;
			NameScope val178 = _scope101;
			BindingExtension val179 = new BindingExtension();
			Image val180 = new Image();
			BindingExtension val181 = new BindingExtension();
			StaticResourceExtension val182 = new StaticResourceExtension();
			StaticResourceExtension val183 = new StaticResourceExtension();
			AppThemeBindingExtension val184 = new AppThemeBindingExtension();
			Label val185 = new Label();
			Grid val186 = new Grid();
			NameScope val187 = new NameScope();
			NameScope.SetNameScope((BindableObject)(object)val186, (INameScope)(object)val187);
			((Element)val180).transientNamescope = (INameScope)(object)val187;
			((Element)val185).transientNamescope = (INameScope)(object)val187;
			((BindableObject)val186).SetValue(VisualElement.HeightRequestProperty, (object)52.0);
			((BindableObject)val186).SetValue(Layout.PaddingProperty, (object)new Thickness(20.0, 0.0));
			((BindableObject)val186).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
			{
				new ColumnDefinition(new GridLength(24.0)),
				new ColumnDefinition(GridLength.Star)
			}));
			((BindableObject)val186).SetValue(Grid.ColumnSpacingProperty, (object)16.0);
			((BindableObject)val180).SetValue(Grid.ColumnProperty, (object)0);
			val179.Path = "FlyoutIcon";
			val179.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BaseShellItem, ImageSource>((Func<BaseShellItem, ValueTuple<ImageSource, bool>>)((BaseShellItem P_0) => (P_0 != null) ? (P_0.FlyoutIcon, true) : default((ImageSource, bool))), (Action<BaseShellItem, ImageSource>)delegate(BaseShellItem P_0, ImageSource P_1)
			{
				if (P_0 != null)
				{
					P_0.FlyoutIcon = P_1;
				}
			}, new Tuple<Func<BaseShellItem, object>, string>[1]
			{
				new Tuple<Func<BaseShellItem, object>, string>((BaseShellItem P_0) => P_0, "FlyoutIcon")
			});
			((BindingBase)val179.TypedBinding).Mode = val179.Mode;
			val179.TypedBinding.Converter = val179.Converter;
			val179.TypedBinding.ConverterParameter = val179.ConverterParameter;
			((BindingBase)val179.TypedBinding).StringFormat = val179.StringFormat;
			val179.TypedBinding.Source = val179.Source;
			val179.TypedBinding.UpdateSourceEventName = val179.UpdateSourceEventName;
			((BindingBase)val179.TypedBinding).FallbackValue = val179.FallbackValue;
			((BindingBase)val179.TypedBinding).TargetNullValue = val179.TargetNullValue;
			BindingBase typedBinding = (BindingBase)(object)val179.TypedBinding;
			((BindableObject)val180).SetBinding(Image.SourceProperty, typedBinding);
			((BindableObject)val180).SetValue(VisualElement.HeightRequestProperty, (object)22.0);
			((BindableObject)val180).SetValue(VisualElement.WidthRequestProperty, (object)22.0);
			((BindableObject)val180).SetValue(Image.AspectProperty, (object)(Aspect)0);
			((BindableObject)val180).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((Layout)val186).Children.Add((IView)(object)val180);
			((BindableObject)val185).SetValue(Grid.ColumnProperty, (object)1);
			val181.Path = "Title";
			val181.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BaseShellItem, string>((Func<BaseShellItem, ValueTuple<string, bool>>)((BaseShellItem P_0) => (P_0 != null) ? (P_0.Title, true) : default((string, bool))), (Action<BaseShellItem, string>)delegate(BaseShellItem P_0, string P_1)
			{
				if (P_0 != null)
				{
					P_0.Title = P_1;
				}
			}, new Tuple<Func<BaseShellItem, object>, string>[1]
			{
				new Tuple<Func<BaseShellItem, object>, string>((BaseShellItem P_0) => P_0, "Title")
			});
			((BindingBase)val181.TypedBinding).Mode = val181.Mode;
			val181.TypedBinding.Converter = val181.Converter;
			val181.TypedBinding.ConverterParameter = val181.ConverterParameter;
			((BindingBase)val181.TypedBinding).StringFormat = val181.StringFormat;
			val181.TypedBinding.Source = val181.Source;
			val181.TypedBinding.UpdateSourceEventName = val181.UpdateSourceEventName;
			((BindingBase)val181.TypedBinding).FallbackValue = val181.FallbackValue;
			((BindingBase)val181.TypedBinding).TargetNullValue = val181.TargetNullValue;
			BindingBase typedBinding2 = (BindingBase)(object)val181.TypedBinding;
			((BindableObject)val185).SetBinding(Label.TextProperty, typedBinding2);
			((BindableObject)val185).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val185).SetValue(Label.FontSizeProperty, (object)16.0);
			val182.Key = "TextPrimaryLight";
			StaticResourceExtension val188 = new StaticResourceExtension
			{
				Key = "TextPrimaryLight"
			};
			XamlServiceProvider val189 = new XamlServiceProvider();
			Type? typeFromHandle12 = typeof(IProvideValueTarget);
			int length;
			object[] array13 = new object[(length = parentValues.Length) + 3];
			Array.Copy(parentValues, 0, array13, 3, length);
			array13[0] = val184;
			array13[1] = val185;
			array13[2] = val186;
			SimpleValueTargetProvider val190 = new SimpleValueTargetProvider(array13, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[6] { val187, val187, val187, val187, val114, val77 }, (object)root);
			object obj17 = (object)val190;
			val189.Add(typeFromHandle12, (object)val190);
			val189.Add(typeof(IReferenceProvider), obj17);
			val189.Add(typeof(IRootObjectProvider), obj17);
			val189.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(53, 24)));
			object light4 = val188.ProvideValue((IServiceProvider)val189);
			val184.Light = light4;
			val183.Key = "TextPrimaryDark";
			StaticResourceExtension val191 = new StaticResourceExtension
			{
				Key = "TextPrimaryDark"
			};
			XamlServiceProvider val192 = new XamlServiceProvider();
			Type? typeFromHandle13 = typeof(IProvideValueTarget);
			int length2;
			object[] array14 = new object[(length2 = parentValues.Length) + 3];
			Array.Copy(parentValues, 0, array14, 3, length2);
			array14[0] = val184;
			array14[1] = val185;
			array14[2] = val186;
			SimpleValueTargetProvider val193 = new SimpleValueTargetProvider(array14, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[6] { val187, val187, val187, val187, val114, val77 }, (object)root);
			object obj18 = (object)val193;
			val192.Add(typeFromHandle13, (object)val193);
			val192.Add(typeof(IReferenceProvider), obj18);
			val192.Add(typeof(IRootObjectProvider), obj18);
			val192.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(53, 24)));
			object dark4 = val191.ProvideValue((IServiceProvider)val192);
			val184.Dark = dark4;
			XamlServiceProvider val194 = new XamlServiceProvider();
			val194.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val185, (object)Label.TextColorProperty));
			val194.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(53, 24)));
			BindingBase val195 = ((IMarkupExtension<BindingBase>)(object)val184).ProvideValue((IServiceProvider)val194);
			((BindableObject)val185).SetBinding(Label.TextColorProperty, val195);
			((Layout)val186).Children.Add((IView)(object)val185);
			return val186;
		};
		((BindableObject)appShell).SetValue(Shell.ItemTemplateProperty, (object)val15);
		val16.Key = "Primary";
		StaticResourceExtension val68 = new StaticResourceExtension
		{
			Key = "Primary"
		};
		XamlServiceProvider val69 = new XamlServiceProvider();
		Type? typeFromHandle9 = typeof(IProvideValueTarget);
		object[] array10 = new object[0 + 2];
		array10[0] = val21;
		array10[1] = appShell;
		SimpleValueTargetProvider val70 = new SimpleValueTargetProvider(array10, (object)VisualElement.BackgroundColorProperty, (INameScope[])(object)new NameScope[3] { val37, val37, val37 }, (object)appShell);
		object obj11 = (object)val70;
		val69.Add(typeFromHandle9, (object)val70);
		val69.Add(typeof(IReferenceProvider), obj11);
		val69.Add(typeof(IRootObjectProvider), obj11);
		val69.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(60, 15)));
		object obj12 = val68.ProvideValue((IServiceProvider)val69);
		((BindableObject)val21).SetValue(VisualElement.BackgroundColorProperty, (obj12 == null || !typeof(BindingBase).IsAssignableFrom(obj12.GetType())) ? obj12 : obj12);
		((BindableObject)val21).SetValue(VisualElement.HeightRequestProperty, (object)150.0);
		((BindableObject)val21).SetValue(Layout.PaddingProperty, (object)new Thickness(20.0, 40.0, 20.0, 20.0));
		((BindableObject)val20).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val20).SetValue(StackBase.SpacingProperty, (object)6.0);
		((BindableObject)val17).SetValue(Image.SourceProperty, (object)ImageSource.FromFile("app_logo.svg"));
		((BindableObject)val17).SetValue(VisualElement.HeightRequestProperty, (object)52.0);
		((BindableObject)val17).SetValue(VisualElement.WidthRequestProperty, (object)52.0);
		((BindableObject)val17).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Start);
		((Layout)val20).Children.Add((IView)(object)val17);
		((BindableObject)val19).SetValue(Label.TextProperty, (object)"SMS Forwarder");
		((BindableObject)val19).SetValue(Label.FontSizeProperty, (object)22.0);
		((BindableObject)val19).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		val18.Key = "OnPrimary";
		StaticResourceExtension val71 = new StaticResourceExtension
		{
			Key = "OnPrimary"
		};
		XamlServiceProvider val72 = new XamlServiceProvider();
		Type? typeFromHandle10 = typeof(IProvideValueTarget);
		object[] array11 = new object[0 + 4];
		array11[0] = val19;
		array11[1] = val20;
		array11[2] = val21;
		array11[3] = appShell;
		SimpleValueTargetProvider val73 = new SimpleValueTargetProvider(array11, (object)Label.TextColorProperty, (INameScope[])(object)new NameScope[5] { val37, val37, val37, val37, val37 }, (object)appShell);
		object obj13 = (object)val73;
		val72.Add(typeFromHandle10, (object)val73);
		val72.Add(typeof(IReferenceProvider), obj13);
		val72.Add(typeof(IRootObjectProvider), obj13);
		val72.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(73, 24)));
		object obj14 = val71.ProvideValue((IServiceProvider)val72);
		((BindableObject)val19).SetValue(Label.TextColorProperty, (obj14 == null || !typeof(BindingBase).IsAssignableFrom(obj14.GetType())) ? obj14 : obj14);
		((BindableObject)val19).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Start);
		((Layout)val20).Children.Add((IView)(object)val19);
		((Layout)val21).Children.Add((IView)(object)val20);
		((BindableObject)appShell).SetValue(Shell.FlyoutHeaderProperty, (object)val21);
		((BindableObject)val24).SetValue(Layout.PaddingProperty, (object)new Thickness(20.0, 12.0));
		((BindableObject)val23).SetValue(Label.TextProperty, (object)"v2026.07.31.0");
		((BindableObject)val23).SetValue(Label.FontSizeProperty, (object)12.0);
		val22.Key = "HintText";
		StaticResourceExtension val74 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val75 = new XamlServiceProvider();
		Type? typeFromHandle11 = typeof(IProvideValueTarget);
		object[] array12 = new object[0 + 3];
		array12[0] = val23;
		array12[1] = val24;
		array12[2] = appShell;
		SimpleValueTargetProvider val76 = new SimpleValueTargetProvider(array12, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[4] { val37, val37, val37, val37 }, (object)appShell);
		object obj15 = (object)val76;
		val75.Add(typeFromHandle11, (object)val76);
		val75.Add(typeof(IReferenceProvider), obj15);
		val75.Add(typeof(IRootObjectProvider), obj15);
		val75.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(121, 20)));
		object obj16 = val74.ProvideValue((IServiceProvider)val75);
		((BindableObject)val23).SetValue(VisualElement.StyleProperty, (obj16 == null || !typeof(BindingBase).IsAssignableFrom(obj16.GetType())) ? obj16 : obj16);
		((BindableObject)val23).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val23).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val24).Children.Add((IView)(object)val23);
		((BindableObject)appShell).SetValue(Shell.FlyoutFooterProperty, (object)val24);
		((BindableObject)val27).SetValue(BaseShellItem.TitleProperty, (object)"Configuración");
		((BindableObject)val27).SetValue(BaseShellItem.FlyoutIconProperty, (object)ImageSource.FromFile("settings.svg"));
		((BindableObject)val26).SetValue(BaseShellItem.TitleProperty, (object)"Configuración");
		((BindableObject)val26).SetValue(ShellContent.ContentTemplateProperty, (object)val25);
		((BaseShellItem)val26).Route = "MainPage";
		((ICollection<ShellSection>)((BindableObject)val27).GetValue(ShellItem.ItemsProperty)).Add(ShellSection.op_Implicit(val26));
		((ICollection<ShellItem>)((BindableObject)appShell).GetValue(Shell.ItemsProperty)).Add((ShellItem)(object)val27);
		((BindableObject)val30).SetValue(BaseShellItem.TitleProperty, (object)"Mensajes");
		((BindableObject)val30).SetValue(BaseShellItem.FlyoutIconProperty, (object)ImageSource.FromFile("messages.svg"));
		((BindableObject)val29).SetValue(BaseShellItem.TitleProperty, (object)"Mensajes");
		((BindableObject)val29).SetValue(ShellContent.ContentTemplateProperty, (object)val28);
		((BaseShellItem)val29).Route = "MessagesPage";
		((ICollection<ShellSection>)((BindableObject)val30).GetValue(ShellItem.ItemsProperty)).Add(ShellSection.op_Implicit(val29));
		((ICollection<ShellItem>)((BindableObject)appShell).GetValue(Shell.ItemsProperty)).Add((ShellItem)(object)val30);
		((BindableObject)val33).SetValue(BaseShellItem.TitleProperty, (object)"Diagnósticos");
		((BindableObject)val33).SetValue(BaseShellItem.FlyoutIconProperty, (object)ImageSource.FromFile("diagnostics.svg"));
		((BindableObject)val32).SetValue(BaseShellItem.TitleProperty, (object)"Diagnósticos");
		((BindableObject)val32).SetValue(ShellContent.ContentTemplateProperty, (object)val31);
		((BaseShellItem)val32).Route = "DiagnosticsPage";
		((ICollection<ShellSection>)((BindableObject)val33).GetValue(ShellItem.ItemsProperty)).Add(ShellSection.op_Implicit(val32));
		((ICollection<ShellItem>)((BindableObject)appShell).GetValue(Shell.ItemsProperty)).Add((ShellItem)(object)val33);
		((BindableObject)val36).SetValue(BaseShellItem.TitleProperty, (object)"Acerca de");
		((BindableObject)val36).SetValue(BaseShellItem.FlyoutIconProperty, (object)ImageSource.FromFile("about.svg"));
		((BindableObject)val35).SetValue(BaseShellItem.TitleProperty, (object)"Acerca de");
		((BindableObject)val35).SetValue(ShellContent.ContentTemplateProperty, (object)val34);
		((BaseShellItem)val35).Route = "AboutPage";
		((ICollection<ShellSection>)((BindableObject)val36).GetValue(ShellItem.ItemsProperty)).Add(ShellSection.op_Implicit(val35));
		((ICollection<ShellItem>)((BindableObject)appShell).GetValue(Shell.ItemsProperty)).Add((ShellItem)(object)val36);
	}
}
