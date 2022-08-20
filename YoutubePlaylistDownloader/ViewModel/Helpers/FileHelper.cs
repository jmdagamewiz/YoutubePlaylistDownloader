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

        public static string CreatePlaylistFolder(string folderPath, Playlist playlist)
        {
            // creates playlist folder inside specifiec parent folder
            string playlistFolderPath;

            string temporaryPlaylistAuthor = playlist.Author;

            if (playlist.Author == "")
            {
                temporaryPlaylistAuthor = "None";
            }

            if (playlist.Title.Contains("|"))
                playlistFolderPath = System.IO.Path.Combine(folderPath,
                    "[YPDA Playlist] " + playlist.Title.Replace("|", "-") + " - " + temporaryPlaylistAuthor);
            else
                playlistFolderPath = System.IO.Path.Combine(folderPath,
                    "[YPDA Playlist] " + playlist.Title + " - " + temporaryPlaylistAuthor);

            if (!System.IO.Directory.Exists(playlistFolderPath))
            {
                System.IO.Directory.CreateDirectory(playlistFolderPath);
            }

            return playlistFolderPath;
        }
    }

}
