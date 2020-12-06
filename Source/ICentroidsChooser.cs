using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
{
    public interface ICentroidsChooser
    {
        List<Song> GetCentroids(List<Song> data, int clustersCount);
    }
}
