using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Tests.TestDoubles;
using ControleDeWebServices.ViewModels;
using FluentAssertions;
using Moq;

namespace ControleDeWebServices.Tests.ViewModels;

public class SistemasListViewModelTests
{
    [Fact]
    public void Incluir_DesabilitaAcoesDeLista()
    {
        var viewModel = new SistemasListViewModel(
            Mock.Of<ISistemasService>(),
            new RecordingToastService(),
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled));

        viewModel.Incluir();

        viewModel.IsEditing.Should().BeTrue();
        viewModel.CanUseListActions.Should().BeFalse();
        viewModel.IsListEnabled.Should().BeFalse();
        viewModel.IncluirCommand.CanExecute(null).Should().BeFalse();
        viewModel.EditarCommand.CanExecute(null).Should().BeFalse();
        viewModel.ExcluirCommand.CanExecute(null).Should().BeFalse();
    }

    [Fact]
    public void Cancelar_ReabilitaAcoesDeLista()
    {
        var viewModel = new SistemasListViewModel(
            Mock.Of<ISistemasService>(),
            new RecordingToastService(),
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled));

        viewModel.Incluir();
        viewModel.Cancelar();

        viewModel.IsEditing.Should().BeFalse();
        viewModel.CanUseListActions.Should().BeTrue();
        viewModel.IsListEnabled.Should().BeTrue();
        viewModel.IncluirCommand.CanExecute(null).Should().BeTrue();
    }

    [Fact]
    public async Task ExcluirCommand_ComItemEConfirmacao_ChamaServico()
    {
        var service = new Mock<ISistemasService>();
        service.Setup(item => item.Listar()).Returns(Array.Empty<SistemaListItem>());
        var confirm = new FixedConfirmDialogService(ConfirmDialogResult.Confirmed);
        var viewModel = new SistemasListViewModel(
            service.Object,
            new RecordingToastService(),
            confirm);
        var sistema = new SistemaListItem { IdSistemas = 7, NomeSistema = "Sistema Teste" };

        viewModel.ExcluirCommand.CanExecute(sistema).Should().BeTrue();
        await viewModel.ExcluirCommand.ExecuteAsync(sistema);

        service.Verify(item => item.Excluir(7), Times.Once);
        confirm.Requests.Should().ContainSingle(request =>
            request.Title == "Excluir este sistema?" &&
            request.ConfirmText == "Excluir" &&
            request.IsDestructive);
    }

    [Fact]
    public async Task ExcluirCommand_ComConfirmacaoCancelada_NaoChamaServico()
    {
        var service = new Mock<ISistemasService>();
        var viewModel = new SistemasListViewModel(
            service.Object,
            new RecordingToastService(),
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled));
        var sistema = new SistemaListItem { IdSistemas = 7, NomeSistema = "Sistema Teste" };

        await viewModel.ExcluirCommand.ExecuteAsync(sistema);

        service.Verify(item => item.Excluir(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ExcluirCommand_SemItem_MostraWarning()
    {
        var toast = new RecordingToastService();
        var viewModel = new SistemasListViewModel(
            Mock.Of<ISistemasService>(),
            toast,
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled));

        await viewModel.ExcluirCommand.ExecuteAsync(null);

        toast.Requests.Should().ContainSingle(request =>
            request.Kind == ToastKind.Warning &&
            request.Message.Contains("Selecione um sistema"));
    }
}
