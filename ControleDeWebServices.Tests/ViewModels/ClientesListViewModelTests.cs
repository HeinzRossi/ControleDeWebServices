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
        toast.Requests.Should().Contain(request => request.Kind == ToastKind.Success);
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
