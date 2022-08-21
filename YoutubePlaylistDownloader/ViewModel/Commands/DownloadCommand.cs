using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubePlaylistDownloader.Model;

namespace YoutubePlaylistDownloader.ViewModel.Commands
{
    public class DownloadCommand : ICommand
    {
        YouTubePlaylistDownloaderVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            ObservableCollection<VideoDisplayExternal> selectedVideoDisplayExternals = parameter as ObservableCollection<VideoDisplayExternal>;

            if (selectedVideoDisplayExternals == null)
            {
                return false;
            }
            else
            {
                if (selectedVideoDisplayExternals.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public void Execute(object? parameter)
        {
            VM.DownloadVideos();   
        }

        public DownloadCommand(YouTubePlaylistDownloaderVM vm)
        {
            VM = vm;
        }
    }
}
