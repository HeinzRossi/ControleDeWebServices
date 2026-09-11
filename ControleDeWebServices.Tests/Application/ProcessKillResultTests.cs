using ControleDeWebServices.Application.Operacao;
using FluentAssertions;

namespace ControleDeWebServices.Tests.Application;

public class ProcessKillResultTests
{
    [Fact]
    public void ProcessKillResult_NasceSemFalhas()
    {
        var result = new ProcessKillResult();

        result.KilledCount.Should().Be(0);
        result.Failures.Should().BeEmpty();
    }
}
