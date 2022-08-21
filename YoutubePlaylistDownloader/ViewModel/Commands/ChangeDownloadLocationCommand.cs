using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YoutubePlaylistDownloader.ViewModel.Commands
{
    public class ChangeDownloadLocationCommand : ICommand
    {
        public YouTubePlaylistDownloaderVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.ChangeDownloadFolderLocation();
        }

        public ChangeDownloadLocationCommand(YouTubePlaylistDownloaderVM vm)
        {
            VM = vm;
        }
    }
}
