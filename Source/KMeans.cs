using System;
using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
{
    public class KMeans : ISongsClusterizer
    {
        #region Fields

        IDistanceFunc distanceFunc;
        ICentroidsChooser centroidsChooser;

        #endregion



        #region Properties

        public List<Song> Songs { get; }

        public int ClustersCount { get; }

        public int MaxIterationsCount { get; }
        
        public IDistanceFunc DistanceFunc
        {
            get => distanceFunc ?? (distanceFunc = new EuclidDistanceFunc());
            set => distanceFunc = value;
        }

        public ICentroidsChooser CentroidsChooser
        {
            get => centroidsChooser ?? (centroidsChooser = new RandomCentroidsChooser());
            set => centroidsChooser = value;
        }

        public List<Song> Centroids { get; private set; }

        public List<ClusterizedSong> ClusterizedSongs { get; private set; }

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
            Centroids = CentroidsChooser.GetCentroids(Songs, ClustersCount);
            PerformInitialIteration();
            RecalculateCentroids();

            Console.WriteLine($"K-means initial iteration completed.");

            bool songMovedBetweenClusters = true;

            for (int iterationIndex = 0;
                 (iterationIndex < MaxIterationsCount || MaxIterationsCount < 0) && songMovedBetweenClusters;
                 iterationIndex++)
            {
                songMovedBetweenClusters = IterateClusters();
                RegroupClusters();
                RecalculateCentroids();
                Console.WriteLine($"K-means iteration completed ({iterationIndex}/{MaxIterationsCount}).");
            }
        }


        void PerformInitialIteration()
        {
            ClusterizedSongs = new List<ClusterizedSong>(Songs.Capacity);
            Clusters = new List<List<Song>>(ClustersCount);

            for (int i = 0; i < ClustersCount; i++)
            {
                Clusters.Add(new List<Song>());
            }

            for (int songIndex = 0; songIndex < Songs.Count; songIndex++)
            {
                Song song = Songs[songIndex];

                int clusterIndex = GetNearestCentroidIndex(song, Centroids, DistanceFunc);

                ClusterizedSongs.Add(new ClusterizedSong
                {
                    Song = song,
                    ClusterIndex = clusterIndex,
                });

                Clusters[clusterIndex].Add(song);
            }
        }


        int GetNearestCentroidIndex(Song song, List<Song> centroids, IDistanceFunc distanceFunc)
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


        bool IterateClusters()
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


        void RegroupClusters()
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


        void RecalculateCentroids()
        {
            for (int clusterIndex = 0; clusterIndex < ClustersCount; clusterIndex++)
            {
                List<Song> cluster = Clusters[clusterIndex];
                Song centroid = Centroids[clusterIndex];

                int clusterCardinality = cluster.Count;

                if (clusterCardinality <= 0)
                {
                    continue;
                }

                centroid.CopyValues(cluster[0]);

                for (int songIndex = 1; songIndex < clusterCardinality; songIndex++)
                {
                    centroid.Add(cluster[songIndex]);
                }

                centroid.Divide(Convert.ToDouble(clusterCardinality));
            }
        }

        #endregion
    }
}
