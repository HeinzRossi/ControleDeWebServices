using CommunityToolkit.Mvvm.ComponentModel;
using ControleDeWebServices.Application.Cadastros;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class SecaoEditViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int? idSecao;

        [ObservableProperty]
        private string nomeSecao;

        public bool IsNew => !IdSecao.HasValue;

        public string Titulo => IsNew ? "Nova secao" : "Editar secao";

        public static SecaoEditViewModel Novo()
        {
            return FromEditor(new SecaoEditor());
        }

        public static SecaoEditViewModel FromEditor(SecaoEditor editor)
        {
            return new SecaoEditViewModel
            {
                IdSecao = editor.IdSecao,
                NomeSecao = editor.NomeSecao
            };
        }

        public SecaoEditor ToEditor()
        {
            return new SecaoEditor
            {
                IdSecao = IdSecao,
                NomeSecao = NomeSecao?.Trim()
            };
        }
    }
}
