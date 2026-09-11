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
}
