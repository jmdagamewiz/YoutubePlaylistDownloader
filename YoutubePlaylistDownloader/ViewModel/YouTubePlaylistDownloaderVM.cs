using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubePlaylistDownloader.Model;
using YoutubePlaylistDownloader.ViewModel.Commands;
using YoutubePlaylistDownloader.ViewModel.Helpers;

namespace YoutubePlaylistDownloader.ViewModel
{
    public class YouTubePlaylistDownloaderVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SearchCommand SearchCommand { get; set; }
        public UpdateSelectedVideosCommand UpdateSelectedVideosCommand { get; set; }    
        public DownloadCommand DownloadCommand { get; set; }
        public ObservableCollection<Video> Videos { get; set; }
        public ObservableCollection<Video> SelectedVideos { get; set; }
        public ObservableCollection<bool> CheckBoxesIsChecked { get; set; }
        public ObservableCollection<VideoDisplayExternal> VideoDisplayExternals { get; set; }

        private Playlist selectedPlaylist;
        public Playlist SelectedPlaylist
        {
            get { return selectedPlaylist; }
            set
            {
                selectedPlaylist = value;
                OnPropertyChanged("SelectedPlaylist");
            }   
        }

        private string searchTextBoxString;

        public string SearchTextBoxString
        {
            get { return searchTextBoxString; }
            set
            {
                searchTextBoxString = value;
                OnPropertyChanged("SearchTextBoxString");
            }
        }
            
        public void AddVideoToSelectedVideos(Video video)
        {
            SelectedVideos.Add(video);
        }

        public void RemoveVideoFromSelectedVideos(Video video)
        {
            SelectedVideos.Remove(video);
        }

        public async void SearchUrl()
        {
            // clears information about previous query
            string InputUrl = SearchTextBoxString;
            SearchTextBoxString = "";
            Videos.Clear();
            SelectedVideos.Clear();
            SelectedPlaylist = null;

            Playlist newPlaylist = await YouTubeHelper.GetPlaylistInformation(InputUrl);
            var videos = await YouTubeHelper.GetPlaylistVideos(InputUrl);

            // updates NumOfVideos property
            newPlaylist.NumOfVideos = videos.Count;
            SelectedPlaylist = newPlaylist;

            if (videos.Count > 0 || videos != null)
            {
                foreach (Video video in videos)
                {
                    VideoDisplayExternals.Add(new VideoDisplayExternal()
                    {
                        IsCheckBoxChecked = true,
                        Video = video
                    });

                    SelectedVideos.Add(video);
                }
            }
        }

        public async void DownloadVideos()
        {
            // get user's Downloads folder and creates playlist folder in it
            string folderPath = FileHelper.GetDownloadsFolderPath();
            string playlistFolderPath = FileHelper.CreatePlaylistFolder(folderPath, SelectedPlaylist);

            var youtube = new YoutubeClient();

            foreach (VideoDisplayExternal videoDisplayExternal in VideoDisplayExternals)
            {
                Video video = videoDisplayExternal.Video;

                // get streams
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Url);
                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

                string fileName = $"{video.Title}.{streamInfo.Container}";

                if (fileName.Contains("|"))
                {
                    fileName = fileName.Replace("|", "-");
                }

                string downloadPath = System.IO.Path.Combine(playlistFolderPath, fileName);

                // download video
                await youtube.Videos.Streams.DownloadAsync(streamInfo, downloadPath);
            }
        }

        public YouTubePlaylistDownloaderVM()
        {
            UpdateSelectedVideosCommand = new UpdateSelectedVideosCommand(this);
            DownloadCommand = new DownloadCommand(this);
            SearchCommand = new SearchCommand(this);

            Videos = new ObservableCollection<Video>();
            SelectedVideos = new ObservableCollection<Video>();
            CheckBoxesIsChecked = new ObservableCollection<bool>();
            VideoDisplayExternals = new ObservableCollection<VideoDisplayExternal>();

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedPlaylist = new Playlist()
                {
                    Title = "Ben and Ben playlist",
                    Author = "J-ann Espinosa",
                    Url = "https://www.youtube.com/watch?v=daZoiEGyFgg&list=PLJDRRt7pTG4GmNRZMZIsWzURi6OhdKmoh",
                    NumOfVideos = 22
                };
                Videos = new ObservableCollection<Video>()
                    {
                        new Video()
                        {
                            Title = "Ben&Ben - Kathang Isip (Official Lyric Video) Other Text To Make Title Longer And Another Text To Make Title Even Longer",
                            Author = "SINDIKATO",
                            Url = "https://www.youtube.com/watch?v=daZoiEGyFgg",
                            ThumbnailUrl = "https://i.ytimg.com/vi/daZoiEGyFgg/hq720.jpg?sqp=-oaymwEcCNAFEJQDSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLA_MrKk46lbC62yJT6mJCp3NBi56A",
                            Duration = new TimeSpan(0, 5, 33)
                        }
                    };
            }
         
        }

    }
}
