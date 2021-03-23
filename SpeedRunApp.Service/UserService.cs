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

        public UserViewModel GetUser(string userID)
        {
            var user = _userRepo.GetUserViews(i => i.ID == userID);
            var userVM = new UserViewModel(user);

            return userVM;
        }

        public IEnumerable<SearchResult> SearchUsers(string searchText)
        {
            return _userRepo.SearchUsers(searchText);
        }

        public SpeedRunGridContainerViewModel GetSpeedRunGrid(int userID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridViewsByUserID(userID);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i));
            var games = _gameRepo.GetGamesByUserID(userID);
            var tabItems = games.Select(i => new GameViewModel(i, runVMs));
            var gridVM = new SpeedRunGridContainerViewModel(new SpeedRunGridTabViewModel("User", tabItems), runVMs);

            return gridVM;
        }
    }
}
