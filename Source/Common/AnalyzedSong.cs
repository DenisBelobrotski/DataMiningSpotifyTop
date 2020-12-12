namespace DataMiningSpotifyTop.Source.Common
{
    public class AnalyzedSong
    {
        #region Properties

        public Song AssociatedSong { get; set; }

        public int ClusterIndex { get; set; }
        
        public double CentroidDistance { get; set; }

        #endregion
    }
}
