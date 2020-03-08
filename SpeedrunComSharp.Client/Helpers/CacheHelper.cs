using System;
using System.Collections.Generic;
using System.Text;
using SpeedrunComSharp.Model;

namespace SpeedrunComSharp.Client
{
    public static class CacheHelper
    {
        public static IEnumerable<T> Cache<T>(this IEnumerable<T> enumerable)
        {
            return new CachedEnumerable<T>(enumerable);
        }
    }
}
