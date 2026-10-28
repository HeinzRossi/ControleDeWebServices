using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Tests.TestDoubles;
using ControleDeWebServices.ViewModels;
using FluentAssertions;
using Moq;

namespace ControleDeWebServices.Tests.ViewModels;

public class ImportarParametrosViewModelTests
{
    [Fact]
    public async Task ImportarAsync_SemSistemaSelecionado_MostraWarningENaoConfirma()
    {
        var toast = new RecordingToastService();
        var confirm = new FixedConfirmDialogService(ConfirmDialogResult.Confirmed);
        var viewModel = new ImportarParametrosViewModel(Mock.Of<IImportarParametrosService>(), toast, confirm);

        await viewModel.ImportarAsync();

        toast.Requests.Should().ContainSingle();
        toast.Requests[0].Kind.Should().Be(ToastKind.Warning);
        confirm.Requests.Should().BeEmpty();
    }

    [Fact]
    public async Task ImportarAsync_ComConfirmacao_ChamaServicoEMostraSucesso()
    {
        var toast = new RecordingToastService();
        var confirm = new FixedConfirmDialogService(ConfirmDialogResult.Confirmed);
        var service = new Mock<IImportarParametrosService>();
        service.Setup(item => item.ListarUfs(9)).Returns(Array.Empty<string>());
        service
            .Setup(item => item.Importar(4, 9))
            .Returns(new ImportacaoParametrosResultado { Inseridos = 2, Atualizados = 1 });
        var viewModel = new ImportarParametrosViewModel(service.Object, toast, confirm);
        viewModel.Carregar(9);
        viewModel.SelectedSistema = new ImportacaoSistemaOption { IdClientesSistema = 4, NomeSistema = "Origem" };

        await viewModel.ImportarAsync();

        service.Verify(item => item.Importar(4, 9), Times.Once);
        confirm.Requests.Should().ContainSingle();
        toast.Requests.Should().Contain(request => request.Kind == ToastKind.Success);
    }
}
