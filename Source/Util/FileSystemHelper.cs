using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataMiningSpotifyTop.Source.PostProcess;
using DataMiningSpotifyTop.Source.Process;
using Newtonsoft.Json;

namespace DataMiningSpotifyTop.Source.Util
{
    public static class FileSystemHelper
    {
        const string PredictionsDirectoryPath = @"Predictions";
        const string ModelsDirectoryPath = @"Models";


        static string CurrentDate => GetFormattedDate(DateTime.Now);


        public static string GetFormattedDate(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy_HH.mm.ss.fff");
        }


        public static void SaveObject(string filePath, object data)
        {
            string outputJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.AppendAllText(filePath, outputJson);
        }


        public static T ReadObject<T>(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<T>(fileContent);
        }


        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }


        public static void ClearPredictions()
        {
            if (Directory.Exists(PredictionsDirectoryPath))
            {
                DeleteDirectory(PredictionsDirectoryPath);
            }
        }


        public static void SavePredictions(List<Prediction> predictions, DateTime dateTime, int order)
        {
            if (!Directory.Exists(PredictionsDirectoryPath))
            {
                Directory.CreateDirectory(PredictionsDirectoryPath);
            }

            string date = GetFormattedDate(dateTime);
            string fileName = $"predictions***ord_{order}***date_{date}.json";
            string filePath = $"{PredictionsDirectoryPath}/{fileName}";

            SaveObject(filePath, predictions);
        }


        public static void SaveKMeansModel(KMeansModel model, DateTime dateTime, int order)
        {
            if (!Directory.Exists(ModelsDirectoryPath))
            {
                Directory.CreateDirectory(ModelsDirectoryPath);
            }

            int clustersCount = model.Centroids.Count;
            string date = GetFormattedDate(dateTime);
            string fileName = $"model_k_means***ord_{order}***k_{clustersCount}***date_{date}.json";
            string filePath = $"{ModelsDirectoryPath}/{fileName}";

            SaveObject(filePath, model);
        }


        public static KMeansModel ReadKMeansModel(string filePath)
        {
            return ReadObject<KMeansModel>(filePath);
        }


        public static List<KMeansModel> ReadAllModels()
        {
            List<KMeansModel> models = new List<KMeansModel>();

            if (!Directory.Exists(ModelsDirectoryPath))
            {
                return models;
            }

            List<string> paths = Directory.GetFiles(ModelsDirectoryPath).ToList();

            foreach (string path in paths)
            {
                try
                {
                    KMeansModel model = ReadKMeansModel(path);
                    models.Add(model);
                }
                catch (JsonReaderException exception)
                {
                    Console.WriteLine($"An error occured while reading json at path {path}");
                }
            }

            return models;
        }
    }
}