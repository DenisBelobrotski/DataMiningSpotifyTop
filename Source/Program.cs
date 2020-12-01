using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMiningSpotifyTop.Source
{
    class Program
    {
        public static void Main(string[] args)
        {
            SongsReader songsReader = new SongsReader(@"../../Resources/top10s.csv", ',');
            songsReader.ReadSongs();

            List<Song> songs = songsReader.Songs;
            // songs.ForEach(Console.WriteLine);
            Console.WriteLine($"Songs count: {songs.Count}");

            SongsAnalyzer songsAnalyzer = new SongsAnalyzer(songs);
            songsAnalyzer.Analyze();
            Console.WriteLine();
            Console.WriteLine($"Min songs values: {songsAnalyzer.MinSongsValues}");
            Console.WriteLine($"Max songs values: {songsAnalyzer.MaxSongsValues}");

            Console.WriteLine();
            Console.WriteLine("Songs by genre:");

            List<string> genres = songsAnalyzer.SongsByGenre.Keys.ToList();

            genres.Sort((left, right) =>
                            songsAnalyzer.SongsByGenre[left].Count
                                         .CompareTo(songsAnalyzer.SongsByGenre[right].Count) * -1);

            foreach (string genre in genres)
            {
                Console.WriteLine($"{genre}-----{songsAnalyzer.SongsByGenre[genre].Count}");
            }

            Console.WriteLine();
            Console.WriteLine("Songs by year:");

            List<int> years = songsAnalyzer.SongsByYear.Keys.ToList();
            years.Sort();

            foreach (int year in songsAnalyzer.SongsByYear.Keys)
            {
                Console.WriteLine($"{year}-----{songsAnalyzer.SongsByYear[year].Count}");
            }
        }
    }
}
