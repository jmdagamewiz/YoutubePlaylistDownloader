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
using System.Windows.Navigation;
using System.Windows.Shapes;
using YoutubePlaylistDownloader.Model;

namespace YoutubePlaylistDownloader.View.UserControls
{
    /// <summary>
    /// Interaction logic for VideoDisplay.xaml
    /// </summary>
    public partial class VideoDisplay : UserControl
    {
        public VideoDisplayExternal VideoDisplayExternal
        {
            get { return (VideoDisplayExternal)GetValue(VideoDisplayExternalProperty); }
            set { SetValue(VideoDisplayExternalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VideoDisplayExternal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VideoDisplayExternalProperty =
            DependencyProperty.Register("VideoDisplayExternal", typeof(VideoDisplayExternal), typeof(VideoDisplay), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoDisplay videoControl = d as VideoDisplay;

            if (videoControl != null)
            {
                videoControl.DataContext = videoControl.VideoDisplayExternal;
            }
        }


        public VideoDisplay()
        {
            InitializeComponent();
        }
    }
}
