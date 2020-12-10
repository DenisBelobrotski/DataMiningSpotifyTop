using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMiningSpotifyTop.Source
{
    public class SongSuccessPredictor
    {
        #region Properties

        public List<double> Probabilities { get; }

        BaseKMeans Clusterizer { get; }

        IDistanceFunc DistanceFunc { get; }

        #endregion



        #region Object lifecycle

        public SongSuccessPredictor(BaseKMeans clusterizer, IDistanceFunc distanceFunc)
        {
            Clusterizer = clusterizer;
            DistanceFunc = distanceFunc;
            Probabilities = new List<double>(Clusterizer.Clusters.Capacity);
        }

        #endregion



        #region Methods

        public void PrepareData()
        {
            double totalSongsCount = Convert.ToDouble(Clusterizer.ClusterizedSongs.Count);

            Probabilities.Clear();

            foreach (List<Song> cluster in Clusterizer.Clusters)
            {
                double clusterCardinality = Convert.ToDouble(cluster.Count);
                Probabilities.Add(clusterCardinality / totalSongsCount);
            }
        }


        public Prediction PredictSuccess(Song song)
        {
            int clusterIndex = Clusterizer.GetAssociatedClusterIndex(song);
            Song clusterCentroid = Clusterizer.Centroids[clusterIndex];
            double portion = Probabilities[clusterIndex];
            List<double> portions = new List<double>(Probabilities);
            double distance = DistanceFunc.GetDistance(song, clusterCentroid);
            List<double> distances = Clusterizer.Centroids
                                                .Select(centroid => DistanceFunc.GetDistance(song, centroid))
                                                .ToList();
            List<int> sizes = Clusterizer.Clusters.Select(cluster => cluster.Count).ToList();
            int clusterSize = Clusterizer.Clusters[clusterIndex].Count;

            return new Prediction
            {
                ClusterIndex = clusterIndex,
                ClusterCentroid = clusterCentroid,
                ClusterPortion = portion,
                ClusterPortions = portions,
                CentroidDistance = distance,
                CentroidDistances = distances,
                ClusterSizes = sizes,
                ClusterSize = clusterSize,
            };
        }

        #endregion
    }
}
