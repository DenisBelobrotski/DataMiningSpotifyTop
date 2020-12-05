using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
{
    public interface ISongsClusterizer
    {
        List<ClusterizedSong> ClusterizedSongs { get; }

        List<List<Song>> Clusters { get; }

        void Clusterize();
    }
}
