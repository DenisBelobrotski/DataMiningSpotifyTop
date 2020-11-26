using System;

namespace DataMiningSpotifyTop.Source
{
    class Program
    {
        public static void Main(string[] args)
        {
            SongsReader songsReader = new SongsReader(@"../../Resources/top10s.csv", ',');
            songsReader.ReadSongs();
            songsReader.Songs.ForEach(Console.WriteLine);
        }
    }
}
