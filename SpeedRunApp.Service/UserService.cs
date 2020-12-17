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

        public Tuple<SpeedRunGridViewModel, IEnumerable<SpeedRunViewModel>> GetSpeedRunGrid(string userID)
        {
            var runs = _speedRunRepo.GetSpeedRunsByUserID(userID);
            var runVMs = runs.Select(i => new SpeedRunViewModel(i));
            var gameIDs = runVMs.Select(i => i.Game.ID).Distinct();
            var games = _gameRepo.GetGameViews(i => gameIDs.Contains(i.ID));
            var gridItems = games.Select(i => new SpeedRunGridItem(i));
            var gridVM = new SpeedRunGridViewModel("User", gridItems);

            return new Tuple<SpeedRunGridViewModel, IEnumerable<SpeedRunViewModel>>(gridVM, runVMs);
        }
    }
}
