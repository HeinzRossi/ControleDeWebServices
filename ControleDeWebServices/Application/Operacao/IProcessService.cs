namespace ControleDeWebServices.Application.Operacao
{
    public interface IProcessService
    {
        void Start(string fileName);
        ProcessKillResult KillByPrefixes(params string[] prefixes);
    }
}
