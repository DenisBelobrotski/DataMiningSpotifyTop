using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMiningSpotifyTop.Source
{
    public class OriginalSongsAnalyzer
    {
        #region Properties

        public Song MinSongsValues { get; private set; }

        public Song MaxSongsValues { get; private set; }

        public Dictionary<string, List<Song>> SongsByGenre { get; private set; }

        public Dictionary<int, List<Song>> SongsByYear { get; private set; }

        List<Song> Songs { get; }

        #endregion



        #region Object lifecycle

        public OriginalSongsAnalyzer(List<Song> songs)
        {
            Songs = songs;
        }

        #endregion



        #region Methods

        public void Analyze()
        {
            if (Songs.Count <= 0)
            {
                throw new Exception("Empty songs list.");
            }

            MinSongsValues = Songs.First().Clone("min");
            MinSongsValues.Title = "min";
            MinSongsValues.Artist = "min";
            MinSongsValues.Genre = "min";

            MaxSongsValues = Songs.First().Clone("max");
            MaxSongsValues.Title = "max";
            MaxSongsValues.Artist = "max";
            MaxSongsValues.Genre = "max";

            SongsByGenre = new Dictionary<string, List<Song>>();
            SongsByYear = new Dictionary<int, List<Song>>();

            foreach (Song song in Songs)
            {
                MinSong(MinSongsValues, song);
                MaxSong(MaxSongsValues, song);

                AddSongByGenre(SongsByGenre, song);
                AddSongByYear(SongsByYear, song);
            }
        }


        void MinSong(Song min, Song candidate)
        {
            min.Year = Math.Min(min.Year, candidate.Year);
            min.BeatsPerMinute = Math.Min(min.BeatsPerMinute, candidate.BeatsPerMinute);
            min.Energy = Math.Min(min.Energy, candidate.Energy);
            min.Danceability = Math.Min(min.Danceability, candidate.Danceability);
            min.Loudness = Math.Min(min.Loudness, candidate.Loudness);
            min.Liveness = Math.Min(min.Liveness, candidate.Liveness);
            min.Valence = Math.Min(min.Valence, candidate.Valence);
            min.Duration = Math.Min(min.Duration, candidate.Duration);
            min.Acousticness = Math.Min(min.Acousticness, candidate.Acousticness);
            min.Speechiness = Math.Min(min.Speechiness, candidate.Speechiness);
            min.Popularity = Math.Min(min.Popularity, candidate.Popularity);
        }


        void MaxSong(Song max, Song candidate)
        {
            max.Year = Math.Max(max.Year, candidate.Year);
            max.BeatsPerMinute = Math.Max(max.BeatsPerMinute, candidate.BeatsPerMinute);
            max.Energy = Math.Max(max.Energy, candidate.Energy);
            max.Danceability = Math.Max(max.Danceability, candidate.Danceability);
            max.Loudness = Math.Max(max.Loudness, candidate.Loudness);
            max.Liveness = Math.Max(max.Liveness, candidate.Liveness);
            max.Valence = Math.Max(max.Valence, candidate.Valence);
            max.Duration = Math.Max(max.Duration, candidate.Duration);
            max.Acousticness = Math.Max(max.Acousticness, candidate.Acousticness);
            max.Speechiness = Math.Max(max.Speechiness, candidate.Speechiness);
            max.Popularity = Math.Max(max.Popularity, candidate.Popularity);
        }


        void AddSongByGenre(Dictionary<string, List<Song>> songsByGenre, Song song)
        {
            string genre = song.Genre;

            if (!songsByGenre.TryGetValue(genre, out List<Song> genreSongs))
            {
                songsByGenre[genre] = new List<Song> { song };
            }
            else
            {
                genreSongs.Add(song);
            }
        }


        void AddSongByYear(Dictionary<int, List<Song>> songsByYear, Song song)
        {
            int year = song.Year;

            if (!SongsByYear.TryGetValue(year, out List<Song> yearSongs))
            {
                songsByYear[year] = new List<Song> { song };
            }
            else
            {
                yearSongs.Add(song);
            }
        }

        #endregion
    }
}
