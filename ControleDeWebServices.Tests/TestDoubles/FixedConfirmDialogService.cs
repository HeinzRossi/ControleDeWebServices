using ControleDeWebServices.Application.Feedback;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeWebServices.Tests.TestDoubles;

internal sealed class FixedConfirmDialogService : IConfirmDialogService
{
    private readonly ConfirmDialogResult result;

    public FixedConfirmDialogService(ConfirmDialogResult result)
    {
        this.result = result;
    }

    public List<ConfirmDialogRequest> Requests { get; } = new();

    public Task<ConfirmDialogResult> ConfirmAsync(ConfirmDialogRequest request)
    {
        Requests.Add(request);
        return Task.FromResult(result);
    }
}
