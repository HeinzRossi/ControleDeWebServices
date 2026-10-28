using ControleDeWebServices.Application.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ControleDeWebServices.Infrastructure.Logging
{
    public sealed class FileLogger : ILogger
    {
        private static readonly object SyncRoot = new object();
        private readonly string categoryName;
        private readonly IAppLoggerPathProvider pathProvider;

        public FileLogger(string categoryName, IAppLoggerPathProvider pathProvider)
        {
            this.categoryName = categoryName;
            this.pathProvider = pathProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel) || formatter == null)
            {
                return;
            }

            var message = Sanitize(formatter(state, exception));
            var exceptionText = exception == null ? string.Empty : $" | {Sanitize(exception.GetType().Name)}: {Sanitize(exception.Message)}";
            var line = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz} [{logLevel}] {categoryName}: {message}{exceptionText}";

            try
            {
                var path = pathProvider.GetLogFilePath();
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                lock (SyncRoot)
                {
                    File.AppendAllText(path, line + Environment.NewLine);
                }
            }
            catch
            {
                // Logging nunca deve interromper o fluxo operacional do sistema.
            }
        }

        private static string Sanitize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var sanitized = Regex.Replace(value, "(Password|Pwd|Senha)\\s*=\\s*[^;\\s]+", "$1=***", RegexOptions.IgnoreCase);
            sanitized = Regex.Replace(sanitized, "(User ID|Usuario|Username)\\s*=\\s*[^;\\s]+", "$1=***", RegexOptions.IgnoreCase);
            return sanitized.Replace(Environment.NewLine, " ");
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new NullScope();

            public void Dispose()
            {
            }
        }
    }
}
