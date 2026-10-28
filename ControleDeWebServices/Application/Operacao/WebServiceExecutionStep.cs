namespace ControleDeWebServices.Application.Operacao
{
    public sealed class WebServiceExecutionStep
    {
        public WebServiceExecutionStep(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
