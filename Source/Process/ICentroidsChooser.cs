using System.Collections.Generic;
using DataMiningSpotifyTop.Source.Common;

namespace DataMiningSpotifyTop.Source.Process
{
    public interface ICentroidsChooser
    {
        List<Song> GetCentroids(List<Song> songs, int clustersCount);
    }
}
