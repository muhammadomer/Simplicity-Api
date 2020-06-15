using log4net;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System;

namespace SimplicityOnlineWebApi.Commons
{
    public class Log4NetAdapter : ILogger
    {
        private ILog _logger;

        public Log4NetAdapter(string loggerName)
        {
            _logger = LogManager.GetLogger(GetType()); // to verify again - Faheem (25/03/2020)
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null; ;
        }

        public IDisposable BeginScopeImpl(object state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return _logger.IsDebugEnabled;
                case LogLevel.Information:
                    return _logger.IsInfoEnabled;
                case LogLevel.Warning:
                    return _logger.IsWarnEnabled;
                case LogLevel.Error:
                    return _logger.IsErrorEnabled;
                case LogLevel.Critical:
                    return _logger.IsFatalEnabled;
                default:
                    throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel));
            }
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            string message = null;
            if (null != formatter)
            {
                message = formatter(state, exception);
            }
            else
            {
                //message = new LogFormatter().Format(state, exception); // to fix later - Faheem (25/03/2020)
            }
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    _logger.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    _logger.Fatal(message, exception);
                    break;
                default:
                    _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                    _logger.Info(message, exception);
                    break;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
