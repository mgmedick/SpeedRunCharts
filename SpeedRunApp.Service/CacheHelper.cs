using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Interfaces.Services;
using System.Collections.Generic;

namespace SpeedRunApp.Service
{
    public class CacheHelper : ICacheHelper
    {
        public IMemoryCache _cache { get; set; }
        //public IGameService _gameService { get; set; }

        public CacheHelper(IMemoryCache cache)//, IGameService gameService)
        {
            _cache = cache;
            //_gameService = gameService;
        }

        //public IEnumerable<Vari> GetPlatforms()
        //{
        //    IEnumerable<Platform> platforms = null;
        //    if (!Cache.TryGetValue<IEnumerable<Platform>>("platforms", out platforms))
        //    {
        //        // not found in cache, obtain it
        //        platforms = _platformService.GetPlatforms();
        //        Cache.Set("platforms", platforms);
        //    }

        //    return platforms;
        //}
    }
}
