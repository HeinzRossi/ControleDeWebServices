using ControleDeWebServices.Application.Feedback;
using System.Collections.Generic;

namespace ControleDeWebServices.Tests.TestDoubles;

internal sealed class RecordingToastService : IToastService
{
    public List<ToastRequest> Requests { get; } = new();

    public void Show(ToastRequest request)
    {
        Requests.Add(request);
    }
}
