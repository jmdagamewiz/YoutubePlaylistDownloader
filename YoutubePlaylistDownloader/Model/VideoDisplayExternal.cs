using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubePlaylistDownloader.Model
{
    public class VideoDisplayExternal : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsCheckBoxChecked { get; set; }
        public Video Video { get; set; }
        public List<YoutubeExplode.Videos.Streams.MuxedStreamInfo> Streams { get; set; }
        public YoutubeExplode.Videos.Streams.MuxedStreamInfo SelectedStream { get; set; }

        private double downloadProgress;

        public double DownloadProgress
        {
            get { return downloadProgress; }
            set
            {
                downloadProgress = value;
                OnPropertyChanged(nameof(DownloadProgress));
            }
        }
    }
}
