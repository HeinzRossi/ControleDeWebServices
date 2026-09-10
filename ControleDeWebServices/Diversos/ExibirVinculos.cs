using System.Windows;

namespace ControleDeWebServices.Diversos
{
    public class ExibirVinculos
    {
        public Visibility Clientes { get; set; }
        public Visibility Sistemas { get; set; }
        public Visibility ServicosDisponiveis { get; set; }
        public Visibility ServicosVinculados { get; set; }

        public ExibirVinculos()
        {
            Clientes = Visibility.Hidden;
            Sistemas = Visibility.Hidden;
            ServicosDisponiveis = Visibility.Hidden;
            ServicosVinculados = Visibility.Hidden;
        }
    }
}
