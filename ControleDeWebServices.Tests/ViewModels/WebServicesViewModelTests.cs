using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Operacao;
using ControleDeWebServices.Tests.TestDoubles;
using ControleDeWebServices.ViewModels;
using FluentAssertions;
using Moq;

namespace ControleDeWebServices.Tests.ViewModels;

public class WebServicesViewModelTests
{
    [Fact]
    public async Task ExecutarSelecionadoAsync_SemSelecao_MostraWarning()
    {
        var toast = new RecordingToastService();
        var viewModel = CreateViewModel(toast: toast);

        await viewModel.ExecutarSelecionadoAsync(null);

        toast.Requests.Should().ContainSingle();
        toast.Requests[0].Kind.Should().Be(ToastKind.Warning);
        toast.Requests[0].Message.Should().Contain("Selecione um WebService");
    }

    [Fact]
    public async Task ExecutarSelecionadoAsync_ComSucesso_RegistraEtapas()
    {
        var toast = new RecordingToastService();
        var services = new Mock<IWebServicesService>();
        services.Setup(service => service.ListarMaisAcessados()).Returns(Array.Empty<WebServiceItem>());
        services.Setup(service => service.ListarUfs()).Returns(Array.Empty<string>());

        var execution = new Mock<IWebServiceExecutionService>();
        execution
            .Setup(service => service.ExecutarAsync(10, It.IsAny<Action<WebServiceExecutionStep>>()))
            .Returns<int, Action<WebServiceExecutionStep>>((_, progress) =>
            {
                progress(new WebServiceExecutionStep("Validando configuração"));
                progress(new WebServiceExecutionStep("Registrando acesso"));
                return Task.FromResult(new WebServiceExecutionResult
                {
                    Success = true,
                    Message = "WebService executado com sucesso.",
                    CompletedSteps = new[] { "Validando configuração", "Registrando acesso" }
                });
            });

        var viewModel = CreateViewModel(services.Object, execution.Object, toast);
        var item = new WebServiceItem { IdClientesSistema = 10, NomeSistema = "Sistema" };

        await viewModel.ExecutarSelecionadoAsync(item);

        viewModel.ExecutionSteps.Should().ContainInOrder("Validando configuração", "Registrando acesso");
        viewModel.ExecutionStatus.Should().Be("WebService executado com sucesso.");
        toast.Requests.Should().Contain(request => request.Kind == ToastKind.Success);
    }

    [Fact]
    public async Task ExecutarSelecionadoAsync_ComExcecao_MostraErroSemPropagar()
    {
        var toast = new RecordingToastService();
        var execution = new Mock<IWebServiceExecutionService>();
        execution
            .Setup(service => service.ExecutarAsync(20, It.IsAny<Action<WebServiceExecutionStep>>()))
            .ThrowsAsync(new InvalidOperationException("configuração inválida"));

        var viewModel = CreateViewModel(executionService: execution.Object, toast: toast);
        var item = new WebServiceItem { IdClientesSistema = 20, NomeSistema = "Sistema" };

        var action = () => viewModel.ExecutarSelecionadoAsync(item);

        await action.Should().NotThrowAsync();
        viewModel.IsBusy.Should().BeFalse();
        toast.Requests.Should().Contain(request => request.Kind == ToastKind.Error && request.Message.Contains("configuração inválida"));
    }

    [Fact]
    public void AtualizarUrl_ComSelecao_ChamaServicoEMostraSucesso()
    {
        var toast = new RecordingToastService();
        var services = new Mock<IWebServicesService>();
        services.Setup(service => service.AtualizarUrl(15)).Returns(AtualizarUrlResult.Succeeded(2));
        var viewModel = CreateViewModel(services.Object, toast: toast);
        var item = new WebServiceItem { IdClientesSistema = 15 };

        viewModel.AtualizarUrl(item);

        services.Verify(service => service.AtualizarUrl(15), Times.Once);
        toast.Requests.Should().Contain(request => request.Kind == ToastKind.Success);
    }

    [Fact]
    public void AtualizarUrl_ComConfiguracaoIncompleta_MostraWarning()
    {
        var toast = new RecordingToastService();
        var services = new Mock<IWebServicesService>();
        services
            .Setup(service => service.AtualizarUrl(15))
            .Returns(AtualizarUrlResult.Warning("Configure servidor, banco de dados e usuário antes de atualizar a URL."));
        var viewModel = CreateViewModel(services.Object, toast: toast);
        var item = new WebServiceItem { IdClientesSistema = 15 };

        viewModel.AtualizarUrl(item);

        toast.Requests.Should().ContainSingle(request =>
            request.Kind == ToastKind.Warning &&
            request.Message.Contains("Configure servidor"));
        toast.Requests.Should().NotContain(request => request.Kind == ToastKind.Success);
    }

    [Fact]
    public void AtualizarUrl_QuandoServicoFalha_MostraError()
    {
        var toast = new RecordingToastService();
        var services = new Mock<IWebServicesService>();
        services
            .Setup(service => service.AtualizarUrl(15))
            .Throws(new InvalidOperationException("falha de conexão"));
        var viewModel = CreateViewModel(services.Object, toast: toast);
        var item = new WebServiceItem { IdClientesSistema = 15 };

        viewModel.AtualizarUrl(item);

        toast.Requests.Should().ContainSingle(request =>
            request.Kind == ToastKind.Error &&
            request.Message.Contains("falha de conexão"));
    }

    private static WebServicesViewModel CreateViewModel(
        IWebServicesService? services = null,
        IWebServiceExecutionService? executionService = null,
        RecordingToastService? toast = null)
    {
        var servicesMock = new Mock<IWebServicesService>();
        servicesMock.Setup(service => service.ListarMaisAcessados()).Returns(Array.Empty<WebServiceItem>());
        servicesMock.Setup(service => service.ListarUfs()).Returns(Array.Empty<string>());

        return new WebServicesViewModel(
            services ?? servicesMock.Object,
            executionService ?? Mock.Of<IWebServiceExecutionService>(),
            toast ?? new RecordingToastService());
    }
}
