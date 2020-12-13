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

        BaseKMeans Model { get; }
        
        public List<AnalyzedSong> AnalyzedSongs { get; private set; }
        
        public List<double> IntraClusterMeanDistances { get; private set; }
        
        public double IntraClusterMeanDistance { get; private set; }

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
            List<Song> centroids = Model.Centroids;
            
            IntraClusterMeanDistances = new List<double>(centroids.Capacity);
            IntraClusterMeanDistance = 0.0;
            
            List<int> clusterCounts = new List<int>(centroids.Capacity);
            int songsCount = 0;

            for (int i = 0; i < centroids.Count; i++)
            {
                IntraClusterMeanDistances.Add(0.0);
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

                IntraClusterMeanDistances[index] += distance;
                IntraClusterMeanDistance += distance;

                clusterCounts[index] += 1;
                songsCount += 1;

                return analyzedSong;
            }).ToList();
            
            for (int i = 0; i < IntraClusterMeanDistances.Count; i++)
            {
                IntraClusterMeanDistances[i] /= clusterCounts[i];
            }

            IntraClusterMeanDistance /= songsCount;
        }


        public void SortAnalyzedSongs()
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

        #endregion
    }
}
