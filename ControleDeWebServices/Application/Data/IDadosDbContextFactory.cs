namespace ControleDeWebServices.Application.Data
{
    public interface IDadosDbContextFactory
    {
        DadosDBContext Create();
    }
}
