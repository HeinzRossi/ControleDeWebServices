using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using FluentAssertions;

namespace ControleDeWebServices.Tests.Configuracoes;

public class AtualizarPadroesTests
{
    [Fact]
    public void AtualizarUrl_ComConfiguracaoNula_RetornaWarning()
    {
        var service = new AtualizarPadroes();

        var result = service.AtualizarUrl(null!);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("WebService");
        result.RowsAffected.Should().Be(0);
    }

    [Fact]
    public void AtualizarUrl_ComConfiguracaoIncompleta_RetornaWarningSemAbrirConexao()
    {
        var service = new AtualizarPadroes();
        var clienteSistema = new ClienteSistemas
        {
            IdClientesSistema = 1,
            TipoConexao = (int)TipoConexao.SQLSERVER,
            Servidor = "",
            DataBase = "",
            Usuario = ""
        };

        var result = service.AtualizarUrl(clienteSistema);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Configure servidor");
        result.RowsAffected.Should().Be(0);
    }

    [Fact]
    public void AtualizarUrl_ComTipoConexaoNaoSuportado_RetornaWarning()
    {
        var service = new AtualizarPadroes();
        var clienteSistema = new ClienteSistemas
        {
            IdClientesSistema = 1,
            TipoConexao = (int)TipoConexao.ORACLE,
            Servidor = "servidor",
            DataBase = "banco",
            Usuario = "usuario"
        };

        var result = service.AtualizarUrl(clienteSistema);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("não suportado");
    }

    [Fact]
    public void AtualizarUrl_ComPostgreSqlSemPorta_RetornaWarningSemAbrirConexao()
    {
        var service = new AtualizarPadroes();
        var clienteSistema = new ClienteSistemas
        {
            IdClientesSistema = 1,
            TipoConexao = (int)TipoConexao.POSTGRESQL,
            Servidor = "servidor",
            DataBase = "banco",
            Usuario = "usuario",
            Porta = 0
        };

        var result = service.AtualizarUrl(clienteSistema);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("porta");
    }
}
