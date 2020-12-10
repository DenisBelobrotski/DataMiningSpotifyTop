namespace DataMiningSpotifyTop.Source
{
    public class EuclidDistanceFunc : IDistanceFunc
    {
        public double GetDistance(Song from, Song to)
        {
            return Song.EuclidDistance(from, to);
        }
    }
}
