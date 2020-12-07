using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DataMiningSpotifyTop.Source
{
    public static class FileSystemHelper
    {
        const string PredictionsDirectoryPath = @"Predictions";
        const string ModelsDirectoryPath = @"Models";


        static string CurrentDate => DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss.fff");


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


        public static void SavePredictions(List<Prediction> predictions)
        {
            if (!Directory.Exists(PredictionsDirectoryPath))
            {
                Directory.CreateDirectory(PredictionsDirectoryPath);
            }

            string fileName = $"predictions_{CurrentDate}.json";
            string filePath = $"{PredictionsDirectoryPath}/{fileName}";

            SaveObject(filePath, predictions);
        }


        public static void SaveKMeansModel(KMeansModel model)
        {
            if (!Directory.Exists(ModelsDirectoryPath))
            {
                Directory.CreateDirectory(ModelsDirectoryPath);
            }

            string fileName = $"k_means_model_{CurrentDate}.json";
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
