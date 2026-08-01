using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Net;
using Android.Provider;
using Microsoft.Maui.ApplicationModel;
using SMSForwarder.Services;

namespace SMSForwarder.Platforms.Android;

/// <summary>
/// Selector de contactos basado en ACTION_PICK sobre la tabla de telefonos.
///
/// La app de contactos del sistema delega permiso de lectura sobre el URI que devuelve,
/// asi que leer ese URI no requiere READ_CONTACTS.
///
/// Detalle importante: se hace ACTION_PICK sobre Phone.ContentUri (no sobre
/// Contacts.ContentUri) para que el URI devuelto sea ya la fila del telefono y el numero
/// se lea de ahi directamente. Consultar despues Phone.ContentUri por contact id -que es
/// lo que hace Contacts.Default.PickContactAsync de MAUI- queda fuera de esa delegacion
/// y volveria a exigir el permiso, que es justo lo que esta clase evita.
/// </summary>
public class ContactPicker : IContactPicker
{
	private const int PickPhoneRequestCode = 9001;

	private static TaskCompletionSource<string?>? _pending;

	public Task<string?> PickPhoneNumberAsync()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		Activity currentActivity = Platform.CurrentActivity;
		if (currentActivity == null)
		{
			return Task.FromResult<string>(null);
		}
		_pending?.TrySetResult(null);
		TaskCompletionSource<string> taskCompletionSource = (_pending = new TaskCompletionSource<string>());
		try
		{
			Intent val = new Intent("android.intent.action.PICK", Phone.ContentUri);
			currentActivity.StartActivityForResult(val, 9001);
		}
		catch (Exception)
		{
			_pending = null;
			throw;
		}
		return taskCompletionSource.Task;
	}

	/// <summary>
	/// Enlazado desde MainActivity.OnActivityResult.
	/// Devuelve true si el resultado pertenecia al selector de contactos.
	/// </summary>
	public static bool HandleActivityResult(int requestCode, Result resultCode, Intent? data)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if (requestCode != 9001)
		{
			return false;
		}
		TaskCompletionSource<string> pending = _pending;
		_pending = null;
		if (pending == null)
		{
			return true;
		}
		if ((int)resultCode != -1 || ((data != null) ? data.Data : null) == null)
		{
			pending.TrySetResult(null);
			return true;
		}
		try
		{
			pending.TrySetResult(ReadNumber(data.Data));
		}
		catch (Exception)
		{
			pending.TrySetResult(null);
		}
		return true;
	}

	private static string? ReadNumber(Uri uri)
	{
		ContentResolver contentResolver = Application.Context.ContentResolver;
		if (contentResolver == null)
		{
			return null;
		}
		string text = "data1";
		ICursor val = contentResolver.Query(uri, new string[1] { text }, (string)null, (string[])null, (string)null);
		try
		{
			if (val == null || !val.MoveToFirst())
			{
				return null;
			}
			int columnIndex = val.GetColumnIndex(text);
			return (columnIndex < 0) ? null : val.GetString(columnIndex);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
