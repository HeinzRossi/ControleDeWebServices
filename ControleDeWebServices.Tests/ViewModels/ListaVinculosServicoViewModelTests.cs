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

    [Fact]
    public async Task ExcluirCommand_ComSelecaoEConfirmacao_ChamaServico()
    {
        var vinculo = new Mock<IVinculoClienteServicoService>();
        vinculo.Setup(service => service.ListarClientesComVinculos()).Returns(Array.Empty<ClienteVinculoListItem>());
        vinculo.Setup(service => service.ListarSistemasComServicosDoCliente(It.IsAny<int>())).Returns(Array.Empty<ClienteServicoSistemaResumo>());
        vinculo.Setup(service => service.ListarServicosDoSistema(It.IsAny<int>(), It.IsAny<int>())).Returns(Array.Empty<ClienteServicoResumo>());
        var confirm = new FixedConfirmDialogService(ConfirmDialogResult.Confirmed);
        var viewModel = CreateViewModel(vinculo, confirm);
        viewModel.SelectedCliente = new ClienteVinculoListItem { IdCliente = 21, NomeCliente = "Cliente Teste", Uf = "SP" };
        viewModel.SelectedSistema = new ClienteServicoSistemaResumo { IdCliente = 21, IdSistemas = 34, NomeSistema = "Sistema Teste" };

        viewModel.ExcluirCommand.CanExecute(null).Should().BeTrue();
        await viewModel.ExcluirCommand.ExecuteAsync(null);

        vinculo.Verify(service => service.ExcluirVinculosDoSistema(21, 34), Times.Once);
        confirm.Requests.Should().ContainSingle(request =>
            request.Title == "Excluir vínculos do sistema?" &&
            request.ConfirmText == "Excluir" &&
            request.IsDestructive);
    }

    [Fact]
    public async Task ExcluirCommand_SemSelecao_MostraWarning()
    {
        var toast = new RecordingToastService();
        var viewModel = CreateViewModel(toastService: toast);

        viewModel.ExcluirCommand.CanExecute(null).Should().BeTrue();
        await viewModel.ExcluirCommand.ExecuteAsync(null);

        toast.Requests.Should().ContainSingle(request =>
            request.Kind == ToastKind.Warning &&
            request.Message.Contains("Selecione um cliente e um sistema"));
    }

    private static ListaVinculosServicoViewModel CreateViewModel()
    {
        return CreateViewModel(new Mock<IVinculoClienteServicoService>(), new FixedConfirmDialogService(ConfirmDialogResult.Canceled));
    }

    private static ListaVinculosServicoViewModel CreateViewModel(
        Mock<IVinculoClienteServicoService>? vinculo = null,
        FixedConfirmDialogService? confirmDialogService = null,
        RecordingToastService? toastService = null)
    {
        vinculo ??= new Mock<IVinculoClienteServicoService>();
        vinculo.Setup(service => service.ListarUfs()).Returns(Array.Empty<string>());
        vinculo.Setup(service => service.ListarClientesComVinculos()).Returns(Array.Empty<ClienteVinculoListItem>());
        vinculo.Setup(service => service.ListarSistemasComServicosDoCliente(It.IsAny<int>())).Returns(Array.Empty<ClienteServicoSistemaResumo>());
        vinculo.Setup(service => service.ListarServicosDoSistema(It.IsAny<int>(), It.IsAny<int>())).Returns(Array.Empty<ClienteServicoResumo>());
        toastService ??= new RecordingToastService();
        confirmDialogService ??= new FixedConfirmDialogService(ConfirmDialogResult.Canceled);

        return new ListaVinculosServicoViewModel(
            vinculo.Object,
            toastService,
            confirmDialogService,
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
