using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DataMiningSpotifyTop.Source
{
    public class Song
    {
        #region Fields

        [DataMember(Name = "id", Order = 0)] public string Id { get; set; }

        [DataMember(Name = "title", Order = 1)]
        public string Title { get; set; }

        [DataMember(Name = "artist", Order = 2)]
        public string Artist { get; set; }

        [DataMember(Name = "top genre", Order = 3)]
        public string Genre { get; set; }

        [DataMember(Name = "year", Order = 4)] public int Year { get; set; }

        [DataMember(Name = "bpm", Order = 5)] public int BeatsPerMinute { get; set; }

        [DataMember(Name = "nrgy", Order = 6)] public int Energy { get; set; }

        [DataMember(Name = "dnce", Order = 7)] public int Danceability { get; set; }

        [DataMember(Name = "dB", Order = 8)] public int Loudness { get; set; }

        [DataMember(Name = "live", Order = 9)] public int Liveness { get; set; }

        [DataMember(Name = "val", Order = 10)] public int Valence { get; set; }

        [DataMember(Name = "dur", Order = 11)] public int Duration { get; set; }

        [DataMember(Name = "acous", Order = 12)]
        public int Acousticness { get; set; }

        [DataMember(Name = "spch", Order = 13)]
        public int Speechiness { get; set; }

        [DataMember(Name = "pop", Order = 14)] public int Popularity { get; set; }

        #endregion



        #region Methods

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


        public Song Clone(string id)
        {
            return new Song
            {
                Id = id,
                Title = Title,
                Artist = Artist,
                Genre = Genre,
                Year = Year,
                BeatsPerMinute = BeatsPerMinute,
                Energy = Energy,
                Danceability = Danceability,
                Loudness = Loudness,
                Liveness = Liveness,
                Valence = Valence,
                Duration = Duration,
                Acousticness = Acousticness,
                Speechiness = Speechiness,
                Popularity = Popularity,
            };
        }

        #endregion
    }
}
