using System;
using System.Collections.Generic;

namespace DataMiningSpotifyTop.Source
{
    public static class ListExtensions
    {
        static readonly Random Random = new Random();
        
        public static T RandomObject<T>(this IList<T> list) 
        {
            return list.Count > 0 ? list[Random.Next(0, list.Count)] : default;
        }
    }
}
