using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Java.Lang;
using Microsoft.Maui.ApplicationModel;

public class SmsPermissions
{
	public class AutoStartPermission : BasePermission
	{
		public override Task<PermissionStatus> CheckStatusAsync()
		{
			try
			{
				object obj = ((object)Platform.CurrentActivity) ?? ((object)Application.Context);
				_ = ((Context)obj).PackageManager;
				_ = ((Context)obj).PackageName;
				return Task.FromResult<PermissionStatus>((PermissionStatus)0);
			}
			catch
			{
				return Task.FromResult<PermissionStatus>((PermissionStatus)0);
			}
		}

		public override async Task<PermissionStatus> RequestAsync()
		{
			try
			{
				object obj = ((object)Platform.CurrentActivity) ?? ((object)Application.Context);
				Intent val = new Intent();
				string packageName = ((Context)obj).PackageName;
				if (IsManufacturer("xiaomi"))
				{
					val.SetComponent(new ComponentName("com.miui.securitycenter", "com.miui.permcenter.autostart.AutoStartManagementActivity"));
				}
				else if (IsManufacturer("huawei"))
				{
					val.SetComponent(new ComponentName("com.huawei.systemmanager", "com.huawei.systemmanager.startupmgr.ui.StartupNormalAppListActivity"));
				}
				else if (IsManufacturer("oppo"))
				{
					val.SetComponent(new ComponentName("com.coloros.safecenter", "com.coloros.safecenter.permission.startup.StartupAppListActivity"));
				}
				else if (IsManufacturer("vivo"))
				{
					val.SetComponent(new ComponentName("com.vivo.permissionmanager", "com.vivo.permissionmanager.activity.BgStartUpManagerActivity"));
				}
				else if (IsManufacturer("samsung"))
				{
					val.SetComponent(new ComponentName("com.samsung.android.lool", "com.samsung.android.sm.ui.battery.BatteryActivity"));
				}
				else if (IsManufacturer("oneplus"))
				{
					val.SetComponent(new ComponentName("com.oneplus.security", "com.oneplus.security.chainlaunch.view.ChainLaunchAppListActivity"));
				}
				else
				{
					val.SetAction("android.settings.APPLICATION_DETAILS_SETTINGS");
					val.SetData(Uri.Parse("package:" + packageName));
				}
				val.AddFlags((ActivityFlags)268435456);
				((Context)obj).StartActivity(val);
				return (PermissionStatus)0;
			}
			catch
			{
				return (PermissionStatus)1;
			}
		}

		public override bool ShouldShowRationale()
		{
			return true;
		}

		public override void EnsureDeclared()
		{
		}

		private bool IsManufacturer(string manufacturer)
		{
			return Build.Manufacturer?.ToLower().Contains(manufacturer) ?? false;
		}
	}

	public class BatteryOptimizationPermission : BasePermission
	{
		public override Task<PermissionStatus> CheckStatusAsync()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Invalid comparison between Unknown and I4
			try
			{
				Context val = (Context)(((object)Platform.CurrentActivity) ?? ((object)Application.Context));
				Object systemService = val.GetSystemService("power");
				PowerManager val2 = (PowerManager)(object)((systemService is PowerManager) ? systemService : null);
				if (val2 != null && (int)VERSION.SdkInt >= 23)
				{
					string packageName = val.PackageName;
					return Task.FromResult<PermissionStatus>((PermissionStatus)((!val2.IsIgnoringBatteryOptimizations(packageName)) ? 1 : 3));
				}
				return Task.FromResult<PermissionStatus>((PermissionStatus)3);
			}
			catch
			{
				return Task.FromResult<PermissionStatus>((PermissionStatus)0);
			}
		}

		public override async Task<PermissionStatus> RequestAsync()
		{
			_ = 1;
			try
			{
				Context val = (Context)(((object)Platform.CurrentActivity) ?? ((object)Application.Context));
				if ((int)VERSION.SdkInt >= 23)
				{
					Intent val2 = new Intent();
					string packageName = val.PackageName;
					val2.SetAction("android.settings.REQUEST_IGNORE_BATTERY_OPTIMIZATIONS");
					val2.SetData(Uri.Parse("package:" + packageName));
					val2.AddFlags((ActivityFlags)268435456);
					val.StartActivity(val2);
					await Task.Delay(1000);
					return await ((BasePermission)this).CheckStatusAsync();
				}
				return (PermissionStatus)3;
			}
			catch
			{
				return (PermissionStatus)1;
			}
		}

		public override bool ShouldShowRationale()
		{
			return true;
		}

		public override void EnsureDeclared()
		{
		}
	}

	public class ReceiveSms : BasePermission
	{
		public override Task<PermissionStatus> CheckStatusAsync()
		{
			return Permissions.CheckStatusAsync<Sms>();
		}

		public override Task<PermissionStatus> RequestAsync()
		{
			return Permissions.RequestAsync<Sms>();
		}

		public override bool ShouldShowRationale()
		{
			return Permissions.ShouldShowRationale<Sms>();
		}

		public override void EnsureDeclared()
		{
		}
	}

	public class SendSms : BasePermission
	{
		public override Task<PermissionStatus> CheckStatusAsync()
		{
			return Permissions.CheckStatusAsync<Sms>();
		}

		public override Task<PermissionStatus> RequestAsync()
		{
			return Permissions.RequestAsync<Sms>();
		}

		public override bool ShouldShowRationale()
		{
			return Permissions.ShouldShowRationale<Sms>();
		}

		public override void EnsureDeclared()
		{
		}
	}

	/// <summary>
	/// Permiso READ_SMS en tiempo de ejecucion, necesario para listar los mensajes del
	/// proveedor del sistema en la pantalla "Mensajes". Al ser la app de SMS por defecto,
	/// el rol concede este permiso; si no lo es, se solicita explicitamente.
	/// </summary>
	public class ReadSmsPermission : BasePlatformPermission
	{
		public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new(string, bool)[1] { ("android.permission.READ_SMS", true) };
	}

	public class BroadCastSms : BasePermission
	{
		public override Task<PermissionStatus> CheckStatusAsync()
		{
			return Permissions.CheckStatusAsync<Sms>();
		}

		public override Task<PermissionStatus> RequestAsync()
		{
			return Permissions.RequestAsync<Sms>();
		}

		public override bool ShouldShowRationale()
		{
			return Permissions.ShouldShowRationale<Sms>();
		}

		public override void EnsureDeclared()
		{
		}
	}
}
