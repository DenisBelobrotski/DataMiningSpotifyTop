using System;

namespace DataMiningSpotifyTop.Source.Common
{
    [Serializable]
    public class ExperimentConfig
    {
        public int ModelsCount { get; set; }
        
        public int ClustersCount { get; set; }
    }
}
