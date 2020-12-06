using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
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

                centroids.Add(centroid);
            }

            return centroids;
        }
    }
}
