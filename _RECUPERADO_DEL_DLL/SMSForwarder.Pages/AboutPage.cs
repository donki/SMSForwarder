using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Android.App;
using Android.Content;
using Android.Net;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Xaml.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;
using SocShared;

namespace SMSForwarder.Pages;

[XamlFilePath("Pages\\AboutPage.xaml")]
public class AboutPage : ContentPage
{
	private const string ContactEmail = "jsoladelarosa@gmail.com";

	private const string DonationUrl = "https://ko-fi.com/josepsola";

	private const string EmailSubject = "Contacto desde SMS Forwarder";

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label VersionLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label ContactTitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label ContactInstructionLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label SupportTitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button DonationButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label SupportDescLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LanguageTitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button SpanishButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button EnglishButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LanguageDescLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label PrivacyTitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label PrivacyTextLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LicenseTitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LicenseTextLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LegalTitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LegalText1Label;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LegalText2Label;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label WarningLabel;

	private static string AppVersion => AppInfo.Current.VersionString;

	public AboutPage()
	{
		InitializeComponent();
		ApplyLanguage(GetCurrentLanguage());
	}

	private static string GetCurrentLanguage()
	{
		return Preferences.Get("AppLanguage", GetSystemLanguage());
	}

	private static string GetSystemLanguage()
	{
		if (!(CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "es"))
		{
			return "en";
		}
		return "es";
	}

	private void ApplyLanguage(string language)
	{
		bool flag = language == "es";
		if (flag)
		{
			SetSpanishTexts();
		}
		else
		{
			SetEnglishTexts();
		}
		UpdateLanguageButtons(flag);
	}

	private void SetSpanishTexts()
	{
		((Page)this).Title = "Acerca de";
		VersionLabel.Text = "Versión " + AppVersion;
		ContactTitleLabel.Text = "Contacto";
		ContactInstructionLabel.Text = "Toca para enviar un correo electrónico";
		SupportTitleLabel.Text = "Apoya el Desarrollo";
		DonationButton.Text = "Ko-fi.com - Invítame un café";
		SupportDescLabel.Text = "Tu apoyo ayuda a mantener y mejorar la aplicación";
		LanguageTitleLabel.Text = "Idioma";
		LanguageDescLabel.Text = "Selecciona tu idioma preferido";
		PrivacyTitleLabel.Text = "Privacidad";
		PrivacyTextLabel.Text = "Esta aplicación no recopila tus datos personales ni los envía a los desarrolladores. La información se procesa en tu dispositivo para la función propia de la app.";
		LicenseTitleLabel.Text = "Licencia";
		LicenseTextLabel.Text = "Esta aplicación es software libre distribuido bajo licencia MIT.";
		LegalTitleLabel.Text = "Aviso Legal";
		LegalText1Label.Text = "Este software se proporciona «tal cual», sin garantías de ningún tipo. El usuario es responsable del uso adecuado de la aplicación y del cumplimiento de las leyes locales.";
		LegalText2Label.Text = "En ningún caso los autores serán responsables de daños directos, indirectos, incidentales o consecuentes que resulten del uso de este software.";
		WarningLabel.Text = "⚠\ufe0f Uso bajo su propio riesgo";
	}

	private void SetEnglishTexts()
	{
		((Page)this).Title = "About";
		VersionLabel.Text = "Version " + AppVersion;
		ContactTitleLabel.Text = "Contact";
		ContactInstructionLabel.Text = "Tap to send an email";
		SupportTitleLabel.Text = "Support Development";
		DonationButton.Text = "Ko-fi.com - Buy me a coffee";
		SupportDescLabel.Text = "Your support helps maintain and improve the app";
		LanguageTitleLabel.Text = "Language";
		LanguageDescLabel.Text = "Select your preferred language";
		PrivacyTitleLabel.Text = "Privacy";
		PrivacyTextLabel.Text = "This app does not collect your personal data or send it to the developers. Information is processed on your device for the app's own purpose.";
		LicenseTitleLabel.Text = "License";
		LicenseTextLabel.Text = "This app is free software distributed under the MIT license.";
		LegalTitleLabel.Text = "Legal Notice";
		LegalText1Label.Text = "This software is provided 'as is', without warranty of any kind. The user is responsible for proper use of the app and compliance with local laws.";
		LegalText2Label.Text = "In no event shall the authors be liable for any direct, indirect, incidental or consequential damages arising from the use of this software.";
		WarningLabel.Text = "⚠\ufe0f Use at your own risk";
	}

	private void UpdateLanguageButtons(bool spanishActive)
	{
		Style resourceStyle = GetResourceStyle("PrimaryButton");
		Style resourceStyle2 = GetResourceStyle("OutlineButton");
		if (resourceStyle != null && resourceStyle2 != null)
		{
			((StyleableElement)SpanishButton).Style = (spanishActive ? resourceStyle : resourceStyle2);
			((StyleableElement)EnglishButton).Style = (spanishActive ? resourceStyle2 : resourceStyle);
		}
	}

	private static Style? GetResourceStyle(string key)
	{
		Application current = Application.Current;
		object obj = default(object);
		if (current != null && current.Resources.TryGetValue(key, ref obj))
		{
			Style val = (Style)((obj is Style) ? obj : null);
			if (val != null)
			{
				return val;
			}
		}
		return null;
	}

	private void OnSpanishClicked(object? sender, EventArgs e)
	{
		Preferences.Set("AppLanguage", "es");
		ApplyLanguage("es");
	}

	private void OnEnglishClicked(object? sender, EventArgs e)
	{
		Preferences.Set("AppLanguage", "en");
		ApplyLanguage("en");
	}

	private async void OnContactEmailClicked(object? sender, EventArgs e)
	{
		int num = 0;
		try
		{
			string currentLanguage = GetCurrentLanguage();
			string value = "SMS Forwarder";
			string appVersion = AppVersion;
			string text = ((currentLanguage == "es") ? $"Hola,\n\nMe pongo en contacto desde la aplicación {value} (versión {appVersion}).\n\n[Escribe tu mensaje aquí]\n\nSaludos." : $"Hello,\n\nI'm contacting you from the {value} app (version {appVersion}).\n\n[Write your message here]\n\nBest regards.");
			Context val = (Context)(((object)Platform.CurrentActivity) ?? ((object)Application.Context));
			if (val != null)
			{
				Intent val2 = new Intent("android.intent.action.SENDTO");
				val2.SetData(Uri.Parse("mailto:jsoladelarosa@gmail.com"));
				val2.PutExtra("android.intent.extra.SUBJECT", "Contacto desde SMS Forwarder");
				val2.PutExtra("android.intent.extra.TEXT", text);
				string text2 = ((currentLanguage == "es") ? "Enviar email con:" : "Send email with:");
				Intent val3 = Intent.CreateChooser(val2, text2);
				if (val3 != null)
				{
					val3.AddFlags((ActivityFlags)268435456);
					val.StartActivity(val3);
					return;
				}
			}
			await Email.ComposeAsync(new EmailMessage
			{
				Subject = "Contacto desde SMS Forwarder",
				Body = text,
				To = new List<string> { "jsoladelarosa@gmail.com" }
			});
		}
		catch (FeatureNotSupportedException)
		{
			num = 1;
		}
		catch (Exception ex2)
		{
			string currentLanguage2 = GetCurrentLanguage();
			string title = ((currentLanguage2 == "es") ? "Error" : "Error");
			string message = ((currentLanguage2 == "es") ? ("No se pudo abrir el cliente de correo: " + ex2.Message) : ("Could not open email client: " + ex2.Message));
			await ModernDialog.AlertAsync((Page)(object)this, title, message, "OK");
			return;
		}
		if (num == 1)
		{
			string currentLanguage3 = GetCurrentLanguage();
			string title2 = ((currentLanguage3 == "es") ? "Error" : "Error");
			string message2 = ((currentLanguage3 == "es") ? "Cliente de correo no disponible en este dispositivo" : "Email client not available on this device");
			await ModernDialog.AlertAsync((Page)(object)this, title2, message2, "OK");
		}
	}

	private async void OnDonationClicked(object? sender, EventArgs e)
	{
		int num = 0;
		try
		{
			Uri uri = new Uri("https://ko-fi.com/josepsola");
			BrowserLaunchOptions val = new BrowserLaunchOptions
			{
				LaunchMode = (BrowserLaunchMode)0,
				TitleMode = (BrowserTitleMode)1,
				PreferredToolbarColor = Color.FromArgb("#E67E22"),
				PreferredControlColor = Color.FromArgb("#FFFFFF")
			};
			await Browser.OpenAsync(uri, val);
		}
		catch (FeatureNotSupportedException)
		{
			num = 1;
		}
		catch (Exception ex2)
		{
			try
			{
				await Clipboard.SetTextAsync("https://ko-fi.com/josepsola");
				string currentLanguage = GetCurrentLanguage();
				string title = ((currentLanguage == "es") ? "Error al abrir enlace" : "Error opening link");
				string message = ((currentLanguage == "es") ? ("No se pudo abrir el navegador (" + ex2.Message + "), enlace copiado al portapapeles.") : ("Could not open browser (" + ex2.Message + "), link copied to clipboard."));
				await ModernDialog.AlertAsync((Page)(object)this, title, message, "OK");
			}
			catch
			{
				string currentLanguage2 = GetCurrentLanguage();
				string title2 = ((currentLanguage2 == "es") ? "Error" : "Error");
				string message2 = ((currentLanguage2 == "es") ? "No se pudo abrir el enlace: https://ko-fi.com/josepsola" : "Could not open link: https://ko-fi.com/josepsola");
				await ModernDialog.AlertAsync((Page)(object)this, title2, message2, "OK");
			}
			return;
		}
		if (num == 1)
		{
			try
			{
				await Clipboard.SetTextAsync("https://ko-fi.com/josepsola");
				string currentLanguage3 = GetCurrentLanguage();
				string title3 = ((currentLanguage3 == "es") ? "Navegador no disponible" : "Browser not available");
				string message3 = ((currentLanguage3 == "es") ? "Enlace copiado al portapapeles:\nhttps://ko-fi.com/josepsola" : "Link copied to clipboard:\nhttps://ko-fi.com/josepsola");
				await ModernDialog.AlertAsync((Page)(object)this, title3, message3, "OK");
			}
			catch
			{
				string currentLanguage4 = GetCurrentLanguage();
				string title4 = ((currentLanguage4 == "es") ? "Error" : "Error");
				string message4 = ((currentLanguage4 == "es") ? "No se pudo abrir el navegador: https://ko-fi.com/josepsola" : "Could not open browser: https://ko-fi.com/josepsola");
				await ModernDialog.AlertAsync((Page)(object)this, title4, message4, "OK");
			}
		}
	}

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	[MemberNotNull("VersionLabel")]
	[MemberNotNull("ContactTitleLabel")]
	[MemberNotNull("ContactInstructionLabel")]
	[MemberNotNull("SupportTitleLabel")]
	[MemberNotNull("DonationButton")]
	[MemberNotNull("SupportDescLabel")]
	[MemberNotNull("LanguageTitleLabel")]
	[MemberNotNull("SpanishButton")]
	[MemberNotNull("EnglishButton")]
	[MemberNotNull("LanguageDescLabel")]
	[MemberNotNull("PrivacyTitleLabel")]
	[MemberNotNull("PrivacyTextLabel")]
	[MemberNotNull("LicenseTitleLabel")]
	[MemberNotNull("LicenseTextLabel")]
	[MemberNotNull("LegalTitleLabel")]
	[MemberNotNull("LegalText1Label")]
	[MemberNotNull("LegalText2Label")]
	[MemberNotNull("WarningLabel")]
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
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Expected O, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c8: Expected O, but got Unknown
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Expected O, but got Unknown
		//IL_080f: Expected O, but got Unknown
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cb: Expected O, but got Unknown
		//IL_09d0: Expected O, but got Unknown
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Expected O, but got Unknown
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Expected O, but got Unknown
		//IL_0a17: Expected O, but got Unknown
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac4: Expected O, but got Unknown
		//IL_0ac9: Expected O, but got Unknown
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Expected O, but got Unknown
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Expected O, but got Unknown
		//IL_0b10: Expected O, but got Unknown
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Expected O, but got Unknown
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Expected O, but got Unknown
		//IL_0b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Expected O, but got Unknown
		//IL_0b61: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c30: Expected O, but got Unknown
		//IL_0c35: Expected O, but got Unknown
		//IL_0c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c72: Expected O, but got Unknown
		//IL_0c6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c77: Expected O, but got Unknown
		//IL_0c7c: Expected O, but got Unknown
		//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d99: Expected O, but got Unknown
		//IL_0d9e: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Expected O, but got Unknown
		//IL_0dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de0: Expected O, but got Unknown
		//IL_0de5: Expected O, but got Unknown
		//IL_0e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecd: Expected O, but got Unknown
		//IL_0ed2: Expected O, but got Unknown
		//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0f: Expected O, but got Unknown
		//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f14: Expected O, but got Unknown
		//IL_0f19: Expected O, but got Unknown
		//IL_0f64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1020: Unknown result type (might be due to invalid IL or missing references)
		//IL_1025: Unknown result type (might be due to invalid IL or missing references)
		//IL_1028: Expected O, but got Unknown
		//IL_102d: Expected O, but got Unknown
		//IL_102d: Unknown result type (might be due to invalid IL or missing references)
		//IL_103f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1051: Unknown result type (might be due to invalid IL or missing references)
		//IL_1060: Unknown result type (might be due to invalid IL or missing references)
		//IL_106a: Expected O, but got Unknown
		//IL_1065: Unknown result type (might be due to invalid IL or missing references)
		//IL_106f: Expected O, but got Unknown
		//IL_1074: Expected O, but got Unknown
		//IL_10d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_114c: Unknown result type (might be due to invalid IL or missing references)
		//IL_114f: Expected O, but got Unknown
		//IL_1154: Expected O, but got Unknown
		//IL_1154: Unknown result type (might be due to invalid IL or missing references)
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_1178: Unknown result type (might be due to invalid IL or missing references)
		//IL_1187: Unknown result type (might be due to invalid IL or missing references)
		//IL_1191: Expected O, but got Unknown
		//IL_118c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1196: Expected O, but got Unknown
		//IL_119b: Expected O, but got Unknown
		//IL_11f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1222: Unknown result type (might be due to invalid IL or missing references)
		//IL_125c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1261: Unknown result type (might be due to invalid IL or missing references)
		//IL_126c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1271: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12da: Expected O, but got Unknown
		//IL_12df: Expected O, but got Unknown
		//IL_12df: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1303: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Unknown result type (might be due to invalid IL or missing references)
		//IL_131c: Expected O, but got Unknown
		//IL_1317: Unknown result type (might be due to invalid IL or missing references)
		//IL_1321: Expected O, but got Unknown
		//IL_1326: Expected O, but got Unknown
		//IL_135f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1406: Unknown result type (might be due to invalid IL or missing references)
		//IL_140b: Unknown result type (might be due to invalid IL or missing references)
		//IL_140e: Expected O, but got Unknown
		//IL_1413: Expected O, but got Unknown
		//IL_1413: Unknown result type (might be due to invalid IL or missing references)
		//IL_1425: Unknown result type (might be due to invalid IL or missing references)
		//IL_1437: Unknown result type (might be due to invalid IL or missing references)
		//IL_1446: Unknown result type (might be due to invalid IL or missing references)
		//IL_1450: Expected O, but got Unknown
		//IL_144b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1455: Expected O, but got Unknown
		//IL_145a: Expected O, but got Unknown
		//IL_1493: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a2: Expected O, but got Unknown
		//IL_14c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1506: Unknown result type (might be due to invalid IL or missing references)
		//IL_150b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1516: Unknown result type (might be due to invalid IL or missing references)
		//IL_151b: Unknown result type (might be due to invalid IL or missing references)
		//IL_157c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1581: Unknown result type (might be due to invalid IL or missing references)
		//IL_1584: Expected O, but got Unknown
		//IL_1589: Expected O, but got Unknown
		//IL_1589: Unknown result type (might be due to invalid IL or missing references)
		//IL_159b: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c6: Expected O, but got Unknown
		//IL_15c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cb: Expected O, but got Unknown
		//IL_15d0: Expected O, but got Unknown
		//IL_160e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1613: Unknown result type (might be due to invalid IL or missing references)
		//IL_161e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1623: Unknown result type (might be due to invalid IL or missing references)
		//IL_1684: Unknown result type (might be due to invalid IL or missing references)
		//IL_1689: Unknown result type (might be due to invalid IL or missing references)
		//IL_168c: Expected O, but got Unknown
		//IL_1691: Expected O, but got Unknown
		//IL_1691: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ce: Expected O, but got Unknown
		//IL_16c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d3: Expected O, but got Unknown
		//IL_16d8: Expected O, but got Unknown
		//IL_1735: Unknown result type (might be due to invalid IL or missing references)
		//IL_173a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1745: Unknown result type (might be due to invalid IL or missing references)
		//IL_174a: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b3: Expected O, but got Unknown
		//IL_17b8: Expected O, but got Unknown
		//IL_17b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_17dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_17eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f5: Expected O, but got Unknown
		//IL_17f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fa: Expected O, but got Unknown
		//IL_17ff: Expected O, but got Unknown
		//IL_185c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1886: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1936: Unknown result type (might be due to invalid IL or missing references)
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_193e: Expected O, but got Unknown
		//IL_1943: Expected O, but got Unknown
		//IL_1943: Unknown result type (might be due to invalid IL or missing references)
		//IL_1955: Unknown result type (might be due to invalid IL or missing references)
		//IL_1967: Unknown result type (might be due to invalid IL or missing references)
		//IL_1976: Unknown result type (might be due to invalid IL or missing references)
		//IL_1980: Expected O, but got Unknown
		//IL_197b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1985: Expected O, but got Unknown
		//IL_198a: Expected O, but got Unknown
		//IL_19c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a84: Expected O, but got Unknown
		//IL_1a89: Expected O, but got Unknown
		//IL_1a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac6: Expected O, but got Unknown
		//IL_1ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acb: Expected O, but got Unknown
		//IL_1ad0: Expected O, but got Unknown
		//IL_1b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bdf: Expected O, but got Unknown
		//IL_1be4: Expected O, but got Unknown
		//IL_1be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c21: Expected O, but got Unknown
		//IL_1c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c26: Expected O, but got Unknown
		//IL_1c2b: Expected O, but got Unknown
		//IL_1c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c85: Expected O, but got Unknown
		//IL_1c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c92: Expected O, but got Unknown
		//IL_1c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9c: Expected O, but got Unknown
		//IL_1ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6d: Expected O, but got Unknown
		//IL_1d72: Expected O, but got Unknown
		//IL_1d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1daf: Expected O, but got Unknown
		//IL_1daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db4: Expected O, but got Unknown
		//IL_1db9: Expected O, but got Unknown
		//IL_1e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f18: Expected O, but got Unknown
		//IL_1f1d: Expected O, but got Unknown
		//IL_1f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5a: Expected O, but got Unknown
		//IL_1f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5f: Expected O, but got Unknown
		//IL_1f64: Expected O, but got Unknown
		//IL_1fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2037: Unknown result type (might be due to invalid IL or missing references)
		//IL_203c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2047: Unknown result type (might be due to invalid IL or missing references)
		//IL_204c: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b5: Expected O, but got Unknown
		//IL_20ba: Expected O, but got Unknown
		//IL_20ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_20cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_20de: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f7: Expected O, but got Unknown
		//IL_20f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fc: Expected O, but got Unknown
		//IL_2101: Expected O, but got Unknown
		//IL_213a: Unknown result type (might be due to invalid IL or missing references)
		//IL_217f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2184: Unknown result type (might be due to invalid IL or missing references)
		//IL_218f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2194: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e9: Expected O, but got Unknown
		//IL_21ee: Expected O, but got Unknown
		//IL_21ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2200: Unknown result type (might be due to invalid IL or missing references)
		//IL_2212: Unknown result type (might be due to invalid IL or missing references)
		//IL_2221: Unknown result type (might be due to invalid IL or missing references)
		//IL_222b: Expected O, but got Unknown
		//IL_2226: Unknown result type (might be due to invalid IL or missing references)
		//IL_2230: Expected O, but got Unknown
		//IL_2235: Expected O, but got Unknown
		//IL_2280: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_22cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_22db: Unknown result type (might be due to invalid IL or missing references)
		//IL_233c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2341: Unknown result type (might be due to invalid IL or missing references)
		//IL_2344: Expected O, but got Unknown
		//IL_2349: Expected O, but got Unknown
		//IL_2349: Unknown result type (might be due to invalid IL or missing references)
		//IL_235b: Unknown result type (might be due to invalid IL or missing references)
		//IL_236d: Unknown result type (might be due to invalid IL or missing references)
		//IL_237c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2386: Expected O, but got Unknown
		//IL_2381: Unknown result type (might be due to invalid IL or missing references)
		//IL_238b: Expected O, but got Unknown
		//IL_2390: Expected O, but got Unknown
		//IL_23ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_23fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2402: Unknown result type (might be due to invalid IL or missing references)
		//IL_2463: Unknown result type (might be due to invalid IL or missing references)
		//IL_2468: Unknown result type (might be due to invalid IL or missing references)
		//IL_246b: Expected O, but got Unknown
		//IL_2470: Expected O, but got Unknown
		//IL_2470: Unknown result type (might be due to invalid IL or missing references)
		//IL_2482: Unknown result type (might be due to invalid IL or missing references)
		//IL_2494: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ad: Expected O, but got Unknown
		//IL_24a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b2: Expected O, but got Unknown
		//IL_24b7: Expected O, but got Unknown
		//IL_2553: Unknown result type (might be due to invalid IL or missing references)
		//IL_2558: Unknown result type (might be due to invalid IL or missing references)
		//IL_2563: Unknown result type (might be due to invalid IL or missing references)
		//IL_2568: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_25bd: Expected O, but got Unknown
		//IL_25c2: Expected O, but got Unknown
		//IL_25c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2602: Expected O, but got Unknown
		//IL_25fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2607: Expected O, but got Unknown
		//IL_260c: Expected O, but got Unknown
		//IL_2657: Unknown result type (might be due to invalid IL or missing references)
		//IL_269d: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2713: Unknown result type (might be due to invalid IL or missing references)
		//IL_2718: Unknown result type (might be due to invalid IL or missing references)
		//IL_271b: Expected O, but got Unknown
		//IL_2720: Expected O, but got Unknown
		//IL_2720: Unknown result type (might be due to invalid IL or missing references)
		//IL_2732: Unknown result type (might be due to invalid IL or missing references)
		//IL_2744: Unknown result type (might be due to invalid IL or missing references)
		//IL_2756: Unknown result type (might be due to invalid IL or missing references)
		//IL_2760: Expected O, but got Unknown
		//IL_275b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2765: Expected O, but got Unknown
		//IL_276a: Expected O, but got Unknown
		//IL_27c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_27cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_27dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_283d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2842: Unknown result type (might be due to invalid IL or missing references)
		//IL_2845: Expected O, but got Unknown
		//IL_284a: Expected O, but got Unknown
		//IL_284a: Unknown result type (might be due to invalid IL or missing references)
		//IL_285c: Unknown result type (might be due to invalid IL or missing references)
		//IL_286e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2880: Unknown result type (might be due to invalid IL or missing references)
		//IL_288a: Expected O, but got Unknown
		//IL_2885: Unknown result type (might be due to invalid IL or missing references)
		//IL_288f: Expected O, but got Unknown
		//IL_2894: Expected O, but got Unknown
		//IL_2925: Unknown result type (might be due to invalid IL or missing references)
		//IL_292a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2935: Unknown result type (might be due to invalid IL or missing references)
		//IL_293a: Unknown result type (might be due to invalid IL or missing references)
		//IL_299b: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a3: Expected O, but got Unknown
		//IL_29a8: Expected O, but got Unknown
		//IL_29a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_29cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_29de: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e8: Expected O, but got Unknown
		//IL_29e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ed: Expected O, but got Unknown
		//IL_29f2: Expected O, but got Unknown
		//IL_2a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ade: Expected O, but got Unknown
		//IL_2ae3: Expected O, but got Unknown
		//IL_2ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b23: Expected O, but got Unknown
		//IL_2b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b28: Expected O, but got Unknown
		//IL_2b2d: Expected O, but got Unknown
		//IL_2b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bce: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c39: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c3c: Expected O, but got Unknown
		//IL_2c41: Expected O, but got Unknown
		//IL_2c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c81: Expected O, but got Unknown
		//IL_2c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c86: Expected O, but got Unknown
		//IL_2c8b: Expected O, but got Unknown
		//IL_2cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d47: Expected O, but got Unknown
		//IL_2d4c: Expected O, but got Unknown
		//IL_2d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8c: Expected O, but got Unknown
		//IL_2d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d91: Expected O, but got Unknown
		//IL_2d96: Expected O, but got Unknown
		//IL_2e00: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb2: Expected O, but got Unknown
		//IL_2eb7: Expected O, but got Unknown
		//IL_2eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2edb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eed: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef7: Expected O, but got Unknown
		//IL_2ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efc: Expected O, but got Unknown
		//IL_2f01: Expected O, but got Unknown
		//IL_2f18: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_2faf: Expected O, but got Unknown
		//IL_2fb4: Expected O, but got Unknown
		//IL_2fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fea: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff4: Expected O, but got Unknown
		//IL_2fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff9: Expected O, but got Unknown
		//IL_2ffe: Expected O, but got Unknown
		//IL_300b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3010: Unknown result type (might be due to invalid IL or missing references)
		//IL_3022: Unknown result type (might be due to invalid IL or missing references)
		//IL_302c: Expected O, but got Unknown
		//IL_302c: Unknown result type (might be due to invalid IL or missing references)
		//IL_303e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3048: Expected O, but got Unknown
		//IL_3043: Unknown result type (might be due to invalid IL or missing references)
		//IL_304d: Expected O, but got Unknown
		//IL_3052: Expected O, but got Unknown
		//IL_3072: Unknown result type (might be due to invalid IL or missing references)
		//IL_30c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30db: Unknown result type (might be due to invalid IL or missing references)
		//IL_3151: Unknown result type (might be due to invalid IL or missing references)
		//IL_3156: Unknown result type (might be due to invalid IL or missing references)
		//IL_3159: Expected O, but got Unknown
		//IL_315e: Expected O, but got Unknown
		//IL_315e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3170: Unknown result type (might be due to invalid IL or missing references)
		//IL_3182: Unknown result type (might be due to invalid IL or missing references)
		//IL_3194: Unknown result type (might be due to invalid IL or missing references)
		//IL_319e: Expected O, but got Unknown
		//IL_3199: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a3: Expected O, but got Unknown
		//IL_31a8: Expected O, but got Unknown
		//IL_3239: Unknown result type (might be due to invalid IL or missing references)
		//IL_323e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3249: Unknown result type (might be due to invalid IL or missing references)
		//IL_324e: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_32cc: Expected O, but got Unknown
		//IL_32d1: Expected O, but got Unknown
		//IL_32d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_32f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3307: Unknown result type (might be due to invalid IL or missing references)
		//IL_3311: Expected O, but got Unknown
		//IL_330c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3316: Expected O, but got Unknown
		//IL_331b: Expected O, but got Unknown
		//IL_33b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_345f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3464: Unknown result type (might be due to invalid IL or missing references)
		//IL_3467: Expected O, but got Unknown
		//IL_346c: Expected O, but got Unknown
		//IL_346c: Unknown result type (might be due to invalid IL or missing references)
		//IL_347e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3490: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ac: Expected O, but got Unknown
		//IL_34a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b1: Expected O, but got Unknown
		//IL_34b6: Expected O, but got Unknown
		//IL_34f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3578: Unknown result type (might be due to invalid IL or missing references)
		//IL_357d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3588: Unknown result type (might be due to invalid IL or missing references)
		//IL_358d: Unknown result type (might be due to invalid IL or missing references)
		//IL_360f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3614: Unknown result type (might be due to invalid IL or missing references)
		//IL_3617: Expected O, but got Unknown
		//IL_361c: Expected O, but got Unknown
		//IL_361c: Unknown result type (might be due to invalid IL or missing references)
		//IL_362e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3640: Unknown result type (might be due to invalid IL or missing references)
		//IL_3652: Unknown result type (might be due to invalid IL or missing references)
		//IL_365c: Expected O, but got Unknown
		//IL_3657: Unknown result type (might be due to invalid IL or missing references)
		//IL_3661: Expected O, but got Unknown
		//IL_3666: Expected O, but got Unknown
		//IL_369f: Unknown result type (might be due to invalid IL or missing references)
		StaticResourceExtension val = new StaticResourceExtension();
		Image val2 = new Image();
		StaticResourceExtension val3 = new StaticResourceExtension();
		StaticResourceExtension val4 = new StaticResourceExtension();
		AppThemeBindingExtension val5 = new AppThemeBindingExtension();
		Label val6 = new Label();
		StaticResourceExtension val7 = new StaticResourceExtension();
		Label val8 = new Label();
		StaticResourceExtension val9 = new StaticResourceExtension();
		Label val10 = new Label();
		VerticalStackLayout val11 = new VerticalStackLayout();
		Border val12 = new Border();
		StaticResourceExtension val13 = new StaticResourceExtension();
		StaticResourceExtension val14 = new StaticResourceExtension();
		Label val15 = new Label();
		StaticResourceExtension val16 = new StaticResourceExtension();
		Button val17 = new Button();
		StaticResourceExtension val18 = new StaticResourceExtension();
		Label val19 = new Label();
		VerticalStackLayout val20 = new VerticalStackLayout();
		Border val21 = new Border();
		StaticResourceExtension val22 = new StaticResourceExtension();
		StaticResourceExtension val23 = new StaticResourceExtension();
		StaticResourceExtension val24 = new StaticResourceExtension();
		Label val25 = new Label();
		StaticResourceExtension val26 = new StaticResourceExtension();
		Button val27 = new Button();
		StaticResourceExtension val28 = new StaticResourceExtension();
		Label val29 = new Label();
		VerticalStackLayout val30 = new VerticalStackLayout();
		Border val31 = new Border();
		StaticResourceExtension val32 = new StaticResourceExtension();
		StaticResourceExtension val33 = new StaticResourceExtension();
		Label val34 = new Label();
		StaticResourceExtension val35 = new StaticResourceExtension();
		Button val36 = new Button();
		StaticResourceExtension val37 = new StaticResourceExtension();
		Button val38 = new Button();
		Grid val39 = new Grid();
		StaticResourceExtension val40 = new StaticResourceExtension();
		Label val41 = new Label();
		VerticalStackLayout val42 = new VerticalStackLayout();
		Border val43 = new Border();
		StaticResourceExtension val44 = new StaticResourceExtension();
		StaticResourceExtension val45 = new StaticResourceExtension();
		Label val46 = new Label();
		StaticResourceExtension val47 = new StaticResourceExtension();
		Label val48 = new Label();
		VerticalStackLayout val49 = new VerticalStackLayout();
		Border val50 = new Border();
		StaticResourceExtension val51 = new StaticResourceExtension();
		StaticResourceExtension val52 = new StaticResourceExtension();
		Label val53 = new Label();
		StaticResourceExtension val54 = new StaticResourceExtension();
		Label val55 = new Label();
		StaticResourceExtension val56 = new StaticResourceExtension();
		Label val57 = new Label();
		VerticalStackLayout val58 = new VerticalStackLayout();
		Border val59 = new Border();
		StaticResourceExtension val60 = new StaticResourceExtension();
		StaticResourceExtension val61 = new StaticResourceExtension();
		StaticResourceExtension val62 = new StaticResourceExtension();
		Label val63 = new Label();
		StaticResourceExtension val64 = new StaticResourceExtension();
		StaticResourceExtension val65 = new StaticResourceExtension();
		AppThemeBindingExtension val66 = new AppThemeBindingExtension();
		RoundRectangle val67 = new RoundRectangle();
		StaticResourceExtension val68 = new StaticResourceExtension();
		Label val69 = new Label();
		StaticResourceExtension val70 = new StaticResourceExtension();
		Label val71 = new Label();
		StaticResourceExtension val72 = new StaticResourceExtension();
		RoundRectangle val73 = new RoundRectangle();
		StaticResourceExtension val74 = new StaticResourceExtension();
		Label val75 = new Label();
		Border val76 = new Border();
		VerticalStackLayout val77 = new VerticalStackLayout();
		Border val78 = new Border();
		VerticalStackLayout val79 = new VerticalStackLayout();
		Border val80 = new Border();
		VerticalStackLayout val81 = new VerticalStackLayout();
		ScrollView val82 = new ScrollView();
		AboutPage aboutPage;
		NameScope val83 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(aboutPage = this))) ?? ((object)new NameScope()));
		NameScope.SetNameScope((BindableObject)(object)aboutPage, (INameScope)(object)val83);
		((Element)val82).transientNamescope = (INameScope)(object)val83;
		((Element)val81).transientNamescope = (INameScope)(object)val83;
		((Element)val12).transientNamescope = (INameScope)(object)val83;
		((Element)val11).transientNamescope = (INameScope)(object)val83;
		((Element)val2).transientNamescope = (INameScope)(object)val83;
		((Element)val6).transientNamescope = (INameScope)(object)val83;
		((Element)val8).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("VersionLabel", (object)val8);
		if (((Element)val8).StyleId == null)
		{
			((Element)val8).StyleId = "VersionLabel";
		}
		((Element)val10).transientNamescope = (INameScope)(object)val83;
		((Element)val21).transientNamescope = (INameScope)(object)val83;
		((Element)val20).transientNamescope = (INameScope)(object)val83;
		((Element)val15).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("ContactTitleLabel", (object)val15);
		if (((Element)val15).StyleId == null)
		{
			((Element)val15).StyleId = "ContactTitleLabel";
		}
		((Element)val17).transientNamescope = (INameScope)(object)val83;
		((Element)val19).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("ContactInstructionLabel", (object)val19);
		if (((Element)val19).StyleId == null)
		{
			((Element)val19).StyleId = "ContactInstructionLabel";
		}
		((Element)val31).transientNamescope = (INameScope)(object)val83;
		((Element)val30).transientNamescope = (INameScope)(object)val83;
		((Element)val25).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("SupportTitleLabel", (object)val25);
		if (((Element)val25).StyleId == null)
		{
			((Element)val25).StyleId = "SupportTitleLabel";
		}
		((Element)val27).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("DonationButton", (object)val27);
		if (((Element)val27).StyleId == null)
		{
			((Element)val27).StyleId = "DonationButton";
		}
		((Element)val29).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("SupportDescLabel", (object)val29);
		if (((Element)val29).StyleId == null)
		{
			((Element)val29).StyleId = "SupportDescLabel";
		}
		((Element)val43).transientNamescope = (INameScope)(object)val83;
		((Element)val42).transientNamescope = (INameScope)(object)val83;
		((Element)val34).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("LanguageTitleLabel", (object)val34);
		if (((Element)val34).StyleId == null)
		{
			((Element)val34).StyleId = "LanguageTitleLabel";
		}
		((Element)val39).transientNamescope = (INameScope)(object)val83;
		((Element)val36).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("SpanishButton", (object)val36);
		if (((Element)val36).StyleId == null)
		{
			((Element)val36).StyleId = "SpanishButton";
		}
		((Element)val38).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("EnglishButton", (object)val38);
		if (((Element)val38).StyleId == null)
		{
			((Element)val38).StyleId = "EnglishButton";
		}
		((Element)val41).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("LanguageDescLabel", (object)val41);
		if (((Element)val41).StyleId == null)
		{
			((Element)val41).StyleId = "LanguageDescLabel";
		}
		((Element)val50).transientNamescope = (INameScope)(object)val83;
		((Element)val49).transientNamescope = (INameScope)(object)val83;
		((Element)val46).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("PrivacyTitleLabel", (object)val46);
		if (((Element)val46).StyleId == null)
		{
			((Element)val46).StyleId = "PrivacyTitleLabel";
		}
		((Element)val48).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("PrivacyTextLabel", (object)val48);
		if (((Element)val48).StyleId == null)
		{
			((Element)val48).StyleId = "PrivacyTextLabel";
		}
		((Element)val59).transientNamescope = (INameScope)(object)val83;
		((Element)val58).transientNamescope = (INameScope)(object)val83;
		((Element)val53).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("LicenseTitleLabel", (object)val53);
		if (((Element)val53).StyleId == null)
		{
			((Element)val53).StyleId = "LicenseTitleLabel";
		}
		((Element)val55).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("LicenseTextLabel", (object)val55);
		if (((Element)val55).StyleId == null)
		{
			((Element)val55).StyleId = "LicenseTextLabel";
		}
		((Element)val57).transientNamescope = (INameScope)(object)val83;
		((Element)val80).transientNamescope = (INameScope)(object)val83;
		((Element)val79).transientNamescope = (INameScope)(object)val83;
		((Element)val63).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("LegalTitleLabel", (object)val63);
		if (((Element)val63).StyleId == null)
		{
			((Element)val63).StyleId = "LegalTitleLabel";
		}
		((Element)val78).transientNamescope = (INameScope)(object)val83;
		((Element)val67).transientNamescope = (INameScope)(object)val83;
		((Element)val77).transientNamescope = (INameScope)(object)val83;
		((Element)val69).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("LegalText1Label", (object)val69);
		if (((Element)val69).StyleId == null)
		{
			((Element)val69).StyleId = "LegalText1Label";
		}
		((Element)val71).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("LegalText2Label", (object)val71);
		if (((Element)val71).StyleId == null)
		{
			((Element)val71).StyleId = "LegalText2Label";
		}
		((Element)val76).transientNamescope = (INameScope)(object)val83;
		((Element)val73).transientNamescope = (INameScope)(object)val83;
		((Element)val75).transientNamescope = (INameScope)(object)val83;
		((INameScope)val83).RegisterName("WarningLabel", (object)val75);
		if (((Element)val75).StyleId == null)
		{
			((Element)val75).StyleId = "WarningLabel";
		}
		VersionLabel = val8;
		ContactTitleLabel = val15;
		ContactInstructionLabel = val19;
		SupportTitleLabel = val25;
		DonationButton = val27;
		SupportDescLabel = val29;
		LanguageTitleLabel = val34;
		SpanishButton = val36;
		EnglishButton = val38;
		LanguageDescLabel = val41;
		PrivacyTitleLabel = val46;
		PrivacyTextLabel = val48;
		LicenseTitleLabel = val53;
		LicenseTextLabel = val55;
		LegalTitleLabel = val63;
		LegalText1Label = val69;
		LegalText2Label = val71;
		WarningLabel = val75;
		((BindableObject)aboutPage).SetValue(Page.TitleProperty, (object)"Acerca de");
		((BindableObject)val81).SetValue(Layout.PaddingProperty, (object)new Thickness(16.0));
		((BindableObject)val81).SetValue(StackBase.SpacingProperty, (object)16.0);
		val.Key = "Card";
		StaticResourceExtension val84 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val85 = new XamlServiceProvider();
		Type? typeFromHandle = typeof(IProvideValueTarget);
		object[] array = new object[0 + 4];
		array[0] = val12;
		array[1] = val81;
		array[2] = val82;
		array[3] = aboutPage;
		SimpleValueTargetProvider val86 = new SimpleValueTargetProvider(array, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj = (object)val86;
		val85.Add(typeFromHandle, (object)val86);
		val85.Add(typeof(IReferenceProvider), obj);
		val85.Add(typeof(IRootObjectProvider), obj);
		val85.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(11, 21)));
		object obj2 = val84.ProvideValue((IServiceProvider)val85);
		((BindableObject)val12).SetValue(VisualElement.StyleProperty, (obj2 == null || !typeof(BindingBase).IsAssignableFrom(obj2.GetType())) ? obj2 : obj2);
		((BindableObject)val11).SetValue(Layout.PaddingProperty, (object)new Thickness(24.0, 20.0));
		((BindableObject)val11).SetValue(StackBase.SpacingProperty, (object)12.0);
		((BindableObject)val2).SetValue(Image.SourceProperty, (object)ImageSource.FromFile("app_logo.svg"));
		((BindableObject)val2).SetValue(VisualElement.WidthRequestProperty, (object)72.0);
		((BindableObject)val2).SetValue(VisualElement.HeightRequestProperty, (object)72.0);
		((BindableObject)val2).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val11).Children.Add((IView)(object)val2);
		((BindableObject)val6).SetValue(Label.TextProperty, (object)"SMS Forwarder");
		((BindableObject)val6).SetValue(Label.FontSizeProperty, (object)28.0);
		((BindableObject)val6).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		val3.Key = "TextPrimaryLight";
		StaticResourceExtension val87 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val88 = new XamlServiceProvider();
		Type? typeFromHandle2 = typeof(IProvideValueTarget);
		object[] array2 = new object[0 + 7];
		array2[0] = val5;
		array2[1] = val6;
		array2[2] = val11;
		array2[3] = val12;
		array2[4] = val81;
		array2[5] = val82;
		array2[6] = aboutPage;
		SimpleValueTargetProvider val89 = new SimpleValueTargetProvider(array2, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[8] { val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj3 = (object)val89;
		val88.Add(typeFromHandle2, (object)val89);
		val88.Add(typeof(IReferenceProvider), obj3);
		val88.Add(typeof(IRootObjectProvider), obj3);
		val88.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(21, 28)));
		object light = val87.ProvideValue((IServiceProvider)val88);
		val5.Light = light;
		val4.Key = "TextPrimaryDark";
		StaticResourceExtension val90 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val91 = new XamlServiceProvider();
		Type? typeFromHandle3 = typeof(IProvideValueTarget);
		object[] array3 = new object[0 + 7];
		array3[0] = val5;
		array3[1] = val6;
		array3[2] = val11;
		array3[3] = val12;
		array3[4] = val81;
		array3[5] = val82;
		array3[6] = aboutPage;
		SimpleValueTargetProvider val92 = new SimpleValueTargetProvider(array3, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[8] { val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj4 = (object)val92;
		val91.Add(typeFromHandle3, (object)val92);
		val91.Add(typeof(IReferenceProvider), obj4);
		val91.Add(typeof(IRootObjectProvider), obj4);
		val91.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(21, 28)));
		object dark = val90.ProvideValue((IServiceProvider)val91);
		val5.Dark = dark;
		XamlServiceProvider val93 = new XamlServiceProvider();
		val93.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val6, (object)Label.TextColorProperty));
		val93.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(21, 28)));
		BindingBase val94 = ((IMarkupExtension<BindingBase>)(object)val5).ProvideValue((IServiceProvider)val93);
		((BindableObject)val6).SetBinding(Label.TextColorProperty, val94);
		((BindableObject)val6).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val11).Children.Add((IView)(object)val6);
		((BindableObject)val8).SetValue(Label.TextProperty, (object)"Versión");
		val7.Key = "HintText";
		StaticResourceExtension val95 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val96 = new XamlServiceProvider();
		Type? typeFromHandle4 = typeof(IProvideValueTarget);
		object[] array4 = new object[0 + 6];
		array4[0] = val8;
		array4[1] = val11;
		array4[2] = val12;
		array4[3] = val81;
		array4[4] = val82;
		array4[5] = aboutPage;
		SimpleValueTargetProvider val97 = new SimpleValueTargetProvider(array4, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj5 = (object)val97;
		val96.Add(typeFromHandle4, (object)val97);
		val96.Add(typeof(IReferenceProvider), obj5);
		val96.Add(typeof(IRootObjectProvider), obj5);
		val96.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(26, 28)));
		object obj6 = val95.ProvideValue((IServiceProvider)val96);
		((BindableObject)val8).SetValue(VisualElement.StyleProperty, (obj6 == null || !typeof(BindingBase).IsAssignableFrom(obj6.GetType())) ? obj6 : obj6);
		((BindableObject)val8).SetValue(Label.FontSizeProperty, (object)16.0);
		((BindableObject)val8).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val11).Children.Add((IView)(object)val8);
		((BindableObject)val10).SetValue(Label.TextProperty, (object)"Socratic");
		((BindableObject)val10).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		val9.Key = "Primary";
		StaticResourceExtension val98 = new StaticResourceExtension
		{
			Key = "Primary"
		};
		XamlServiceProvider val99 = new XamlServiceProvider();
		Type? typeFromHandle5 = typeof(IProvideValueTarget);
		object[] array5 = new object[0 + 6];
		array5[0] = val10;
		array5[1] = val11;
		array5[2] = val12;
		array5[3] = val81;
		array5[4] = val82;
		array5[5] = aboutPage;
		SimpleValueTargetProvider val100 = new SimpleValueTargetProvider(array5, (object)Label.TextColorProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj7 = (object)val100;
		val99.Add(typeFromHandle5, (object)val100);
		val99.Add(typeof(IReferenceProvider), obj7);
		val99.Add(typeof(IRootObjectProvider), obj7);
		val99.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(32, 28)));
		object obj8 = val98.ProvideValue((IServiceProvider)val99);
		((BindableObject)val10).SetValue(Label.TextColorProperty, (obj8 == null || !typeof(BindingBase).IsAssignableFrom(obj8.GetType())) ? obj8 : obj8);
		((BindableObject)val10).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val11).Children.Add((IView)(object)val10);
		((BindableObject)val12).SetValue(Border.ContentProperty, (object)val11);
		((Layout)val81).Children.Add((IView)(object)val12);
		val13.Key = "Card";
		StaticResourceExtension val101 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val102 = new XamlServiceProvider();
		Type? typeFromHandle6 = typeof(IProvideValueTarget);
		object[] array6 = new object[0 + 4];
		array6[0] = val21;
		array6[1] = val81;
		array6[2] = val82;
		array6[3] = aboutPage;
		SimpleValueTargetProvider val103 = new SimpleValueTargetProvider(array6, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj9 = (object)val103;
		val102.Add(typeFromHandle6, (object)val103);
		val102.Add(typeof(IReferenceProvider), obj9);
		val102.Add(typeof(IRootObjectProvider), obj9);
		val102.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(38, 21)));
		object obj10 = val101.ProvideValue((IServiceProvider)val102);
		((BindableObject)val21).SetValue(VisualElement.StyleProperty, (obj10 == null || !typeof(BindingBase).IsAssignableFrom(obj10.GetType())) ? obj10 : obj10);
		((BindableObject)val20).SetValue(Layout.PaddingProperty, (object)new Thickness(25.0, 20.0));
		((BindableObject)val20).SetValue(StackBase.SpacingProperty, (object)15.0);
		((BindableObject)val15).SetValue(Label.TextProperty, (object)"Contacto");
		val14.Key = "CardTitle";
		StaticResourceExtension val104 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val105 = new XamlServiceProvider();
		Type? typeFromHandle7 = typeof(IProvideValueTarget);
		object[] array7 = new object[0 + 6];
		array7[0] = val15;
		array7[1] = val20;
		array7[2] = val21;
		array7[3] = val81;
		array7[4] = val82;
		array7[5] = aboutPage;
		SimpleValueTargetProvider val106 = new SimpleValueTargetProvider(array7, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj11 = (object)val106;
		val105.Add(typeFromHandle7, (object)val106);
		val105.Add(typeof(IReferenceProvider), obj11);
		val105.Add(typeof(IRootObjectProvider), obj11);
		val105.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(42, 28)));
		object obj12 = val104.ProvideValue((IServiceProvider)val105);
		((BindableObject)val15).SetValue(VisualElement.StyleProperty, (obj12 == null || !typeof(BindingBase).IsAssignableFrom(obj12.GetType())) ? obj12 : obj12);
		((Layout)val20).Children.Add((IView)(object)val15);
		((BindableObject)val17).SetValue(Button.TextProperty, (object)"jsoladelarosa@gmail.com");
		val16.Key = "OutlineButton";
		StaticResourceExtension val107 = new StaticResourceExtension
		{
			Key = "OutlineButton"
		};
		XamlServiceProvider val108 = new XamlServiceProvider();
		Type? typeFromHandle8 = typeof(IProvideValueTarget);
		object[] array8 = new object[0 + 6];
		array8[0] = val17;
		array8[1] = val20;
		array8[2] = val21;
		array8[3] = val81;
		array8[4] = val82;
		array8[5] = aboutPage;
		SimpleValueTargetProvider val109 = new SimpleValueTargetProvider(array8, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj13 = (object)val109;
		val108.Add(typeFromHandle8, (object)val109);
		val108.Add(typeof(IReferenceProvider), obj13);
		val108.Add(typeof(IRootObjectProvider), obj13);
		val108.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(45, 29)));
		object obj14 = val107.ProvideValue((IServiceProvider)val108);
		((BindableObject)val17).SetValue(VisualElement.StyleProperty, (obj14 == null || !typeof(BindingBase).IsAssignableFrom(obj14.GetType())) ? obj14 : obj14);
		((BindableObject)val17).SetValue(Button.LineBreakModeProperty, (object)(LineBreakMode)1);
		((BindableObject)val17).SetValue(Button.PaddingProperty, (object)new Thickness(12.0, 10.0));
		val17.Clicked += aboutPage.OnContactEmailClicked;
		((BindableObject)val17).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Fill);
		((Layout)val20).Children.Add((IView)(object)val17);
		((BindableObject)val19).SetValue(Label.TextProperty, (object)"Toca para enviar un correo electrónico");
		val18.Key = "HintText";
		StaticResourceExtension val110 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val111 = new XamlServiceProvider();
		Type? typeFromHandle9 = typeof(IProvideValueTarget);
		object[] array9 = new object[0 + 6];
		array9[0] = val19;
		array9[1] = val20;
		array9[2] = val21;
		array9[3] = val81;
		array9[4] = val82;
		array9[5] = aboutPage;
		SimpleValueTargetProvider val112 = new SimpleValueTargetProvider(array9, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj15 = (object)val112;
		val111.Add(typeFromHandle9, (object)val112);
		val111.Add(typeof(IReferenceProvider), obj15);
		val111.Add(typeof(IRootObjectProvider), obj15);
		val111.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(53, 28)));
		object obj16 = val110.ProvideValue((IServiceProvider)val111);
		((BindableObject)val19).SetValue(VisualElement.StyleProperty, (obj16 == null || !typeof(BindingBase).IsAssignableFrom(obj16.GetType())) ? obj16 : obj16);
		((BindableObject)val19).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val20).Children.Add((IView)(object)val19);
		((BindableObject)val21).SetValue(Border.ContentProperty, (object)val20);
		((Layout)val81).Children.Add((IView)(object)val21);
		val22.Key = "Card";
		StaticResourceExtension val113 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val114 = new XamlServiceProvider();
		Type? typeFromHandle10 = typeof(IProvideValueTarget);
		object[] array10 = new object[0 + 4];
		array10[0] = val31;
		array10[1] = val81;
		array10[2] = val82;
		array10[3] = aboutPage;
		SimpleValueTargetProvider val115 = new SimpleValueTargetProvider(array10, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj17 = (object)val115;
		val114.Add(typeFromHandle10, (object)val115);
		val114.Add(typeof(IReferenceProvider), obj17);
		val114.Add(typeof(IRootObjectProvider), obj17);
		val114.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(59, 21)));
		object obj18 = val113.ProvideValue((IServiceProvider)val114);
		((BindableObject)val31).SetValue(VisualElement.StyleProperty, (obj18 == null || !typeof(BindingBase).IsAssignableFrom(obj18.GetType())) ? obj18 : obj18);
		((BindableObject)val31).SetValue(VisualElement.IsVisibleProperty, ((TypeConverter)new VisibilityConverter()).ConvertFromInvariantString("False"));
		((BindableObject)val30).SetValue(Layout.PaddingProperty, (object)new Thickness(25.0, 20.0));
		((BindableObject)val30).SetValue(StackBase.SpacingProperty, (object)15.0);
		((BindableObject)val25).SetValue(Label.TextProperty, (object)"Apoya el Desarrollo");
		val23.Key = "CardTitle";
		StaticResourceExtension val116 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val117 = new XamlServiceProvider();
		Type? typeFromHandle11 = typeof(IProvideValueTarget);
		object[] array11 = new object[0 + 6];
		array11[0] = val25;
		array11[1] = val30;
		array11[2] = val31;
		array11[3] = val81;
		array11[4] = val82;
		array11[5] = aboutPage;
		SimpleValueTargetProvider val118 = new SimpleValueTargetProvider(array11, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj19 = (object)val118;
		val117.Add(typeFromHandle11, (object)val118);
		val117.Add(typeof(IReferenceProvider), obj19);
		val117.Add(typeof(IRootObjectProvider), obj19);
		val117.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(63, 28)));
		object obj20 = val116.ProvideValue((IServiceProvider)val117);
		((BindableObject)val25).SetValue(VisualElement.StyleProperty, (obj20 == null || !typeof(BindingBase).IsAssignableFrom(obj20.GetType())) ? obj20 : obj20);
		val24.Key = "Accent";
		StaticResourceExtension val119 = new StaticResourceExtension
		{
			Key = "Accent"
		};
		XamlServiceProvider val120 = new XamlServiceProvider();
		Type? typeFromHandle12 = typeof(IProvideValueTarget);
		object[] array12 = new object[0 + 6];
		array12[0] = val25;
		array12[1] = val30;
		array12[2] = val31;
		array12[3] = val81;
		array12[4] = val82;
		array12[5] = aboutPage;
		SimpleValueTargetProvider val121 = new SimpleValueTargetProvider(array12, (object)Label.TextColorProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj21 = (object)val121;
		val120.Add(typeFromHandle12, (object)val121);
		val120.Add(typeof(IReferenceProvider), obj21);
		val120.Add(typeof(IRootObjectProvider), obj21);
		val120.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(64, 28)));
		object obj22 = val119.ProvideValue((IServiceProvider)val120);
		((BindableObject)val25).SetValue(Label.TextColorProperty, (obj22 == null || !typeof(BindingBase).IsAssignableFrom(obj22.GetType())) ? obj22 : obj22);
		((Layout)val30).Children.Add((IView)(object)val25);
		((BindableObject)val27).SetValue(Button.TextProperty, (object)"Ko-fi.com - Invítame un café");
		val26.Key = "AccentButton";
		StaticResourceExtension val122 = new StaticResourceExtension
		{
			Key = "AccentButton"
		};
		XamlServiceProvider val123 = new XamlServiceProvider();
		Type? typeFromHandle13 = typeof(IProvideValueTarget);
		object[] array13 = new object[0 + 6];
		array13[0] = val27;
		array13[1] = val30;
		array13[2] = val31;
		array13[3] = val81;
		array13[4] = val82;
		array13[5] = aboutPage;
		SimpleValueTargetProvider val124 = new SimpleValueTargetProvider(array13, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj23 = (object)val124;
		val123.Add(typeFromHandle13, (object)val124);
		val123.Add(typeof(IReferenceProvider), obj23);
		val123.Add(typeof(IRootObjectProvider), obj23);
		val123.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(68, 29)));
		object obj24 = val122.ProvideValue((IServiceProvider)val123);
		((BindableObject)val27).SetValue(VisualElement.StyleProperty, (obj24 == null || !typeof(BindingBase).IsAssignableFrom(obj24.GetType())) ? obj24 : obj24);
		((BindableObject)val27).SetValue(Button.LineBreakModeProperty, (object)(LineBreakMode)1);
		((BindableObject)val27).SetValue(Button.PaddingProperty, (object)new Thickness(12.0, 10.0));
		val27.Clicked += aboutPage.OnDonationClicked;
		((BindableObject)val27).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Fill);
		((Layout)val30).Children.Add((IView)(object)val27);
		((BindableObject)val29).SetValue(Label.TextProperty, (object)"Tu apoyo ayuda a mantener y mejorar la aplicación");
		val28.Key = "HintText";
		StaticResourceExtension val125 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val126 = new XamlServiceProvider();
		Type? typeFromHandle14 = typeof(IProvideValueTarget);
		object[] array14 = new object[0 + 6];
		array14[0] = val29;
		array14[1] = val30;
		array14[2] = val31;
		array14[3] = val81;
		array14[4] = val82;
		array14[5] = aboutPage;
		SimpleValueTargetProvider val127 = new SimpleValueTargetProvider(array14, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj25 = (object)val127;
		val126.Add(typeFromHandle14, (object)val127);
		val126.Add(typeof(IReferenceProvider), obj25);
		val126.Add(typeof(IRootObjectProvider), obj25);
		val126.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(76, 28)));
		object obj26 = val125.ProvideValue((IServiceProvider)val126);
		((BindableObject)val29).SetValue(VisualElement.StyleProperty, (obj26 == null || !typeof(BindingBase).IsAssignableFrom(obj26.GetType())) ? obj26 : obj26);
		((BindableObject)val29).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val29).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
		((Layout)val30).Children.Add((IView)(object)val29);
		((BindableObject)val31).SetValue(Border.ContentProperty, (object)val30);
		((Layout)val81).Children.Add((IView)(object)val31);
		val32.Key = "Card";
		StaticResourceExtension val128 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val129 = new XamlServiceProvider();
		Type? typeFromHandle15 = typeof(IProvideValueTarget);
		object[] array15 = new object[0 + 4];
		array15[0] = val43;
		array15[1] = val81;
		array15[2] = val82;
		array15[3] = aboutPage;
		SimpleValueTargetProvider val130 = new SimpleValueTargetProvider(array15, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj27 = (object)val130;
		val129.Add(typeFromHandle15, (object)val130);
		val129.Add(typeof(IReferenceProvider), obj27);
		val129.Add(typeof(IRootObjectProvider), obj27);
		val129.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(83, 21)));
		object obj28 = val128.ProvideValue((IServiceProvider)val129);
		((BindableObject)val43).SetValue(VisualElement.StyleProperty, (obj28 == null || !typeof(BindingBase).IsAssignableFrom(obj28.GetType())) ? obj28 : obj28);
		((BindableObject)val42).SetValue(Layout.PaddingProperty, (object)new Thickness(25.0, 20.0));
		((BindableObject)val42).SetValue(StackBase.SpacingProperty, (object)15.0);
		((BindableObject)val34).SetValue(Label.TextProperty, (object)"Idioma");
		val33.Key = "CardTitle";
		StaticResourceExtension val131 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val132 = new XamlServiceProvider();
		Type? typeFromHandle16 = typeof(IProvideValueTarget);
		object[] array16 = new object[0 + 6];
		array16[0] = val34;
		array16[1] = val42;
		array16[2] = val43;
		array16[3] = val81;
		array16[4] = val82;
		array16[5] = aboutPage;
		SimpleValueTargetProvider val133 = new SimpleValueTargetProvider(array16, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj29 = (object)val133;
		val132.Add(typeFromHandle16, (object)val133);
		val132.Add(typeof(IReferenceProvider), obj29);
		val132.Add(typeof(IRootObjectProvider), obj29);
		val132.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(87, 28)));
		object obj30 = val131.ProvideValue((IServiceProvider)val132);
		((BindableObject)val34).SetValue(VisualElement.StyleProperty, (obj30 == null || !typeof(BindingBase).IsAssignableFrom(obj30.GetType())) ? obj30 : obj30);
		((Layout)val42).Children.Add((IView)(object)val34);
		((BindableObject)val39).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
		{
			new ColumnDefinition(GridLength.Star),
			new ColumnDefinition(GridLength.Star)
		}));
		((BindableObject)val39).SetValue(Grid.ColumnSpacingProperty, (object)10.0);
		((BindableObject)val36).SetValue(Grid.ColumnProperty, (object)0);
		((BindableObject)val36).SetValue(Button.TextProperty, (object)"\ud83c\uddea\ud83c\uddf8 Español");
		val35.Key = "PrimaryButton";
		StaticResourceExtension val134 = new StaticResourceExtension
		{
			Key = "PrimaryButton"
		};
		XamlServiceProvider val135 = new XamlServiceProvider();
		Type? typeFromHandle17 = typeof(IProvideValueTarget);
		object[] array17 = new object[0 + 7];
		array17[0] = val36;
		array17[1] = val39;
		array17[2] = val42;
		array17[3] = val43;
		array17[4] = val81;
		array17[5] = val82;
		array17[6] = aboutPage;
		SimpleValueTargetProvider val136 = new SimpleValueTargetProvider(array17, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj31 = (object)val136;
		val135.Add(typeFromHandle17, (object)val136);
		val135.Add(typeof(IReferenceProvider), obj31);
		val135.Add(typeof(IRootObjectProvider), obj31);
		val135.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(93, 33)));
		object obj32 = val134.ProvideValue((IServiceProvider)val135);
		((BindableObject)val36).SetValue(VisualElement.StyleProperty, (obj32 == null || !typeof(BindingBase).IsAssignableFrom(obj32.GetType())) ? obj32 : obj32);
		((BindableObject)val36).SetValue(Button.FontSizeProperty, (object)14.0);
		((BindableObject)val36).SetValue(Button.LineBreakModeProperty, (object)(LineBreakMode)1);
		((BindableObject)val36).SetValue(Button.PaddingProperty, (object)new Thickness(10.0, 10.0));
		val36.Clicked += aboutPage.OnSpanishClicked;
		((Layout)val39).Children.Add((IView)(object)val36);
		((BindableObject)val38).SetValue(Grid.ColumnProperty, (object)1);
		((BindableObject)val38).SetValue(Button.TextProperty, (object)"\ud83c\uddfa\ud83c\uddf8 English");
		val37.Key = "OutlineButton";
		StaticResourceExtension val137 = new StaticResourceExtension
		{
			Key = "OutlineButton"
		};
		XamlServiceProvider val138 = new XamlServiceProvider();
		Type? typeFromHandle18 = typeof(IProvideValueTarget);
		object[] array18 = new object[0 + 7];
		array18[0] = val38;
		array18[1] = val39;
		array18[2] = val42;
		array18[3] = val43;
		array18[4] = val81;
		array18[5] = val82;
		array18[6] = aboutPage;
		SimpleValueTargetProvider val139 = new SimpleValueTargetProvider(array18, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj33 = (object)val139;
		val138.Add(typeFromHandle18, (object)val139);
		val138.Add(typeof(IReferenceProvider), obj33);
		val138.Add(typeof(IRootObjectProvider), obj33);
		val138.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(102, 33)));
		object obj34 = val137.ProvideValue((IServiceProvider)val138);
		((BindableObject)val38).SetValue(VisualElement.StyleProperty, (obj34 == null || !typeof(BindingBase).IsAssignableFrom(obj34.GetType())) ? obj34 : obj34);
		((BindableObject)val38).SetValue(Button.FontSizeProperty, (object)14.0);
		((BindableObject)val38).SetValue(Button.LineBreakModeProperty, (object)(LineBreakMode)1);
		((BindableObject)val38).SetValue(Button.PaddingProperty, (object)new Thickness(10.0, 10.0));
		val38.Clicked += aboutPage.OnEnglishClicked;
		((Layout)val39).Children.Add((IView)(object)val38);
		((Layout)val42).Children.Add((IView)(object)val39);
		((BindableObject)val41).SetValue(Label.TextProperty, (object)"Selecciona tu idioma preferido");
		val40.Key = "HintText";
		StaticResourceExtension val140 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val141 = new XamlServiceProvider();
		Type? typeFromHandle19 = typeof(IProvideValueTarget);
		object[] array19 = new object[0 + 6];
		array19[0] = val41;
		array19[1] = val42;
		array19[2] = val43;
		array19[3] = val81;
		array19[4] = val82;
		array19[5] = aboutPage;
		SimpleValueTargetProvider val142 = new SimpleValueTargetProvider(array19, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj35 = (object)val142;
		val141.Add(typeFromHandle19, (object)val142);
		val141.Add(typeof(IReferenceProvider), obj35);
		val141.Add(typeof(IRootObjectProvider), obj35);
		val141.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(111, 28)));
		object obj36 = val140.ProvideValue((IServiceProvider)val141);
		((BindableObject)val41).SetValue(VisualElement.StyleProperty, (obj36 == null || !typeof(BindingBase).IsAssignableFrom(obj36.GetType())) ? obj36 : obj36);
		((BindableObject)val41).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val42).Children.Add((IView)(object)val41);
		((BindableObject)val43).SetValue(Border.ContentProperty, (object)val42);
		((Layout)val81).Children.Add((IView)(object)val43);
		val44.Key = "Card";
		StaticResourceExtension val143 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val144 = new XamlServiceProvider();
		Type? typeFromHandle20 = typeof(IProvideValueTarget);
		object[] array20 = new object[0 + 4];
		array20[0] = val50;
		array20[1] = val81;
		array20[2] = val82;
		array20[3] = aboutPage;
		SimpleValueTargetProvider val145 = new SimpleValueTargetProvider(array20, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj37 = (object)val145;
		val144.Add(typeFromHandle20, (object)val145);
		val144.Add(typeof(IReferenceProvider), obj37);
		val144.Add(typeof(IRootObjectProvider), obj37);
		val144.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(117, 21)));
		object obj38 = val143.ProvideValue((IServiceProvider)val144);
		((BindableObject)val50).SetValue(VisualElement.StyleProperty, (obj38 == null || !typeof(BindingBase).IsAssignableFrom(obj38.GetType())) ? obj38 : obj38);
		((BindableObject)val49).SetValue(Layout.PaddingProperty, (object)new Thickness(25.0, 20.0));
		((BindableObject)val49).SetValue(StackBase.SpacingProperty, (object)10.0);
		((BindableObject)val46).SetValue(Label.TextProperty, (object)"Privacidad");
		val45.Key = "CardTitle";
		StaticResourceExtension val146 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val147 = new XamlServiceProvider();
		Type? typeFromHandle21 = typeof(IProvideValueTarget);
		object[] array21 = new object[0 + 6];
		array21[0] = val46;
		array21[1] = val49;
		array21[2] = val50;
		array21[3] = val81;
		array21[4] = val82;
		array21[5] = aboutPage;
		SimpleValueTargetProvider val148 = new SimpleValueTargetProvider(array21, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj39 = (object)val148;
		val147.Add(typeFromHandle21, (object)val148);
		val147.Add(typeof(IReferenceProvider), obj39);
		val147.Add(typeof(IRootObjectProvider), obj39);
		val147.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(121, 28)));
		object obj40 = val146.ProvideValue((IServiceProvider)val147);
		((BindableObject)val46).SetValue(VisualElement.StyleProperty, (obj40 == null || !typeof(BindingBase).IsAssignableFrom(obj40.GetType())) ? obj40 : obj40);
		((Layout)val49).Children.Add((IView)(object)val46);
		((BindableObject)val48).SetValue(Label.TextProperty, (object)"Esta aplicación no recopila tus datos personales ni los envía a los desarrolladores. La información se procesa en tu dispositivo para la función propia de la app.");
		val47.Key = "BodyText";
		StaticResourceExtension val149 = new StaticResourceExtension
		{
			Key = "BodyText"
		};
		XamlServiceProvider val150 = new XamlServiceProvider();
		Type? typeFromHandle22 = typeof(IProvideValueTarget);
		object[] array22 = new object[0 + 6];
		array22[0] = val48;
		array22[1] = val49;
		array22[2] = val50;
		array22[3] = val81;
		array22[4] = val82;
		array22[5] = aboutPage;
		SimpleValueTargetProvider val151 = new SimpleValueTargetProvider(array22, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj41 = (object)val151;
		val150.Add(typeFromHandle22, (object)val151);
		val150.Add(typeof(IReferenceProvider), obj41);
		val150.Add(typeof(IRootObjectProvider), obj41);
		val150.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(125, 28)));
		object obj42 = val149.ProvideValue((IServiceProvider)val150);
		((BindableObject)val48).SetValue(VisualElement.StyleProperty, (obj42 == null || !typeof(BindingBase).IsAssignableFrom(obj42.GetType())) ? obj42 : obj42);
		((BindableObject)val48).SetValue(Label.FontSizeProperty, (object)13.0);
		((BindableObject)val48).SetValue(Label.LineHeightProperty, (object)1.4);
		((Layout)val49).Children.Add((IView)(object)val48);
		((BindableObject)val50).SetValue(Border.ContentProperty, (object)val49);
		((Layout)val81).Children.Add((IView)(object)val50);
		val51.Key = "Card";
		StaticResourceExtension val152 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val153 = new XamlServiceProvider();
		Type? typeFromHandle23 = typeof(IProvideValueTarget);
		object[] array23 = new object[0 + 4];
		array23[0] = val59;
		array23[1] = val81;
		array23[2] = val82;
		array23[3] = aboutPage;
		SimpleValueTargetProvider val154 = new SimpleValueTargetProvider(array23, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj43 = (object)val154;
		val153.Add(typeFromHandle23, (object)val154);
		val153.Add(typeof(IReferenceProvider), obj43);
		val153.Add(typeof(IRootObjectProvider), obj43);
		val153.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(132, 21)));
		object obj44 = val152.ProvideValue((IServiceProvider)val153);
		((BindableObject)val59).SetValue(VisualElement.StyleProperty, (obj44 == null || !typeof(BindingBase).IsAssignableFrom(obj44.GetType())) ? obj44 : obj44);
		((BindableObject)val58).SetValue(Layout.PaddingProperty, (object)new Thickness(25.0, 20.0));
		((BindableObject)val58).SetValue(StackBase.SpacingProperty, (object)10.0);
		((BindableObject)val53).SetValue(Label.TextProperty, (object)"Licencia");
		val52.Key = "CardTitle";
		StaticResourceExtension val155 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val156 = new XamlServiceProvider();
		Type? typeFromHandle24 = typeof(IProvideValueTarget);
		object[] array24 = new object[0 + 6];
		array24[0] = val53;
		array24[1] = val58;
		array24[2] = val59;
		array24[3] = val81;
		array24[4] = val82;
		array24[5] = aboutPage;
		SimpleValueTargetProvider val157 = new SimpleValueTargetProvider(array24, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj45 = (object)val157;
		val156.Add(typeFromHandle24, (object)val157);
		val156.Add(typeof(IReferenceProvider), obj45);
		val156.Add(typeof(IRootObjectProvider), obj45);
		val156.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(136, 28)));
		object obj46 = val155.ProvideValue((IServiceProvider)val156);
		((BindableObject)val53).SetValue(VisualElement.StyleProperty, (obj46 == null || !typeof(BindingBase).IsAssignableFrom(obj46.GetType())) ? obj46 : obj46);
		((Layout)val58).Children.Add((IView)(object)val53);
		((BindableObject)val55).SetValue(Label.TextProperty, (object)"Esta aplicación es software libre distribuido bajo licencia MIT.");
		val54.Key = "BodyText";
		StaticResourceExtension val158 = new StaticResourceExtension
		{
			Key = "BodyText"
		};
		XamlServiceProvider val159 = new XamlServiceProvider();
		Type? typeFromHandle25 = typeof(IProvideValueTarget);
		object[] array25 = new object[0 + 6];
		array25[0] = val55;
		array25[1] = val58;
		array25[2] = val59;
		array25[3] = val81;
		array25[4] = val82;
		array25[5] = aboutPage;
		SimpleValueTargetProvider val160 = new SimpleValueTargetProvider(array25, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj47 = (object)val160;
		val159.Add(typeFromHandle25, (object)val160);
		val159.Add(typeof(IReferenceProvider), obj47);
		val159.Add(typeof(IRootObjectProvider), obj47);
		val159.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(140, 28)));
		object obj48 = val158.ProvideValue((IServiceProvider)val159);
		((BindableObject)val55).SetValue(VisualElement.StyleProperty, (obj48 == null || !typeof(BindingBase).IsAssignableFrom(obj48.GetType())) ? obj48 : obj48);
		((BindableObject)val55).SetValue(Label.FontSizeProperty, (object)13.0);
		((BindableObject)val55).SetValue(Label.LineHeightProperty, (object)1.4);
		((Layout)val58).Children.Add((IView)(object)val55);
		((BindableObject)val57).SetValue(Label.TextProperty, (object)"MIT License · Copyright © 2026 Socratic");
		val56.Key = "HintText";
		StaticResourceExtension val161 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val162 = new XamlServiceProvider();
		Type? typeFromHandle26 = typeof(IProvideValueTarget);
		object[] array26 = new object[0 + 6];
		array26[0] = val57;
		array26[1] = val58;
		array26[2] = val59;
		array26[3] = val81;
		array26[4] = val82;
		array26[5] = aboutPage;
		SimpleValueTargetProvider val163 = new SimpleValueTargetProvider(array26, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj49 = (object)val163;
		val162.Add(typeFromHandle26, (object)val163);
		val162.Add(typeof(IReferenceProvider), obj49);
		val162.Add(typeof(IRootObjectProvider), obj49);
		val162.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(145, 28)));
		object obj50 = val161.ProvideValue((IServiceProvider)val162);
		((BindableObject)val57).SetValue(VisualElement.StyleProperty, (obj50 == null || !typeof(BindingBase).IsAssignableFrom(obj50.GetType())) ? obj50 : obj50);
		((BindableObject)val57).SetValue(Label.FontSizeProperty, (object)11.0);
		((Layout)val58).Children.Add((IView)(object)val57);
		((BindableObject)val59).SetValue(Border.ContentProperty, (object)val58);
		((Layout)val81).Children.Add((IView)(object)val59);
		val60.Key = "Card";
		StaticResourceExtension val164 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val165 = new XamlServiceProvider();
		Type? typeFromHandle27 = typeof(IProvideValueTarget);
		object[] array27 = new object[0 + 4];
		array27[0] = val80;
		array27[1] = val81;
		array27[2] = val82;
		array27[3] = aboutPage;
		SimpleValueTargetProvider val166 = new SimpleValueTargetProvider(array27, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj51 = (object)val166;
		val165.Add(typeFromHandle27, (object)val166);
		val165.Add(typeof(IReferenceProvider), obj51);
		val165.Add(typeof(IRootObjectProvider), obj51);
		val165.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(151, 21)));
		object obj52 = val164.ProvideValue((IServiceProvider)val165);
		((BindableObject)val80).SetValue(VisualElement.StyleProperty, (obj52 == null || !typeof(BindingBase).IsAssignableFrom(obj52.GetType())) ? obj52 : obj52);
		((BindableObject)val79).SetValue(Layout.PaddingProperty, (object)new Thickness(25.0, 20.0));
		((BindableObject)val79).SetValue(StackBase.SpacingProperty, (object)15.0);
		((BindableObject)val63).SetValue(Label.TextProperty, (object)"Aviso Legal");
		val61.Key = "CardTitle";
		StaticResourceExtension val167 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val168 = new XamlServiceProvider();
		Type? typeFromHandle28 = typeof(IProvideValueTarget);
		object[] array28 = new object[0 + 6];
		array28[0] = val63;
		array28[1] = val79;
		array28[2] = val80;
		array28[3] = val81;
		array28[4] = val82;
		array28[5] = aboutPage;
		SimpleValueTargetProvider val169 = new SimpleValueTargetProvider(array28, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj53 = (object)val169;
		val168.Add(typeFromHandle28, (object)val169);
		val168.Add(typeof(IReferenceProvider), obj53);
		val168.Add(typeof(IRootObjectProvider), obj53);
		val168.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(155, 28)));
		object obj54 = val167.ProvideValue((IServiceProvider)val168);
		((BindableObject)val63).SetValue(VisualElement.StyleProperty, (obj54 == null || !typeof(BindingBase).IsAssignableFrom(obj54.GetType())) ? obj54 : obj54);
		val62.Key = "Danger";
		StaticResourceExtension val170 = new StaticResourceExtension
		{
			Key = "Danger"
		};
		XamlServiceProvider val171 = new XamlServiceProvider();
		Type? typeFromHandle29 = typeof(IProvideValueTarget);
		object[] array29 = new object[0 + 6];
		array29[0] = val63;
		array29[1] = val79;
		array29[2] = val80;
		array29[3] = val81;
		array29[4] = val82;
		array29[5] = aboutPage;
		SimpleValueTargetProvider val172 = new SimpleValueTargetProvider(array29, (object)Label.TextColorProperty, (INameScope[])(object)new NameScope[7] { val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj55 = (object)val172;
		val171.Add(typeFromHandle29, (object)val172);
		val171.Add(typeof(IReferenceProvider), obj55);
		val171.Add(typeof(IRootObjectProvider), obj55);
		val171.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(156, 28)));
		object obj56 = val170.ProvideValue((IServiceProvider)val171);
		((BindableObject)val63).SetValue(Label.TextColorProperty, (obj56 == null || !typeof(BindingBase).IsAssignableFrom(obj56.GetType())) ? obj56 : obj56);
		((Layout)val79).Children.Add((IView)(object)val63);
		((BindableObject)val78).SetValue(Border.StrokeThicknessProperty, (object)0.0);
		((BindableObject)val78).SetValue(Border.PaddingProperty, (object)new Thickness(15.0));
		val64.Key = "PageBackgroundLight";
		StaticResourceExtension val173 = new StaticResourceExtension
		{
			Key = "PageBackgroundLight"
		};
		XamlServiceProvider val174 = new XamlServiceProvider();
		Type? typeFromHandle30 = typeof(IProvideValueTarget);
		object[] array30 = new object[0 + 7];
		array30[0] = val66;
		array30[1] = val78;
		array30[2] = val79;
		array30[3] = val80;
		array30[4] = val81;
		array30[5] = val82;
		array30[6] = aboutPage;
		SimpleValueTargetProvider val175 = new SimpleValueTargetProvider(array30, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[8] { val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj57 = (object)val175;
		val174.Add(typeFromHandle30, (object)val175);
		val174.Add(typeof(IReferenceProvider), obj57);
		val174.Add(typeof(IRootObjectProvider), obj57);
		val174.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(160, 29)));
		object light2 = val173.ProvideValue((IServiceProvider)val174);
		val66.Light = light2;
		val65.Key = "PageBackgroundDark";
		StaticResourceExtension val176 = new StaticResourceExtension
		{
			Key = "PageBackgroundDark"
		};
		XamlServiceProvider val177 = new XamlServiceProvider();
		Type? typeFromHandle31 = typeof(IProvideValueTarget);
		object[] array31 = new object[0 + 7];
		array31[0] = val66;
		array31[1] = val78;
		array31[2] = val79;
		array31[3] = val80;
		array31[4] = val81;
		array31[5] = val82;
		array31[6] = aboutPage;
		SimpleValueTargetProvider val178 = new SimpleValueTargetProvider(array31, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[8] { val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj58 = (object)val178;
		val177.Add(typeFromHandle31, (object)val178);
		val177.Add(typeof(IReferenceProvider), obj58);
		val177.Add(typeof(IRootObjectProvider), obj58);
		val177.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(160, 29)));
		object dark2 = val176.ProvideValue((IServiceProvider)val177);
		val66.Dark = dark2;
		XamlServiceProvider val179 = new XamlServiceProvider();
		val179.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val78, (object)VisualElement.BackgroundColorProperty));
		val179.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(160, 29)));
		BindingBase val180 = ((IMarkupExtension<BindingBase>)(object)val66).ProvideValue((IServiceProvider)val179);
		((BindableObject)val78).SetBinding(VisualElement.BackgroundColorProperty, val180);
		((BindableObject)val67).SetValue(RoundRectangle.CornerRadiusProperty, (object)new CornerRadius(8.0));
		((BindableObject)val78).SetValue(Border.StrokeShapeProperty, (object)val67);
		((BindableObject)val77).SetValue(StackBase.SpacingProperty, (object)10.0);
		((BindableObject)val69).SetValue(Label.TextProperty, (object)"Este software se proporciona «tal cual», sin garantías de ningún tipo. El usuario es responsable del uso adecuado de la aplicación y del cumplimiento de las leyes locales.");
		val68.Key = "BodyText";
		StaticResourceExtension val181 = new StaticResourceExtension
		{
			Key = "BodyText"
		};
		XamlServiceProvider val182 = new XamlServiceProvider();
		Type? typeFromHandle32 = typeof(IProvideValueTarget);
		object[] array32 = new object[0 + 8];
		array32[0] = val69;
		array32[1] = val77;
		array32[2] = val78;
		array32[3] = val79;
		array32[4] = val80;
		array32[5] = val81;
		array32[6] = val82;
		array32[7] = aboutPage;
		SimpleValueTargetProvider val183 = new SimpleValueTargetProvider(array32, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[9] { val83, val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj59 = (object)val183;
		val182.Add(typeFromHandle32, (object)val183);
		val182.Add(typeof(IReferenceProvider), obj59);
		val182.Add(typeof(IRootObjectProvider), obj59);
		val182.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(167, 36)));
		object obj60 = val181.ProvideValue((IServiceProvider)val182);
		((BindableObject)val69).SetValue(VisualElement.StyleProperty, (obj60 == null || !typeof(BindingBase).IsAssignableFrom(obj60.GetType())) ? obj60 : obj60);
		((BindableObject)val69).SetValue(Label.FontSizeProperty, (object)13.0);
		((BindableObject)val69).SetValue(Label.LineHeightProperty, (object)1.4);
		((Layout)val77).Children.Add((IView)(object)val69);
		((BindableObject)val71).SetValue(Label.TextProperty, (object)"En ningún caso los autores serán responsables de daños directos, indirectos, incidentales o consecuentes que resulten del uso de este software.");
		val70.Key = "BodyText";
		StaticResourceExtension val184 = new StaticResourceExtension
		{
			Key = "BodyText"
		};
		XamlServiceProvider val185 = new XamlServiceProvider();
		Type? typeFromHandle33 = typeof(IProvideValueTarget);
		object[] array33 = new object[0 + 8];
		array33[0] = val71;
		array33[1] = val77;
		array33[2] = val78;
		array33[3] = val79;
		array33[4] = val80;
		array33[5] = val81;
		array33[6] = val82;
		array33[7] = aboutPage;
		SimpleValueTargetProvider val186 = new SimpleValueTargetProvider(array33, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[9] { val83, val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj61 = (object)val186;
		val185.Add(typeFromHandle33, (object)val186);
		val185.Add(typeof(IReferenceProvider), obj61);
		val185.Add(typeof(IRootObjectProvider), obj61);
		val185.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(173, 36)));
		object obj62 = val184.ProvideValue((IServiceProvider)val185);
		((BindableObject)val71).SetValue(VisualElement.StyleProperty, (obj62 == null || !typeof(BindingBase).IsAssignableFrom(obj62.GetType())) ? obj62 : obj62);
		((BindableObject)val71).SetValue(Label.FontSizeProperty, (object)13.0);
		((BindableObject)val71).SetValue(Label.LineHeightProperty, (object)1.4);
		((Layout)val77).Children.Add((IView)(object)val71);
		((BindableObject)val76).SetValue(Border.StrokeThicknessProperty, (object)0.0);
		((BindableObject)val76).SetValue(Border.PaddingProperty, (object)new Thickness(12.0));
		val72.Key = "Danger";
		StaticResourceExtension val187 = new StaticResourceExtension
		{
			Key = "Danger"
		};
		XamlServiceProvider val188 = new XamlServiceProvider();
		Type? typeFromHandle34 = typeof(IProvideValueTarget);
		object[] array34 = new object[0 + 8];
		array34[0] = val76;
		array34[1] = val77;
		array34[2] = val78;
		array34[3] = val79;
		array34[4] = val80;
		array34[5] = val81;
		array34[6] = val82;
		array34[7] = aboutPage;
		SimpleValueTargetProvider val189 = new SimpleValueTargetProvider(array34, (object)VisualElement.BackgroundColorProperty, (INameScope[])(object)new NameScope[9] { val83, val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj63 = (object)val189;
		val188.Add(typeFromHandle34, (object)val189);
		val188.Add(typeof(IReferenceProvider), obj63);
		val188.Add(typeof(IRootObjectProvider), obj63);
		val188.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(179, 37)));
		object obj64 = val187.ProvideValue((IServiceProvider)val188);
		((BindableObject)val76).SetValue(VisualElement.BackgroundColorProperty, (obj64 == null || !typeof(BindingBase).IsAssignableFrom(obj64.GetType())) ? obj64 : obj64);
		((BindableObject)val73).SetValue(RoundRectangle.CornerRadiusProperty, (object)new CornerRadius(6.0));
		((BindableObject)val76).SetValue(Border.StrokeShapeProperty, (object)val73);
		((BindableObject)val75).SetValue(Label.TextProperty, (object)"⚠\ufe0f Uso bajo su propio riesgo");
		((BindableObject)val75).SetValue(Label.FontSizeProperty, (object)13.0);
		((BindableObject)val75).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		((BindableObject)val75).SetValue(Label.LineHeightProperty, (object)1.4);
		val74.Key = "White";
		StaticResourceExtension val190 = new StaticResourceExtension
		{
			Key = "White"
		};
		XamlServiceProvider val191 = new XamlServiceProvider();
		Type? typeFromHandle35 = typeof(IProvideValueTarget);
		object[] array35 = new object[0 + 9];
		array35[0] = val75;
		array35[1] = val76;
		array35[2] = val77;
		array35[3] = val78;
		array35[4] = val79;
		array35[5] = val80;
		array35[6] = val81;
		array35[7] = val82;
		array35[8] = aboutPage;
		SimpleValueTargetProvider val192 = new SimpleValueTargetProvider(array35, (object)Label.TextColorProperty, (INameScope[])(object)new NameScope[10] { val83, val83, val83, val83, val83, val83, val83, val83, val83, val83 }, (object)aboutPage);
		object obj65 = (object)val192;
		val191.Add(typeFromHandle35, (object)val192);
		val191.Add(typeof(IReferenceProvider), obj65);
		val191.Add(typeof(IRootObjectProvider), obj65);
		val191.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(188, 40)));
		object obj66 = val190.ProvideValue((IServiceProvider)val191);
		((BindableObject)val75).SetValue(Label.TextColorProperty, (obj66 == null || !typeof(BindingBase).IsAssignableFrom(obj66.GetType())) ? obj66 : obj66);
		((BindableObject)val75).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val76).SetValue(Border.ContentProperty, (object)val75);
		((Layout)val77).Children.Add((IView)(object)val76);
		((BindableObject)val78).SetValue(Border.ContentProperty, (object)val77);
		((Layout)val79).Children.Add((IView)(object)val78);
		((BindableObject)val80).SetValue(Border.ContentProperty, (object)val79);
		((Layout)val81).Children.Add((IView)(object)val80);
		val82.Content = (View)(object)val81;
		((BindableObject)aboutPage).SetValue(ContentPage.ContentProperty, (object)val82);
	}
}
