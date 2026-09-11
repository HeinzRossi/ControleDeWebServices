using ControleDeWebServices.Application.Logging;
using System;
using System.IO;

namespace ControleDeWebServices.Infrastructure.Logging
{
    public sealed class AppLoggerPathProvider : IAppLoggerPathProvider
    {
        public string GetLogFilePath()
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            return Path.Combine(directory, $"ControleDeWebServices-{DateTime.Now:yyyyMMdd}.log");
        }
    }
}
