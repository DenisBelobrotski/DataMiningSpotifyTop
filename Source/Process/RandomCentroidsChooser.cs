using System.Collections.Generic;
using DataMiningSpotifyTop.Source.Common;
using DataMiningSpotifyTop.Source.Util;

namespace DataMiningSpotifyTop.Source.Process
{
    public class RandomCentroidsChooser : ICentroidsChooser
    {
        public List<Song> GetCentroids(List<Song> songs, int clustersCount)
        {
            List<Song> centroids = new List<Song>(clustersCount);

            for (int i = 0; i < clustersCount; i++)
            {
                string stub = $"centroid_{i}";

                Song centroid = songs.RandomObject().Clone(stub);
                centroid.Title = stub;
                centroid.Artist = stub;
                centroid.Genre = stub;
                centroid.Year = 0;
                centroid.Popularity = 0.0f;

                centroids.Add(centroid);
            }

            return centroids;
        }
    }
}
