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
}
