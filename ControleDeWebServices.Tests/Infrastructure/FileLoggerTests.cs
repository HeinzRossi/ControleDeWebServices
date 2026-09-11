using ControleDeWebServices.Application.Logging;
using ControleDeWebServices.Infrastructure.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace ControleDeWebServices.Tests.Infrastructure;

public class FileLoggerTests
{
    [Fact]
    public void Log_CriaArquivoNoCaminhoConfigurado()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "app.log");
        var logger = new FileLogger("Teste", new FixedPathProvider(path));

        logger.LogInformation("Mensagem operacional {Id}", 10);

        File.Exists(path).Should().BeTrue();
        File.ReadAllText(path).Should().Contain("Mensagem operacional 10");
    }

    [Fact]
    public void Log_RemoveDadosSensiveisDaMensagem()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "app.log");
        var logger = new FileLogger("Teste", new FixedPathProvider(path));

        logger.LogError("Falha com Password=123;User ID=admin;Senha=abc");

        var content = File.ReadAllText(path);
        content.Should().Contain("Password=***");
        content.Should().Contain("User ID=***");
        content.Should().Contain("Senha=***");
        content.Should().NotContain("123");
        content.Should().NotContain("admin");
        content.Should().NotContain("abc");
    }

    private sealed class FixedPathProvider : IAppLoggerPathProvider
    {
        private readonly string path;

        public FixedPathProvider(string path)
        {
            this.path = path;
        }

        public string GetLogFilePath()
        {
            return path;
        }
    }
}
