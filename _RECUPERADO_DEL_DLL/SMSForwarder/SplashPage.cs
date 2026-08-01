using System;
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Xaml.Internals;
using Microsoft.Maui.Graphics;
using SMSForwarder.Services;
using SocShared;

namespace SMSForwarder;

[XamlFilePath("SplashPage.xaml")]
public class SplashPage : ContentPage
{
	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Border LogoFrame;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label TitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label SubtitleLabel;

	public SplashPage()
	{
		InitializeComponent();
		StartAnimations();
		NavigateToMainPage();
	}

	private async void StartAnimations()
	{
		((VisualElement)LogoFrame).Scale = 0.0;
		await ViewExtensions.ScaleTo((VisualElement)(object)LogoFrame, 1.0, 800u, Easing.BounceOut);
		((VisualElement)TitleLabel).Opacity = 0.0;
		await ViewExtensions.FadeTo((VisualElement)(object)TitleLabel, 1.0, 600u, (Easing)null);
		((VisualElement)SubtitleLabel).Opacity = 0.0;
		await ViewExtensions.FadeTo((VisualElement)(object)SubtitleLabel, 1.0, 600u, (Easing)null);
	}

	private async void NavigateToMainPage()
	{
		await Task.Delay(3000);
		try
		{
			new PermissionService();
			PermissionStatus smsReceiveStatus = await ((BasePermission)new SmsPermissions.ReceiveSms()).CheckStatusAsync();
			PermissionStatus val = await ((BasePermission)new SmsPermissions.SendSms()).CheckStatusAsync();
			if (((int)smsReceiveStatus != 3 || (int)val != 3) && await ModernDialog.AlertAsync((Page)(object)this, "Permisos Requeridos", "SMS Forwarder necesita permisos SMS para funcionar. ¿Desea concederlos ahora?", "Sí", "Ahora no"))
			{
				await ((BasePermission)new SmsPermissions.ReceiveSms()).RequestAsync();
				await ((BasePermission)new SmsPermissions.SendSms()).RequestAsync();
			}
		}
		catch (Exception)
		{
		}
		App.NavigateToMainApp();
	}

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	[MemberNotNull("LogoFrame")]
	[MemberNotNull("TitleLabel")]
	[MemberNotNull("SubtitleLabel")]
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
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_0202: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0306: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_034d: Expected O, but got Unknown
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_04a6: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04ed: Expected O, but got Unknown
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Expected O, but got Unknown
		//IL_06c6: Expected O, but got Unknown
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Expected O, but got Unknown
		//IL_070d: Expected O, but got Unknown
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_08c2: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Expected O, but got Unknown
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Expected O, but got Unknown
		//IL_0909: Expected O, but got Unknown
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Expected O, but got Unknown
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		StaticResourceExtension val = new StaticResourceExtension();
		StaticResourceExtension val2 = new StaticResourceExtension();
		StaticResourceExtension val3 = new StaticResourceExtension();
		Shadow val4 = new Shadow();
		Image val5 = new Image();
		Border val6 = new Border();
		StaticResourceExtension val7 = new StaticResourceExtension();
		Label val8 = new Label();
		Label val9 = new Label();
		StaticResourceExtension val10 = new StaticResourceExtension();
		ActivityIndicator val11 = new ActivityIndicator();
		VerticalStackLayout val12 = new VerticalStackLayout();
		VerticalStackLayout val13 = new VerticalStackLayout();
		Label val14 = new Label();
		Grid val15 = new Grid();
		SplashPage splashPage;
		NameScope val16 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(splashPage = this))) ?? ((object)new NameScope()));
		NameScope.SetNameScope((BindableObject)(object)splashPage, (INameScope)(object)val16);
		((Element)val15).transientNamescope = (INameScope)(object)val16;
		((Element)val13).transientNamescope = (INameScope)(object)val16;
		((Element)val6).transientNamescope = (INameScope)(object)val16;
		((INameScope)val16).RegisterName("LogoFrame", (object)val6);
		if (((Element)val6).StyleId == null)
		{
			((Element)val6).StyleId = "LogoFrame";
		}
		((Element)val4).transientNamescope = (INameScope)(object)val16;
		((Element)val5).transientNamescope = (INameScope)(object)val16;
		((Element)val8).transientNamescope = (INameScope)(object)val16;
		((INameScope)val16).RegisterName("TitleLabel", (object)val8);
		if (((Element)val8).StyleId == null)
		{
			((Element)val8).StyleId = "TitleLabel";
		}
		((Element)val9).transientNamescope = (INameScope)(object)val16;
		((INameScope)val16).RegisterName("SubtitleLabel", (object)val9);
		if (((Element)val9).StyleId == null)
		{
			((Element)val9).StyleId = "SubtitleLabel";
		}
		((Element)val12).transientNamescope = (INameScope)(object)val16;
		((Element)val11).transientNamescope = (INameScope)(object)val16;
		((Element)val14).transientNamescope = (INameScope)(object)val16;
		LogoFrame = val6;
		TitleLabel = val8;
		SubtitleLabel = val9;
		val.Key = "Primary";
		StaticResourceExtension val17 = new StaticResourceExtension
		{
			Key = "Primary"
		};
		XamlServiceProvider val18 = new XamlServiceProvider();
		Type? typeFromHandle = typeof(IProvideValueTarget);
		object[] array = new object[0 + 1];
		array[0] = splashPage;
		SimpleValueTargetProvider val19 = new SimpleValueTargetProvider(array, (object)VisualElement.BackgroundColorProperty, (INameScope[])(object)new NameScope[2] { val16, val16 }, (object)splashPage);
		object obj = (object)val19;
		val18.Add(typeFromHandle, (object)val19);
		val18.Add(typeof(IReferenceProvider), obj);
		val18.Add(typeof(IRootObjectProvider), obj);
		val18.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(5, 14)));
		object obj2 = val17.ProvideValue((IServiceProvider)val18);
		((BindableObject)splashPage).SetValue(VisualElement.BackgroundColorProperty, (obj2 == null || !typeof(BindingBase).IsAssignableFrom(obj2.GetType())) ? obj2 : obj2);
		((BindableObject)splashPage).SetValue(NavigationPage.HasNavigationBarProperty, (object)false);
		((BindableObject)val13).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val13).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val13).SetValue(StackBase.SpacingProperty, (object)24.0);
		val2.Key = "White";
		StaticResourceExtension val20 = new StaticResourceExtension
		{
			Key = "White"
		};
		XamlServiceProvider val21 = new XamlServiceProvider();
		Type? typeFromHandle2 = typeof(IProvideValueTarget);
		object[] array2 = new object[0 + 4];
		array2[0] = val6;
		array2[1] = val13;
		array2[2] = val15;
		array2[3] = splashPage;
		SimpleValueTargetProvider val22 = new SimpleValueTargetProvider(array2, (object)VisualElement.BackgroundColorProperty, (INameScope[])(object)new NameScope[5] { val16, val16, val16, val16, val16 }, (object)splashPage);
		object obj3 = (object)val22;
		val21.Add(typeFromHandle2, (object)val22);
		val21.Add(typeof(IReferenceProvider), obj3);
		val21.Add(typeof(IRootObjectProvider), obj3);
		val21.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(12, 21)));
		object obj4 = val20.ProvideValue((IServiceProvider)val21);
		((BindableObject)val6).SetValue(VisualElement.BackgroundColorProperty, (obj4 == null || !typeof(BindingBase).IsAssignableFrom(obj4.GetType())) ? obj4 : obj4);
		((BindableObject)val6).SetValue(Border.StrokeThicknessProperty, (object)0.0);
		((BindableObject)val6).SetValue(Border.StrokeShapeProperty, (object)new RoundRectangle
		{
			CornerRadius = new CornerRadius(20.0, 20.0, 20.0, 20.0)
		});
		((BindableObject)val6).SetValue(VisualElement.WidthRequestProperty, (object)120.0);
		((BindableObject)val6).SetValue(VisualElement.HeightRequestProperty, (object)120.0);
		((BindableObject)val6).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		val3.Key = "Black";
		StaticResourceExtension val23 = new StaticResourceExtension
		{
			Key = "Black"
		};
		XamlServiceProvider val24 = new XamlServiceProvider();
		Type? typeFromHandle3 = typeof(IProvideValueTarget);
		object[] array3 = new object[0 + 5];
		array3[0] = val4;
		array3[1] = val6;
		array3[2] = val13;
		array3[3] = val15;
		array3[4] = splashPage;
		SimpleValueTargetProvider val25 = new SimpleValueTargetProvider(array3, (object)Shadow.BrushProperty, (INameScope[])(object)new NameScope[6] { val16, val16, val16, val16, val16, val16 }, (object)splashPage);
		object obj5 = (object)val25;
		val24.Add(typeFromHandle3, (object)val25);
		val24.Add(typeof(IReferenceProvider), obj5);
		val24.Add(typeof(IRootObjectProvider), obj5);
		val24.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(20, 29)));
		object obj6 = val23.ProvideValue((IServiceProvider)val24);
		((BindableObject)val4).SetValue(Shadow.BrushProperty, (obj6 == null || !typeof(BindingBase).IsAssignableFrom(obj6.GetType())) ? obj6 : obj6);
		((BindableObject)val4).SetValue(Shadow.OpacityProperty, (object)0.2f);
		((BindableObject)val4).SetValue(Shadow.RadiusProperty, (object)15f);
		((BindableObject)val4).SetValue(Shadow.OffsetProperty, (object)new Point(0.0, 8.0));
		((BindableObject)val6).SetValue(VisualElement.ShadowProperty, (object)val4);
		((BindableObject)val5).SetValue(Image.SourceProperty, (object)ImageSource.FromFile("smsforwadericon.png"));
		((BindableObject)val5).SetValue(VisualElement.WidthRequestProperty, (object)80.0);
		((BindableObject)val5).SetValue(VisualElement.HeightRequestProperty, (object)80.0);
		((BindableObject)val5).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val5).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val6).SetValue(Border.ContentProperty, (object)val5);
		((Layout)val13).Children.Add((IView)(object)val6);
		((BindableObject)val8).SetValue(Label.TextProperty, (object)"SMS Forwarder");
		((BindableObject)val8).SetValue(Label.FontSizeProperty, (object)28.0);
		((BindableObject)val8).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		val7.Key = "White";
		StaticResourceExtension val26 = new StaticResourceExtension
		{
			Key = "White"
		};
		XamlServiceProvider val27 = new XamlServiceProvider();
		Type? typeFromHandle4 = typeof(IProvideValueTarget);
		object[] array4 = new object[0 + 4];
		array4[0] = val8;
		array4[1] = val13;
		array4[2] = val15;
		array4[3] = splashPage;
		SimpleValueTargetProvider val28 = new SimpleValueTargetProvider(array4, (object)Label.TextColorProperty, (INameScope[])(object)new NameScope[5] { val16, val16, val16, val16, val16 }, (object)splashPage);
		object obj7 = (object)val28;
		val27.Add(typeFromHandle4, (object)val28);
		val27.Add(typeof(IReferenceProvider), obj7);
		val27.Add(typeof(IRootObjectProvider), obj7);
		val27.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(32, 20)));
		object obj8 = val26.ProvideValue((IServiceProvider)val27);
		((BindableObject)val8).SetValue(Label.TextColorProperty, (obj8 == null || !typeof(BindingBase).IsAssignableFrom(obj8.GetType())) ? obj8 : obj8);
		((BindableObject)val8).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val13).Children.Add((IView)(object)val8);
		((BindableObject)val9).SetValue(Label.TextProperty, (object)"Reenvío inteligente");
		((BindableObject)val9).SetValue(Label.FontSizeProperty, (object)14.0);
		((BindableObject)val9).SetValue(Label.TextColorProperty, (object)new Color(0.8901961f, 0.9411765f, 84f / 85f, 1f));
		((BindableObject)val9).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val13).Children.Add((IView)(object)val9);
		((BindableObject)val12).SetValue(StackBase.SpacingProperty, (object)12.0);
		((BindableObject)val12).SetValue(View.MarginProperty, (object)new Thickness(0.0, 32.0, 0.0, 0.0));
		((BindableObject)val11).SetValue(ActivityIndicator.IsRunningProperty, (object)true);
		val10.Key = "White";
		StaticResourceExtension val29 = new StaticResourceExtension
		{
			Key = "White"
		};
		XamlServiceProvider val30 = new XamlServiceProvider();
		Type? typeFromHandle5 = typeof(IProvideValueTarget);
		object[] array5 = new object[0 + 5];
		array5[0] = val11;
		array5[1] = val12;
		array5[2] = val13;
		array5[3] = val15;
		array5[4] = splashPage;
		SimpleValueTargetProvider val31 = new SimpleValueTargetProvider(array5, (object)ActivityIndicator.ColorProperty, (INameScope[])(object)new NameScope[6] { val16, val16, val16, val16, val16, val16 }, (object)splashPage);
		object obj9 = (object)val31;
		val30.Add(typeFromHandle5, (object)val31);
		val30.Add(typeof(IReferenceProvider), obj9);
		val30.Add(typeof(IRootObjectProvider), obj9);
		val30.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(44, 36)));
		object obj10 = val29.ProvideValue((IServiceProvider)val30);
		((BindableObject)val11).SetValue(ActivityIndicator.ColorProperty, (obj10 == null || !typeof(BindingBase).IsAssignableFrom(obj10.GetType())) ? obj10 : obj10);
		((BindableObject)val11).SetValue(VisualElement.WidthRequestProperty, (object)32.0);
		((BindableObject)val11).SetValue(VisualElement.HeightRequestProperty, (object)32.0);
		((BindableObject)val11).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val12).Children.Add((IView)(object)val11);
		((Layout)val13).Children.Add((IView)(object)val12);
		((Layout)val15).Children.Add((IView)(object)val13);
		((BindableObject)val14).SetValue(Label.TextProperty, (object)"sOCratic");
		((BindableObject)val14).SetValue(Label.FontSizeProperty, (object)12.0);
		((BindableObject)val14).SetValue(Label.TextColorProperty, (object)new Color(40f / 51f, 76f / 85f, 50f / 51f, 1f));
		((BindableObject)val14).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val14).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.End);
		((BindableObject)val14).SetValue(View.MarginProperty, (object)new Thickness(0.0, 0.0, 0.0, 40.0));
		((Layout)val15).Children.Add((IView)(object)val14);
		((BindableObject)splashPage).SetValue(ContentPage.ContentProperty, (object)val15);
	}
}
