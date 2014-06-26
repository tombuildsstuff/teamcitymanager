namespace TeamCityManager.Infrastructure.Logging
{
    using System;

    public class ConsoleLogger : ILogger
    {
        public void Error(string message, params object[] format)
        {
            var output = string.Format(message, format);
            Console.WriteLine("ERROR: {0}", output);
        }

        public void Exception(string message, Exception exception)
        {
            Error(message);
            Error(exception.ToString());
        }

        public void Info(string message, params object[] format)
        {
            Console.WriteLine(message, format);
        }
    }
}