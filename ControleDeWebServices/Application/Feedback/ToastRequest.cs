using System;

namespace ControleDeWebServices.Application.Feedback
{
    public sealed class ToastRequest
    {
        public ToastRequest(ToastKind kind, string message, string title = null, TimeSpan? duration = null)
        {
            Kind = kind;
            Message = message;
            Title = title;
            Duration = duration ?? TimeSpan.FromSeconds(4);
        }

        public ToastKind Kind { get; }
        public string Message { get; }
        public string Title { get; }
        public TimeSpan Duration { get; }
    }
}
