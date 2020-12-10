using System;
using System.Collections.Generic;
using System.Linq;
using DataMiningSpotifyTop.Source.Common;
using DataMiningSpotifyTop.Source.PostProcess;
using DataMiningSpotifyTop.Source.PreProcess;
using DataMiningSpotifyTop.Source.Process;
using DataMiningSpotifyTop.Source.Util;

namespace DataMiningSpotifyTop.Source
{
    class Program
    {
        const string ResourcesRoot = @"../../Resources";
        const int ExperimentsCount = 1;
        const int ClustersCount = 3;


        // TODO: steps: 1. parse data 1. create models, 2. save models, 3. read model, 4. analyze, 5. predict
        public static void Main(string[] args)
        {
            DateTime runDate = GetRunDate();

            List<Song> songs = GetPreparedTrainingData();

            // TODO: experiments config
            for (int i = 0; i < ExperimentsCount; i++)
            {
                DynamicKMeans trainedKMeans = TrainModel(songs, ClustersCount);
                FileSystemHelper.SaveKMeansModel(trainedKMeans.Model, runDate, i);
            }

            List<StaticKMeans> models = ReadAllModels(songs);

            // TODO: analyze models

            List<Song> analyzingSongs = GetAnalyzingSongs();

            FileSystemHelper.ClearPredictions();

            for (int i = 0; i < models.Count; i++)
            {
                StaticKMeans model = models[i];
                List<Prediction> predictions = MakePredictions(model, analyzingSongs);
                FileSystemHelper.SavePredictions(predictions, runDate, i);
            }
        }


        static DateTime GetRunDate()
        {
            DateTime runDate = DateTime.Now;
            Console.WriteLine("Run date:");
            ConsoleHelper.ShowFullDate(runDate);

            return runDate;
        }


        static List<Song> GetPreparedTrainingData()
        {
            SongsReader songsReader = new SongsReader($"{ResourcesRoot}/top10s.csv", ',');
            songsReader.ReadSongs();

            List<Song> songs = songsReader.Songs;
            Console.WriteLine("Read songs:");
            ConsoleHelper.ShowReadSongs(songs);

            OriginalSongsAnalyzer analyzer = new OriginalSongsAnalyzer(songs);
            analyzer.Analyze();
            ConsoleHelper.ShowAnalyzerResults(analyzer);

            SongsNormalizer normalizer = new SongsNormalizer(songs, true);
            normalizer.Normalize();
            songs = normalizer.NormalizedSongs;

            Console.WriteLine("Normalized songs:");
            ConsoleHelper.ShowReadSongs(songs);

            return songs;
        }


        static DynamicKMeans TrainModel(List<Song> songs, int clustersCount)
        {
            DynamicKMeans kMeans = new DynamicKMeans(songs, clustersCount);
            kMeans.Clusterize();
            ConsoleHelper.ShowClusters(kMeans);

            return kMeans;
        }


        static List<StaticKMeans> ReadAllModels(List<Song> songs)
        {
            List<KMeansModel> models = FileSystemHelper.ReadAllModels();
            List<StaticKMeans> datas = models.Select(model => new StaticKMeans(songs, model)).ToList();
            datas.ForEach(data => data.ReadModel());

            ConsoleHelper.ShowClusters(datas.Last());

            return datas;
        }


        static List<Song> GetAnalyzingSongs()
        {
            SongsReader analyzingSongsReader = new SongsReader($"{ResourcesRoot}/test_data.csv", ',');
            analyzingSongsReader.ReadSongs();

            List<Song> analyzingSongs = analyzingSongsReader.Songs;

            Console.WriteLine("Read songs:");
            ConsoleHelper.ShowReadSongs(analyzingSongs);

            SongsNormalizer normalizer = new SongsNormalizer(analyzingSongs, false);
            normalizer.Normalize();
            analyzingSongs = normalizer.NormalizedSongs;

            Console.WriteLine("Normalized songs:");
            ConsoleHelper.ShowReadSongs(analyzingSongs);

            return analyzingSongs;
        }


        static List<Prediction> MakePredictions(BaseKMeans model, List<Song> analyzingSongs)
        {
            SongSuccessPredictor predictor = new SongSuccessPredictor(model, model.DistanceFunc);
            predictor.PrepareData();

            List<Prediction> predictions = analyzingSongs.Select(predictor.PredictSuccess).ToList();

            Console.WriteLine("Predictions:");
            predictions.ForEach(Console.WriteLine);

            return predictions;
        }
    }
}
