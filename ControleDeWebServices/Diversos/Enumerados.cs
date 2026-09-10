using System.ComponentModel;

namespace ControleDeWebServices.Diversos
{
    public enum TipoConexao : int
    {
        [Description("Firebird")]
        FIREBIRD = 56,
        [Description("Sql Server")]
        MSSQL = 2,
        [Description("Oracle")]
        ORACLE = 0,
        [Description("PostgreSQL")]
        POSTGRESQL = 1,
        [Description("SQLServer")]
        SQLSERVER = 57
    }

    public enum Acao : int
    {
        Inserir,
        Editar,
        Excluir
    }

    public enum StatusServico : int
    {
        Inalterado,
        Inserir,
        Excluir
    }

    public enum Estados : int
    {
        AC, AL, AM,
        AP, BA, CE,
        DF, ES, GO,
        MA, MG, MS,
        MT, PA, PB,
        PE, PI, PR,
        RJ, RN, RS,
        RO, RR, SC,
        SE, SP, TO
    }

    public enum TipoServico : int
    {
        Custas,
        Selos,
        NFSe,
        Launcher,
        SelosMG,
        Caixa,
        ServicosRTD,
        SelosPA,
        SelosTO,
        SelosMA,
        ServicoRC,
        SAECNacional
    }

}
