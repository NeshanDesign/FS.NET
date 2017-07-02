using System;
using NLog;

namespace FsNet.Common.Utils
{
    public static class Logger
    {
        private static readonly Lazy<NLog.Logger> _logger;

        static Logger()
        {
            _logger = new Lazy<NLog.Logger>(LogManager.GetCurrentClassLogger);
        }

        public static void Log(string message, LogLevel level = null)
        {
            var _level = level ?? LogLevel.Debug;
            _logger.Value.Log(_level, message);
        }

        public static void Log(string message, LogLevel level = null, params  object[] messageParams)
        {
            var msg = string.Format(message, messageParams);
            Log(msg, level);
        }

        public static void Info(string message)
        {
            _logger.Value.Info( message);
        }

        public static void Info(string message, params  object[] messageParams)
        {
            var msg = string.Format(message, messageParams);
            Info(msg);
        }

        public static void Error(string message)
        {
            var level = LogLevel.Error;
            _logger.Value.Log(level, message);
        }

        public static void Error(string message, params  object[] messageParams)
        {
            var msg = string.Format(message, messageParams);
            Error(msg);
        }

        public static void Error(Exception exception)
        {
           _logger.Value.Error(exception);
        }

        public static void Warn(string message)
        {
            _logger.Value.Warn(message);
        }

        public static void Warn(string message, params  object[] messageParams)
        {
            var msg = string.Format(message, messageParams);
            Warn(msg);
        }

        public static void Debug(string message)
        {
            _logger.Value.Debug(message);
        }

        public static void Debug(string message, params  object[] messageParams)
        {
            var msg = string.Format(message, messageParams);
            Debug(msg);
        }

        public static void Fatal(string message)
        {
            _logger.Value.Fatal(message);
        }

        public static void Fatal(Exception exception)
        {
            _logger.Value.Fatal(exception);
        }

        public static void Fatal(string message, params  object[] messageParams)
        {
            var msg = string.Format(message, messageParams);
            Fatal(msg);
        }
    }
}
