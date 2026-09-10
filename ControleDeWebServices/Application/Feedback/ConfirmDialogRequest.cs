namespace ControleDeWebServices.Application.Feedback
{
    public sealed class ConfirmDialogRequest
    {
        public ConfirmDialogRequest(
            string title,
            string message,
            string confirmText = "Confirmar",
            string cancelText = "Cancelar",
            bool isDestructive = false)
        {
            Title = title;
            Message = message;
            ConfirmText = confirmText;
            CancelText = cancelText;
            IsDestructive = isDestructive;
        }

        public string Title { get; }
        public string Message { get; }
        public string ConfirmText { get; }
        public string CancelText { get; }
        public bool IsDestructive { get; }
    }
}
