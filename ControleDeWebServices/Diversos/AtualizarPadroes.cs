using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Interface;
using ControleDeWebServices.Modelo;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ControleDeWebServices.Diversos
{
    public sealed class AtualizarPadroes : IAtualizarPadroes, IAtualizarPadroesService
    {
        private readonly ILogger<AtualizarPadroes> logger;

        public AtualizarPadroes()
            : this(NullLogger<AtualizarPadroes>.Instance)
        {
        }

        public AtualizarPadroes(ILogger<AtualizarPadroes> logger)
        {
            this.logger = logger;
        }

        public IAtualizarPadroes AtualizarParametros(ClienteSistemas pclienteSistemas, DadosDBContext pContexto)
        {
            var parametros = pContexto.ParametrosSistema
                .Where(parametro => parametro.IdClientesSistema == pclienteSistemas.IdClientesSistema)
                .Select(parametro => new ParametroEditor
                {
                    Secao = parametro.Secao,
                    Parametro = parametro.Parametro,
                    Valor = parametro.Valor
                })
                .ToList();

            AtualizarParametros(pclienteSistemas, parametros);
            return this;
        }

        public IAtualizarPadroes AtualizarURL(ClienteSistemas pclienteSistemas)
        {
            AtualizarUrl(pclienteSistemas);
            return this;
        }

        public void AtualizarParametros(ClienteSistemas clienteSistemas, IEnumerable<ParametroEditor> parametros)
        {
            logger.LogInformation("Atualizando parâmetros externos para vínculo {IdClientesSistema}", clienteSistemas.IdClientesSistema);

            switch ((TipoConexao)clienteSistemas.TipoConexao)
            {
                case TipoConexao.SQLSERVER:
                case TipoConexao.MSSQL:
                    AtualizarParametrosSqlServer(clienteSistemas, parametros);
                    break;
                case TipoConexao.FIREBIRD:
                    AtualizarParametrosFirebird(clienteSistemas, parametros);
                    break;
                case TipoConexao.POSTGRESQL:
                    AtualizarParametrosPostgreSql(clienteSistemas, parametros);
                    break;
            }
        }

        public AtualizarUrlResult AtualizarUrl(ClienteSistemas clienteSistemas)
        {
            var validation = ValidarConfiguracaoAtualizacaoUrl(clienteSistemas);
            if (validation != null)
            {
                return validation;
            }

            logger.LogInformation("Atualizando URL externa para vínculo {IdClientesSistema}", clienteSistemas.IdClientesSistema);

            int rowsAffected;
            switch ((TipoConexao)clienteSistemas.TipoConexao)
            {
                case TipoConexao.SQLSERVER:
                case TipoConexao.MSSQL:
                    rowsAffected = AtualizarUrlSqlServer(clienteSistemas);
                    break;
                case TipoConexao.FIREBIRD:
                    rowsAffected = AtualizarUrlFirebird(clienteSistemas);
                    break;
                case TipoConexao.POSTGRESQL:
                    rowsAffected = AtualizarUrlPostgreSql(clienteSistemas);
                    break;
                default:
                    return AtualizarUrlResult.Warning("Tipo de conexão não suportado para atualizar a URL.");
            }

            return rowsAffected > 0
                ? AtualizarUrlResult.Succeeded(rowsAffected)
                : AtualizarUrlResult.Warning("Nenhuma URL foi encontrada para atualizar.", rowsAffected);
        }

        private static void AtualizarParametrosSqlServer(ClienteSistemas clienteSistemas, IEnumerable<ParametroEditor> parametros)
        {
            using (var connection = new SqlConnection(BuildSqlServerConnectionString(clienteSistemas)))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "update ini set VALOR = @valor where NOME = @nome and SECAO = @secao";
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@valor", SqlDbType.VarChar, 300);
                command.Parameters.Add("@nome", SqlDbType.VarChar, 300);
                command.Parameters.Add("@secao", SqlDbType.VarChar, 300);

                connection.Open();
                foreach (var parametro in parametros)
                {
                    command.Parameters["@valor"].Value = parametro.Valor ?? string.Empty;
                    command.Parameters["@nome"].Value = parametro.Parametro ?? string.Empty;
                    command.Parameters["@secao"].Value = parametro.Secao ?? string.Empty;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void AtualizarParametrosFirebird(ClienteSistemas clienteSistemas, IEnumerable<ParametroEditor> parametros)
        {
            using (var connection = new FbConnection(BuildFirebirdConnectionString(clienteSistemas)))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "update ini set VALOR = @valor where NOME = @nome and SECAO = @secao";
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@valor", FbDbType.VarChar);
                command.Parameters.Add("@nome", FbDbType.VarChar);
                command.Parameters.Add("@secao", FbDbType.VarChar);

                connection.Open();
                foreach (var parametro in parametros)
                {
                    command.Parameters["@valor"].Value = parametro.Valor ?? string.Empty;
                    command.Parameters["@nome"].Value = parametro.Parametro ?? string.Empty;
                    command.Parameters["@secao"].Value = parametro.Secao ?? string.Empty;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void AtualizarParametrosPostgreSql(ClienteSistemas clienteSistemas, IEnumerable<ParametroEditor> parametros)
        {
            using (var connection = new NpgsqlConnection(BuildPostgreSqlConnectionString(clienteSistemas)))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "update ini set VALOR = @valor where NOME = @nome and SECAO = @secao";
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@valor", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters.Add("@nome", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters.Add("@secao", NpgsqlTypes.NpgsqlDbType.Varchar);

                connection.Open();
                foreach (var parametro in parametros)
                {
                    command.Parameters["@valor"].Value = parametro.Valor ?? string.Empty;
                    command.Parameters["@nome"].Value = parametro.Parametro ?? string.Empty;
                    command.Parameters["@secao"].Value = parametro.Secao ?? string.Empty;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static int AtualizarUrlSqlServer(ClienteSistemas clienteSistemas)
        {
            const string sql = "update ini set VALOR = 'http://127.0.0.1' + SUBSTRING(VALOR, CHARINDEX('/Engegraph', VALOR, 1) - 5, LEN(VALOR)) where NOME like '%URL%' and SECAO <> 'QR_CODE'";

            using (var connection = new SqlConnection(BuildSqlServerConnectionString(clienteSistemas)))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        private static int AtualizarUrlFirebird(ClienteSistemas clienteSistemas)
        {
            const string sql = "update ini set VALOR = 'http://127.0.0.1' || substring(VALOR from (position('/Engegraph' in VALOR) - 5) for char_length(VALOR) - (position('/Engegraph' in VALOR) - 5)) where NOME like '%URL%' and SECAO <> 'QR_CODE' and (position('/Engegraph' in VALOR) - 5) >= 0";

            using (var connection = new FbConnection(BuildFirebirdConnectionString(clienteSistemas)))
            using (var command = new FbCommand(sql, connection))
            {
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        private static int AtualizarUrlPostgreSql(ClienteSistemas clienteSistemas)
        {
            const string sql = "update ini set VALOR = 'http://127.0.0.1' || substring(VALOR, strpos(VALOR, '/Engegraph') - 5, length(VALOR)) where NOME like '%URL%' and SECAO <> 'QR_CODE'";

            using (var connection = new NpgsqlConnection(BuildPostgreSqlConnectionString(clienteSistemas)))
            using (var command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        private static AtualizarUrlResult ValidarConfiguracaoAtualizacaoUrl(ClienteSistemas clienteSistemas)
        {
            if (clienteSistemas == null)
            {
                return AtualizarUrlResult.Warning("WebService não encontrado para atualizar a URL.");
            }

            if (!Enum.IsDefined(typeof(TipoConexao), clienteSistemas.TipoConexao))
            {
                return AtualizarUrlResult.Warning("Tipo de conexão não suportado para atualizar a URL.");
            }

            var tipoConexao = (TipoConexao)clienteSistemas.TipoConexao;
            if (tipoConexao != TipoConexao.SQLSERVER &&
                tipoConexao != TipoConexao.MSSQL &&
                tipoConexao != TipoConexao.FIREBIRD &&
                tipoConexao != TipoConexao.POSTGRESQL)
            {
                return AtualizarUrlResult.Warning("Tipo de conexão não suportado para atualizar a URL.");
            }

            if (string.IsNullOrWhiteSpace(clienteSistemas.Servidor) ||
                string.IsNullOrWhiteSpace(clienteSistemas.DataBase) ||
                string.IsNullOrWhiteSpace(clienteSistemas.Usuario))
            {
                return AtualizarUrlResult.Warning("Configure servidor, banco de dados e usuário antes de atualizar a URL.");
            }

            if ((tipoConexao == TipoConexao.FIREBIRD || tipoConexao == TipoConexao.POSTGRESQL) &&
                clienteSistemas.Porta <= 0)
            {
                return AtualizarUrlResult.Warning("Configure a porta do banco de dados antes de atualizar a URL.");
            }

            return null;
        }

        private static string BuildSqlServerConnectionString(ClienteSistemas clienteSistemas)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = clienteSistemas.Servidor,
                InitialCatalog = clienteSistemas.DataBase,
                UserID = clienteSistemas.Usuario,
                Password = clienteSistemas.Senha,
                TrustServerCertificate = true
            };

            return builder.ConnectionString;
        }

        private static string BuildFirebirdConnectionString(ClienteSistemas clienteSistemas)
        {
            var builder = new FbConnectionStringBuilder
            {
                UserID = clienteSistemas.Usuario,
                Password = clienteSistemas.Senha,
                Database = clienteSistemas.DataBase,
                DataSource = clienteSistemas.Servidor,
                Port = clienteSistemas.Porta
            };

            return builder.ConnectionString;
        }

        private static string BuildPostgreSqlConnectionString(ClienteSistemas clienteSistemas)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = clienteSistemas.Servidor,
                Port = clienteSistemas.Porta,
                Username = clienteSistemas.Usuario,
                Password = clienteSistemas.Senha,
                Database = clienteSistemas.DataBase
            };

            return builder.ConnectionString;
        }
    }
}
