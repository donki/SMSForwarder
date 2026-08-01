using System.Collections.Generic;
using System.Threading.Tasks;
using SMSForwarder.Models;

namespace SMSForwarder.Services;

/// <summary>
/// Acceso a los mensajes SMS del dispositivo (proveedor del sistema).
/// Leer requiere READ_SMS; borrar/marcar/escribir requiere ser la app de SMS por defecto.
/// </summary>
public interface IMessageStore
{
	/// <summary>La plataforma soporta acceso a SMS (Android).</summary>
	bool IsSupported { get; }

	/// <summary>La app es actualmente la app de SMS por defecto del sistema.</summary>
	bool IsDefaultSmsApp { get; }

	/// <summary>El dispositivo admite ser app de SMS por defecto (tiene telefonia y el rol disponible).</summary>
	bool CanBeDefault { get; }

	/// <summary>Pide al usuario convertir esta app en la de SMS por defecto. Devuelve true si acepto.</summary>
	Task<bool> RequestDefaultAsync();

	/// <summary>Mensajes recibidos (buzon de entrada), mas recientes primero.</summary>
	Task<List<SmsMessageItem>> GetInboxAsync();

	/// <summary>Mensajes enviados, mas recientes primero.</summary>
	Task<List<SmsMessageItem>> GetSentAsync();

	/// <summary>Borra un mensaje del proveedor. Requiere ser la app por defecto.</summary>
	Task<bool> DeleteAsync(SmsMessageItem message);

	/// <summary>Marca un mensaje recibido como leido.</summary>
	Task<bool> MarkReadAsync(SmsMessageItem message);

	/// <summary>Envia un SMS y lo guarda en Enviados. Requiere SEND_SMS.</summary>
	Task<bool> SendAsync(string address, string body);
}
