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
            
            // ShowReadSongs(songs);

            SongsAnalyzer analyzer = new SongsAnalyzer(songs);
            analyzer.Analyze();
            
            // ShowAnalyzerResults(analyzer);
            
            SongsNormalizer normalizer = new SongsNormalizer(songs);
            normalizer.Normalize();
            
            // ShowNormalizedSongs(normalizer);
            
            KMeans kMeans = new KMeans(normalizer.NormalizedSongs, 5, -1);
            kMeans.Clusterize();
            
            ShowClusters(kMeans);
        }


        static void ShowReadSongs(List<Song> songs)
        {
            Console.WriteLine($"Songs count: {songs.Count}");
            songs.ForEach(Console.WriteLine);
        }


        static void ShowAnalyzerResults(SongsAnalyzer analyzer)
        {
            ShowBoundValues(analyzer);
            ShowSongsByGenre(analyzer);
            ShowSongsByYear(analyzer);
        }


        static void ShowBoundValues(SongsAnalyzer analyzer)
        {
            Console.WriteLine();
            Console.WriteLine($"Min songs values: {analyzer.MinSongsValues}");
            Console.WriteLine($"Max songs values: {analyzer.MaxSongsValues}");
        }


        static void ShowSongsByGenre(SongsAnalyzer analyzer)
        {
            Console.WriteLine();
            Console.WriteLine("Songs by genre:");

            List<string> genres = analyzer.SongsByGenre.Keys.ToList();

            genres.Sort((left, right) =>
                            analyzer.SongsByGenre[left].Count
                                    .CompareTo(analyzer.SongsByGenre[right].Count) * -1);

            foreach (string genre in genres)
            {
                Console.WriteLine($"{genre}-----{analyzer.SongsByGenre[genre].Count}");
            }
        }


        static void ShowSongsByYear(SongsAnalyzer analyzer)
        {
            Console.WriteLine();
            Console.WriteLine("Songs by year:");

            List<int> years = analyzer.SongsByYear.Keys.ToList();
            years.Sort();

            foreach (int year in analyzer.SongsByYear.Keys)
            {
                Console.WriteLine($"{year}-----{analyzer.SongsByYear[year].Count}");
            }
        }


        static void ShowNormalizedSongs(SongsNormalizer normalizer)
        {
            Console.WriteLine();
            normalizer.NormalizedSongs.ForEach(Console.WriteLine);
            
            SongsAnalyzer analyzer = new SongsAnalyzer(normalizer.NormalizedSongs);
            analyzer.Analyze();

            ShowBoundValues(analyzer);
        }


        static void ShowClusters(KMeans kMeans)
        {
            Console.WriteLine();
            Console.WriteLine($"Clusters count: {kMeans.ClustersCount}");
            
            Console.WriteLine();
            Console.WriteLine($"Centroids:");
            kMeans.Centroids.ForEach(Console.WriteLine);
            
            Console.WriteLine();
            Console.WriteLine($"Clusters size:");
            kMeans.Clusters.ForEach(songs => Console.WriteLine($"Size: {songs.Count}"));
        }
    }
}
