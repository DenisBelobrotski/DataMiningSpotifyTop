using System.Collections.Generic;
using System.Linq;
using DataMiningSpotifyTop.Source.Common;

namespace DataMiningSpotifyTop.Source.PreProcess
{
    public class SongsNormalizer
    {
        #region Properties

        Song MinValues { get; }
        Song MaxValues { get; }

        public List<Song> SourceSongs { get; }
        public List<Song> NormalizedSongs { get; private set; }

        #endregion



        #region Object lifecycle

        public SongsNormalizer(List<Song> sourceSongs, bool isPercentageValues)
        {
            MinValues = new Song
            {
                BeatsPerMinute = 0,
                Energy = 0,
                Danceability = 0,
                Loudness = -60,
                Liveness = 0,
                Valence = 0,
                Duration = 0,
                Acousticness = 0,
                Speechiness = 0,
                Popularity = 0,
            };

            MaxValues = new Song
            {
                BeatsPerMinute = 300,
                Energy = isPercentageValues ? 100 : 1,
                Danceability = isPercentageValues ? 100 : 1,
                Loudness = 0,
                Liveness = isPercentageValues ? 100 : 1,
                Valence = isPercentageValues ? 100 : 1,
                Duration = 600,
                Acousticness = isPercentageValues ? 100 : 1,
                Speechiness = isPercentageValues ? 100 : 1,
                Popularity = isPercentageValues ? 100 : 1,
            };

            SourceSongs = sourceSongs;
        }

        #endregion



        #region Methods

        public void Normalize()
        {
            NormalizedSongs = SourceSongs.Select(song => song.Clone().Normalized(MinValues, MaxValues)).ToList();
        }

        #endregion
    }
}
