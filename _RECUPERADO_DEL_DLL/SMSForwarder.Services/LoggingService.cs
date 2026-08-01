using System;
using System.IO;
using System.Linq;
using Microsoft.Maui.Storage;

namespace SMSForwarder.Services;

public class LoggingService : ILoggingService
{
	private readonly string LogFileName = Path.Combine(FileSystem.AppDataDirectory, "sms_forwarder.log");

	private readonly object LogLock = new object();

	public void LogInfo(string message)
	{
		WriteLog("INFO", message);
	}

	public void LogError(string message, Exception ex = null)
	{
		string message2 = ((ex != null) ? $"{message} - Exception: {ex}" : message);
		WriteLog("ERROR", message2);
	}

	public void LogWarning(string message)
	{
		WriteLog("WARNING", message);
	}

	private void WriteLog(string level, string message)
	{
		try
		{
			lock (LogLock)
			{
				string text = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
				File.AppendAllText(LogFileName, text);
				Console.WriteLine(text.TrimEnd());
				TrimLogFile();
			}
		}
		catch
		{
			Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
		}
	}

	private void TrimLogFile()
	{
		try
		{
			if (File.Exists(LogFileName))
			{
				string[] array = File.ReadAllLines(LogFileName);
				if (array.Length > 1000)
				{
					string[] contents = array.Skip(array.Length - 800).ToArray();
					File.WriteAllLines(LogFileName, contents);
				}
			}
		}
		catch
		{
		}
	}

	public string GetLogContents()
	{
		try
		{
			return File.Exists(LogFileName) ? File.ReadAllText(LogFileName) : "No hay logs disponibles.";
		}
		catch
		{
			return "Error al leer los logs.";
		}
	}
}
