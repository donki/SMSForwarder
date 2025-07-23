using System.Text;

namespace SMSForwarder.Services
{
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
            var fullMessage = ex != null ? $"{message} - Exception: {ex}" : message;
            WriteLog("ERROR", fullMessage);
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
                    var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
                    File.AppendAllText(LogFileName, logEntry);
                    
                    // También escribir en consola para debugging
                    Console.WriteLine(logEntry.TrimEnd());
                    
                    // Mantener solo los últimos 1000 líneas para evitar archivos muy grandes
                    TrimLogFile();
                }
            }
            catch
            {
                // Si no podemos escribir logs, al menos intentamos consola
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
            }
        }

        private void TrimLogFile()
        {
            try
            {
                if (File.Exists(LogFileName))
                {
                    var lines = File.ReadAllLines(LogFileName);
                    if (lines.Length > 1000)
                    {
                        var lastLines = lines.Skip(lines.Length - 800).ToArray();
                        File.WriteAllLines(LogFileName, lastLines);
                    }
                }
            }
            catch
            {
                // Si falla el trim, continuamos sin hacer nada
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
}