namespace DataMiningSpotifyTop.Source.Common
{
    public class SquaredEuclidDistanceFunc : IDistanceFunc
    {
        public double GetDistance(Song from, Song to)
        {
            return Song.SquaredEuclidDistance(from, to);
        }
    }
}
