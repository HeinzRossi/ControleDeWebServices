using System.Threading.Tasks;

namespace ControleDeWebServices.Application.Feedback
{
    public interface IConfirmDialogService
    {
        Task<ConfirmDialogResult> ConfirmAsync(ConfirmDialogRequest request);
    }
}
