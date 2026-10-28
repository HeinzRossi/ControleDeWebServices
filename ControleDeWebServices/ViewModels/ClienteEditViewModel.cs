using CommunityToolkit.Mvvm.ComponentModel;
using ControleDeWebServices.Application.Clientes;
using ControleDeWebServices.Diversos;
using System.Collections.Generic;
using System.Linq;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ClienteEditViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int? idCliente;

        [ObservableProperty]
        private string codigoControle;

        [ObservableProperty]
        private string nomeCliente;

        [ObservableProperty]
        private string uf;

        public IReadOnlyList<string> Estados { get; } = Funcoes.GetEnumValues().Select(estado => estado.ToString()).ToList();

        public bool IsNew => !IdCliente.HasValue;

        public string Titulo => IsNew ? "Novo cliente" : "Editar cliente";

        public static ClienteEditViewModel Novo()
        {
            return FromEditor(new ClienteEditor());
        }

        public static ClienteEditViewModel FromEditor(ClienteEditor editor)
        {
            return new ClienteEditViewModel
            {
                IdCliente = editor.IdCliente,
                CodigoControle = editor.CodigoControle == 0 ? string.Empty : editor.CodigoControle.ToString(),
                NomeCliente = editor.NomeCliente,
                Uf = editor.Uf
            };
        }

        public ClienteEditor ToEditor()
        {
            return new ClienteEditor
            {
                IdCliente = IdCliente,
                CodigoControle = int.TryParse(CodigoControle, out var codigoControle) ? codigoControle : 0,
                NomeCliente = NomeCliente?.Trim(),
                Uf = Uf
            };
        }
    }
}
