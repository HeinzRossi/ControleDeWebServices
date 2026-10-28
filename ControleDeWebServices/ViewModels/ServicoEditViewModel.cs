using CommunityToolkit.Mvvm.ComponentModel;
using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Diversos;
using System.Collections.Generic;
using System.Linq;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ServicoEditViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int? idServicos;

        [ObservableProperty]
        private string nomeServico;

        [ObservableProperty]
        private string uf;

        public IReadOnlyList<string> Estados { get; } = Funcoes.GetEnumValues().Select(estado => estado.ToString()).ToList();

        public bool IsNew => !IdServicos.HasValue;

        public string Titulo => IsNew ? "Novo servico" : "Editar servico";

        public static ServicoEditViewModel Novo()
        {
            return FromEditor(new ServicoEditor());
        }

        public static ServicoEditViewModel FromEditor(ServicoEditor editor)
        {
            return new ServicoEditViewModel
            {
                IdServicos = editor.IdServicos,
                NomeServico = editor.NomeServico,
                Uf = editor.Uf
            };
        }

        public ServicoEditor ToEditor()
        {
            return new ServicoEditor
            {
                IdServicos = IdServicos,
                NomeServico = NomeServico?.Trim(),
                Uf = Uf
            };
        }
    }
}
