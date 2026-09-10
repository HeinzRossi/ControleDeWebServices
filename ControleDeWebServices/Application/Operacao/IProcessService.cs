namespace ControleDeWebServices.Application.Operacao
{
    public interface IProcessService
    {
        void Start(string fileName);
        int KillByPrefixes(params string[] prefixes);
    }
}
