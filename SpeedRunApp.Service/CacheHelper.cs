using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Service
{
    public class CacheHelper : ICacheHelper
    {
        public IMemoryCache Cache { get; set; }
        public IPlatformService _platformService { get; set; }

        public CacheHelper(IMemoryCache cache, IPlatformService platformService)
        {
            Cache = cache;
            _platformService = platformService;
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            IEnumerable<Platform> platforms = null;
            if (!Cache.TryGetValue<IEnumerable<Platform>>("platforms", out platforms))
            {
                // not found in cache, obtain it
                platforms = _platformService.GetPlatforms();
                Cache.Set("platforms", platforms);
            }

            return platforms;
        }
    }
}
