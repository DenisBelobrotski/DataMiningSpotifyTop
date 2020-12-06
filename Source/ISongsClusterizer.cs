using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
{
    public interface ISongsClusterizer
    {
        List<ClusterizedSong> ClusterizedSongs { get; }

        List<List<Song>> Clusters { get; }
        
        List<Song> Centroids { get; }

        void Clusterize();

        int GetAssociatedClusterIndex(Song song);
    }
}
