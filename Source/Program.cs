using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DataMiningSpotifyTop.Source
{
    class Program
    {
        const string ResourcesRoot = @"../../Resources";
        const int ExperimentsCount = 5;
        const int ClustersCount = 10;
        const int MaxIterationsCount = 100000;
        
        
        public static void Main(string[] args)
        {
            for (int i = 0; i < ExperimentsCount; i++)
            {
                KMeans kMeans = BuildModel();
                TestPredictor(kMeans, i);
            }
        }


        static KMeans BuildModel()
        {
            SongsReader songsReader = new SongsReader($"{ResourcesRoot}/top10s.csv", ',');
            songsReader.ReadSongs();

            List<Song> songs = songsReader.Songs;
            
            // ShowReadSongs(songs);

            SongsAnalyzer analyzer = new SongsAnalyzer(songs);
            analyzer.Analyze();
            
            // ShowAnalyzerResults(analyzer);
            
            SongsNormalizer normalizer = new SongsNormalizer(songs, true);
            normalizer.Normalize();
            
            // ShowNormalizedSongs(normalizer);
            
            KMeans kMeans = new KMeans(normalizer.NormalizedSongs, ClustersCount, MaxIterationsCount);
            // kMeans.CentroidsChooser = new PlusPlusCentroidsChooser(new SquaredEuclidDistanceFunc());
            kMeans.DistanceFunc = new SquaredEuclidDistanceFunc();
            kMeans.Clusterize();
            
            ShowClusters(kMeans);

            return kMeans;
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
            
            ShowSongsByClusters(kMeans.ClusterizedSongs);
            ShowSongsByClusters(kMeans);
        }


        static void ShowSongsByClusters(List<ClusterizedSong> clusterizedSongs)
        {
            Dictionary<int, List<Song>> songsByCluster = new Dictionary<int, List<Song>>();

            foreach (ClusterizedSong clusterizedSong in clusterizedSongs)
            {
                int key = clusterizedSong.ClusterIndex;
                Song song = clusterizedSong.Song;
                
                if (songsByCluster.TryGetValue(key, out List<Song> clusterSongs))
                {
                    clusterSongs.Add(song);
                }
                else
                {
                    songsByCluster[key] = new List<Song>{ song };
                }
            }
            
            Console.WriteLine();
            Console.WriteLine($"Clusters size:");

            foreach (int cluster in songsByCluster.Keys)
            {
                int songsCount = songsByCluster[cluster].Count;
                Console.WriteLine($"Size: {songsCount}");
            }
        }


        static void ShowSongsByClusters(ISongsClusterizer clusterizer)
        {
            Console.WriteLine();
            Console.WriteLine($"Clusters size:");
            clusterizer.Clusters.ForEach(cluster => Console.WriteLine($"Size: {cluster.Count}"));
        }


        static void TestPredictor(ISongsClusterizer clusterizer, int experimentIndex)
        {
            SongsReader analyzingSongsReader = new SongsReader($"{ResourcesRoot}/test_data.csv", ',');
            analyzingSongsReader.ReadSongs();

            List<Song> analyzingSongs = analyzingSongsReader.Songs;
            
            Console.WriteLine("Read songs:");
            ShowReadSongs(analyzingSongs);
            
            SongsNormalizer normalizer = new SongsNormalizer(analyzingSongs, false);
            normalizer.Normalize();
            analyzingSongs = normalizer.NormalizedSongs;
            
            Console.WriteLine("Normalized songs:");
            ShowReadSongs(analyzingSongs);

            IDistanceFunc distanceFunc = new EuclidDistanceFunc();
            SongSuccessPredictor predictor = new SongSuccessPredictor(clusterizer, distanceFunc);
            predictor.PrepareData();

            List<Prediction> predictions = analyzingSongs.Select(song => predictor.PredictSuccess(song)).ToList();
            
            Console.WriteLine("Predictions:");
            predictions.ForEach(Console.WriteLine);

            const string parentDirectoryPath = @"Predictions";

            if (!Directory.Exists(parentDirectoryPath))
            {
                Directory.CreateDirectory(parentDirectoryPath);
            }

            const string dateFormat = "MM.dd.yyyy_HH:mm:ss";
            string currentDate = DateTime.Now.ToString(dateFormat);
            string fileName = $"predictions_{experimentIndex}_{currentDate}.json";
            string filePath = $"{parentDirectoryPath}/{fileName}";

            string outputJson = JsonConvert.SerializeObject(predictions, Formatting.Indented);
            File.AppendAllText(filePath, outputJson);
        }
    }
}
