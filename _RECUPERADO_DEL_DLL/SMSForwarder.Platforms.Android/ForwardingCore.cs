using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Android.App;
using Android.Content;
using Android.Telephony;

namespace SMSForwarder.Platforms.Android;

/// <summary>
/// Nucleo de reenvio compartido por los dos caminos de entrada de SMS:
///  - <see cref="T:SMSForwarder.Platforms.Android.SmsReceiver" /> (broadcast SMS_RECEIVED) cuando la app NO es la de SMS por defecto.
///  - <see cref="T:SMSForwarder.Platforms.Android.SmsDeliverReceiver" /> (broadcast SMS_DELIVER) cuando SI es la app por defecto.
/// Contiene la deteccion de duplicados/bucles y el envio. Una sola fuente de verdad.
/// </summary>
public static class ForwardingCore
{
	private static string? _lastSender;

	private static string? _lastBody;

	private static DateTime _lastReceived = DateTime.MinValue;

	public static void Forward(Context context, string sender, string messageBody)
	{
		if (_lastSender == sender && _lastBody == messageBody && (DateTime.Now - _lastReceived).TotalSeconds < 5.0)
		{
			SafeLog("Mensaje duplicado detectado, no se reenvia.");
			return;
		}
		_lastSender = sender;
		_lastBody = messageBody;
		_lastReceived = DateTime.Now;
		try
		{
			if (string.IsNullOrEmpty(messageBody))
			{
				SafeLog("Mensaje vacio, no se reenvia");
				return;
			}
			string text = context.PackageName + "_preferences";
			ISharedPreferences sharedPreferences = context.GetSharedPreferences(text, (FileCreationMode)0);
			if (sharedPreferences == null)
			{
				SafeLog("Error: No se pudo acceder a las preferencias");
				return;
			}
			string text2 = sharedPreferences.GetString("phones", (string)null);
			if (string.IsNullOrEmpty(text2))
			{
				SafeLog("No hay numeros guardados en preferencias");
				return;
			}
			List<string> list;
			try
			{
				list = JsonSerializer.Deserialize<List<string>>(text2);
				if (list == null || list.Count == 0)
				{
					SafeLog("No hay numeros para reenviar");
					return;
				}
			}
			catch (Exception ex)
			{
				SafeLog("Error deserializando numeros: " + ex.Message);
				return;
			}
			string cleanSender = CleanPhoneNumber(sender);
			if (list.Any((string phone) => ArePhoneNumbersEqual(cleanSender, CleanPhoneNumber(phone))))
			{
				SafeLog("BUCLE DETECTADO: mensaje desde un numero de reenvio (" + sender + "). No se reenvia.");
				return;
			}
			if (IsForwardedMessage(messageBody))
			{
				SafeLog("BUCLE DETECTADO: el mensaje parece un reenvio de SMSForwarder. No se reenvia.");
				return;
			}
			SafeLog($"Procesando reenvio a {list.Count} numeros");
			string text3 = "[SMSForwarder] De: " + sender + "\n" + messageBody;
			if (text3.Length > 160)
			{
				int num = 160 - "[SMSForwarder] De: ".Length - sender.Length - 4;
				string text4 = ((messageBody.Length > num) ? (messageBody.Substring(0, Math.Max(0, num)) + "...") : messageBody);
				text3 = "[SMSForwarder] De: " + sender + "\n" + text4;
			}
			int num2 = 0;
			int num3 = 0;
			foreach (string item in list.Where((string p) => !string.IsNullOrWhiteSpace(p)))
			{
				try
				{
					SendSms(item, text3);
					num2++;
				}
				catch (Exception ex2)
				{
					num3++;
					SafeLog("Error enviando a " + item + ": " + ex2.Message);
				}
			}
			SafeLog($"Reenvio completado - Exitos: {num2}, Errores: {num3}");
		}
		catch (Exception ex3)
		{
			SafeLog("Error general en Forward: " + ex3.Message);
		}
	}

	private static void SendSms(string phoneNumber, string message)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		SmsManager val = SmsManager.Default;
		try
		{
			if (val == null)
			{
				SafeLog("No se pudo obtener el SmsManager");
				return;
			}
			PendingIntent broadcast = PendingIntent.GetBroadcast(Application.Context, 0, new Intent("SMS_SENT"), (PendingIntentFlags)1140850688);
			if (message.Length > 160)
			{
				IList<string> list = val.DivideMessage(message);
				if (list != null && list.Count > 0)
				{
					List<PendingIntent> list2 = new List<PendingIntent>();
					for (int i = 0; i < list.Count; i++)
					{
						list2.Add(PendingIntent.GetBroadcast(Application.Context, i, new Intent("SMS_SENT"), (PendingIntentFlags)1140850688));
					}
					val.SendMultipartTextMessage(phoneNumber, (string)null, list, (IList<PendingIntent>)list2, (IList<PendingIntent>)null);
				}
			}
			else
			{
				val.SendTextMessage(phoneNumber, (string)null, message, broadcast, (PendingIntent)null);
			}
			SafeLog("SMS enviado a " + phoneNumber);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static void SafeLog(string message)
	{
		try
		{
		}
		catch
		{
		}
	}

	private static string CleanPhoneNumber(string phoneNumber)
	{
		if (string.IsNullOrWhiteSpace(phoneNumber))
		{
			return "";
		}
		return phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "")
			.Replace(")", "")
			.Replace(".", "")
			.Replace("+", "")
			.Trim();
	}

	private static bool ArePhoneNumbersEqual(string phone1, string phone2)
	{
		if (string.IsNullOrWhiteSpace(phone1) || string.IsNullOrWhiteSpace(phone2))
		{
			return false;
		}
		if (phone1 == phone2)
		{
			return true;
		}
		if (Math.Min(phone1.Length, phone2.Length) >= 9)
		{
			return phone1.Substring(phone1.Length - 9) == phone2.Substring(phone2.Length - 9);
		}
		return false;
	}

	private static bool IsForwardedMessage(string messageBody)
	{
		if (string.IsNullOrWhiteSpace(messageBody))
		{
			return false;
		}
		if (messageBody.StartsWith("[SMSForwarder]", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		string[] source = new string[5] { "De:", "From:", "Reenviado:", "Forwarded:", "SMS de:" };
		string messageStart = messageBody.Substring(0, Math.Min(30, messageBody.Length)).ToLower();
		return source.Any((string pattern) => messageStart.Contains(pattern.ToLower()));
	}
}
