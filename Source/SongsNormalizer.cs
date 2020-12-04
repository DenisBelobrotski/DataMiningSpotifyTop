using System.Collections.Generic;
using System.Linq;

namespace DataMiningSpotifyTop.Source
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

        public SongsNormalizer(List<Song> sourceSongs)
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
                Energy = 100,
                Danceability = 100,
                Loudness = 0,
                Liveness = 100,
                Valence = 100,
                Duration = 600,
                Acousticness = 100,
                Speechiness = 100,
                Popularity = 100,
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
