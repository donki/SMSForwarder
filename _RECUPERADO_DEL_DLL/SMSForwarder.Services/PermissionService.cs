using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SocShared;

namespace SMSForwarder.Services;

public class PermissionService
{
	public async Task<bool> CheckAndRequestAllPermissionsAsync()
	{
		List<bool> results = new List<bool>();
		List<bool> list = results;
		list.Add(await CheckAndRequestSmsPermissionsAsync());
		list = results;
		list.Add(await CheckAndRequestBatteryOptimizationAsync());
		await ShowAutostartInformationAsync();
		return results.All((bool r) => r);
	}

	private async Task<bool> CheckAndRequestSmsPermissionsAsync()
	{
		try
		{
			PermissionStatus receiveSmsStatus = await ((BasePermission)new SmsPermissions.ReceiveSms()).CheckStatusAsync();
			PermissionStatus val = await ((BasePermission)new SmsPermissions.SendSms()).CheckStatusAsync();
			if ((int)receiveSmsStatus != 3 || (int)val != 3)
			{
				if (await ModernDialog.AlertAsync(Application.Current.MainPage, "Permisos SMS Requeridos", "Esta aplicación necesita permisos SMS para funcionar correctamente. ¿Desea conceder los permisos?", "Sí", "No"))
				{
					await ((BasePermission)new SmsPermissions.ReceiveSms()).RequestAsync();
					await ((BasePermission)new SmsPermissions.SendSms()).RequestAsync();
					receiveSmsStatus = await ((BasePermission)new SmsPermissions.ReceiveSms()).CheckStatusAsync();
					val = await ((BasePermission)new SmsPermissions.SendSms()).CheckStatusAsync();
					return (int)receiveSmsStatus == 3 && (int)val == 3;
				}
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			await ModernDialog.AlertAsync(Application.Current.MainPage, "Error", "Error al verificar permisos SMS: " + ex.Message, "OK");
			return false;
		}
	}

	private async Task<bool> CheckAndRequestBatteryOptimizationAsync()
	{
		try
		{
			SmsPermissions.BatteryOptimizationPermission batteryPermission = new SmsPermissions.BatteryOptimizationPermission();
			PermissionStatus status = await ((BasePermission)batteryPermission).CheckStatusAsync();
			if ((int)status != 3 && await ModernDialog.AlertAsync(Application.Current.MainPage, "Optimización de Batería", "Para que la aplicación funcione correctamente en segundo plano, es recomendable desactivar la optimización de batería.\n\n¿Desea abrir la configuración?", "Sí", "Ahora no"))
			{
				await ((BasePermission)batteryPermission).RequestAsync();
				await Task.Delay(2000);
				status = await ((BasePermission)batteryPermission).CheckStatusAsync();
				if ((int)status != 3)
				{
					await ModernDialog.AlertAsync(Application.Current.MainPage, "Información", "Si no desactivó la optimización de batería, la aplicación podría no recibir mensajes cuando esté en segundo plano.", "Entendido");
				}
			}
			return (int)status == 3;
		}
		catch (Exception ex)
		{
			await ModernDialog.AlertAsync(Application.Current.MainPage, "Error", "Error al verificar optimización de batería: " + ex.Message, "OK");
			return false;
		}
	}

	private async Task ShowAutostartInformationAsync()
	{
		try
		{
			string text = GetManufacturer().ToLower();
			string text2 = "Para que la aplicación funcione correctamente después de reiniciar el dispositivo, ";
			if (await ModernDialog.AlertAsync(message: text switch
			{
				"xiaomi" => text2 + "vaya a Configuración > Aplicaciones > Administrar aplicaciones > SMS Forwarder > Inicio automático y actívelo.", 
				"huawei" => text2 + "vaya a Configuración > Aplicaciones > SMS Forwarder > Inicio automático y actívelo.", 
				"oppo" => text2 + "vaya a Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático y actívelo.", 
				"vivo" => text2 + "vaya a Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático y actívelo.", 
				"samsung" => text2 + "vaya a Configuración > Aplicaciones > SMS Forwarder > Batería > Optimizar uso de batería y desactívelo.", 
				"oneplus" => text2 + "vaya a Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático y actívelo.", 
				_ => text2 + "asegúrese de que la aplicación tenga permisos para ejecutarse en segundo plano.", 
			} + "\n\n¿Desea abrir la configuración ahora?", page: Application.Current.MainPage, title: "Configuración de Autostart", accept: "Sí", cancel: "Ahora no"))
			{
				await ((BasePermission)new SmsPermissions.AutoStartPermission()).RequestAsync();
			}
		}
		catch (Exception ex)
		{
			await ModernDialog.AlertAsync(Application.Current.MainPage, "Error", "Error al mostrar información de autostart: " + ex.Message, "OK");
		}
	}

	private string GetManufacturer()
	{
		return Build.Manufacturer ?? "unknown";
	}

	public async Task<bool> CheckBatteryOptimizationStatusAsync()
	{
		try
		{
			return (int)(await ((BasePermission)new SmsPermissions.BatteryOptimizationPermission()).CheckStatusAsync()) == 3;
		}
		catch
		{
			return false;
		}
	}

	public async Task ShowPermissionStatusAsync()
	{
		try
		{
			PermissionStatus smsStatus = await ((BasePermission)new SmsPermissions.ReceiveSms()).CheckStatusAsync();
			bool flag = await CheckBatteryOptimizationStatusAsync();
			string manufacturer = GetManufacturer();
			string message = $"Estado de permisos:\n\n\ud83d\udcf1 SMS: {(((int)smsStatus == 3) ? "✅ Concedido" : "❌ Denegado")}\n\ud83d\udd0b Optimización batería: {(flag ? "✅ Desactivada" : "❌ Activada")}\n\ud83d\ude80 Fabricante: {manufacturer}\n\nPara un funcionamiento óptimo, todos los permisos deben estar concedidos.";
			await ModernDialog.AlertAsync(Application.Current.MainPage, "Estado de Permisos", message, "OK");
		}
		catch (Exception ex)
		{
			await ModernDialog.AlertAsync(Application.Current.MainPage, "Error", "Error al verificar estado: " + ex.Message, "OK");
		}
	}
}
