using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace YoutubePlaylistDownloader.ViewModel.Converters
{
    public class TimeSpanToDurationStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan duration = (TimeSpan)value;

            if (duration.Hours > 0)
            {
                return duration.ToString(@"hh\:mm\:ss");
                //return $"{duration.Hours}:{duration.Minutes}:{duration.Seconds}";
            }
            else
            {
                return duration.ToString(@"mm\:ss");
                //return $"{duration.Minutes}:{duration.Seconds}";
            } 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string durationString = (string)value;

            string[] partsList = durationString.Split(":");

            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            if (partsList.Length == 3)
            {
                hours = Int32.Parse(partsList[0]);
                minutes = Int32.Parse(partsList[1]);
                seconds = Int32.Parse(partsList[2]);
            }
            else if (partsList.Length == 2)
            {
                minutes = Int32.Parse(partsList[0]);
                seconds = Int32.Parse(partsList[1]);
            }

            return new TimeSpan(hours, minutes, seconds);
        }
    }
}
