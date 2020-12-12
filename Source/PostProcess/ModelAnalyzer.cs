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
        
        public List<double> MeanDistances { get; private set; }
        
        public double MeanDistance { get; private set; }

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
            
            MeanDistances = new List<double>(centroids.Capacity);
            MeanDistance = 0.0;
            
            List<int> clusterCounts = new List<int>(centroids.Capacity);
            int songsCount = 0;

            for (int i = 0; i < centroids.Count; i++)
            {
                MeanDistances.Add(0.0);
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

                MeanDistances[index] += distance;
                MeanDistance += distance;

                clusterCounts[index] += 1;
                songsCount += 1;

                return analyzedSong;
            }).ToList();
            
            for (int i = 0; i < MeanDistances.Count; i++)
            {
                MeanDistances[i] /= clusterCounts[i];
            }

            MeanDistance /= songsCount;
        }


        public void SortAnalyzedSongs()
        {
            AnalyzedSongs.Sort((left, right) =>
            {
                throw new NotImplementedException();
            });
        }

        #endregion
    }
}
