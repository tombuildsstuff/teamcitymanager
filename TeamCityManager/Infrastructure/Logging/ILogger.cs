namespace TeamCityManager.Infrastructure.Logging
{
    using System;

    public interface ILogger
    {
        void Error(string message, params object[] format);

        void Exception(string message, Exception exception);

        void Info(string message, params object[] format);
    }
}