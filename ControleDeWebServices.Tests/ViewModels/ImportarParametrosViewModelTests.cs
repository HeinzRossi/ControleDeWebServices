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
}
