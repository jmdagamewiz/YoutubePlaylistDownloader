using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YoutubePlaylistDownloader.View
{
    /// <summary>
    /// Interaction logic for YouTubePlaylistDownloaderWindow.xaml
    /// </summary>
    public partial class YouTubePlaylistDownloaderWindow : Window
    {
        public YouTubePlaylistDownloaderWindow()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
