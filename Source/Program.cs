using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMiningSpotifyTop.Source
{
    class Program
    {
        const string ResourcesRoot = @"../../Resources";
        const int ExperimentsCount = 1;
        const int ClustersCount = 3;


        public static void Main(string[] args)
        {
            List<Song> songs = ReadOriginalSongs();
            
            //TODO: predictions based on models (static k-means)
            //TODO: predict on each read model
            //TODO: analyze models
            //TODO: steps: 1. create, 2. save, 3. read, 4. analyze, 5. predict
            Create(songs);
            Read(songs);
        }


        static void Create(List<Song> songs)
        {
            for (int i = 0; i < ExperimentsCount; i++)
            {
                DynamicKMeans kMeans = BuildModel(songs);
                
                SongSuccessPredictor predictor = new SongSuccessPredictor(kMeans, kMeans.DistanceFunc);
                predictor.PrepareData();
                
                List<Prediction> predictions = BuildPredictions(predictor);
                FileSystemHelper.SavePredictions(predictions);

                KMeansModel model = kMeans.Model;
                FileSystemHelper.SaveKMeansModel(model);
            }
        }


        static void Read(List<Song> songs)
        {
            List<KMeansModel> models = FileSystemHelper.ReadAllModels();
            List<StaticKMeans> datas = models.Select(model => new StaticKMeans(songs, model)).ToList();
            datas.ForEach(data => data.ReadModel());
            
            ShowClusters(datas.Last());
            
            // SongSuccessPredictor test = new SongSuccessPredictor(datas.Last(), datas.Last().DistanceFunc);
            // test.PrepareData();
                
            // List<Prediction> tests = BuildPredictions(test);
        }


        static List<Song> ReadOriginalSongs()
        {
            SongsReader songsReader = new SongsReader($"{ResourcesRoot}/top10s.csv", ',');
            songsReader.ReadSongs();

            List<Song> originalSongs = songsReader.Songs;
            
            // ShowReadSongs(originalSongs);
            
            SongsAnalyzer analyzer = new SongsAnalyzer(originalSongs);
            analyzer.Analyze();
            
            // ShowAnalyzerResults(analyzer);
            
            SongsNormalizer normalizer = new SongsNormalizer(originalSongs, true);
            normalizer.Normalize();
            
            // ShowNormalizedSongs(normalizer);

            return normalizer.NormalizedSongs;
        }


        static DynamicKMeans BuildModel(List<Song> songs)
        {
            DynamicKMeans kMeans = new DynamicKMeans(songs, ClustersCount);
            // kMeans.CentroidsChooser = new PlusPlusCentroidsChooser(new SquaredEuclidDistanceFunc());
            // kMeans.DistanceFunc = new EuclidDistanceFunc();
            // kMeans.MaxIterationsCount = 2;
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


        static void ShowClusters(BaseKMeans kMeans)
        {
            Console.WriteLine();
            Console.WriteLine($"Clusters count: {kMeans.ClustersCount}");
            
            Console.WriteLine();
            Console.WriteLine($"Centroids:");
            kMeans.Centroids.ForEach(Console.WriteLine);
            
            // ShowSongsByClusters(kMeans.ClusterizedSongs);
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


        static void ShowSongsByClusters(BaseKMeans clusterizer)
        {
            Console.WriteLine();
            Console.WriteLine($"Clusters size:");
            clusterizer.Clusters.ForEach(cluster => Console.WriteLine($"Size: {cluster.Count}"));
        }


        static List<Prediction> BuildPredictions(SongSuccessPredictor predictor)
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

            List<Prediction> predictions = analyzingSongs.Select(predictor.PredictSuccess).ToList();
            
            Console.WriteLine("Predictions:");
            predictions.ForEach(Console.WriteLine);

            return predictions;
        }
    }
}
