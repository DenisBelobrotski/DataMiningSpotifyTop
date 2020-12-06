namespace DataMiningSpotifyTop.Source
{
    public interface IDistanceFunc
    {
        double GetDistance(Song from, Song to);
    }
}
