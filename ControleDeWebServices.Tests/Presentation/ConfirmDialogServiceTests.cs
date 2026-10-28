using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Presentation.Feedback;
using FluentAssertions;

namespace ControleDeWebServices.Tests.Presentation;

public class ConfirmDialogServiceTests
{
    [Fact]
    public async Task ConfirmAsync_Confirm_ConcluiComoConfirmado()
    {
        var service = new ConfirmDialogService();

        var task = service.ConfirmAsync(new ConfirmDialogRequest(
            "Excluir este registro?",
            "Confirme a exclusao.",
            "Excluir",
            "Cancelar",
            true));

        service.IsOpen.Should().BeTrue();
        service.CurrentRequest.Should().NotBeNull();

        service.Confirm();

        (await task).Should().Be(ConfirmDialogResult.Confirmed);
        service.IsOpen.Should().BeFalse();
        service.CurrentRequest.Should().BeNull();
    }

    [Fact]
    public async Task ConfirmAsync_Cancel_ConcluiComoCancelado()
    {
        var service = new ConfirmDialogService();

        var task = service.ConfirmAsync(new ConfirmDialogRequest(
            "Excluir este registro?",
            "Confirme a exclusao.",
            "Excluir",
            "Cancelar",
            true));

        service.Cancel();

        (await task).Should().Be(ConfirmDialogResult.Canceled);
        service.IsOpen.Should().BeFalse();
    }
}
