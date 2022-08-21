using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubePlaylistDownloader.ViewModel.Helpers;

namespace YoutubePlaylistDownloader.ViewModel.Commands
{
    public class SearchCommand : ICommand
    {
        public YouTubePlaylistDownloaderVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            string SearchText = parameter as string;

            if (!YouTubeHelper.IsLinkAPlaylist(SearchText))
                return false;

            return true;
        }

        public void Execute(object? parameter)
        {
            VM.SearchUrl();
        }

        public SearchCommand(YouTubePlaylistDownloaderVM vm)
        {
            VM = vm;
        }
    }
}
