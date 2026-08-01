using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Android.App;
using Android.Telephony;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Xaml.Internals;
using Microsoft.Maui.Storage;
using SMSForwarder.Services;
using SocShared;

namespace SMSForwarder;

[XamlFilePath("DiagnosticsPage.xaml")]
public class DiagnosticsPage : ContentPage
{
	private readonly ILoggingService _loggingService;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label PermissionsStatus;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label PhonesCount;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LogsLabel;

	public DiagnosticsPage(ILoggingService loggingService)
	{
		InitializeComponent();
		_loggingService = loggingService;
		RefreshStatus();
	}

	private async void OnRefreshClicked(object sender, EventArgs e)
	{
		await RefreshStatus();
	}

	private async Task RefreshStatus()
	{
		try
		{
			PermissionStatus receiveSmsStatus = await Permissions.CheckStatusAsync<SmsPermissions.ReceiveSms>();
			PermissionStatus value = await Permissions.CheckStatusAsync<SmsPermissions.SendSms>();
			PermissionsStatus.Text = $"Recibir SMS: {receiveSmsStatus}\nEnviar SMS: {value}";
			List<string> list = JsonSerializer.Deserialize<List<string>>(Preferences.Default.Get<string>("phones", "[]", (string)null));
			PhonesCount.Text = $"{list?.Count ?? 0} números configurados";
			LogsLabel.Text = _loggingService.GetLogContents();
			_loggingService.LogInfo("Estado de diagnósticos actualizado");
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al actualizar diagnósticos", ex);
			await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al actualizar el estado", "OK");
		}
	}

	private async void OnClearLogsClicked(object sender, EventArgs e)
	{
		try
		{
			string path = Path.Combine(FileSystem.AppDataDirectory, "sms_forwarder.log");
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			LogsLabel.Text = "Logs limpiados";
			_loggingService.LogInfo("Logs limpiados por el usuario");
		}
		catch (Exception ex)
		{
			await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al limpiar logs: " + ex.Message, "OK");
		}
	}

	private async void OnTestSmsClicked(object sender, EventArgs e)
	{
		try
		{
			List<string> list = JsonSerializer.Deserialize<List<string>>(Preferences.Default.Get<string>("phones", "[]", (string)null));
			if (list == null || list.Count == 0)
			{
				await ModernDialog.AlertAsync((Page)(object)this, "Sin números", "No hay números configurados para enviar SMS de prueba", "OK");
				return;
			}
			string text = $"SMS de prueba desde SMSForwarder - {DateTime.Now:HH:mm:ss}";
			SmsManager val = SmsManager.Default;
			foreach (string item in list)
			{
				try
				{
					val.SendTextMessage(item, (string)null, text, (PendingIntent)null, (PendingIntent)null);
					_loggingService.LogInfo("SMS de prueba enviado a " + item);
				}
				catch (Exception ex)
				{
					_loggingService.LogError("Error enviando SMS de prueba a " + item, ex);
				}
			}
			await ModernDialog.AlertAsync((Page)(object)this, "SMS de prueba", $"SMS de prueba enviado a {list.Count} números", "OK");
			await RefreshStatus();
		}
		catch (Exception ex2)
		{
			_loggingService.LogError("Error en SMS de prueba", ex2);
			await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al enviar SMS de prueba: " + ex2.Message, "OK");
		}
	}

	private async void OnCheckPermissionsClicked(object sender, EventArgs e)
	{
		try
		{
			await new PermissionService().ShowPermissionStatusAsync();
			await RefreshStatus();
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al verificar permisos", ex);
			await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al verificar el estado de los permisos", "OK");
		}
	}

	private async void OnConfigureAllPermissionsClicked(object sender, EventArgs e)
	{
		try
		{
			if (!(await new PermissionService().CheckAndRequestAllPermissionsAsync()))
			{
				await ModernDialog.AlertAsync((Page)(object)this, "Atención", "Algunos permisos no pudieron ser configurados. Revise la configuración manualmente.", "OK");
			}
			else
			{
				await ModernDialog.AlertAsync((Page)(object)this, "Éxito", "Todos los permisos han sido configurados correctamente", "OK");
			}
			await RefreshStatus();
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al configurar permisos", ex);
			await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al configurar los permisos", "OK");
		}
	}

	private async void OnBatteryOptimizationClicked(object sender, EventArgs e)
	{
		try
		{
			SmsPermissions.BatteryOptimizationPermission batteryPermission = new SmsPermissions.BatteryOptimizationPermission();
			if ((int)(await ((BasePermission)batteryPermission).CheckStatusAsync()) == 3)
			{
				await ModernDialog.AlertAsync((Page)(object)this, "Estado de Batería", "✅ La optimización de batería está desactivada correctamente", "OK");
			}
			else if (await ModernDialog.AlertAsync((Page)(object)this, "Optimización de Batería", "La optimización de batería está activada. Esto puede impedir que la aplicación funcione en segundo plano.\n\n¿Desea abrir la configuración?", "Sí", "No"))
			{
				await ((BasePermission)batteryPermission).RequestAsync();
			}
			await RefreshStatus();
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al gestionar optimización de batería", ex);
			await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al acceder a la configuración de batería", "OK");
		}
	}

	private async void OnAutostartClicked(object sender, EventArgs e)
	{
		try
		{
			SmsPermissions.AutoStartPermission autostartPermission = new SmsPermissions.AutoStartPermission();
			await ModernDialog.AlertAsync((Page)(object)this, "Configuración de Autostart", "Se abrirá la configuración de autostart. Busque 'SMS Forwarder' en la lista y active el inicio automático para asegurar que la aplicación funcione después de reiniciar el dispositivo.", "Entendido");
			await ((BasePermission)autostartPermission).RequestAsync();
			await RefreshStatus();
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al gestionar autostart", ex);
			await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al acceder a la configuración de autostart", "OK");
		}
	}

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	[MemberNotNull("PermissionsStatus")]
	[MemberNotNull("PhonesCount")]
	[MemberNotNull("LogsLabel")]
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
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0509: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected O, but got Unknown
		//IL_0550: Expected O, but got Unknown
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Expected O, but got Unknown
		//IL_063c: Expected O, but got Unknown
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Expected O, but got Unknown
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Expected O, but got Unknown
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_068a: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_0751: Expected O, but got Unknown
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Expected O, but got Unknown
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Expected O, but got Unknown
		//IL_0798: Expected O, but got Unknown
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Expected O, but got Unknown
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0899: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_0912: Expected O, but got Unknown
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Expected O, but got Unknown
		//IL_0959: Expected O, but got Unknown
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab8: Expected O, but got Unknown
		//IL_0abd: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Expected O, but got Unknown
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Expected O, but got Unknown
		//IL_0b04: Expected O, but got Unknown
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0d: Expected O, but got Unknown
		//IL_0c12: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Expected O, but got Unknown
		//IL_0c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Expected O, but got Unknown
		//IL_0c59: Expected O, but got Unknown
		//IL_0ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d59: Expected O, but got Unknown
		//IL_0d5e: Expected O, but got Unknown
		//IL_0d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9b: Expected O, but got Unknown
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da0: Expected O, but got Unknown
		//IL_0da5: Expected O, but got Unknown
		//IL_0de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f04: Expected O, but got Unknown
		//IL_0f09: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f46: Expected O, but got Unknown
		//IL_0f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4b: Expected O, but got Unknown
		//IL_0f50: Expected O, but got Unknown
		//IL_0fd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1051: Unknown result type (might be due to invalid IL or missing references)
		//IL_1056: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Expected O, but got Unknown
		//IL_105e: Expected O, but got Unknown
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1070: Unknown result type (might be due to invalid IL or missing references)
		//IL_1082: Unknown result type (might be due to invalid IL or missing references)
		//IL_1091: Unknown result type (might be due to invalid IL or missing references)
		//IL_109b: Expected O, but got Unknown
		//IL_1096: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a0: Expected O, but got Unknown
		//IL_10a5: Expected O, but got Unknown
		//IL_1158: Unknown result type (might be due to invalid IL or missing references)
		//IL_115d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1168: Unknown result type (might be due to invalid IL or missing references)
		//IL_116d: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cc: Expected O, but got Unknown
		//IL_11d1: Expected O, but got Unknown
		//IL_11d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1204: Unknown result type (might be due to invalid IL or missing references)
		//IL_120e: Expected O, but got Unknown
		//IL_1209: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Expected O, but got Unknown
		//IL_1218: Expected O, but got Unknown
		//IL_1256: Unknown result type (might be due to invalid IL or missing references)
		//IL_125b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1266: Unknown result type (might be due to invalid IL or missing references)
		//IL_126b: Unknown result type (might be due to invalid IL or missing references)
		//IL_12db: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e3: Expected O, but got Unknown
		//IL_12e8: Expected O, but got Unknown
		//IL_12e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_130c: Unknown result type (might be due to invalid IL or missing references)
		//IL_131b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1325: Expected O, but got Unknown
		//IL_1320: Unknown result type (might be due to invalid IL or missing references)
		//IL_132a: Expected O, but got Unknown
		//IL_132f: Expected O, but got Unknown
		//IL_1346: Unknown result type (might be due to invalid IL or missing references)
		//IL_134b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1356: Unknown result type (might be due to invalid IL or missing references)
		//IL_135b: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d3: Expected O, but got Unknown
		//IL_13d8: Expected O, but got Unknown
		//IL_13d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_140b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1415: Expected O, but got Unknown
		//IL_1410: Unknown result type (might be due to invalid IL or missing references)
		//IL_141a: Expected O, but got Unknown
		//IL_141f: Expected O, but got Unknown
		//IL_142c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1431: Unknown result type (might be due to invalid IL or missing references)
		//IL_1443: Unknown result type (might be due to invalid IL or missing references)
		//IL_144d: Expected O, but got Unknown
		//IL_144d: Unknown result type (might be due to invalid IL or missing references)
		//IL_145c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1466: Expected O, but got Unknown
		//IL_1461: Unknown result type (might be due to invalid IL or missing references)
		//IL_146b: Expected O, but got Unknown
		//IL_1470: Expected O, but got Unknown
		//IL_1487: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_150f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1514: Unknown result type (might be due to invalid IL or missing references)
		//IL_151f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1524: Unknown result type (might be due to invalid IL or missing references)
		//IL_157b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1580: Unknown result type (might be due to invalid IL or missing references)
		//IL_1583: Expected O, but got Unknown
		//IL_1588: Expected O, but got Unknown
		//IL_1588: Unknown result type (might be due to invalid IL or missing references)
		//IL_159a: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c5: Expected O, but got Unknown
		//IL_15c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ca: Expected O, but got Unknown
		//IL_15cf: Expected O, but got Unknown
		//IL_1674: Unknown result type (might be due to invalid IL or missing references)
		//IL_1679: Unknown result type (might be due to invalid IL or missing references)
		//IL_1684: Unknown result type (might be due to invalid IL or missing references)
		//IL_1689: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e8: Expected O, but got Unknown
		//IL_16ed: Expected O, but got Unknown
		//IL_16ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1711: Unknown result type (might be due to invalid IL or missing references)
		//IL_1720: Unknown result type (might be due to invalid IL or missing references)
		//IL_172a: Expected O, but got Unknown
		//IL_1725: Unknown result type (might be due to invalid IL or missing references)
		//IL_172f: Expected O, but got Unknown
		//IL_1734: Expected O, but got Unknown
		//IL_1772: Unknown result type (might be due to invalid IL or missing references)
		//IL_1777: Unknown result type (might be due to invalid IL or missing references)
		//IL_1782: Unknown result type (might be due to invalid IL or missing references)
		//IL_1787: Unknown result type (might be due to invalid IL or missing references)
		//IL_17de: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e6: Expected O, but got Unknown
		//IL_17eb: Expected O, but got Unknown
		//IL_17eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_180f: Unknown result type (might be due to invalid IL or missing references)
		//IL_181e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1828: Expected O, but got Unknown
		//IL_1823: Unknown result type (might be due to invalid IL or missing references)
		//IL_182d: Expected O, but got Unknown
		//IL_1832: Expected O, but got Unknown
		//IL_189b: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a6: Expected O, but got Unknown
		//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b3: Expected O, but got Unknown
		//IL_18b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bd: Expected O, but got Unknown
		//IL_191a: Unknown result type (might be due to invalid IL or missing references)
		//IL_191f: Unknown result type (might be due to invalid IL or missing references)
		//IL_192a: Unknown result type (might be due to invalid IL or missing references)
		//IL_192f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1990: Unknown result type (might be due to invalid IL or missing references)
		//IL_1995: Unknown result type (might be due to invalid IL or missing references)
		//IL_1998: Expected O, but got Unknown
		//IL_199d: Expected O, but got Unknown
		//IL_199d: Unknown result type (might be due to invalid IL or missing references)
		//IL_19af: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19da: Expected O, but got Unknown
		//IL_19d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19df: Expected O, but got Unknown
		//IL_19e4: Expected O, but got Unknown
		//IL_1a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b19: Expected O, but got Unknown
		//IL_1b1e: Expected O, but got Unknown
		//IL_1b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5b: Expected O, but got Unknown
		//IL_1b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b60: Expected O, but got Unknown
		//IL_1b65: Expected O, but got Unknown
		//IL_1bf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c67: Expected O, but got Unknown
		//IL_1c6c: Expected O, but got Unknown
		//IL_1c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca9: Expected O, but got Unknown
		//IL_1ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cae: Expected O, but got Unknown
		//IL_1cb3: Expected O, but got Unknown
		//IL_1cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7e: Expected O, but got Unknown
		//IL_1d83: Expected O, but got Unknown
		//IL_1d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc0: Expected O, but got Unknown
		//IL_1dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc5: Expected O, but got Unknown
		//IL_1dca: Expected O, but got Unknown
		//IL_1de1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6e: Expected O, but got Unknown
		//IL_1e73: Expected O, but got Unknown
		//IL_1e73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb0: Expected O, but got Unknown
		//IL_1eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb5: Expected O, but got Unknown
		//IL_1eba: Expected O, but got Unknown
		//IL_1ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ecc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ede: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee8: Expected O, but got Unknown
		//IL_1ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f01: Expected O, but got Unknown
		//IL_1efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f06: Expected O, but got Unknown
		//IL_1f0b: Expected O, but got Unknown
		//IL_1f2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd5: Expected O, but got Unknown
		//IL_1fda: Expected O, but got Unknown
		//IL_1fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffe: Unknown result type (might be due to invalid IL or missing references)
		//IL_200d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2017: Expected O, but got Unknown
		//IL_2012: Unknown result type (might be due to invalid IL or missing references)
		//IL_201c: Expected O, but got Unknown
		//IL_2021: Expected O, but got Unknown
		//IL_20c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_20cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_20db: Unknown result type (might be due to invalid IL or missing references)
		//IL_2132: Unknown result type (might be due to invalid IL or missing references)
		//IL_2137: Unknown result type (might be due to invalid IL or missing references)
		//IL_213a: Expected O, but got Unknown
		//IL_213f: Expected O, but got Unknown
		//IL_213f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2151: Unknown result type (might be due to invalid IL or missing references)
		//IL_2163: Unknown result type (might be due to invalid IL or missing references)
		//IL_2172: Unknown result type (might be due to invalid IL or missing references)
		//IL_217c: Expected O, but got Unknown
		//IL_2177: Unknown result type (might be due to invalid IL or missing references)
		//IL_2181: Expected O, but got Unknown
		//IL_2186: Expected O, but got Unknown
		//IL_21c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2249: Unknown result type (might be due to invalid IL or missing references)
		//IL_224e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2251: Expected O, but got Unknown
		//IL_2256: Expected O, but got Unknown
		//IL_2256: Unknown result type (might be due to invalid IL or missing references)
		//IL_2268: Unknown result type (might be due to invalid IL or missing references)
		//IL_227a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2289: Unknown result type (might be due to invalid IL or missing references)
		//IL_2293: Expected O, but got Unknown
		//IL_228e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2298: Expected O, but got Unknown
		//IL_229d: Expected O, but got Unknown
		//IL_22b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2339: Unknown result type (might be due to invalid IL or missing references)
		//IL_233e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2341: Expected O, but got Unknown
		//IL_2346: Expected O, but got Unknown
		//IL_2346: Unknown result type (might be due to invalid IL or missing references)
		//IL_2358: Unknown result type (might be due to invalid IL or missing references)
		//IL_236a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2379: Unknown result type (might be due to invalid IL or missing references)
		//IL_2383: Expected O, but got Unknown
		//IL_237e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2388: Expected O, but got Unknown
		//IL_238d: Expected O, but got Unknown
		//IL_239a: Unknown result type (might be due to invalid IL or missing references)
		//IL_239f: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_23bb: Expected O, but got Unknown
		//IL_23bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d4: Expected O, but got Unknown
		//IL_23cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d9: Expected O, but got Unknown
		//IL_23de: Expected O, but got Unknown
		//IL_23f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_242f: Unknown result type (might be due to invalid IL or missing references)
		//IL_247d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2482: Unknown result type (might be due to invalid IL or missing references)
		//IL_248d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2492: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f1: Expected O, but got Unknown
		//IL_24f6: Expected O, but got Unknown
		//IL_24f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2508: Unknown result type (might be due to invalid IL or missing references)
		//IL_251a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2529: Unknown result type (might be due to invalid IL or missing references)
		//IL_2533: Expected O, but got Unknown
		//IL_252e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2538: Expected O, but got Unknown
		//IL_253d: Expected O, but got Unknown
		//IL_25e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_264e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2653: Unknown result type (might be due to invalid IL or missing references)
		//IL_2656: Expected O, but got Unknown
		//IL_265b: Expected O, but got Unknown
		//IL_265b: Unknown result type (might be due to invalid IL or missing references)
		//IL_266d: Unknown result type (might be due to invalid IL or missing references)
		//IL_267f: Unknown result type (might be due to invalid IL or missing references)
		//IL_268e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2698: Expected O, but got Unknown
		//IL_2693: Unknown result type (might be due to invalid IL or missing references)
		//IL_269d: Expected O, but got Unknown
		//IL_26a2: Expected O, but got Unknown
		//IL_2741: Unknown result type (might be due to invalid IL or missing references)
		//IL_2746: Unknown result type (might be due to invalid IL or missing references)
		//IL_2751: Unknown result type (might be due to invalid IL or missing references)
		//IL_2756: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b5: Expected O, but got Unknown
		//IL_27ba: Expected O, but got Unknown
		//IL_27ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_27cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_27de: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f7: Expected O, but got Unknown
		//IL_27f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_27fc: Expected O, but got Unknown
		//IL_2801: Expected O, but got Unknown
		//IL_283f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2844: Unknown result type (might be due to invalid IL or missing references)
		//IL_284f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2854: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_28cc: Expected O, but got Unknown
		//IL_28d1: Expected O, but got Unknown
		//IL_28d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2904: Unknown result type (might be due to invalid IL or missing references)
		//IL_290e: Expected O, but got Unknown
		//IL_2909: Unknown result type (might be due to invalid IL or missing references)
		//IL_2913: Expected O, but got Unknown
		//IL_2918: Expected O, but got Unknown
		//IL_292f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2934: Unknown result type (might be due to invalid IL or missing references)
		//IL_293f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2944: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_29bc: Expected O, but got Unknown
		//IL_29c1: Expected O, but got Unknown
		//IL_29c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_29fe: Expected O, but got Unknown
		//IL_29f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a03: Expected O, but got Unknown
		//IL_2a08: Expected O, but got Unknown
		//IL_2a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a36: Expected O, but got Unknown
		//IL_2a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a4f: Expected O, but got Unknown
		//IL_2a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a54: Expected O, but got Unknown
		//IL_2a59: Expected O, but got Unknown
		//IL_2a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0d: Expected O, but got Unknown
		//IL_2b12: Expected O, but got Unknown
		//IL_2b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4f: Expected O, but got Unknown
		//IL_2b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b54: Expected O, but got Unknown
		//IL_2b59: Expected O, but got Unknown
		//IL_2b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c94: Expected O, but got Unknown
		//IL_2c99: Expected O, but got Unknown
		//IL_2c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd6: Expected O, but got Unknown
		//IL_2cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cdb: Expected O, but got Unknown
		//IL_2ce0: Expected O, but got Unknown
		//IL_2d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dee: Expected O, but got Unknown
		//IL_2df3: Expected O, but got Unknown
		//IL_2df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e30: Expected O, but got Unknown
		//IL_2e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e35: Expected O, but got Unknown
		//IL_2e3a: Expected O, but got Unknown
		StaticResourceExtension val = new StaticResourceExtension();
		StaticResourceExtension val2 = new StaticResourceExtension();
		AppThemeBindingExtension val3 = new AppThemeBindingExtension();
		Label val4 = new Label();
		StaticResourceExtension val5 = new StaticResourceExtension();
		Label val6 = new Label();
		VerticalStackLayout val7 = new VerticalStackLayout();
		StaticResourceExtension val8 = new StaticResourceExtension();
		Label val9 = new Label();
		StaticResourceExtension val10 = new StaticResourceExtension();
		Label val11 = new Label();
		StaticResourceExtension val12 = new StaticResourceExtension();
		Label val13 = new Label();
		VerticalStackLayout val14 = new VerticalStackLayout();
		Border val15 = new Border();
		StaticResourceExtension val16 = new StaticResourceExtension();
		Label val17 = new Label();
		StaticResourceExtension val18 = new StaticResourceExtension();
		Label val19 = new Label();
		StaticResourceExtension val20 = new StaticResourceExtension();
		Label val21 = new Label();
		VerticalStackLayout val22 = new VerticalStackLayout();
		Border val23 = new Border();
		Grid val24 = new Grid();
		StaticResourceExtension val25 = new StaticResourceExtension();
		StaticResourceExtension val26 = new StaticResourceExtension();
		StaticResourceExtension val27 = new StaticResourceExtension();
		AppThemeBindingExtension val28 = new AppThemeBindingExtension();
		Label val29 = new Label();
		StaticResourceExtension val30 = new StaticResourceExtension();
		Button val31 = new Button();
		StaticResourceExtension val32 = new StaticResourceExtension();
		StaticResourceExtension val33 = new StaticResourceExtension();
		Button val34 = new Button();
		StaticResourceExtension val35 = new StaticResourceExtension();
		Button val36 = new Button();
		StaticResourceExtension val37 = new StaticResourceExtension();
		Button val38 = new Button();
		Grid val39 = new Grid();
		StaticResourceExtension val40 = new StaticResourceExtension();
		StaticResourceExtension val41 = new StaticResourceExtension();
		StaticResourceExtension val42 = new StaticResourceExtension();
		AppThemeBindingExtension val43 = new AppThemeBindingExtension();
		StaticResourceExtension val44 = new StaticResourceExtension();
		Label val45 = new Label();
		Border val46 = new Border();
		VerticalStackLayout val47 = new VerticalStackLayout();
		StaticResourceExtension val48 = new StaticResourceExtension();
		StaticResourceExtension val49 = new StaticResourceExtension();
		StaticResourceExtension val50 = new StaticResourceExtension();
		AppThemeBindingExtension val51 = new AppThemeBindingExtension();
		Label val52 = new Label();
		StaticResourceExtension val53 = new StaticResourceExtension();
		Button val54 = new Button();
		StaticResourceExtension val55 = new StaticResourceExtension();
		Button val56 = new Button();
		VerticalStackLayout val57 = new VerticalStackLayout();
		StaticResourceExtension val58 = new StaticResourceExtension();
		StaticResourceExtension val59 = new StaticResourceExtension();
		StaticResourceExtension val60 = new StaticResourceExtension();
		AppThemeBindingExtension val61 = new AppThemeBindingExtension();
		Label val62 = new Label();
		StaticResourceExtension val63 = new StaticResourceExtension();
		StaticResourceExtension val64 = new StaticResourceExtension();
		Label val65 = new Label();
		ScrollView val66 = new ScrollView();
		Border val67 = new Border();
		StaticResourceExtension val68 = new StaticResourceExtension();
		Button val69 = new Button();
		VerticalStackLayout val70 = new VerticalStackLayout();
		VerticalStackLayout val71 = new VerticalStackLayout();
		ScrollView val72 = new ScrollView();
		DiagnosticsPage diagnosticsPage;
		NameScope val73 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(diagnosticsPage = this))) ?? ((object)new NameScope()));
		NameScope.SetNameScope((BindableObject)(object)diagnosticsPage, (INameScope)(object)val73);
		((Element)val72).transientNamescope = (INameScope)(object)val73;
		((Element)val71).transientNamescope = (INameScope)(object)val73;
		((Element)val7).transientNamescope = (INameScope)(object)val73;
		((Element)val4).transientNamescope = (INameScope)(object)val73;
		((Element)val6).transientNamescope = (INameScope)(object)val73;
		((Element)val24).transientNamescope = (INameScope)(object)val73;
		((Element)val15).transientNamescope = (INameScope)(object)val73;
		((Element)val14).transientNamescope = (INameScope)(object)val73;
		((Element)val9).transientNamescope = (INameScope)(object)val73;
		((Element)val11).transientNamescope = (INameScope)(object)val73;
		((Element)val13).transientNamescope = (INameScope)(object)val73;
		((INameScope)val73).RegisterName("PermissionsStatus", (object)val13);
		if (((Element)val13).StyleId == null)
		{
			((Element)val13).StyleId = "PermissionsStatus";
		}
		((Element)val23).transientNamescope = (INameScope)(object)val73;
		((Element)val22).transientNamescope = (INameScope)(object)val73;
		((Element)val17).transientNamescope = (INameScope)(object)val73;
		((Element)val19).transientNamescope = (INameScope)(object)val73;
		((Element)val21).transientNamescope = (INameScope)(object)val73;
		((INameScope)val73).RegisterName("PhonesCount", (object)val21);
		if (((Element)val21).StyleId == null)
		{
			((Element)val21).StyleId = "PhonesCount";
		}
		((Element)val47).transientNamescope = (INameScope)(object)val73;
		((Element)val29).transientNamescope = (INameScope)(object)val73;
		((Element)val31).transientNamescope = (INameScope)(object)val73;
		((Element)val34).transientNamescope = (INameScope)(object)val73;
		((Element)val39).transientNamescope = (INameScope)(object)val73;
		((Element)val36).transientNamescope = (INameScope)(object)val73;
		((Element)val38).transientNamescope = (INameScope)(object)val73;
		((Element)val46).transientNamescope = (INameScope)(object)val73;
		((Element)val45).transientNamescope = (INameScope)(object)val73;
		((Element)val57).transientNamescope = (INameScope)(object)val73;
		((Element)val52).transientNamescope = (INameScope)(object)val73;
		((Element)val54).transientNamescope = (INameScope)(object)val73;
		((Element)val56).transientNamescope = (INameScope)(object)val73;
		((Element)val70).transientNamescope = (INameScope)(object)val73;
		((Element)val62).transientNamescope = (INameScope)(object)val73;
		((Element)val67).transientNamescope = (INameScope)(object)val73;
		((Element)val66).transientNamescope = (INameScope)(object)val73;
		((Element)val65).transientNamescope = (INameScope)(object)val73;
		((INameScope)val73).RegisterName("LogsLabel", (object)val65);
		if (((Element)val65).StyleId == null)
		{
			((Element)val65).StyleId = "LogsLabel";
		}
		((Element)val69).transientNamescope = (INameScope)(object)val73;
		PermissionsStatus = val13;
		PhonesCount = val21;
		LogsLabel = val65;
		((BindableObject)diagnosticsPage).SetValue(Page.TitleProperty, (object)"Diagnósticos");
		((BindableObject)val71).SetValue(Layout.PaddingProperty, (object)new Thickness(24.0));
		((BindableObject)val71).SetValue(StackBase.SpacingProperty, (object)20.0);
		((BindableObject)val7).SetValue(StackBase.SpacingProperty, (object)8.0);
		((BindableObject)val4).SetValue(Label.TextProperty, (object)"Diagnósticos");
		((BindableObject)val4).SetValue(Label.FontSizeProperty, (object)24.0);
		((BindableObject)val4).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		val.Key = "TextPrimaryLight";
		StaticResourceExtension val74 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val75 = new XamlServiceProvider();
		Type? typeFromHandle = typeof(IProvideValueTarget);
		object[] array = new object[0 + 6];
		array[0] = val3;
		array[1] = val4;
		array[2] = val7;
		array[3] = val71;
		array[4] = val72;
		array[5] = diagnosticsPage;
		SimpleValueTargetProvider val76 = new SimpleValueTargetProvider(array, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj = (object)val76;
		val75.Add(typeFromHandle, (object)val76);
		val75.Add(typeof(IReferenceProvider), obj);
		val75.Add(typeof(IRootObjectProvider), obj);
		val75.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(15, 24)));
		object light = val74.ProvideValue((IServiceProvider)val75);
		val3.Light = light;
		val2.Key = "TextPrimaryDark";
		StaticResourceExtension val77 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val78 = new XamlServiceProvider();
		Type? typeFromHandle2 = typeof(IProvideValueTarget);
		object[] array2 = new object[0 + 6];
		array2[0] = val3;
		array2[1] = val4;
		array2[2] = val7;
		array2[3] = val71;
		array2[4] = val72;
		array2[5] = diagnosticsPage;
		SimpleValueTargetProvider val79 = new SimpleValueTargetProvider(array2, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj2 = (object)val79;
		val78.Add(typeFromHandle2, (object)val79);
		val78.Add(typeof(IReferenceProvider), obj2);
		val78.Add(typeof(IRootObjectProvider), obj2);
		val78.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(15, 24)));
		object dark = val77.ProvideValue((IServiceProvider)val78);
		val3.Dark = dark;
		XamlServiceProvider val80 = new XamlServiceProvider();
		val80.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val4, (object)Label.TextColorProperty));
		val80.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(15, 24)));
		BindingBase val81 = ((IMarkupExtension<BindingBase>)(object)val3).ProvideValue((IServiceProvider)val80);
		((BindableObject)val4).SetBinding(Label.TextColorProperty, val81);
		((BindableObject)val4).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val7).Children.Add((IView)(object)val4);
		((BindableObject)val6).SetValue(Label.TextProperty, (object)"Monitoreo y estado del sistema");
		val5.Key = "HintText";
		StaticResourceExtension val82 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val83 = new XamlServiceProvider();
		Type? typeFromHandle3 = typeof(IProvideValueTarget);
		object[] array3 = new object[0 + 5];
		array3[0] = val6;
		array3[1] = val7;
		array3[2] = val71;
		array3[3] = val72;
		array3[4] = diagnosticsPage;
		SimpleValueTargetProvider val84 = new SimpleValueTargetProvider(array3, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj3 = (object)val84;
		val83.Add(typeFromHandle3, (object)val84);
		val83.Add(typeof(IReferenceProvider), obj3);
		val83.Add(typeof(IRootObjectProvider), obj3);
		val83.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(18, 24)));
		object obj4 = val82.ProvideValue((IServiceProvider)val83);
		((BindableObject)val6).SetValue(VisualElement.StyleProperty, (obj4 == null || !typeof(BindingBase).IsAssignableFrom(obj4.GetType())) ? obj4 : obj4);
		((BindableObject)val6).SetValue(Label.FontSizeProperty, (object)14.0);
		((BindableObject)val6).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val7).Children.Add((IView)(object)val6);
		((Layout)val71).Children.Add((IView)(object)val7);
		((BindableObject)val24).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
		{
			new ColumnDefinition(GridLength.Star),
			new ColumnDefinition(GridLength.Star)
		}));
		((BindableObject)val24).SetValue(Grid.ColumnSpacingProperty, (object)12.0);
		((BindableObject)val24).SetValue(Grid.RowSpacingProperty, (object)12.0);
		((BindableObject)val15).SetValue(Grid.ColumnProperty, (object)0);
		val8.Key = "Card";
		StaticResourceExtension val85 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val86 = new XamlServiceProvider();
		Type? typeFromHandle4 = typeof(IProvideValueTarget);
		object[] array4 = new object[0 + 5];
		array4[0] = val15;
		array4[1] = val24;
		array4[2] = val71;
		array4[3] = val72;
		array4[4] = diagnosticsPage;
		SimpleValueTargetProvider val87 = new SimpleValueTargetProvider(array4, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj5 = (object)val87;
		val86.Add(typeFromHandle4, (object)val87);
		val86.Add(typeof(IReferenceProvider), obj5);
		val86.Add(typeof(IRootObjectProvider), obj5);
		val86.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(25, 41)));
		object obj6 = val85.ProvideValue((IServiceProvider)val86);
		((BindableObject)val15).SetValue(VisualElement.StyleProperty, (obj6 == null || !typeof(BindingBase).IsAssignableFrom(obj6.GetType())) ? obj6 : obj6);
		((BindableObject)val15).SetValue(Border.PaddingProperty, (object)new Thickness(16.0));
		((BindableObject)val14).SetValue(StackBase.SpacingProperty, (object)8.0);
		((BindableObject)val9).SetValue(Label.TextProperty, (object)"\ud83d\udd12");
		((BindableObject)val9).SetValue(Label.FontSizeProperty, (object)24.0);
		((BindableObject)val9).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val14).Children.Add((IView)(object)val9);
		((BindableObject)val11).SetValue(Label.TextProperty, (object)"Permisos");
		val10.Key = "BodyText";
		StaticResourceExtension val88 = new StaticResourceExtension
		{
			Key = "BodyText"
		};
		XamlServiceProvider val89 = new XamlServiceProvider();
		Type? typeFromHandle5 = typeof(IProvideValueTarget);
		object[] array5 = new object[0 + 7];
		array5[0] = val11;
		array5[1] = val14;
		array5[2] = val15;
		array5[3] = val24;
		array5[4] = val71;
		array5[5] = val72;
		array5[6] = diagnosticsPage;
		SimpleValueTargetProvider val90 = new SimpleValueTargetProvider(array5, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val73, val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj7 = (object)val90;
		val89.Add(typeFromHandle5, (object)val90);
		val89.Add(typeof(IReferenceProvider), obj7);
		val89.Add(typeof(IRootObjectProvider), obj7);
		val89.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(28, 48)));
		object obj8 = val88.ProvideValue((IServiceProvider)val89);
		((BindableObject)val11).SetValue(VisualElement.StyleProperty, (obj8 == null || !typeof(BindingBase).IsAssignableFrom(obj8.GetType())) ? obj8 : obj8);
		((BindableObject)val11).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		((BindableObject)val11).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
		((Layout)val14).Children.Add((IView)(object)val11);
		((BindableObject)val13).SetValue(Label.TextProperty, (object)"Verificando...");
		val12.Key = "HintText";
		StaticResourceExtension val91 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val92 = new XamlServiceProvider();
		Type? typeFromHandle6 = typeof(IProvideValueTarget);
		object[] array6 = new object[0 + 7];
		array6[0] = val13;
		array6[1] = val14;
		array6[2] = val15;
		array6[3] = val24;
		array6[4] = val71;
		array6[5] = val72;
		array6[6] = diagnosticsPage;
		SimpleValueTargetProvider val93 = new SimpleValueTargetProvider(array6, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val73, val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj9 = (object)val93;
		val92.Add(typeFromHandle6, (object)val93);
		val92.Add(typeof(IReferenceProvider), obj9);
		val92.Add(typeof(IRootObjectProvider), obj9);
		val92.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(29, 81)));
		object obj10 = val91.ProvideValue((IServiceProvider)val92);
		((BindableObject)val13).SetValue(VisualElement.StyleProperty, (obj10 == null || !typeof(BindingBase).IsAssignableFrom(obj10.GetType())) ? obj10 : obj10);
		((BindableObject)val13).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
		((Layout)val14).Children.Add((IView)(object)val13);
		((BindableObject)val15).SetValue(Border.ContentProperty, (object)val14);
		((Layout)val24).Children.Add((IView)(object)val15);
		((BindableObject)val23).SetValue(Grid.ColumnProperty, (object)1);
		val16.Key = "Card";
		StaticResourceExtension val94 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val95 = new XamlServiceProvider();
		Type? typeFromHandle7 = typeof(IProvideValueTarget);
		object[] array7 = new object[0 + 5];
		array7[0] = val23;
		array7[1] = val24;
		array7[2] = val71;
		array7[3] = val72;
		array7[4] = diagnosticsPage;
		SimpleValueTargetProvider val96 = new SimpleValueTargetProvider(array7, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj11 = (object)val96;
		val95.Add(typeFromHandle7, (object)val96);
		val95.Add(typeof(IReferenceProvider), obj11);
		val95.Add(typeof(IRootObjectProvider), obj11);
		val95.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(33, 41)));
		object obj12 = val94.ProvideValue((IServiceProvider)val95);
		((BindableObject)val23).SetValue(VisualElement.StyleProperty, (obj12 == null || !typeof(BindingBase).IsAssignableFrom(obj12.GetType())) ? obj12 : obj12);
		((BindableObject)val23).SetValue(Border.PaddingProperty, (object)new Thickness(16.0));
		((BindableObject)val22).SetValue(StackBase.SpacingProperty, (object)8.0);
		((BindableObject)val17).SetValue(Label.TextProperty, (object)"\ud83d\udcf1");
		((BindableObject)val17).SetValue(Label.FontSizeProperty, (object)24.0);
		((BindableObject)val17).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val22).Children.Add((IView)(object)val17);
		((BindableObject)val19).SetValue(Label.TextProperty, (object)"Números");
		val18.Key = "BodyText";
		StaticResourceExtension val97 = new StaticResourceExtension
		{
			Key = "BodyText"
		};
		XamlServiceProvider val98 = new XamlServiceProvider();
		Type? typeFromHandle8 = typeof(IProvideValueTarget);
		object[] array8 = new object[0 + 7];
		array8[0] = val19;
		array8[1] = val22;
		array8[2] = val23;
		array8[3] = val24;
		array8[4] = val71;
		array8[5] = val72;
		array8[6] = diagnosticsPage;
		SimpleValueTargetProvider val99 = new SimpleValueTargetProvider(array8, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val73, val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj13 = (object)val99;
		val98.Add(typeFromHandle8, (object)val99);
		val98.Add(typeof(IReferenceProvider), obj13);
		val98.Add(typeof(IRootObjectProvider), obj13);
		val98.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(36, 47)));
		object obj14 = val97.ProvideValue((IServiceProvider)val98);
		((BindableObject)val19).SetValue(VisualElement.StyleProperty, (obj14 == null || !typeof(BindingBase).IsAssignableFrom(obj14.GetType())) ? obj14 : obj14);
		((BindableObject)val19).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		((BindableObject)val19).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
		((Layout)val22).Children.Add((IView)(object)val19);
		((BindableObject)val21).SetValue(Label.TextProperty, (object)"0");
		val20.Key = "HintText";
		StaticResourceExtension val100 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val101 = new XamlServiceProvider();
		Type? typeFromHandle9 = typeof(IProvideValueTarget);
		object[] array9 = new object[0 + 7];
		array9[0] = val21;
		array9[1] = val22;
		array9[2] = val23;
		array9[3] = val24;
		array9[4] = val71;
		array9[5] = val72;
		array9[6] = diagnosticsPage;
		SimpleValueTargetProvider val102 = new SimpleValueTargetProvider(array9, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val73, val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj15 = (object)val102;
		val101.Add(typeFromHandle9, (object)val102);
		val101.Add(typeof(IReferenceProvider), obj15);
		val101.Add(typeof(IRootObjectProvider), obj15);
		val101.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(37, 62)));
		object obj16 = val100.ProvideValue((IServiceProvider)val101);
		((BindableObject)val21).SetValue(VisualElement.StyleProperty, (obj16 == null || !typeof(BindingBase).IsAssignableFrom(obj16.GetType())) ? obj16 : obj16);
		((BindableObject)val21).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
		((Layout)val22).Children.Add((IView)(object)val21);
		((BindableObject)val23).SetValue(Border.ContentProperty, (object)val22);
		((Layout)val24).Children.Add((IView)(object)val23);
		((Layout)val71).Children.Add((IView)(object)val24);
		((BindableObject)val47).SetValue(StackBase.SpacingProperty, (object)12.0);
		((BindableObject)val29).SetValue(Label.TextProperty, (object)"Configuración de Permisos");
		val25.Key = "CardTitle";
		StaticResourceExtension val103 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val104 = new XamlServiceProvider();
		Type? typeFromHandle10 = typeof(IProvideValueTarget);
		object[] array10 = new object[0 + 5];
		array10[0] = val29;
		array10[1] = val47;
		array10[2] = val71;
		array10[3] = val72;
		array10[4] = diagnosticsPage;
		SimpleValueTargetProvider val105 = new SimpleValueTargetProvider(array10, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj17 = (object)val105;
		val104.Add(typeFromHandle10, (object)val105);
		val104.Add(typeof(IReferenceProvider), obj17);
		val104.Add(typeof(IRootObjectProvider), obj17);
		val104.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(45, 24)));
		object obj18 = val103.ProvideValue((IServiceProvider)val104);
		((BindableObject)val29).SetValue(VisualElement.StyleProperty, (obj18 == null || !typeof(BindingBase).IsAssignableFrom(obj18.GetType())) ? obj18 : obj18);
		val26.Key = "TextPrimaryLight";
		StaticResourceExtension val106 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val107 = new XamlServiceProvider();
		Type? typeFromHandle11 = typeof(IProvideValueTarget);
		object[] array11 = new object[0 + 6];
		array11[0] = val28;
		array11[1] = val29;
		array11[2] = val47;
		array11[3] = val71;
		array11[4] = val72;
		array11[5] = diagnosticsPage;
		SimpleValueTargetProvider val108 = new SimpleValueTargetProvider(array11, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj19 = (object)val108;
		val107.Add(typeFromHandle11, (object)val108);
		val107.Add(typeof(IReferenceProvider), obj19);
		val107.Add(typeof(IRootObjectProvider), obj19);
		val107.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(46, 24)));
		object light2 = val106.ProvideValue((IServiceProvider)val107);
		val28.Light = light2;
		val27.Key = "TextPrimaryDark";
		StaticResourceExtension val109 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val110 = new XamlServiceProvider();
		Type? typeFromHandle12 = typeof(IProvideValueTarget);
		object[] array12 = new object[0 + 6];
		array12[0] = val28;
		array12[1] = val29;
		array12[2] = val47;
		array12[3] = val71;
		array12[4] = val72;
		array12[5] = diagnosticsPage;
		SimpleValueTargetProvider val111 = new SimpleValueTargetProvider(array12, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj20 = (object)val111;
		val110.Add(typeFromHandle12, (object)val111);
		val110.Add(typeof(IReferenceProvider), obj20);
		val110.Add(typeof(IRootObjectProvider), obj20);
		val110.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(46, 24)));
		object dark2 = val109.ProvideValue((IServiceProvider)val110);
		val28.Dark = dark2;
		XamlServiceProvider val112 = new XamlServiceProvider();
		val112.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val29, (object)Label.TextColorProperty));
		val112.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(46, 24)));
		BindingBase val113 = ((IMarkupExtension<BindingBase>)(object)val28).ProvideValue((IServiceProvider)val112);
		((BindableObject)val29).SetBinding(Label.TextColorProperty, val113);
		((BindableObject)val29).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val29).SetValue(View.MarginProperty, (object)new Thickness(0.0, 8.0, 0.0, 0.0));
		((Layout)val47).Children.Add((IView)(object)val29);
		((BindableObject)val31).SetValue(Button.TextProperty, (object)"\ud83d\udd0d Verificar Estado de Permisos");
		val31.Clicked += diagnosticsPage.OnCheckPermissionsClicked;
		val30.Key = "PrimaryButton";
		StaticResourceExtension val114 = new StaticResourceExtension
		{
			Key = "PrimaryButton"
		};
		XamlServiceProvider val115 = new XamlServiceProvider();
		Type? typeFromHandle13 = typeof(IProvideValueTarget);
		object[] array13 = new object[0 + 5];
		array13[0] = val31;
		array13[1] = val47;
		array13[2] = val71;
		array13[3] = val72;
		array13[4] = diagnosticsPage;
		SimpleValueTargetProvider val116 = new SimpleValueTargetProvider(array13, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj21 = (object)val116;
		val115.Add(typeFromHandle13, (object)val116);
		val115.Add(typeof(IReferenceProvider), obj21);
		val115.Add(typeof(IRootObjectProvider), obj21);
		val115.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(52, 25)));
		object obj22 = val114.ProvideValue((IServiceProvider)val115);
		((BindableObject)val31).SetValue(VisualElement.StyleProperty, (obj22 == null || !typeof(BindingBase).IsAssignableFrom(obj22.GetType())) ? obj22 : obj22);
		((BindableObject)val31).SetValue(Button.FontSizeProperty, (object)14.0);
		((BindableObject)val31).SetValue(VisualElement.HeightRequestProperty, (object)44.0);
		((Layout)val47).Children.Add((IView)(object)val31);
		((BindableObject)val34).SetValue(Button.TextProperty, (object)"⚙\ufe0f Configurar Todos los Permisos");
		val34.Clicked += diagnosticsPage.OnConfigureAllPermissionsClicked;
		val32.Key = "PrimaryButton";
		StaticResourceExtension val117 = new StaticResourceExtension
		{
			Key = "PrimaryButton"
		};
		XamlServiceProvider val118 = new XamlServiceProvider();
		Type? typeFromHandle14 = typeof(IProvideValueTarget);
		object[] array14 = new object[0 + 5];
		array14[0] = val34;
		array14[1] = val47;
		array14[2] = val71;
		array14[3] = val72;
		array14[4] = diagnosticsPage;
		SimpleValueTargetProvider val119 = new SimpleValueTargetProvider(array14, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj23 = (object)val119;
		val118.Add(typeFromHandle14, (object)val119);
		val118.Add(typeof(IReferenceProvider), obj23);
		val118.Add(typeof(IRootObjectProvider), obj23);
		val118.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(58, 25)));
		object obj24 = val117.ProvideValue((IServiceProvider)val118);
		((BindableObject)val34).SetValue(VisualElement.StyleProperty, (obj24 == null || !typeof(BindingBase).IsAssignableFrom(obj24.GetType())) ? obj24 : obj24);
		val33.Key = "Accent";
		StaticResourceExtension val120 = new StaticResourceExtension
		{
			Key = "Accent"
		};
		XamlServiceProvider val121 = new XamlServiceProvider();
		Type? typeFromHandle15 = typeof(IProvideValueTarget);
		object[] array15 = new object[0 + 5];
		array15[0] = val34;
		array15[1] = val47;
		array15[2] = val71;
		array15[3] = val72;
		array15[4] = diagnosticsPage;
		SimpleValueTargetProvider val122 = new SimpleValueTargetProvider(array15, (object)VisualElement.BackgroundColorProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj25 = (object)val122;
		val121.Add(typeFromHandle15, (object)val122);
		val121.Add(typeof(IReferenceProvider), obj25);
		val121.Add(typeof(IRootObjectProvider), obj25);
		val121.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(59, 25)));
		object obj26 = val120.ProvideValue((IServiceProvider)val121);
		((BindableObject)val34).SetValue(VisualElement.BackgroundColorProperty, (obj26 == null || !typeof(BindingBase).IsAssignableFrom(obj26.GetType())) ? obj26 : obj26);
		((BindableObject)val34).SetValue(VisualElement.HeightRequestProperty, (object)44.0);
		((Layout)val47).Children.Add((IView)(object)val34);
		((BindableObject)val39).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
		{
			new ColumnDefinition(GridLength.Star),
			new ColumnDefinition(GridLength.Star)
		}));
		((BindableObject)val39).SetValue(Grid.ColumnSpacingProperty, (object)8.0);
		((BindableObject)val36).SetValue(Grid.ColumnProperty, (object)0);
		((BindableObject)val36).SetValue(Button.TextProperty, (object)"\ud83d\udd0b Batería");
		val36.Clicked += diagnosticsPage.OnBatteryOptimizationClicked;
		val35.Key = "AccentButton";
		StaticResourceExtension val123 = new StaticResourceExtension
		{
			Key = "AccentButton"
		};
		XamlServiceProvider val124 = new XamlServiceProvider();
		Type? typeFromHandle16 = typeof(IProvideValueTarget);
		object[] array16 = new object[0 + 6];
		array16[0] = val36;
		array16[1] = val39;
		array16[2] = val47;
		array16[3] = val71;
		array16[4] = val72;
		array16[5] = diagnosticsPage;
		SimpleValueTargetProvider val125 = new SimpleValueTargetProvider(array16, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj27 = (object)val125;
		val124.Add(typeFromHandle16, (object)val125);
		val124.Add(typeof(IReferenceProvider), obj27);
		val124.Add(typeof(IRootObjectProvider), obj27);
		val124.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(66, 29)));
		object obj28 = val123.ProvideValue((IServiceProvider)val124);
		((BindableObject)val36).SetValue(VisualElement.StyleProperty, (obj28 == null || !typeof(BindingBase).IsAssignableFrom(obj28.GetType())) ? obj28 : obj28);
		((BindableObject)val36).SetValue(Button.FontSizeProperty, (object)12.0);
		((BindableObject)val36).SetValue(VisualElement.HeightRequestProperty, (object)40.0);
		((Layout)val39).Children.Add((IView)(object)val36);
		((BindableObject)val38).SetValue(Grid.ColumnProperty, (object)1);
		((BindableObject)val38).SetValue(Button.TextProperty, (object)"\ud83d\ude80 Autostart");
		val38.Clicked += diagnosticsPage.OnAutostartClicked;
		val37.Key = "OutlineButton";
		StaticResourceExtension val126 = new StaticResourceExtension
		{
			Key = "OutlineButton"
		};
		XamlServiceProvider val127 = new XamlServiceProvider();
		Type? typeFromHandle17 = typeof(IProvideValueTarget);
		object[] array17 = new object[0 + 6];
		array17[0] = val38;
		array17[1] = val39;
		array17[2] = val47;
		array17[3] = val71;
		array17[4] = val72;
		array17[5] = diagnosticsPage;
		SimpleValueTargetProvider val128 = new SimpleValueTargetProvider(array17, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj29 = (object)val128;
		val127.Add(typeFromHandle17, (object)val128);
		val127.Add(typeof(IReferenceProvider), obj29);
		val127.Add(typeof(IRootObjectProvider), obj29);
		val127.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(73, 29)));
		object obj30 = val126.ProvideValue((IServiceProvider)val127);
		((BindableObject)val38).SetValue(VisualElement.StyleProperty, (obj30 == null || !typeof(BindingBase).IsAssignableFrom(obj30.GetType())) ? obj30 : obj30);
		((BindableObject)val38).SetValue(Button.FontSizeProperty, (object)12.0);
		((BindableObject)val38).SetValue(VisualElement.HeightRequestProperty, (object)40.0);
		((Layout)val39).Children.Add((IView)(object)val38);
		((Layout)val47).Children.Add((IView)(object)val39);
		val40.Key = "Card";
		StaticResourceExtension val129 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val130 = new XamlServiceProvider();
		Type? typeFromHandle18 = typeof(IProvideValueTarget);
		object[] array18 = new object[0 + 5];
		array18[0] = val46;
		array18[1] = val47;
		array18[2] = val71;
		array18[3] = val72;
		array18[4] = diagnosticsPage;
		SimpleValueTargetProvider val131 = new SimpleValueTargetProvider(array18, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj31 = (object)val131;
		val130.Add(typeFromHandle18, (object)val131);
		val130.Add(typeof(IReferenceProvider), obj31);
		val130.Add(typeof(IRootObjectProvider), obj31);
		val130.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(78, 25)));
		object obj32 = val129.ProvideValue((IServiceProvider)val130);
		((BindableObject)val46).SetValue(VisualElement.StyleProperty, (obj32 == null || !typeof(BindingBase).IsAssignableFrom(obj32.GetType())) ? obj32 : obj32);
		val41.Key = "PageBackgroundLight";
		StaticResourceExtension val132 = new StaticResourceExtension
		{
			Key = "PageBackgroundLight"
		};
		XamlServiceProvider val133 = new XamlServiceProvider();
		Type? typeFromHandle19 = typeof(IProvideValueTarget);
		object[] array19 = new object[0 + 6];
		array19[0] = val43;
		array19[1] = val46;
		array19[2] = val47;
		array19[3] = val71;
		array19[4] = val72;
		array19[5] = diagnosticsPage;
		SimpleValueTargetProvider val134 = new SimpleValueTargetProvider(array19, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj33 = (object)val134;
		val133.Add(typeFromHandle19, (object)val134);
		val133.Add(typeof(IReferenceProvider), obj33);
		val133.Add(typeof(IRootObjectProvider), obj33);
		val133.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(79, 25)));
		object light3 = val132.ProvideValue((IServiceProvider)val133);
		val43.Light = light3;
		val42.Key = "PageBackgroundDark";
		StaticResourceExtension val135 = new StaticResourceExtension
		{
			Key = "PageBackgroundDark"
		};
		XamlServiceProvider val136 = new XamlServiceProvider();
		Type? typeFromHandle20 = typeof(IProvideValueTarget);
		object[] array20 = new object[0 + 6];
		array20[0] = val43;
		array20[1] = val46;
		array20[2] = val47;
		array20[3] = val71;
		array20[4] = val72;
		array20[5] = diagnosticsPage;
		SimpleValueTargetProvider val137 = new SimpleValueTargetProvider(array20, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj34 = (object)val137;
		val136.Add(typeFromHandle20, (object)val137);
		val136.Add(typeof(IReferenceProvider), obj34);
		val136.Add(typeof(IRootObjectProvider), obj34);
		val136.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(79, 25)));
		object dark3 = val135.ProvideValue((IServiceProvider)val136);
		val43.Dark = dark3;
		XamlServiceProvider val138 = new XamlServiceProvider();
		val138.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val46, (object)VisualElement.BackgroundColorProperty));
		val138.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(79, 25)));
		BindingBase val139 = ((IMarkupExtension<BindingBase>)(object)val43).ProvideValue((IServiceProvider)val138);
		((BindableObject)val46).SetBinding(VisualElement.BackgroundColorProperty, val139);
		((BindableObject)val46).SetValue(Border.PaddingProperty, (object)new Thickness(12.0));
		((BindableObject)val45).SetValue(Label.TextProperty, (object)"\ud83d\udca1 Para un funcionamiento óptimo, configure todos los permisos. Esto asegura que la aplicación pueda recibir y reenviar SMS incluso cuando esté en segundo plano.");
		val44.Key = "HintText";
		StaticResourceExtension val140 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val141 = new XamlServiceProvider();
		Type? typeFromHandle21 = typeof(IProvideValueTarget);
		object[] array21 = new object[0 + 6];
		array21[0] = val45;
		array21[1] = val46;
		array21[2] = val47;
		array21[3] = val71;
		array21[4] = val72;
		array21[5] = diagnosticsPage;
		SimpleValueTargetProvider val142 = new SimpleValueTargetProvider(array21, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj35 = (object)val142;
		val141.Add(typeFromHandle21, (object)val142);
		val141.Add(typeof(IReferenceProvider), obj35);
		val141.Add(typeof(IRootObjectProvider), obj35);
		val141.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(82, 28)));
		object obj36 = val140.ProvideValue((IServiceProvider)val141);
		((BindableObject)val45).SetValue(VisualElement.StyleProperty, (obj36 == null || !typeof(BindingBase).IsAssignableFrom(obj36.GetType())) ? obj36 : obj36);
		((BindableObject)val45).SetValue(Label.LineBreakModeProperty, (object)(LineBreakMode)1);
		((BindableObject)val46).SetValue(Border.ContentProperty, (object)val45);
		((Layout)val47).Children.Add((IView)(object)val46);
		((Layout)val71).Children.Add((IView)(object)val47);
		((BindableObject)val57).SetValue(StackBase.SpacingProperty, (object)12.0);
		((BindableObject)val52).SetValue(Label.TextProperty, (object)"Herramientas de Diagnóstico");
		val48.Key = "CardTitle";
		StaticResourceExtension val143 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val144 = new XamlServiceProvider();
		Type? typeFromHandle22 = typeof(IProvideValueTarget);
		object[] array22 = new object[0 + 5];
		array22[0] = val52;
		array22[1] = val57;
		array22[2] = val71;
		array22[3] = val72;
		array22[4] = diagnosticsPage;
		SimpleValueTargetProvider val145 = new SimpleValueTargetProvider(array22, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj37 = (object)val145;
		val144.Add(typeFromHandle22, (object)val145);
		val144.Add(typeof(IReferenceProvider), obj37);
		val144.Add(typeof(IRootObjectProvider), obj37);
		val144.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(90, 24)));
		object obj38 = val143.ProvideValue((IServiceProvider)val144);
		((BindableObject)val52).SetValue(VisualElement.StyleProperty, (obj38 == null || !typeof(BindingBase).IsAssignableFrom(obj38.GetType())) ? obj38 : obj38);
		val49.Key = "TextPrimaryLight";
		StaticResourceExtension val146 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val147 = new XamlServiceProvider();
		Type? typeFromHandle23 = typeof(IProvideValueTarget);
		object[] array23 = new object[0 + 6];
		array23[0] = val51;
		array23[1] = val52;
		array23[2] = val57;
		array23[3] = val71;
		array23[4] = val72;
		array23[5] = diagnosticsPage;
		SimpleValueTargetProvider val148 = new SimpleValueTargetProvider(array23, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj39 = (object)val148;
		val147.Add(typeFromHandle23, (object)val148);
		val147.Add(typeof(IReferenceProvider), obj39);
		val147.Add(typeof(IRootObjectProvider), obj39);
		val147.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(91, 24)));
		object light4 = val146.ProvideValue((IServiceProvider)val147);
		val51.Light = light4;
		val50.Key = "TextPrimaryDark";
		StaticResourceExtension val149 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val150 = new XamlServiceProvider();
		Type? typeFromHandle24 = typeof(IProvideValueTarget);
		object[] array24 = new object[0 + 6];
		array24[0] = val51;
		array24[1] = val52;
		array24[2] = val57;
		array24[3] = val71;
		array24[4] = val72;
		array24[5] = diagnosticsPage;
		SimpleValueTargetProvider val151 = new SimpleValueTargetProvider(array24, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj40 = (object)val151;
		val150.Add(typeFromHandle24, (object)val151);
		val150.Add(typeof(IReferenceProvider), obj40);
		val150.Add(typeof(IRootObjectProvider), obj40);
		val150.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(91, 24)));
		object dark4 = val149.ProvideValue((IServiceProvider)val150);
		val51.Dark = dark4;
		XamlServiceProvider val152 = new XamlServiceProvider();
		val152.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val52, (object)Label.TextColorProperty));
		val152.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(91, 24)));
		BindingBase val153 = ((IMarkupExtension<BindingBase>)(object)val51).ProvideValue((IServiceProvider)val152);
		((BindableObject)val52).SetBinding(Label.TextColorProperty, val153);
		((BindableObject)val52).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val52).SetValue(View.MarginProperty, (object)new Thickness(0.0, 16.0, 0.0, 0.0));
		((Layout)val57).Children.Add((IView)(object)val52);
		((BindableObject)val54).SetValue(Button.TextProperty, (object)"\ud83d\udd04 Actualizar Estado");
		val54.Clicked += diagnosticsPage.OnRefreshClicked;
		val53.Key = "PrimaryButton";
		StaticResourceExtension val154 = new StaticResourceExtension
		{
			Key = "PrimaryButton"
		};
		XamlServiceProvider val155 = new XamlServiceProvider();
		Type? typeFromHandle25 = typeof(IProvideValueTarget);
		object[] array25 = new object[0 + 5];
		array25[0] = val54;
		array25[1] = val57;
		array25[2] = val71;
		array25[3] = val72;
		array25[4] = diagnosticsPage;
		SimpleValueTargetProvider val156 = new SimpleValueTargetProvider(array25, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj41 = (object)val156;
		val155.Add(typeFromHandle25, (object)val156);
		val155.Add(typeof(IReferenceProvider), obj41);
		val155.Add(typeof(IRootObjectProvider), obj41);
		val155.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(97, 25)));
		object obj42 = val154.ProvideValue((IServiceProvider)val155);
		((BindableObject)val54).SetValue(VisualElement.StyleProperty, (obj42 == null || !typeof(BindingBase).IsAssignableFrom(obj42.GetType())) ? obj42 : obj42);
		((BindableObject)val54).SetValue(Button.FontSizeProperty, (object)14.0);
		((BindableObject)val54).SetValue(VisualElement.HeightRequestProperty, (object)44.0);
		((Layout)val57).Children.Add((IView)(object)val54);
		((BindableObject)val56).SetValue(Button.TextProperty, (object)"\ud83d\udce4 Enviar SMS de Prueba");
		val56.Clicked += diagnosticsPage.OnTestSmsClicked;
		val55.Key = "OutlineButton";
		StaticResourceExtension val157 = new StaticResourceExtension
		{
			Key = "OutlineButton"
		};
		XamlServiceProvider val158 = new XamlServiceProvider();
		Type? typeFromHandle26 = typeof(IProvideValueTarget);
		object[] array26 = new object[0 + 5];
		array26[0] = val56;
		array26[1] = val57;
		array26[2] = val71;
		array26[3] = val72;
		array26[4] = diagnosticsPage;
		SimpleValueTargetProvider val159 = new SimpleValueTargetProvider(array26, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj43 = (object)val159;
		val158.Add(typeFromHandle26, (object)val159);
		val158.Add(typeof(IReferenceProvider), obj43);
		val158.Add(typeof(IRootObjectProvider), obj43);
		val158.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(103, 25)));
		object obj44 = val157.ProvideValue((IServiceProvider)val158);
		((BindableObject)val56).SetValue(VisualElement.StyleProperty, (obj44 == null || !typeof(BindingBase).IsAssignableFrom(obj44.GetType())) ? obj44 : obj44);
		((BindableObject)val56).SetValue(VisualElement.HeightRequestProperty, (object)44.0);
		((Layout)val57).Children.Add((IView)(object)val56);
		((Layout)val71).Children.Add((IView)(object)val57);
		((BindableObject)val70).SetValue(StackBase.SpacingProperty, (object)12.0);
		((BindableObject)val62).SetValue(Label.TextProperty, (object)"Registro de actividad");
		val58.Key = "CardTitle";
		StaticResourceExtension val160 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val161 = new XamlServiceProvider();
		Type? typeFromHandle27 = typeof(IProvideValueTarget);
		object[] array27 = new object[0 + 5];
		array27[0] = val62;
		array27[1] = val70;
		array27[2] = val71;
		array27[3] = val72;
		array27[4] = diagnosticsPage;
		SimpleValueTargetProvider val162 = new SimpleValueTargetProvider(array27, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj45 = (object)val162;
		val161.Add(typeFromHandle27, (object)val162);
		val161.Add(typeof(IReferenceProvider), obj45);
		val161.Add(typeof(IRootObjectProvider), obj45);
		val161.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(110, 24)));
		object obj46 = val160.ProvideValue((IServiceProvider)val161);
		((BindableObject)val62).SetValue(VisualElement.StyleProperty, (obj46 == null || !typeof(BindingBase).IsAssignableFrom(obj46.GetType())) ? obj46 : obj46);
		val59.Key = "TextPrimaryLight";
		StaticResourceExtension val163 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val164 = new XamlServiceProvider();
		Type? typeFromHandle28 = typeof(IProvideValueTarget);
		object[] array28 = new object[0 + 6];
		array28[0] = val61;
		array28[1] = val62;
		array28[2] = val70;
		array28[3] = val71;
		array28[4] = val72;
		array28[5] = diagnosticsPage;
		SimpleValueTargetProvider val165 = new SimpleValueTargetProvider(array28, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj47 = (object)val165;
		val164.Add(typeFromHandle28, (object)val165);
		val164.Add(typeof(IReferenceProvider), obj47);
		val164.Add(typeof(IRootObjectProvider), obj47);
		val164.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(111, 24)));
		object light5 = val163.ProvideValue((IServiceProvider)val164);
		val61.Light = light5;
		val60.Key = "TextPrimaryDark";
		StaticResourceExtension val166 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val167 = new XamlServiceProvider();
		Type? typeFromHandle29 = typeof(IProvideValueTarget);
		object[] array29 = new object[0 + 6];
		array29[0] = val61;
		array29[1] = val62;
		array29[2] = val70;
		array29[3] = val71;
		array29[4] = val72;
		array29[5] = diagnosticsPage;
		SimpleValueTargetProvider val168 = new SimpleValueTargetProvider(array29, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[7] { val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj48 = (object)val168;
		val167.Add(typeFromHandle29, (object)val168);
		val167.Add(typeof(IReferenceProvider), obj48);
		val167.Add(typeof(IRootObjectProvider), obj48);
		val167.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(111, 24)));
		object dark5 = val166.ProvideValue((IServiceProvider)val167);
		val61.Dark = dark5;
		XamlServiceProvider val169 = new XamlServiceProvider();
		val169.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val62, (object)Label.TextColorProperty));
		val169.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(111, 24)));
		BindingBase val170 = ((IMarkupExtension<BindingBase>)(object)val61).ProvideValue((IServiceProvider)val169);
		((BindableObject)val62).SetBinding(Label.TextColorProperty, val170);
		((BindableObject)val62).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val70).Children.Add((IView)(object)val62);
		val63.Key = "Card";
		StaticResourceExtension val171 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val172 = new XamlServiceProvider();
		Type? typeFromHandle30 = typeof(IProvideValueTarget);
		object[] array30 = new object[0 + 5];
		array30[0] = val67;
		array30[1] = val70;
		array30[2] = val71;
		array30[3] = val72;
		array30[4] = diagnosticsPage;
		SimpleValueTargetProvider val173 = new SimpleValueTargetProvider(array30, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj49 = (object)val173;
		val172.Add(typeFromHandle30, (object)val173);
		val172.Add(typeof(IReferenceProvider), obj49);
		val172.Add(typeof(IRootObjectProvider), obj49);
		val172.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(114, 25)));
		object obj50 = val171.ProvideValue((IServiceProvider)val172);
		((BindableObject)val67).SetValue(VisualElement.StyleProperty, (obj50 == null || !typeof(BindingBase).IsAssignableFrom(obj50.GetType())) ? obj50 : obj50);
		((BindableObject)val67).SetValue(Border.PaddingProperty, (object)new Thickness(12.0));
		((BindableObject)val66).SetValue(VisualElement.HeightRequestProperty, (object)180.0);
		((BindableObject)val65).SetValue(Label.TextProperty, (object)"Sin actividad reciente...");
		((BindableObject)val65).SetValue(Label.FontSizeProperty, (object)12.0);
		((BindableObject)val65).SetValue(Label.FontFamilyProperty, (object)"Monospace");
		val64.Key = "BodyText";
		StaticResourceExtension val174 = new StaticResourceExtension
		{
			Key = "BodyText"
		};
		XamlServiceProvider val175 = new XamlServiceProvider();
		Type? typeFromHandle31 = typeof(IProvideValueTarget);
		object[] array31 = new object[0 + 7];
		array31[0] = val65;
		array31[1] = val66;
		array31[2] = val67;
		array31[3] = val70;
		array31[4] = val71;
		array31[5] = val72;
		array31[6] = diagnosticsPage;
		SimpleValueTargetProvider val176 = new SimpleValueTargetProvider(array31, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val73, val73, val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj51 = (object)val176;
		val175.Add(typeFromHandle31, (object)val176);
		val175.Add(typeof(IReferenceProvider), obj51);
		val175.Add(typeof(IRootObjectProvider), obj51);
		val175.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(120, 32)));
		object obj52 = val174.ProvideValue((IServiceProvider)val175);
		((BindableObject)val65).SetValue(VisualElement.StyleProperty, (obj52 == null || !typeof(BindingBase).IsAssignableFrom(obj52.GetType())) ? obj52 : obj52);
		((BindableObject)val65).SetValue(Label.LineBreakModeProperty, (object)(LineBreakMode)1);
		val66.Content = (View)(object)val65;
		((BindableObject)val67).SetValue(Border.ContentProperty, (object)val66);
		((Layout)val70).Children.Add((IView)(object)val67);
		((BindableObject)val69).SetValue(Button.TextProperty, (object)"Limpiar registro");
		val69.Clicked += diagnosticsPage.OnClearLogsClicked;
		val68.Key = "OutlineButton";
		StaticResourceExtension val177 = new StaticResourceExtension
		{
			Key = "OutlineButton"
		};
		XamlServiceProvider val178 = new XamlServiceProvider();
		Type? typeFromHandle32 = typeof(IProvideValueTarget);
		object[] array32 = new object[0 + 5];
		array32[0] = val69;
		array32[1] = val70;
		array32[2] = val71;
		array32[3] = val72;
		array32[4] = diagnosticsPage;
		SimpleValueTargetProvider val179 = new SimpleValueTargetProvider(array32, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val73, val73, val73, val73, val73, val73 }, (object)diagnosticsPage);
		object obj53 = (object)val179;
		val178.Add(typeFromHandle32, (object)val179);
		val178.Add(typeof(IReferenceProvider), obj53);
		val178.Add(typeof(IRootObjectProvider), obj53);
		val178.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(127, 25)));
		object obj54 = val177.ProvideValue((IServiceProvider)val178);
		((BindableObject)val69).SetValue(VisualElement.StyleProperty, (obj54 == null || !typeof(BindingBase).IsAssignableFrom(obj54.GetType())) ? obj54 : obj54);
		((BindableObject)val69).SetValue(VisualElement.HeightRequestProperty, (object)40.0);
		((Layout)val70).Children.Add((IView)(object)val69);
		((Layout)val71).Children.Add((IView)(object)val70);
		val72.Content = (View)(object)val71;
		((BindableObject)diagnosticsPage).SetValue(ContentPage.ContentProperty, (object)val72);
	}
}
