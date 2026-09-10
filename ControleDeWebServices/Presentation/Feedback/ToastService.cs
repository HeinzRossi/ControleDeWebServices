using ControleDeWebServices.Application.Feedback;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace ControleDeWebServices.Presentation.Feedback
{
    public sealed class ToastService : IToastService
    {
        private const int MaxVisibleToasts = 4;

        public ObservableCollection<ToastNotification> Toasts { get; } = new ObservableCollection<ToastNotification>();

        public void Show(ToastRequest request)
        {
            if (request == null)
            {
                return;
            }

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var toast = new ToastNotification(request);

                while (Toasts.Count >= MaxVisibleToasts)
                {
                    Toasts.RemoveAt(0);
                }

                Toasts.Add(toast);
                ScheduleRemoval(toast);
            });
        }

        public void Dismiss(ToastNotification toast)
        {
            if (toast == null)
            {
                return;
            }

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var item = Toasts.FirstOrDefault(current => current.Id == toast.Id);
                if (item != null)
                {
                    Toasts.Remove(item);
                }
            });
        }

        private void ScheduleRemoval(ToastNotification toast)
        {
            var timer = new DispatcherTimer
            {
                Interval = toast.Duration
            };

            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                Dismiss(toast);
            };

            timer.Start();
        }
    }
}
