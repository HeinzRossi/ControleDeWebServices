using ControleDeWebServices.Application.Platform;
using Microsoft.Win32;

namespace ControleDeWebServices.Presentation.Platform
{
    public sealed class FilePickerService : IFilePickerService
    {
        public string PickFile(string title)
        {
            var dialog = new OpenFileDialog
            {
                Title = title
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}
