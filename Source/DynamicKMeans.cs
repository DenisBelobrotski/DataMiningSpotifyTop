using System;
using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
{
    public class DynamicKMeans : BaseKMeans
    {
        #region Fields

        ICentroidsChooser centroidsChooser;

        #endregion



        #region Properties

        public override int ClustersCount { get; }

        public int MaxIterationsCount { set; get; } = 100000;

        public ICentroidsChooser CentroidsChooser
        {
            get => centroidsChooser ?? (centroidsChooser = new RandomCentroidsChooser());
            set => centroidsChooser = value;
        }
        
        public KMeansModel Model => new KMeansModel
        {
            Centroids = new List<Song>(Centroids),
        };

        #endregion



        #region Object lifecycle

        public DynamicKMeans(List<Song> songs, int clustersCount)
        {
            Songs = songs;
            ClustersCount = clustersCount;
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
