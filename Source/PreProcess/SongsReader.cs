using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvSerialization;

namespace DataMiningSpotifyTop.Source
{
    public class SongsReader
    {
        #region Fields

        string filePath;
        char delimiter;
        List<Song> songs;

        #endregion



        #region Properties

        public List<Song> Songs => songs ?? (songs = ReadSongs());

        #endregion



        #region Object lifecycle

        public SongsReader(string filePath, char delimiter)
        {
            this.filePath = filePath;
            this.delimiter = delimiter;
        }

        #endregion



        #region Methods

        public List<Song> ReadSongs()
        {
            string fileContent = File.ReadAllText(filePath);
            fileContent = SongsFileCleaner.CleanText(fileContent);

            return CsvSerializer<Song>.Deserialize(delimiter, fileContent).ToList();
        }

        #endregion
    }
}
