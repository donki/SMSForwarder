using System;
using System.CodeDom.Compiler;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Xaml;
using __XamlGeneratedCode__;

namespace SMSForwarder;

[XamlFilePath("App.xaml")]
public class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new Window((Page)(object)new AppShell());
	}

	public static void NavigateToMainApp()
	{
		Application current = Application.Current;
		Window val = ((current == null) ? null : current.Windows?.FirstOrDefault());
		if (val != null)
		{
			val.Page = (Page)(object)new AppShell();
		}
	}

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		ResourceDictionary val = new ResourceDictionary();
		ResourceDictionary val2 = new ResourceDictionary();
		ResourceDictionary val3 = new ResourceDictionary();
		App app;
		NameScope val4 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(app = this))) ?? ((object)new NameScope()));
		NameScope.SetNameScope((BindableObject)(object)app, (INameScope)(object)val4);
		((Application)app).Resources = val3;
		Uri uri = new Uri("Resources/Styles/Colors.xaml;assembly=SMSForwarder", UriKind.RelativeOrAbsolute);
		val.SetAndCreateSource<__Type7ADD195249298C8C>(uri);
		val.Source = uri;
		val3.MergedDictionaries.Add(val);
		Uri uri2 = new Uri("Resources/Styles/Styles.xaml;assembly=SMSForwarder", UriKind.RelativeOrAbsolute);
		val2.SetAndCreateSource<__Type4D64B3AA5FB73812>(uri2);
		val2.Source = uri2;
		val3.MergedDictionaries.Add(val2);
		((Application)app).Resources = val3;
	}
}
