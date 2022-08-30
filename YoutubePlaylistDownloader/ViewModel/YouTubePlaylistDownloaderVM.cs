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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

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
        public DownloadCommand DownloadCommand { get; set; }
        public ChangeDownloadLocationCommand ChangeDownloadFolderCommand { get; set; }
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

        private string downloadFolderLocation;

        public string DownloadFolderLocation
        {
            get { return downloadFolderLocation; }
            set
            {
                downloadFolderLocation = value;
                OnPropertyChanged("DownloadFolderLocation");
            }
        }

        private bool isNotDownloading;

        public bool IsNotDownloading
        {   
            get { return isNotDownloading; }
            set
            {
                isNotDownloading = value;
                OnPropertyChanged("IsNotDownloading");
            }
        }

        private bool isNotSearching;

        public bool IsNotSearching
        {   
            get { return isNotSearching; }
            set
            {
                isNotSearching = value;
                OnPropertyChanged("IsNotSearching");
            }
        }

        private VideoDisplayExternal currentlyDownloadingVideoDisplayExternal;

        public VideoDisplayExternal CurrentlyDownloadingVideoDisplayExternal
        {   
            get { return currentlyDownloadingVideoDisplayExternal; }
            set
            {
                currentlyDownloadingVideoDisplayExternal = value;
                OnPropertyChanged("CurrentlyDownloadingVideoDisplayExternal");
            }
        }


        public async void SearchUrl()
        {
            IsNotSearching = false;
            IsNotDownloading = false;

            string InputUrl = SearchTextBoxString;

            // clears information about previous query
            VideoDisplayExternals.Clear();
            SelectedPlaylist = null;

            try
            {
                Playlist newPlaylist = await YouTubeHelper.GetPlaylistInformation(InputUrl);

                List<Video> searchedVideos = await YouTubeHelper.GetPlaylistVideos(InputUrl);

                // updates NumOfVideos property
                newPlaylist.NumOfVideos = searchedVideos.Count;
                SelectedPlaylist = newPlaylist;

                if (searchedVideos.Count > 0 || searchedVideos != null)
                {
                    var youtube = new YoutubeClient();

                    foreach (Video video in searchedVideos)
                    {
                        var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(video.Url);
                        var streamInfo = streamInfoSet.GetMuxedStreams();
                        var streamsList = streamInfo.ToList();

                        VideoDisplayExternals.Add(new VideoDisplayExternal()
                        {
                            IsCheckBoxChecked = true,
                            Video = video,
                            Streams = streamsList,
                            SelectedStream = streamsList.Last(),
                            DownloadProgress = 0
                        });
                    }
                }
            }
            catch
            {
                
            }

            IsNotSearching = true;
            IsNotDownloading = true;
        }

        public async void DownloadVideos()
        {
            IsNotDownloading = false;
            IsNotSearching = false;

            // get user's Downloads folder and creates playlist folder in it
            string folderPath = DownloadFolderLocation;

            if (!FileHelper.IsNameValid(SelectedPlaylist.Title))
            {
                SelectedPlaylist.Title = FileHelper.ToValidFileName(SelectedPlaylist.Title);
            }
            
            if (!FileHelper.IsNameValid(SelectedPlaylist.Author))
            {
                SelectedPlaylist.Author = FileHelper.ToValidFileName(SelectedPlaylist.Author);
            }

            string playlistFolderPath = FileHelper.CreatePlaylistFolder(folderPath, SelectedPlaylist.Title, SelectedPlaylist.Author);

            // erases previous ProgressBar values (if there are any)
            foreach (VideoDisplayExternal videoDisplayExternal in VideoDisplayExternals)
            {
                videoDisplayExternal.DownloadProgress = 0;
            }

            // start downloading each video
            var youtube = new YoutubeClient();
            foreach (VideoDisplayExternal videoDisplayExternal in VideoDisplayExternals)
            {
                if (videoDisplayExternal.IsCheckBoxChecked)
                {
                    Video video = videoDisplayExternal.Video;

                    // get stream selected in ComboBox
                    var streamInfo = videoDisplayExternal.SelectedStream;

                    string fileName = $"{video.Title}.{streamInfo.Container}";

                    if (!FileHelper.IsNameValid(fileName))
                    {
                        fileName = FileHelper.ToValidFileName(fileName);
                    }

                    string downloadPath = System.IO.Path.Combine(playlistFolderPath, fileName);
          
                    CurrentlyDownloadingVideoDisplayExternal = videoDisplayExternal;
                    var progressHandler = new Progress<double>(ShowDownloadProgress);

                    // download video
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, downloadPath, progressHandler);
                    CurrentlyDownloadingVideoDisplayExternal = null;
                }
            }

            IsNotDownloading = true;
            IsNotSearching = true;
        }

        private void ShowDownloadProgress(double ProgressValue)
        {
            CurrentlyDownloadingVideoDisplayExternal.DownloadProgress = ProgressValue * 100;
        }

        public void ChangeDownloadFolderLocation()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = DownloadFolderLocation;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DownloadFolderLocation = dialog.FileName;
            }
        }

        public YouTubePlaylistDownloaderVM()
        {
            DownloadCommand = new DownloadCommand(this);
            SearchCommand = new SearchCommand(this);
            ChangeDownloadFolderCommand = new ChangeDownloadLocationCommand(this);

            VideoDisplayExternals = new ObservableCollection<VideoDisplayExternal>();

            DownloadFolderLocation = FileHelper.GetDownloadsFolderPath();

            IsNotSearching = true;
            IsNotDownloading = true;

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedPlaylist = new Playlist()
                {
                    Title = "Ben and Ben playlist",
                    Author = "J-ann Espinosa",
                    Url = "https://www.youtube.com/watch?v=daZoiEGyFgg&list=PLJDRRt7pTG4GmNRZMZIsWzURi6OhdKmoh",
                    NumOfVideos = 22
                };

                VideoDisplayExternals = new ObservableCollection<VideoDisplayExternal>()
                {
                    new VideoDisplayExternal()
                    {
                        IsCheckBoxChecked = true,
                        Video = new Video()
                        {
                            Title = "Ben&Ben - Kathang Isip (Official Lyric Video) Other Text To Make Title Longer And Another Text To Make Title Even Longer",
                            Author = "SINDIKATO",
                            Url = "https://www.youtube.com/watch?v=daZoiEGyFgg",
                            ThumbnailUrl = "https://i.ytimg.com/vi/daZoiEGyFgg/hq720.jpg?sqp=-oaymwEcCNAFEJQDSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLA_MrKk46lbC62yJT6mJCp3NBi56A",
                            Duration = new TimeSpan(0, 5, 33)
                        }
                    }
                };

            }
         
        }

    }
}
