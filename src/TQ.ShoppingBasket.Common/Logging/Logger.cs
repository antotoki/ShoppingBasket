using Serilog;

namespace TQ.ShoppingBasket.Common.Logging
{
    public class Logger : ILogger
    {
        public Logger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
        }
    }
}