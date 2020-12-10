namespace DataMiningSpotifyTop.Source.Common
{
    public class EuclidDistanceFunc : IDistanceFunc
    {
        public double GetDistance(Song from, Song to)
        {
            return Song.EuclidDistance(from, to);
        }
    }
}
