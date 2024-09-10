using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Mail;
using System.Net;
using Serilog;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Interfaces.Repositories;
using System.Threading.Tasks;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model;
using System.Linq.Expressions;
using SpeedRunCommon.Extensions;

namespace SpeedRunApp.Service
{
    public class CacheService : ICacheService
    {
        public IMemoryCache _cache { get; set; }
        public IPlayerRepository _playerRepo { get; set; }
        public IGameRepository _gameRepo { get; set; }
        public ISpeedRunRepository _speedRunRepo { get; set; }
        public ILogger _logger { get; set; }

        public CacheService(IMemoryCache cache, IPlayerRepository playerRepo, IGameRepository gameRepo, ISpeedRunRepository speedRunRepo, ILogger logger)
        {
            _cache = cache;
            _playerRepo = playerRepo;
            _gameRepo = gameRepo;
            _speedRunRepo = speedRunRepo;
            _logger = logger;
        }

        public async Task RefreshCache()
        {
            try
            {
                await Task.Run(() => GetAllCache());
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "RefreshCache");              
            }
        }

        public void GetAllCache()
        {
            GetGameViews(true);
            GetPlayerViews(true);
        }

        public IEnumerable<GameView> GetGameViews(bool refresh = false)
        {
            IEnumerable<GameView> games = null;
            if (!_cache.TryGetValue<IEnumerable<GameView>>("games", out games) || refresh)
            {
                games = _gameRepo.GetGameViews();
                foreach(var game in games)
                {
                    game.SantizedName = game.Name.SanatizeName();
                    game.SantizedNameNoSpace = game.SantizedName.Replace(" ", string.Empty);
                }

                _cache.Set("games", games);
            }
            
            return games;
        }

        public IEnumerable<PlayerView> GetPlayerViews(bool refresh = false)
        {
            IEnumerable<PlayerView> players = null;
            if (!_cache.TryGetValue<IEnumerable<PlayerView>>("players", out players) || refresh)
            {
                players = _playerRepo.GetPlayerViews();
                foreach(var player in players)
                {
                    player.SantizedName = player.Name.SanatizeName();
                    player.SantizedNameNoSpace = player.SantizedName.Replace(" ", string.Empty);
                }

                _cache.Set("players", players);
            }
            
            return players;
        }        
    }
}
