using ControleDeWebServices.Migrations;
using ControleDeWebServices.Modelo;
using PeanutButter.INI;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;

namespace ControleDeWebServices
{
    public class DadosDBContext : DbContext
    {
        public DadosDBContext()
        {
            try
            {
                INIFile file = new INIFile(Directory.GetCurrentDirectory() + "\\Config.ini");
                Config config = new Config()
                {
                    Servidor = file.GetValue("CONEXAO", "Servidor"),
                    DataBase = file.GetValue("CONEXAO", "DataBase"),
                    OSAuthent = file.GetValue("CONEXAO", "OSAuthent"),
                    Usuario = file.GetValue("CONEXAO", "Usuario"),
                    Senha = file.GetValue("CONEXAO", "Senha")
                };

                if (config.OSAuthent == "FALSE")
                    Database.Connection.ConnectionString = "Data Source=" + config.Servidor + ";Initial Catalog=" + config.DataBase + ";User ID=" + config.Usuario + ";Password=" + config.Senha;
                else
                    Database.Connection.ConnectionString = "Data Source=" + config.Servidor + ";Initial Catalog=" + config.DataBase + ";Integrated Security=True";
                Database.SetInitializer<DadosDBContext>(new MigrateDatabaseToLatestVersion<DadosDBContext, Configuration>());
                Database.Initialize(false);
            }
            catch (Exception E)
            {
                Trace.TraceError("Não foi possível inicializar a conexão principal do sistema. {0}", E);
                throw new InvalidOperationException("Não foi possível inicializar a conexão principal do sistema.", E);
            }
        }
        public DbSet<Sistemas> Sistemas { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Servicos> Servicos { get; set; }
        public DbSet<ClienteServicos> ClienteServicos { get; set; }
        public DbSet<ClienteSistemas> ClienteSistemas { get; set; }
        public DbSet<ParametrosSistema> ParametrosSistema { get; set; }
        public DbSet<Secao> Secao { get; set; }
    }
}
