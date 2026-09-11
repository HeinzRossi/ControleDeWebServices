namespace ControleDeWebServices.Application.Configuracoes
{
    public sealed class AtualizarUrlResult
    {
        private AtualizarUrlResult(bool success, string message, int rowsAffected)
        {
            Success = success;
            Message = message;
            RowsAffected = rowsAffected;
        }

        public bool Success { get; }
        public string Message { get; }
        public int RowsAffected { get; }

        public static AtualizarUrlResult Succeeded(int rowsAffected)
        {
            return new AtualizarUrlResult(true, "URL atualizada com sucesso.", rowsAffected);
        }

        public static AtualizarUrlResult Warning(string message, int rowsAffected = 0)
        {
            return new AtualizarUrlResult(false, message, rowsAffected);
        }
    }
}
