using ControleDeWebServices.Application.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace ControleDeWebServices.Infrastructure.Logging
{
    public sealed class FileLoggerFactory : ILoggerFactory
    {
        private readonly IAppLoggerPathProvider pathProvider;
        private readonly ConcurrentDictionary<string, FileLogger> loggers = new ConcurrentDictionary<string, FileLogger>();

        public FileLoggerFactory(IAppLoggerPathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, category => new FileLogger(category, pathProvider));
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
