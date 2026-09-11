using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Platform;
using ControleDeWebServices.Application.Vinculos;
using ControleDeWebServices.Tests.TestDoubles;
using ControleDeWebServices.ViewModels;
using FluentAssertions;
using Moq;

namespace ControleDeWebServices.Tests.ViewModels;

public class ListaVinculosSistemaViewModelTests
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
        viewModel.SelectedSistema = new ClienteSistemaResumo { IdClientesSistema = 7, NomeSistema = "Sistema" };

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
    public void Incluir_DuranteEdicao_NaoSubstituiEditorAtual()
    {
        var viewModel = CreateViewModel();

        viewModel.Incluir();
        var editor = viewModel.Editor;
        viewModel.Incluir();

        viewModel.Editor.Should().BeSameAs(editor);
    }

    private static ListaVinculosSistemaViewModel CreateViewModel()
    {
        var vinculo = new Mock<IVinculoClienteSistemaService>();
        vinculo.Setup(service => service.ListarUfs()).Returns(Array.Empty<string>());
        vinculo.Setup(service => service.ListarSistemasDoCliente(It.IsAny<int>())).Returns(Array.Empty<ClienteSistemaResumo>());

        return new ListaVinculosSistemaViewModel(
            vinculo.Object,
            new RecordingToastService(),
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled),
            () => new VinculoClienteSistemaViewModel(vinculo.Object, new RecordingToastService()),
            CreateConfiguracaoSistemaViewModel);
    }

    private static ConfiguracaoSistemaViewModel CreateConfiguracaoSistemaViewModel()
    {
        var configuracao = new Mock<IConfiguracaoSistemaService>();
        configuracao.Setup(service => service.Obter(It.IsAny<int>())).Returns(new ConfiguracaoSistemaEditor());
        configuracao.Setup(service => service.ListarTiposConexao()).Returns(Array.Empty<TipoConexaoOption>());
        configuracao.Setup(service => service.ListarSecoes()).Returns(Array.Empty<SecaoOption>());

        var importacao = new Mock<IImportarParametrosService>();
        importacao.Setup(service => service.ListarUfs(It.IsAny<int>())).Returns(Array.Empty<string>());

        return new ConfiguracaoSistemaViewModel(
            configuracao.Object,
            importacao.Object,
            Mock.Of<IFilePickerService>(),
            new RecordingToastService(),
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled));
    }
}
