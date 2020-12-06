using System;
using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
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

        public List<Song> GetCentroids(List<Song> data, int clustersCount)
        {
            Song randomPoint = data.RandomObject();
            List<double> distances = new List<double>(data.Capacity);
            
            throw new NotImplementedException();
        }

        #endregion
    }
}
