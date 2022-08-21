using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using YoutubePlaylistDownloader.Model;

namespace YoutubePlaylistDownloader.ViewModel.Helpers
{
    public class FileHelper
    {
        [DllImport("shell32",
        CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern string SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
            nint hToken = 0);

        public static string GetDownloadsFolderPath()
        {
            return SHGetKnownFolderPath(new("374DE290-123F-4565-9164-39C4925E467B"), 0);
        }

        public static string CreatePlaylistFolder(string folderPath, string playlistTitle, string playlistAuthor)
        {
            // creates playlist folder inside specifiec parent folder
            string playlistFolderPath;

            playlistFolderPath = System.IO.Path.Combine(folderPath, "[YPDA Playlist] " + playlistTitle + " - " + playlistAuthor);

            if (!System.IO.Directory.Exists(playlistFolderPath))
                System.IO.Directory.CreateDirectory(playlistFolderPath);
            
            return playlistFolderPath;
        }


        public static bool IsNameValid(string fileName)
        {
            // if name doesn't contain invalid characters
            if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) < 0)
                return true;
            else
                return false;

        }

        public static string ToValidFileName(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c.ToString(), "");
            }

            return fileName;
        }

    }

}
