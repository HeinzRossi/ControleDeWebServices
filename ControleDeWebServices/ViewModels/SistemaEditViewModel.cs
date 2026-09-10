using CommunityToolkit.Mvvm.ComponentModel;
using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Diversos;
using System.Collections.Generic;
using System.Linq;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class SistemaEditViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int? idSistemas;

        [ObservableProperty]
        private string nomeSistema;

        [ObservableProperty]
        private string uf;

        public IReadOnlyList<string> Estados { get; } = Funcoes.GetEnumValues().Select(estado => estado.ToString()).ToList();

        public bool IsNew => !IdSistemas.HasValue;

        public string Titulo => IsNew ? "Novo sistema" : "Editar sistema";

        public static SistemaEditViewModel Novo()
        {
            return FromEditor(new SistemaEditor());
        }

        public static SistemaEditViewModel FromEditor(SistemaEditor editor)
        {
            return new SistemaEditViewModel
            {
                IdSistemas = editor.IdSistemas,
                NomeSistema = editor.NomeSistema,
                Uf = editor.Uf
            };
        }

        public SistemaEditor ToEditor()
        {
            return new SistemaEditor
            {
                IdSistemas = IdSistemas,
                NomeSistema = NomeSistema?.Trim(),
                Uf = Uf
            };
        }
    }
}
