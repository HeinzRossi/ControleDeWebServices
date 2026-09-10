using ControleDeWebServices.Application.Data;

namespace ControleDeWebServices.Infrastructure.Data
{
    public sealed class DadosDbContextFactory : IDadosDbContextFactory
    {
        public DadosDBContext Create()
        {
            return new DadosDBContext();
        }
    }
}
