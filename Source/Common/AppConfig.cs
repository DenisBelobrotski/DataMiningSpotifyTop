using System;
using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source.Common
{
    [Serializable]
    public class AppConfig
    {
        public List<ExperimentConfig> ExperimentConfigs { get; set; }
    }
}
