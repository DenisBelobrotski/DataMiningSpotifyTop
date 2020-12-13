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
                string id = $"centroid_{i}";
                Song centroid = songs.RandomObject().CloneCleared(id);

                centroids.Add(centroid);
            }

            return centroids;
        }
    }
}
