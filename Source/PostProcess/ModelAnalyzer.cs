using System;
using System.Collections.Generic;
using System.Linq;
using DataMiningSpotifyTop.Source.Common;
using DataMiningSpotifyTop.Source.Process;

namespace DataMiningSpotifyTop.Source.PostProcess
{
    public class ModelAnalyzer
    {
        #region Properties

        public BaseKMeans Model { get; }

        public int ClustersCount => Model.ClustersCount;

        public List<AnalyzedSong> AnalyzedSongs { get; private set; }

        public List<double> IntraClusterDistances { get; private set; }

        public double IntraClusterMeanDistance { get; private set; }

        public double InterClusterMeanDistance { get; private set; }

        public double SilhouetteCoefMean { get; private set; }

        #endregion



        #region Object lifecycle

        public ModelAnalyzer(BaseKMeans model)
        {
            Model = model;
        }

        #endregion



        #region Methods

        public void Analyze()
        {
            AnalyzeIntraClusterDistance();
            SortAnalyzedSongs();
            AnalyzeInterClusterDistance();
            SilhouetteCoefMean = AnalyzeSilhouetteCoef(AnalyzedSongs, Model);
        }


        void AnalyzeIntraClusterDistance()
        {
            List<Song> centroids = Model.Centroids;

            IntraClusterDistances = new List<double>(centroids.Capacity);
            IntraClusterMeanDistance = 0.0;

            List<int> clusterCounts = new List<int>(centroids.Capacity);
            int songsCount = 0;

            for (int i = 0; i < centroids.Count; i++)
            {
                IntraClusterDistances.Add(0.0);
                clusterCounts.Add(0);
            }

            AnalyzedSongs = Model.ClusterizedSongs.Select(song =>
            {
                double distance = Model.DistanceFunc.GetDistance(song.Song, centroids[song.ClusterIndex]);
                int index = song.ClusterIndex;

                AnalyzedSong analyzedSong = new AnalyzedSong
                {
                    AssociatedSong = song.Song,
                    ClusterIndex = index,
                    CentroidDistance = distance,
                };

                IntraClusterDistances[index] += distance;
                IntraClusterMeanDistance += distance;

                clusterCounts[index] += 1;
                songsCount += 1;

                return analyzedSong;
            }).ToList();

            for (int i = 0; i < IntraClusterDistances.Count; i++)
            {
                IntraClusterDistances[i] /= clusterCounts[i];
            }

            IntraClusterMeanDistance /= songsCount;
        }


        void SortAnalyzedSongs()
        {
            AnalyzedSongs.Sort((left, right) =>
            {
                int comparisonResult = left.ClusterIndex.CompareTo(right.ClusterIndex);

                if (comparisonResult == 0)
                {
                    comparisonResult = left.CentroidDistance.CompareTo(right.CentroidDistance);
                }

                return comparisonResult;
            });
        }


        void AnalyzeInterClusterDistance()
        {
            List<Song> centroids = Model.Centroids;
            int clustersCount = centroids.Count;

            if (clustersCount == 0)
            {
                throw new ArgumentException("No clusters.");
            }

            const string id = "centroids_center";
            Song centroidsCenter = centroids[0].CloneCleared(id);

            for (int i = 1; i < clustersCount; i++)
            {
                Song centroid = centroids[i];
                centroidsCenter.Add(centroid);
            }

            centroidsCenter.Divide(clustersCount);

            IDistanceFunc distanceFunc = Model.DistanceFunc;

            InterClusterMeanDistance = 0.0;

            foreach (Song centroid in centroids)
            {
                InterClusterMeanDistance += distanceFunc.GetDistance(centroid, centroidsCenter);
            }

            InterClusterMeanDistance /= clustersCount;
        }


        double AnalyzeSilhouetteCoef(List<AnalyzedSong> analyzedSongs, BaseKMeans model)
        {
            double silhouetteCoefMean = 0.0;
            
            foreach (AnalyzedSong analyzedSong in AnalyzedSongs)
            {
                int nearestClusterIndex = model.GetNearestClusterIndex(analyzedSong.AssociatedSong);
                int secondNearestClusterIndex = model.GetSecondNearestClusterIndex(analyzedSong.AssociatedSong);

                List<Song> nearestCluster = model.Clusters[nearestClusterIndex];
                List<Song> secondNearestCluster = model.Clusters[secondNearestClusterIndex];

                double nearestClusterMeanDistance = 0.0;
                double secondNearestClusterMeanDistance = 0.0;

                foreach (Song song in nearestCluster)
                {
                    double distance = model.DistanceFunc.GetDistance(song, analyzedSong.AssociatedSong);
                    nearestClusterMeanDistance += distance;
                }
                
                foreach (Song song in secondNearestCluster)
                {
                    double distance = model.DistanceFunc.GetDistance(song, analyzedSong.AssociatedSong);
                    secondNearestClusterMeanDistance += distance;
                }

                nearestClusterMeanDistance /= nearestCluster.Count;
                secondNearestClusterMeanDistance /= secondNearestCluster.Count;

                double silhouetteCoef =
                    (secondNearestClusterMeanDistance - nearestClusterMeanDistance) /
                    Math.Max(nearestClusterMeanDistance, secondNearestClusterMeanDistance);

                analyzedSong.SilhouetteCoef = silhouetteCoef;

                silhouetteCoefMean += silhouetteCoef;
            }

            silhouetteCoefMean /= analyzedSongs.Count;

            return silhouetteCoefMean;
        }

        #endregion
    }
}
