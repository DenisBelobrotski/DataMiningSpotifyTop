using System.Collections.Generic;
using DataMiningSpotifyTop.Source.Common;

namespace DataMiningSpotifyTop.Source.Process
{
    public abstract class BaseKMeans
    {
        #region Fields

        IDistanceFunc distanceFunc;

        #endregion



        #region Properties

        public List<Song> Songs { get; protected set; }

        public IDistanceFunc DistanceFunc
        {
            get => distanceFunc ?? (distanceFunc = new SquaredEuclidDistanceFunc());
            set => distanceFunc = value;
        }

        public abstract int ClustersCount { get; }

        public List<Song> Centroids { get; protected set; }

        public List<ClusterizedSong> ClusterizedSongs { get; protected set; }

        public List<List<Song>> Clusters { get; protected set; }

        #endregion



        #region Methods

        public int GetAssociatedClusterIndex(Song song)
        {
            return GetNearestCentroidIndex(song, Centroids, DistanceFunc);
        }


        protected bool IterateClusters()
        {
            bool songMovedBetweenClusters = false;

            foreach (ClusterizedSong song in ClusterizedSongs)
            {
                int currentSongCluster = song.ClusterIndex;
                int clusterIndex = GetNearestCentroidIndex(song.Song, Centroids, DistanceFunc);

                if (currentSongCluster != clusterIndex)
                {
                    songMovedBetweenClusters = true;
                    song.ClusterIndex = clusterIndex;
                }
            }

            return songMovedBetweenClusters;
        }


        protected void RegroupClusters()
        {
            foreach (List<Song> cluster in Clusters)
            {
                cluster.Clear();
            }

            foreach (ClusterizedSong clusterizedSong in ClusterizedSongs)
            {
                Clusters[clusterizedSong.ClusterIndex].Add(clusterizedSong.Song);
            }
        }


        protected int GetNearestCentroidIndex(Song song, List<Song> centroids, IDistanceFunc distanceFunc)
        {
            int nearestCentroidIndex = 0;
            Song centroid = centroids[nearestCentroidIndex];
            double minDistance = distanceFunc.GetDistance(centroid, song);

            for (int clusterIndex = 1; clusterIndex < centroids.Count; clusterIndex++)
            {
                centroid = centroids[clusterIndex];
                double distance = distanceFunc.GetDistance(centroid, song);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCentroidIndex = clusterIndex;
                }
            }

            return nearestCentroidIndex;
        }

        #endregion
    }
}
