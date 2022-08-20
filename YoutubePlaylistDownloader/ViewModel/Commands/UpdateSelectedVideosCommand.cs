using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubePlaylistDownloader.Model;

namespace YoutubePlaylistDownloader.ViewModel.Commands
{
    public class UpdateSelectedVideosCommand : ICommand
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
            var values = parameter as object[];
            Video video = (Video)values[0];
            bool isChecked = (bool)values[1];
            
            if (isChecked)
            {
                VM.AddVideoToSelectedVideos(video);
            }
            else
            {
                VM.RemoveVideoFromSelectedVideos(video);
            }
        }

        public UpdateSelectedVideosCommand(YouTubePlaylistDownloaderVM vm)
        {
            VM = vm;
        }
    }
}
