using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubePlaylistDownloader.Model;

namespace YoutubePlaylistDownloader.ViewModel.Helpers
{
    public class YouTubeHelper
    {

        public static bool IsLinkAPlaylist(string Url)
        {
            // TODO: to be deprecated          
            return true;
        }

        public async static Task<Playlist> GetPlaylistInformation(string Url)
        {
            YoutubeClient youtube = new YoutubeClient();

            var playlist = await youtube.Playlists.GetAsync(Url);

            // NumOfVideos reassigned with correct value after this method
            Playlist playlistInfo = new Playlist()
            {
                Url = playlist.Url,
                Title = playlist.Title,
                NumOfVideos = 0
            };

            if (playlist.Author != null)
            {
                playlistInfo.Author = playlist.Author.ChannelTitle;
            }
            else
            {
                playlistInfo.Author = "None";
            }

            return playlistInfo;
        }

        public async static Task<List<Video>> GetPlaylistVideos(string Url)
        {
            YoutubeClient youtube = new YoutubeClient();
            List<Video> videos = new List<Video>();

            await foreach(var video in youtube.Playlists.GetVideosAsync(Url))
            {
                videos.Add(new Video()
                {
                    Title = video.Title,
                    Author = video.Author.ChannelTitle,
                    Duration = video.Duration,
                    ThumbnailUrl = video.Thumbnails[0].Url,
                    Url = video.Url
                });
            }

            return videos;
        }
        


    }
}
