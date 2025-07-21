namespace SMSForwarder.Services
{
    public interface ILoggingService
    {
        void LogInfo(string message);
        void LogError(string message, Exception ex = null);
        void LogWarning(string message);
        string GetLogContents();
    }
}