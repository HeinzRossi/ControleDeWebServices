using ControleDeWebServices.Interface;
using ControleDeWebServices.Modelo;
using FirebirdSql.Data.FirebirdClient;
using System;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using Npgsql;

namespace ControleDeWebServices.Diversos
{
    class AtualizarPadroes : IAtualizarPadroes
    {
        public IAtualizarPadroes AtualizarParametros(ClienteSistemas pclienteSistemas, DadosDBContext pContexto)
        {
            try
            {
                var parametros = (from parametrosSistema in pContexto.ParametrosSistema
                                                where parametrosSistema.IdClientesSistema == pclienteSistemas.IdClientesSistema
                                                select new{ parametrosSistema.Secao, parametrosSistema.Parametro, parametrosSistema.Valor });

                switch ((TipoConexao)pclienteSistemas.TipoConexao)
                {
                    case TipoConexao.SQLSERVER:
                        {
                            SqlConnection sqlconnection = new SqlConnection();
                            SqlCommand sqlCommand = new SqlCommand();
                            SqlDataReader reader;

                            sqlconnection.ConnectionString = string.Concat("Data Source=", pclienteSistemas.Servidor,
                                                                            ";Initial Catalog=", pclienteSistemas.DataBase,
                                                                            ";User ID=", pclienteSistemas.Usuario,
                                                                            ";Password=", pclienteSistemas.Senha);

                            foreach (var parametrosSistema in parametros)
                            {
                                sqlCommand.CommandText = string.Concat("update ini set VALOR = '", parametrosSistema.Valor, "' where NOME = '", parametrosSistema.Parametro, "' And SECAO = '", parametrosSistema.Secao, "'");

                                sqlCommand.CommandType = System.Data.CommandType.Text;
                                sqlCommand.Connection = sqlconnection;
                                sqlconnection.Open();

                                reader = sqlCommand.ExecuteReader();
                                sqlconnection.Close();
                            }
                            break;
                        }
                    case TipoConexao.MSSQL:
                        {
                            SqlConnection sqlconnection = new SqlConnection();
                            SqlCommand sqlCommand = new SqlCommand();
                            SqlDataReader reader;

                            sqlconnection.ConnectionString = string.Concat("Data Source=", pclienteSistemas.Servidor,
                                                                            ";Initial Catalog=", pclienteSistemas.DataBase,
                                                                            ";User ID=", pclienteSistemas.Usuario,
                                                                            ";Password=", pclienteSistemas.Senha);

                            foreach (var parametrosSistema in parametros)
                            {
                                sqlCommand.CommandText = string.Concat("update ini set VALOR = '", parametrosSistema.Valor, "' where NOME = '", parametrosSistema.Parametro, "' And SECAO = '", parametrosSistema.Secao, "'");

                                sqlCommand.CommandType = System.Data.CommandType.Text;
                                sqlCommand.Connection = sqlconnection;
                                sqlconnection.Open();

                                reader = sqlCommand.ExecuteReader();
                                sqlconnection.Close();
                            }
                            break;
                        }
                    case TipoConexao.FIREBIRD:
                        {
                            FbConnection fbConnection = new FbConnection();
                            FbCommand fbCommand = new FbCommand();

                            fbConnection.ConnectionString = string.Concat("User=", pclienteSistemas.Usuario,
                                                                            ";Password=", pclienteSistemas.Senha,
                                                                            ";DataBase=", pclienteSistemas.DataBase,
                                                                            ";Data Source=", pclienteSistemas.Servidor,
                                                                            ";Port=", pclienteSistemas.Porta);

                            foreach (var parametrosSistema in parametros)
                            {
                                fbCommand.CommandText = string.Concat("update ini set VALOR = '", parametrosSistema.Valor, "' where NOME = '", parametrosSistema.Parametro, "' And SECAO = '", parametrosSistema.Secao, "'");
                                fbCommand.CommandType = System.Data.CommandType.Text;
                                fbCommand.Connection = fbConnection;
                                fbConnection.Open();
                                fbCommand.ExecuteNonQuery();
                                fbConnection.Close();
                            }
                            break;
                        }
                    case TipoConexao.POSTGRESQL:
                        {
                            string connString = string.Concat("Host=", pclienteSistemas.Servidor,
                                                                ";Port=", pclienteSistemas.Porta,
                                                                ";Username=", pclienteSistemas.Usuario,
                                                                ";Password=", pclienteSistemas.Senha,
                                                                ";Database=", pclienteSistemas.DataBase);

                            using (var conn = new NpgsqlConnection(connString))
                            {
                                conn.Open();

                                foreach (var parametrosSistema in parametros)
                                {
                                    string sql = string.Concat("update ini set VALOR = '", parametrosSistema.Valor, "' where NOME = '", parametrosSistema.Parametro, "' And SECAO = '", parametrosSistema.Secao, "'");
                                    using (var cmd = new NpgsqlCommand(sql, conn))
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Erro na atualização dos Dados!" + "\n" + E.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return this;
        }

        public IAtualizarPadroes AtualizarURL(ClienteSistemas pclienteSistemas)
        {
            try
            {

                switch ((TipoConexao)pclienteSistemas.TipoConexao)
                {
                    case TipoConexao.SQLSERVER: 
                        {
                            SqlConnection sqlconnection = new SqlConnection();
                            SqlCommand sqlCommand = new SqlCommand();
                            SqlDataReader reader;

                            sqlconnection.ConnectionString = string.Concat("Data Source=", pclienteSistemas.Servidor,
                                                                           ";Initial Catalog=", pclienteSistemas.DataBase,
                                                                           ";User ID=", pclienteSistemas.Usuario,
                                                                           ";Password=", pclienteSistemas.Senha);
                            sqlCommand.CommandText = "update ini set VALOR = 'http://127.0.0.1'+SUBSTRING(VALOR,CHARINDEX('/Engegraph',valor,1)-5,LEN(valor)) where NOME like '%URL%' AND SECAO <> 'QR_CODE'";
                            sqlCommand.CommandType = System.Data.CommandType.Text;
                            sqlCommand.Connection = sqlconnection;
                            sqlconnection.Open();

                            reader = sqlCommand.ExecuteReader();
                            sqlconnection.Close();
                            break;
                        }
                    case TipoConexao.MSSQL:
                        {
                            SqlConnection sqlconnection = new SqlConnection();
                            SqlCommand sqlCommand = new SqlCommand();
                            SqlDataReader reader;

                            sqlconnection.ConnectionString = string.Concat("Data Source=", pclienteSistemas.Servidor,
                                                                           ";Initial Catalog=", pclienteSistemas.DataBase,
                                                                           ";User ID=", pclienteSistemas.Usuario,
                                                                           ";Password=", pclienteSistemas.Senha);
                            sqlCommand.CommandText = "update ini set VALOR = 'http://127.0.0.1'+SUBSTRING(VALOR,CHARINDEX('/Engegraph',valor,1)-5,LEN(valor)) where NOME like '%URL%' AND SECAO <> 'QR_CODE'";
                            sqlCommand.CommandType = System.Data.CommandType.Text;
                            sqlCommand.Connection = sqlconnection;
                            sqlconnection.Open();

                            reader = sqlCommand.ExecuteReader();
                            sqlconnection.Close();
                            break;
                        }
                    case TipoConexao.FIREBIRD:
                        {
                            FbConnection fbConnection = new FbConnection();
                            FbCommand fbCommand = new FbCommand();

                            fbConnection.ConnectionString = string.Concat("User=", pclienteSistemas.Usuario,
                                                                          ";Password=", pclienteSistemas.Senha,
                                                                          ";DataBase=", pclienteSistemas.DataBase,
                                                                          ";DataSource=", pclienteSistemas.Servidor,
                                                                          ";Port=", pclienteSistemas.Porta);
                            fbCommand.CommandText = "UPDATE ini SET valor = 'http://127.0.0.1' || SUBSTRING(VALOR from (POSITION('/Engegraph' in VALOR)-5) FOR CHAR_LENGTH(VALOR) - (POSITION('/Engegraph' in VALOR)-5)) where NOME like '%URL%' AND SECAO<> 'QR_CODE' AND(POSITION('/Engegraph' in VALOR) - 5) >= 0; ";
                            fbCommand.CommandType = System.Data.CommandType.Text;
                            fbCommand.Connection = fbConnection;
                            fbConnection.Open();
                            fbCommand.ExecuteNonQuery();
                            fbConnection.Close();
                            break;

                        }
                    case TipoConexao.POSTGRESQL: 
                        { 
                            string connString = string.Concat("Host=", pclienteSistemas.Servidor,
                                                              ";Port=", pclienteSistemas.Porta,
                                                              ";Username=", pclienteSistemas.Usuario,
                                                              ";Password=", pclienteSistemas.Senha,
                                                              ";Database=",pclienteSistemas.DataBase);

                            using (var conn = new NpgsqlConnection(connString))
                            {
                                conn.Open();

                                string sql = "update ini set VALOR = 'http://127.0.0.1'||SUBSTRING(VALOR,strpos(VALOR,'/Engegraph')-5,LENGTH(valor)) where NOME like '%URL%' AND SECAO <> 'QR_CODE'";
                                using (var cmd = new NpgsqlCommand(sql, conn))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            break;
                        }
                }
                MessageBox.Show("URL atualizada com Sucesso!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception E)
            {
                MessageBox.Show("Erro na atualização dos Dados!" + "\n" + E.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return this;
        }
    }
}
