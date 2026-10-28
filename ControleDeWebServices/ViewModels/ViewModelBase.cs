using CommunityToolkit.Mvvm.ComponentModel;

namespace ControleDeWebServices.ViewModels
{
    public abstract partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string statusMessage;
    }
}
