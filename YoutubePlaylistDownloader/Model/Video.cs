using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubePlaylistDownloader.Model
{
    public class Video
    {
        public int Id { get; set; }
        public int VideoDownloadId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
