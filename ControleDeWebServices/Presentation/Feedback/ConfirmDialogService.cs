using CommunityToolkit.Mvvm.ComponentModel;
using ControleDeWebServices.Application.Feedback;
using System.Threading.Tasks;

namespace ControleDeWebServices.Presentation.Feedback
{
    public sealed partial class ConfirmDialogService : ObservableObject, IConfirmDialogService
    {
        private TaskCompletionSource<ConfirmDialogResult> completionSource;

        [ObservableProperty]
        private bool isOpen;

        [ObservableProperty]
        private ConfirmDialogRequest currentRequest;

        public Task<ConfirmDialogResult> ConfirmAsync(ConfirmDialogRequest request)
        {
            if (IsOpen && completionSource != null)
            {
                completionSource.TrySetResult(ConfirmDialogResult.Canceled);
            }

            CurrentRequest = request;
            IsOpen = true;
            completionSource = new TaskCompletionSource<ConfirmDialogResult>();
            return completionSource.Task;
        }

        public void Confirm()
        {
            Complete(ConfirmDialogResult.Confirmed);
        }

        public void Cancel()
        {
            Complete(ConfirmDialogResult.Canceled);
        }

        private void Complete(ConfirmDialogResult result)
        {
            IsOpen = false;
            CurrentRequest = null;
            completionSource?.TrySetResult(result);
            completionSource = null;
        }
    }
}
