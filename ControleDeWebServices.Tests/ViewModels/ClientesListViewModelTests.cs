using ControleDeWebServices.Application.Clientes;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Tests.TestDoubles;
using ControleDeWebServices.ViewModels;
using FluentAssertions;
using Moq;

namespace ControleDeWebServices.Tests.ViewModels;

public class ClientesListViewModelTests
{
    [Fact]
    public void Editar_SemSelecao_MostraWarning()
    {
        var toast = new RecordingToastService();
        var viewModel = new ClientesListViewModel(
            Mock.Of<IClientesService>(),
            toast,
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled));

        viewModel.Editar(null);

        toast.Requests.Should().ContainSingle();
        toast.Requests[0].Kind.Should().Be(ToastKind.Warning);
        toast.Requests[0].Message.Should().Contain("Selecione um cliente");
    }

    [Fact]
    public void Salvar_NovoClienteValido_ChamaServicoEVoltaParaLista()
    {
        var toast = new RecordingToastService();
        var service = new Mock<IClientesService>();
        service.Setup(item => item.Listar()).Returns(Array.Empty<ClienteListItem>());
        var viewModel = new ClientesListViewModel(
            service.Object,
            toast,
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled));

        viewModel.Incluir();
        viewModel.Editor.CodigoControle = "123";
        viewModel.Editor.NomeCliente = "Cliente Teste";
        viewModel.Editor.Uf = "SP";
        viewModel.Salvar();

        service.Verify(item => item.Salvar(It.Is<ClienteEditor>(editor =>
            editor.CodigoControle == 123 &&
            editor.NomeCliente == "Cliente Teste" &&
            editor.Uf == "SP")), Times.Once);
        viewModel.IsEditing.Should().BeFalse();
        viewModel.CanUseListActions.Should().BeTrue();
        viewModel.IsListEnabled.Should().BeTrue();
        toast.Requests.Should().Contain(request => request.Kind == ToastKind.Success);
    }

    [Fact]
    public void Incluir_DesabilitaAcoesDeLista()
    {
        var viewModel = new ClientesListViewModel(
            Mock.Of<IClientesService>(),
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
        var viewModel = new ClientesListViewModel(
            Mock.Of<IClientesService>(),
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
    public void Incluir_DuranteEdicao_NaoSubstituiEditorAtual()
    {
        var viewModel = new ClientesListViewModel(
            Mock.Of<IClientesService>(),
            new RecordingToastService(),
            new FixedConfirmDialogService(ConfirmDialogResult.Canceled));

        viewModel.Incluir();
        viewModel.Editor.NomeCliente = "Cliente em edição";
        var editor = viewModel.Editor;
        viewModel.Incluir();

        viewModel.Editor.Should().BeSameAs(editor);
        viewModel.Editor.NomeCliente.Should().Be("Cliente em edição");
    }

    [Fact]
    public async Task ExcluirAsync_QuandoServicoFalha_MostraErro()
    {
        var toast = new RecordingToastService();
        var service = new Mock<IClientesService>();
        service.Setup(item => item.Excluir(8)).Throws(new InvalidOperationException("Cliente nao encontrado."));
        var viewModel = new ClientesListViewModel(
            service.Object,
            toast,
            new FixedConfirmDialogService(ConfirmDialogResult.Confirmed));

        await viewModel.ExcluirAsync(new ClienteListItem { IdCliente = 8, NomeCliente = "Cliente" });

        toast.Requests.Should().Contain(request => request.Kind == ToastKind.Error && request.Message.Contains("Cliente nao encontrado"));
    }
}
