using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DataMiningSpotifyTop.Source.Common
{
    public class Song
    {
        #region Fields

        // not important
        [DataMember(Name = "id", Order = 0)]
        public string Id { get; set; }

        // not important
        [DataMember(Name = "title", Order = 1)]
        public string Title { get; set; }

        // not important
        [DataMember(Name = "artist", Order = 2)]
        public string Artist { get; set; }

        // not important
        [DataMember(Name = "top genre", Order = 3)]
        public string Genre { get; set; }

        // not important
        [DataMember(Name = "year", Order = 4)]
        public int Year { get; set; }

        [DataMember(Name = "bpm", Order = 5)]
        public double BeatsPerMinute { get; set; }

        [DataMember(Name = "nrgy", Order = 6)]
        public double Energy { get; set; }

        [DataMember(Name = "dnce", Order = 7)]
        public double Danceability { get; set; }

        [DataMember(Name = "dB", Order = 8)]
        public double Loudness { get; set; }

        [DataMember(Name = "live", Order = 9)]
        public double Liveness { get; set; }

        [DataMember(Name = "val", Order = 10)]
        public double Valence { get; set; }

        [DataMember(Name = "dur", Order = 11)]
        public double Duration { get; set; }

        [DataMember(Name = "acous", Order = 12)]
        public double Acousticness { get; set; }

        [DataMember(Name = "spch", Order = 13)]
        public double Speechiness { get; set; }

        // not important
        [DataMember(Name = "pop", Order = 14)]
        public double Popularity { get; set; }

        #endregion



        #region Methods

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


        public Song Clone()
        {
            return Clone(Id);
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
                // Popularity = Popularity,
            };
        }


        public Song CloneCleared(string id)
        {
            Song clone = Clone(id);
            clone.Title = id;
            clone.Artist = id;
            clone.Genre = id;
            clone.Year = 0;
            clone.Popularity = 0.0f;

            return clone;
        }


        public void Normalize(Song min, Song max)
        {
            BeatsPerMinute = NormalizedValue(BeatsPerMinute, min.BeatsPerMinute, max.BeatsPerMinute);
            Energy = NormalizedValue(Energy, min.Energy, max.Energy);
            Danceability = NormalizedValue(Danceability, min.Danceability, max.Danceability);
            Loudness = NormalizedValue(Loudness, min.Loudness, max.Loudness);
            Liveness = NormalizedValue(Liveness, min.Liveness, max.Liveness);
            Valence = NormalizedValue(Valence, min.Valence, max.Valence);
            Duration = NormalizedValue(Duration, min.Duration, max.Duration);
            Acousticness = NormalizedValue(Acousticness, min.Acousticness, max.Acousticness);
            Speechiness = NormalizedValue(Speechiness, min.Speechiness, max.Speechiness);
            // Popularity = NormalizedValue(Popularity, min.Popularity, max.Popularity);
        }


        double NormalizedValue(double value, double min, double max)
        {
            return (value - min) / (max - min);
        }


        public Song Normalized(Song min, Song max)
        {
            Song result = Clone();
            result.Normalize(min, max);

            return result;
        }


        public void Add(Song value)
        {
            BeatsPerMinute += value.BeatsPerMinute;
            Energy += value.Energy;
            Danceability += value.Danceability;
            Loudness += value.Loudness;
            Liveness += value.Liveness;
            Valence += value.Valence;
            Duration += value.Duration;
            Acousticness += value.Acousticness;
            Speechiness += value.Speechiness;
            // Popularity += value.Popularity;
        }


        public void Divide(double divider)
        {
            BeatsPerMinute /= divider;
            Energy /= divider;
            Danceability /= divider;
            Loudness /= divider;
            Liveness /= divider;
            Valence /= divider;
            Duration /= divider;
            Acousticness /= divider;
            Speechiness /= divider;
            // Popularity /= divider;
        }


        public void CopyValues(Song from)
        {
            BeatsPerMinute = from.BeatsPerMinute;
            Energy = from.Energy;
            Danceability = from.Danceability;
            Loudness = from.Loudness;
            Liveness = from.Liveness;
            Valence = from.Valence;
            Duration = from.Duration;
            Acousticness = from.Acousticness;
            Speechiness = from.Speechiness;
            Popularity = from.Popularity;
        }


        public static double SquaredEuclidDistance(Song from, Song to)
        {
            return
                DistanceSquareComponent(from.BeatsPerMinute, to.BeatsPerMinute) +
                DistanceSquareComponent(from.Energy, to.Energy) +
                DistanceSquareComponent(from.Danceability, to.Danceability) +
                DistanceSquareComponent(from.Loudness, to.Loudness) +
                DistanceSquareComponent(from.Liveness, to.Liveness) +
                DistanceSquareComponent(from.Valence, to.Valence) +
                DistanceSquareComponent(from.Duration, to.Duration) +
                DistanceSquareComponent(from.Acousticness, to.Acousticness) +
                DistanceSquareComponent(from.Speechiness, to.Speechiness);

            // DistanceSquareComponent(from.Popularity, to.Popularity);
        }


        static double DistanceSquareComponent(double from, double to)
        {
            return (from - to) * (from - to);
        }


        public static double EuclidDistance(Song from, Song to)
        {
            return Math.Sqrt(SquaredEuclidDistance(from, to));
        }

        #endregion
    }
}
