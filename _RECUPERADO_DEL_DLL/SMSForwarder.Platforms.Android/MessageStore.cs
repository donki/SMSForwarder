using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.App.Roles;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Telephony;
using Java.Lang;
using Microsoft.Maui.ApplicationModel;
using SMSForwarder.Models;
using SMSForwarder.Services;

namespace SMSForwarder.Platforms.Android;

/// <summary>
/// Implementacion Android de <see cref="T:SMSForwarder.Services.IMessageStore" />: acceso al proveedor de SMS del
/// sistema (content://sms) y peticion del rol de app de SMS por defecto.
/// </summary>
public class MessageStore : IMessageStore
{
	public const int RequestDefaultCode = 8123;

	private static TaskCompletionSource<bool>? _pendingDefault;

	private const string ColId = "_id";

	private const string ColAddress = "address";

	private const string ColBody = "body";

	private const string ColDate = "date";

	private const string ColRead = "read";

	private const string ColType = "type";

	public bool IsSupported => true;

	public bool IsDefaultSmsApp => IsAppDefault(Application.Context);

	public bool CanBeDefault
	{
		get
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Invalid comparison between Unknown and I4
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			try
			{
				Context val = (Context)(((object)Platform.CurrentActivity) ?? ((object)Application.Context));
				PackageManager packageManager = val.PackageManager;
				if (packageManager == null || !packageManager.HasSystemFeature("android.hardware.telephony"))
				{
					return false;
				}
				if ((int)VERSION.SdkInt >= 29)
				{
					RoleManager val2 = (RoleManager)val.GetSystemService("role");
					return val2 != null && val2.IsRoleAvailable("android.app.role.SMS");
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
	}

	/// <summary>True si el paquete de esta app es el gestor de SMS por defecto del sistema.</summary>
	public static bool IsAppDefault(Context context)
	{
		try
		{
			string defaultSmsPackage = Sms.GetDefaultSmsPackage(context);
			return defaultSmsPackage != null && defaultSmsPackage == context.PackageName;
		}
		catch
		{
			return false;
		}
	}

	public Task<bool> RequestDefaultAsync()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		Activity currentActivity = Platform.CurrentActivity;
		if (currentActivity == null)
		{
			return Task.FromResult(result: false);
		}
		if (IsAppDefault((Context)(object)currentActivity))
		{
			return Task.FromResult(result: true);
		}
		_pendingDefault?.TrySetResult(result: false);
		TaskCompletionSource<bool> taskCompletionSource = (_pendingDefault = new TaskCompletionSource<bool>());
		try
		{
			if ((int)VERSION.SdkInt >= 29)
			{
				RoleManager val = (RoleManager)((Context)currentActivity).GetSystemService("role");
				if (val != null && val.IsRoleAvailable("android.app.role.SMS"))
				{
					if (val.IsRoleHeld("android.app.role.SMS"))
					{
						_pendingDefault = null;
						return Task.FromResult(result: true);
					}
					Intent val2 = val.CreateRequestRoleIntent("android.app.role.SMS");
					currentActivity.StartActivityForResult(val2, 8123);
					return taskCompletionSource.Task;
				}
			}
			Intent val3 = new Intent("android.provider.Telephony.ACTION_CHANGE_DEFAULT");
			val3.PutExtra("package", ((Context)currentActivity).PackageName);
			currentActivity.StartActivityForResult(val3, 8123);
			return taskCompletionSource.Task;
		}
		catch (Exception)
		{
			_pendingDefault = null;
			return Task.FromResult(result: false);
		}
	}

	/// <summary>Enlazado desde MainActivity.OnActivityResult. Devuelve true si el resultado era nuestro.</summary>
	public static bool HandleActivityResult(int requestCode, Context context)
	{
		if (requestCode != 8123)
		{
			return false;
		}
		TaskCompletionSource<bool>? pendingDefault = _pendingDefault;
		_pendingDefault = null;
		pendingDefault?.TrySetResult(IsAppDefault(context));
		return true;
	}

	public Task<List<SmsMessageItem>> GetInboxAsync()
	{
		return Task.Run(() => Query(Inbox.ContentUri, isInbox: true));
	}

	public Task<List<SmsMessageItem>> GetSentAsync()
	{
		return Task.Run(() => Query(Sent.ContentUri, isInbox: false));
	}

	private static List<SmsMessageItem> Query(Uri? uri, bool isInbox)
	{
		List<SmsMessageItem> list = new List<SmsMessageItem>();
		if (uri == null)
		{
			return list;
		}
		ContentResolver contentResolver = Application.Context.ContentResolver;
		if (contentResolver == null)
		{
			return list;
		}
		try
		{
			ICursor val = contentResolver.Query(uri, new string[5] { "_id", "address", "body", "date", "read" }, (string)null, (string[])null, "date DESC");
			try
			{
				if (val == null)
				{
					return list;
				}
				int columnIndex = val.GetColumnIndex("_id");
				int columnIndex2 = val.GetColumnIndex("address");
				int columnIndex3 = val.GetColumnIndex("body");
				int columnIndex4 = val.GetColumnIndex("date");
				int columnIndex5 = val.GetColumnIndex("read");
				while (val.MoveToNext() && list.Count < 500)
				{
					long millis = ((columnIndex4 >= 0) ? val.GetLong(columnIndex4) : 0);
					list.Add(new SmsMessageItem
					{
						Id = ((columnIndex >= 0) ? val.GetLong(columnIndex) : 0),
						Address = ((columnIndex2 >= 0) ? (val.GetString(columnIndex2) ?? "") : ""),
						Body = ((columnIndex3 >= 0) ? (val.GetString(columnIndex3) ?? "") : ""),
						Date = FromEpoch(millis),
						IsRead = (columnIndex5 >= 0 && val.GetInt(columnIndex5) == 1),
						IsInbox = isInbox
					});
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception)
		{
		}
		return list;
	}

	public Task<bool> DeleteAsync(SmsMessageItem message)
	{
		return Task.Run(delegate
		{
			ContentResolver contentResolver = Application.Context.ContentResolver;
			if (contentResolver == null || message.Id <= 0)
			{
				return false;
			}
			try
			{
				Uri val = ContentUris.WithAppendedId(Sms.ContentUri, message.Id);
				return contentResolver.Delete(val, (string)null, (string[])null) > 0;
			}
			catch (Exception)
			{
				return false;
			}
		});
	}

	public Task<bool> MarkReadAsync(SmsMessageItem message)
	{
		return Task.Run(delegate
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			ContentResolver contentResolver = Application.Context.ContentResolver;
			if (contentResolver == null || message.Id <= 0)
			{
				return false;
			}
			try
			{
				ContentValues val = new ContentValues();
				val.Put("read", 1);
				Uri val2 = ContentUris.WithAppendedId(Sms.ContentUri, message.Id);
				return contentResolver.Update(val2, val, (string)null, (string[])null) > 0;
			}
			catch (Exception)
			{
				return false;
			}
		});
	}

	public Task<bool> SendAsync(string address, string body)
	{
		return Task.Run(delegate
		{
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			if (string.IsNullOrWhiteSpace(address) || string.IsNullOrEmpty(body))
			{
				return false;
			}
			try
			{
				SmsManager val = SmsManager.Default;
				try
				{
					if (val == null)
					{
						return false;
					}
					if (body.Length > 160)
					{
						IList<string> list = val.DivideMessage(body);
						val.SendMultipartTextMessage(address, (string)null, list, (IList<PendingIntent>)null, (IList<PendingIntent>)null);
					}
					else
					{
						val.SendTextMessage(address, (string)null, body, (PendingIntent)null, (PendingIntent)null);
					}
					try
					{
						ContentResolver contentResolver = Application.Context.ContentResolver;
						ContentValues val2 = new ContentValues();
						val2.Put("address", address);
						val2.Put("body", body);
						val2.Put("date", JavaSystem.CurrentTimeMillis());
						val2.Put("read", 1);
						val2.Put("type", 2);
						if (contentResolver != null)
						{
							contentResolver.Insert(Sent.ContentUri, val2);
						}
					}
					catch
					{
					}
					return true;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception)
			{
				return false;
			}
		});
	}

	/// <summary>
	/// Guarda un SMS entrante en el buzon del sistema. La llama <see cref="T:SMSForwarder.Platforms.Android.SmsDeliverReceiver" />
	/// cuando la app es la predeterminada (responsable de la persistencia).
	/// </summary>
	public static void PersistInbox(Context context, string sender, string body, long timestampMillis)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			ContentResolver contentResolver = context.ContentResolver;
			if (contentResolver != null)
			{
				ContentValues val = new ContentValues();
				val.Put("address", sender);
				val.Put("body", body);
				val.Put("date", (timestampMillis > 0) ? timestampMillis : JavaSystem.CurrentTimeMillis());
				val.Put("read", 0);
				val.Put("type", 1);
				contentResolver.Insert(Inbox.ContentUri, val);
			}
		}
		catch (Exception)
		{
		}
	}

	private static DateTime FromEpoch(long millis)
	{
		if (millis <= 0)
		{
			return DateTime.MinValue;
		}
		try
		{
			return DateTimeOffset.FromUnixTimeMilliseconds(millis).LocalDateTime;
		}
		catch
		{
			return DateTime.MinValue;
		}
	}
}
