using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMiningSpotifyTop.Source
{
    public static class ConsoleHelper
    {
        public static void ShowReadSongs(List<Song> songs)
        {
            Console.WriteLine($"Songs count: {songs.Count}");
            songs.ForEach(Console.WriteLine);
        }


        public static void ShowAnalyzerResults(OriginalSongsAnalyzer analyzer)
        {
            ShowBoundValues(analyzer);
            ShowSongsByGenre(analyzer);
            ShowSongsByYear(analyzer);
        }


        public static void ShowClusters(BaseKMeans kMeans)
        {
            Console.WriteLine();
            Console.WriteLine($"Clusters count: {kMeans.ClustersCount}");

            Console.WriteLine();
            Console.WriteLine($"Centroids:");
            kMeans.Centroids.ForEach(Console.WriteLine);

            ShowSongsByClusters(kMeans);
        }


        public static void Wait(string message)
        {
            Console.WriteLine($"{message} and press Enter to continue...");
            Console.Read();
        }


        public static void ShowFullDate(DateTime dateTime)
        {
            string date = dateTime.ToString("dd.MM.yyyy_HH.mm.ss.fff");
            Console.WriteLine(date);
        }


        static void ShowBoundValues(OriginalSongsAnalyzer analyzer)
        {
            Console.WriteLine();
            Console.WriteLine($"Min songs values: {analyzer.MinSongsValues}");
            Console.WriteLine($"Max songs values: {analyzer.MaxSongsValues}");
        }


        static void ShowSongsByGenre(OriginalSongsAnalyzer analyzer)
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


        static void ShowSongsByYear(OriginalSongsAnalyzer analyzer)
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


        static void ShowSongsByClusters(BaseKMeans clusterizer)
        {
            Console.WriteLine();
            Console.WriteLine($"Clusters size:");
            clusterizer.Clusters.ForEach(cluster => Console.WriteLine($"Size: {cluster.Count}"));
        }
    }
}
