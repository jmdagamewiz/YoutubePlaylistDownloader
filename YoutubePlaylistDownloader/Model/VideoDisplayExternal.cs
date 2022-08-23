using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubePlaylistDownloader.Model
{
    public class VideoDisplayExternal   
    {
        public bool IsCheckBoxChecked { get; set; }
        public Video Video { get; set; }
        public List<YoutubeExplode.Videos.Streams.MuxedStreamInfo> Streams { get; set; }
        public YoutubeExplode.Videos.Streams.MuxedStreamInfo SelectedStream { get; set; }
    }
}
