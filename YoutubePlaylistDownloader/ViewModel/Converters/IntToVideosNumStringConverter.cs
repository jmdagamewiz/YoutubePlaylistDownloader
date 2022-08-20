using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace YoutubePlaylistDownloader.ViewModel.Converters
{
    public class IntToVideosNumStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int NumVideos = (int)value;

            return $"{NumVideos} videos";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string VideosNumString = (string)value;

            int VideoNum = Int32.Parse(VideosNumString.Split(" ")[0]);

            return VideoNum;
        }
    }
}
