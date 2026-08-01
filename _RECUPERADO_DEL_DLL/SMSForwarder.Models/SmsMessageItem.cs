using System;

namespace SMSForwarder.Models;

/// <summary>
/// Un mensaje SMS leido del proveedor del sistema (buzon de entrada o enviados).
/// Modelo neutro de plataforma para poder mostrarlo en la UI de MAUI.
/// </summary>
public class SmsMessageItem
{
	public long Id { get; set; }

	public string Address { get; set; } = "";

	public string Body { get; set; } = "";

	public DateTime Date { get; set; }

	public bool IsRead { get; set; }

	public bool IsInbox { get; set; }

	public string DateText
	{
		get
		{
			if (!(Date == DateTime.MinValue))
			{
				return Date.ToString("dd/MM/yyyy HH:mm");
			}
			return "";
		}
	}

	public string DirectionIcon
	{
		get
		{
			if (!IsInbox)
			{
				return "\ud83d\udce4";
			}
			return "\ud83d\udce5";
		}
	}

	public string Snippet
	{
		get
		{
			if (Body.Length <= 100)
			{
				return Body;
			}
			return Body.Substring(0, 100) + "…";
		}
	}

	public string DisplayAddress
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(Address))
			{
				return Address;
			}
			return "(desconocido)";
		}
	}
}
