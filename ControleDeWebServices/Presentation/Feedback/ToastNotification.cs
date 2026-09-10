using ControleDeWebServices.Application.Feedback;
using System;
using System.Windows.Media;

namespace ControleDeWebServices.Presentation.Feedback
{
    public sealed class ToastNotification
    {
        public ToastNotification(ToastRequest request)
        {
            Id = Guid.NewGuid();
            Kind = request.Kind;
            Title = request.Title ?? GetDefaultTitle(request.Kind);
            Message = request.Message;
            Duration = request.Duration;
            Background = GetBackground(request.Kind);
            Foreground = Brushes.White;
        }

        public Guid Id { get; }
        public ToastKind Kind { get; }
        public string Title { get; }
        public string Message { get; }
        public TimeSpan Duration { get; }
        public Brush Background { get; }
        public Brush Foreground { get; }

        private static string GetDefaultTitle(ToastKind kind)
        {
            switch (kind)
            {
                case ToastKind.Success:
                    return "Sucesso";
                case ToastKind.Warning:
                    return "Atenção";
                case ToastKind.Error:
                    return "Erro";
                default:
                    return "Informação";
            }
        }

        private static Brush GetBackground(ToastKind kind)
        {
            switch (kind)
            {
                case ToastKind.Success:
                    return new SolidColorBrush(Color.FromRgb(78, 154, 87));
                case ToastKind.Warning:
                    return new SolidColorBrush(Color.FromRgb(183, 126, 37));
                case ToastKind.Error:
                    return new SolidColorBrush(Color.FromRgb(185, 78, 69));
                default:
                    return new SolidColorBrush(Color.FromRgb(47, 111, 143));
            }
        }
    }
}
