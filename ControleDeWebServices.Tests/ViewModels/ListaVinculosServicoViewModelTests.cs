using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Platform;
using ControleDeWebServices.Application.Vinculos;
using ControleDeWebServices.Tests.TestDoubles;
using ControleDeWebServices.ViewModels;
using FluentAssertions;
using Moq;

namespace ControleDeWebServices.Tests.ViewModels;

public class ListaVinculosServicoViewModelTests
{
    [Fact]
    public void Incluir_DesabilitaAcoesDoTopo()
    {
        var viewModel = CreateViewModel();

        viewModel.Incluir();

        viewModel.IsEditing.Should().BeTrue();
        viewModel.CanUseListActions.Should().BeFalse();
        viewModel.IsListEnabled.Should().BeFalse();
        viewModel.IncluirCommand.CanExecute(null).Should().BeFalse();
        viewModel.EditarCommand.CanExecute(null).Should().BeFalse();
        viewModel.ConfigurarCommand.CanExecute(null).Should().BeFalse();
        viewModel.ExcluirCommand.CanExecute(null).Should().BeFalse();
    }

    [Fact]
    public void CancelarEdicao_ReabilitaAcoesDoTopo()
    {
        var viewModel = CreateViewModel();

        viewModel.Incluir();
        viewModel.CancelarEdicao();

        viewModel.IsEditing.Should().BeFalse();
        viewModel.CanUseListActions.Should().BeTrue();
        viewModel.IsListEnabled.Should().BeTrue();
        viewModel.IncluirCommand.CanExecute(null).Should().BeTrue();
    }

    [Fact]
    public void Configurar_DesabilitaAcoesDoTopo()
    {
        var viewModel = CreateViewModel();
        viewModel.SelectedServico = new ClienteServicoResumo { IdClienteServico = 9, NomeServico = "Serviço" };

        viewModel.Configurar();

        viewModel.IsConfiguring.Should().BeTrue();
        viewModel.CanUseListActions.Should().BeFalse();
        viewModel.IsListEnabled.Should().BeFalse();
        viewModel.IncluirCommand.CanExecute(null).Should().BeFalse();
        viewModel.EditarCommand.CanExecute(null).Should().BeFalse();
        viewModel.ConfigurarCommand.CanExecute(null).Should().BeFalse();
        viewModel.ExcluirCommand.CanExecute(null).Should().BeFalse();
    }

    [Fact]
    public void Incluir_DuranteConfiguracao_NaoSubstituiConfiguracaoAtual()
    {
        var viewModel = CreateViewModel();
        viewModel.SelectedServico = new ClienteServicoResumo { IdClienteServico = 9, NomeServico = "Serviço" };

        viewModel.Configurar();
        var configuracao = viewModel.Configuracao;
        viewModel.Incluir();

        viewModel.Configuracao.Should().BeSameAs(configuracao);
        viewModel.IsConfiguring.Should().BeTrue();
    }

    private static ListaVinculosServicoViewModel CreateViewModel()
    {
        var vinculo = new Mock<IVinculoClienteServicoService>();
        vinculo.Setup(service => service.ListarUfs()).Returns(Array.Empty<string>());
        vinculo.Setup(service => service.ListarSistemasComServicosDoCliente(It.IsAny<int>())).Returns(Array.Empty<ClienteServicoSistemaResumo>());
        vinculo.Setup(service => service.ListarServicosDoSistema(It.IsAny<int>(), It.IsAny<int>())).Returns(Array.Empty<ClienteServicoResumo>());

        return new ListaVinculosServicoViewModel(
            vinculo.Object,
            new RecordingToastService(),
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled),
            () => new VinculoClienteServicoViewModel(vinculo.Object, new RecordingToastService()),
            CreateConfiguracaoServicoViewModel);
    }

    private static ConfiguracaoServicoViewModel CreateConfiguracaoServicoViewModel()
    {
        var configuracao = new Mock<IConfiguracaoServicoService>();
        configuracao.Setup(service => service.Obter(It.IsAny<int>())).Returns(new ConfiguracaoServicoEditor());
        configuracao.Setup(service => service.ListarTiposConexao()).Returns(Array.Empty<TipoConexaoOption>());

        return new ConfiguracaoServicoViewModel(
            configuracao.Object,
            Mock.Of<IFilePickerService>(),
            new RecordingToastService());
    }
}
