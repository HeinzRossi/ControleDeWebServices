using ControleDeWebServices.Application.Feedback;

namespace ControleDeWebServices.Presentation.Feedback
{
    public sealed class NoOpToastService : IToastService
    {
        public void Show(ToastRequest request)
        {
        }
    }
}
