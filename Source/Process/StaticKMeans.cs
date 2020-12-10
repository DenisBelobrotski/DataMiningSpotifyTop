using System.Collections.Generic;
using System.Linq;

namespace DataMiningSpotifyTop.Source
{
    public class StaticKMeans : BaseKMeans
    {
        #region Fields

        int clustersCount = 0;

        #endregion



        #region Properties

        public override int ClustersCount => clustersCount;

        public KMeansModel Model { get; }

        #endregion



        #region Object lifecycle

        public StaticKMeans(List<Song> songs, KMeansModel model)
        {
            Songs = songs;
            Model = model;
        }

        #endregion



        #region Methods

        public void ReadModel()
        {
            Centroids = Model.Centroids;
            clustersCount = Centroids.Count;

            ClusterizedSongs = Songs.Select(song => new ClusterizedSong { Song = song, ClusterIndex = int.MinValue })
                                    .ToList();

            Clusters = new List<List<Song>>(ClustersCount);

            for (int i = 0; i < ClustersCount; i++)
            {
                Clusters.Add(new List<Song>());
            }

            IterateClusters();
            RegroupClusters();
        }

        #endregion
    }
}
