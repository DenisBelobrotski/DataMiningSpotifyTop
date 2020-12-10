namespace DataMiningSpotifyTop.Source.Common
{
    public interface IDistanceFunc
    {
        double GetDistance(Song from, Song to);
    }
}
