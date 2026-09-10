using ControleDeWebServices.Application.Feedback;
using System.Threading.Tasks;

namespace ControleDeWebServices.Presentation.Feedback
{
    public sealed class NoOpConfirmDialogService : IConfirmDialogService
    {
        public Task<ConfirmDialogResult> ConfirmAsync(ConfirmDialogRequest request)
        {
            return Task.FromResult(ConfirmDialogResult.Canceled);
        }
    }
}
