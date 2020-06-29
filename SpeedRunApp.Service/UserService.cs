using System.Collections.Generic;
using System.Linq;
using System;
using SpeedRunApp.Model;
using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Client;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Interfaces.Services;
using SpeedRunCommon;

namespace SpeedRunApp.Service
{
    public class UserService : IUserService
    {
        private readonly IMemoryCache _cache = null;

        public UserService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public UserDetailsViewModel GetUser(string userID, bool cacheSpeedRuns)
        {
            ClientContainer clientContainer = new ClientContainer();
            var user = clientContainer.Users.GetUser(userID);
            var userVM = new UserDetailsViewModel(user);

            //if (cacheSpeedRuns)
            //{
            //    var userSpeedRunsCacheKey = "userSpeedRuns_" + userID;
            //    var userSpeedRuns = (IEnumerable<SpeedRunDTO>)_cache.Get(userSpeedRunsCacheKey);
            //    if (userSpeedRuns == null)
            //    {
            //        userSpeedRuns = userDTO.SpeedRuns;
            //        _cache.Set(userSpeedRunsCacheKey, userSpeedRuns, new DateTimeOffset(DateTime.Now.AddMinutes(5)));
            //    }
            //}

            return userVM;
        }

        //public IEnumerable<SpeedRunDTO> GetUserSpeedRuns(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID)
        //{
        //    var speedRuns = GetUserSpeedRuns(userID);

        //    if (categoryType == CategoryType.PerGame)
        //    {
        //        speedRuns = speedRuns.Where(i => i.Game.ID == gameID && i.Category.Type == categoryType && i.CategoryID == categoryID);
        //    }
        //    else
        //    {
        //        speedRuns = speedRuns.Where(i => i.Game.ID == gameID && i.Category.Type == categoryType && i.CategoryID == categoryID && i.LevelID == levelID);
        //    }

        //    return speedRuns;
        //}

        //public IEnumerable<SpeedRunDTO> GetUserSpeedRuns(string userID)
        //{
        //    var userSpeedRunsCacheKey = "userSpeedRuns_" + userID;
        //    var userSpeedRuns = (IEnumerable<SpeedRunDTO>)_cache.Get(userSpeedRunsCacheKey);
        //    if (userSpeedRuns == null)
        //    {
        //        ClientContainer clientContainer = new ClientContainer();
        //        var runs = clientContainer.Runs.GetRuns(userId: userID);
        //        userSpeedRuns = runs.Select(i => new SpeedRunDTO(i));
        //    }

        //    return userSpeedRuns.OrderBy(i => i.PrimaryRunTime);
        //}
    }
}
