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
            userVM.ProfileImage = clientContainer.Users.GetUserProfileImageUri(user.Name);

            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true, EmbedPlatform = true };
            userVM.SpeedRuns = clientContainer.Runs.GetRuns(userId: userID, embeds: runEmbeds).ToList();

            return userVM;
        }

        public IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true, EmbedPlatform = true };
            var runs = clientContainer.Runs.GetRuns(userId: userID, embeds: runEmbeds);

            if (categoryType == CategoryType.PerGame)
            {
                runs = runs.Where(i => i.Game.ID == gameID && i.Category.Type == categoryType && i.CategoryID == categoryID);
            }
            else
            {
                runs = runs.Where(i => i.Game.ID == gameID && i.Category.Type == categoryType && i.CategoryID == categoryID && i.LevelID == levelID);
            }

            var runVMs = runs.Select(i => new SpeedRunViewModel(i));

            return runVMs;
        }
    }
}
