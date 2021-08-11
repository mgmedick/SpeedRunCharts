using System;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;
        private readonly IGameRepository _gameRepo = null;

        public UserService(IUserRepository userRepo, ISpeedRunRepository speedRunRepo, IGameRepository gameRepo)
        {
            _userRepo = userRepo;
            _speedRunRepo = speedRunRepo;
            _gameRepo = gameRepo;
        }

        public UserViewModel GetUser(int userID)
        {
            var user = _userRepo.GetUserViews(i => i.ID == userID).FirstOrDefault();
            var userVM = new UserViewModel(user);

            return userVM;
        }

        public IEnumerable<SearchResult> SearchUsers(string searchText)
        {
            return _userRepo.SearchUsers(searchText);
        }

        public SpeedRunGridTabViewModel GetSpeedRunGridTabs(int userID)
        {
            var games = _gameRepo.GetGamesByUserID(userID);
            var tabItems = games.Select(i => new GameViewModel(i));
            //var game = tabItems.FirstOrDefault();
            //var categoryTypeID = game.CategoryTypes.Select(i => i.ID).FirstOrDefault();
            //var categoryID = game.Categories.Select(i => i.ID).FirstOrDefault();
            //var levelID = game.Levels.Select(i => i.ID).FirstOrDefault();
            var gridVM = new SpeedRunGridTabViewModel(tabItems);

            return gridVM;
        }

        /*
        public SpeedRunGridContainerViewModel GetSpeedRunGrid(int userID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridViewsByUserID(userID);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            var games = _gameRepo.GetGamesByUserID(userID);
            var tabItems = games.Select(i => new GameViewModel(i, runVMs.Where(i=>i.GameID == i.GameID).ToList()));
            var gridVM = new SpeedRunGridContainerViewModel(new SpeedRunGridTabViewModel("User", tabItems), runVMs);

            return gridVM;
        }
        */
    }
}
