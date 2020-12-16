using System.Collections.Generic;
using DataMiningSpotifyTop.Source.Common;
using Newtonsoft.Json;

namespace DataMiningSpotifyTop.Source.PostProcess
{
    public class Prediction
    {
        #region Properties

        public string SongId => AssociatedSong.Id;

        public string Artist => AssociatedSong.Artist;

        public string Title => AssociatedSong.Title;

        public int ClusterIndex { get; set; }

        public int ClusterSize { get; set; }

        public double ClusterPortion { get; set; }

        public double CentroidDistance { get; set; }

        public List<int> ClusterSizes { get; set; }

        public List<double> ClusterPortions { get; set; }

        public List<double> CentroidDistances { get; set; }

        [JsonIgnore]
        public Song ClusterCentroid { get; set; }
        
        [JsonIgnore]
        public Song AssociatedSong { get; set; }

        #endregion



        #region Methods

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion
    }
}
