using System;
using System.Collections.Generic;
using DataMiningSpotifyTop.Source.Common;
using DataMiningSpotifyTop.Source.Util;

namespace DataMiningSpotifyTop.Source.Process
{
    public class PlusPlusCentroidsChooser : ICentroidsChooser
    {
        #region Properties

        IDistanceFunc DistanceFunc { get; }

        #endregion



        #region Object lifecycle

        public PlusPlusCentroidsChooser(IDistanceFunc distanceFunc)
        {
            DistanceFunc = distanceFunc;
        }

        #endregion



        #region Methods

        public List<Song> GetCentroids(List<Song> songs, int clustersCount)
        {
            List<Song> centroids = new List<Song>(clustersCount) { songs.RandomObject().Clone() };
            Random random = new Random();

            for (int centroidStep = 1; centroidStep < clustersCount; centroidStep++)
            {
                Song nextCentroid = GetNextCentroid(songs, centroids, random);
                centroids.Add(nextCentroid);
            }

            return centroids;
        }


        Song GetNextCentroid(List<Song> songs, List<Song> centroids, Random random)
        {
            List<double> minDistances = new List<double>(songs.Capacity);
            double minDistancesSum = 0.0;

            foreach (Song song in songs)
            {
                double minDistance = GetCentroidMinDistance(song, centroids, DistanceFunc);
                minDistances.Add(minDistance);
                minDistancesSum += minDistance;
            }

            double randomSum = minDistancesSum * random.NextDouble();
            double temporarySum = 0.0;
            Song nextCentroid = null;

            for (int pointIndex = 0; pointIndex < minDistances.Count; pointIndex++)
            {
                temporarySum += minDistances[pointIndex];

                if (temporarySum > randomSum)
                {
                    nextCentroid = songs[pointIndex];

                    break;
                }
            }

            return nextCentroid;
        }


        double GetCentroidMinDistance(Song song, List<Song> centroids, IDistanceFunc distanceFunc)
        {
            double minDistance = distanceFunc.GetDistance(song, centroids[0]);

            for (int centroidIndex = 0; centroidIndex < centroids.Count; centroidIndex++)
            {
                minDistance = Math.Min(minDistance, distanceFunc.GetDistance(song, centroids[centroidIndex]));
            }

            return minDistance;
        }

        #endregion
    }
}
