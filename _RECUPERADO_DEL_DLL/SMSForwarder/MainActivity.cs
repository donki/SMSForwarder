using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Core.Graphics;
using AndroidX.Core.View;
using Java.Interop;
using Java.Lang;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using SMSForwarder.Platforms.Android;

namespace SMSForwarder;

[Activity(/*Could not decode attribute arguments.*/)]
public class MainActivity : MauiAppCompatActivity
{
	private class SystemBarInsetsListener : Object, IOnApplyWindowInsetsListener, IJavaObject, IDisposable, IJavaPeerable
	{
		public WindowInsetsCompat OnApplyWindowInsets(View? view, WindowInsetsCompat? insets)
		{
			WindowInsetsCompat consumed = WindowInsetsCompat.Consumed;
			if (view == null || insets == null)
			{
				return consumed;
			}
			Insets insets2 = insets.GetInsets(Type.SystemBars() | Type.DisplayCutout());
			if (insets2 != null)
			{
				view.SetPadding(insets2.Left, insets2.Top, insets2.Right, insets2.Bottom);
			}
			return consumed;
		}
	}

	private SmsReceiver smsReceiver;

	private IntentFilter intentFilter_SMS_RECEIVED;

	private void CheckAndRequestAutostart()
	{
		_ = Platform.AppContext;
	}

	public async Task<bool> SolicitarPermisosAsync()
	{
		_ = 1;
		try
		{
			if ((int)(await Permissions.RequestAsync<SmsPermissions.ReceiveSms>()) != 3)
			{
				Console.WriteLine("Permiso RECEIVE_SMS no otorgado.");
				return false;
			}
			if ((int)(await Permissions.RequestAsync<SmsPermissions.SendSms>()) != 3)
			{
				Console.WriteLine("Permiso SEND_SMS no otorgado.");
				return false;
			}
			Console.WriteLine("Todos los permisos fueron otorgados.");
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error al solicitar permisos: " + ex.Message);
			return false;
		}
	}

	protected override async void OnCreate(Bundle savedInstanceState)
	{
		_003C_003En__0(savedInstanceState);
		ApplySystemBarInsets();
		intentFilter_SMS_RECEIVED = new IntentFilter("android.provider.Telephony.SMS_RECEIVED")
		{
			Priority = 1000
		};
		smsReceiver = new SmsReceiver();
		try
		{
			if ((int)VERSION.SdkInt >= 33)
			{
				await Permissions.RequestAsync<PostNotifications>();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Permiso de notificaciones no concedido: " + ex.Message);
		}
		if (await SolicitarPermisosAsync())
		{
			try
			{
				((Context)this).RegisterReceiver((BroadcastReceiver)(object)smsReceiver, intentFilter_SMS_RECEIVED);
				Console.WriteLine("SmsReceiver registrado correctamente.");
			}
			catch (Exception ex2)
			{
				Console.WriteLine("Error al registrar SmsReceiver: " + ex2.Message);
			}
		}
		else
		{
			Console.WriteLine("No se registró SmsReceiver por falta de permisos.");
		}
		CheckAndRequestAutostart();
	}

	protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		((MauiAppCompatActivity)this).OnActivityResult(requestCode, resultCode, data);
		ContactPicker.HandleActivityResult(requestCode, resultCode, data);
		MessageStore.HandleActivityResult(requestCode, (Context)(object)this);
	}

	protected override void OnDestroy()
	{
		((Activity)this).OnDestroy();
		if (smsReceiver != null)
		{
			((Context)this).UnregisterReceiver((BroadcastReceiver)(object)smsReceiver);
			Console.WriteLine("SmsReceiver desregistrado correctamente.");
		}
	}

	public override void OnBackPressed()
	{
		((Activity)this).MoveTaskToBack(true);
		((Activity)this).FinishAndRemoveTask();
	}

	private void ApplySystemBarInsets()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		View val = ((Activity)this).FindViewById(16908290);
		if (val != null)
		{
			val.SetBackgroundColor(Color.ParseColor("#2A1CB8"));
			ViewCompat.SetOnApplyWindowInsetsListener(val, (IOnApplyWindowInsetsListener)(object)new SystemBarInsetsListener());
			WindowInsetsControllerCompat val2 = ((((Activity)this).Window != null) ? WindowCompat.GetInsetsController(((Activity)this).Window, ((Activity)this).Window.DecorView) : null);
			if (val2 != null)
			{
				val2.AppearanceLightStatusBars = false;
				val2.AppearanceLightNavigationBars = false;
			}
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(Bundle? savedInstanceState)
	{
		((MauiAppCompatActivity)this).OnCreate(savedInstanceState);
	}
}
