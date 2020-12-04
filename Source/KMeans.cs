using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
{
    public class KMeans
    {
        #region Properties

        public List<Song> Songs { get; }

        public int ClustersCount { get; }
        
        public int MaxIterationsCount { get; }
        
        public List<Song> Centroids { get; private set; }
        
        public List<List<Song>> Clusters { get; private set; }

        #endregion



        #region Object lifecycle

        public KMeans(List<Song> songs, int clustersCount, int maxIterationsCount)
        {
            Songs = songs;
            ClustersCount = clustersCount;
            MaxIterationsCount = maxIterationsCount;
        }

        #endregion



        #region Methods

        public void Clusterize()
        {
            Centroids = new List<Song>(ClustersCount);
            Clusters = new List<List<Song>>(ClustersCount);
            
            for (int i = 0; i < ClustersCount; i++)
            {
                string stub = $"centroid_{i}";
                
                Song centroid = Songs.RandomObject().Clone(stub);
                centroid.Title = stub;
                centroid.Artist = stub;
                centroid.Genre = stub;

                Centroids.Add(centroid);
                
                Clusters.Add(new List<Song>());
            }

            for (int songIndex = 0; songIndex < Songs.Count; songIndex++)
            {
                Song song = Songs[songIndex];

                int nearestClusterIndex = 0;
                Song centroid = Centroids[nearestClusterIndex];
                double minDistance = Song.EuclidDistance(centroid, song);

                for (int clusterIndex = 1; clusterIndex < Centroids.Count; clusterIndex++)
                {
                    centroid = Centroids[clusterIndex];
                    double distance = Song.EuclidDistance(centroid, song);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestClusterIndex = clusterIndex;
                    }
                }
                
                Clusters[nearestClusterIndex].Add(song);
            }
            
            // iterate songs and clusters
        }

        #endregion
    }
}
