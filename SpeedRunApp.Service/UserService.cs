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
        private readonly ICacheService _cacheService = null;

        public UserService(IUserRepository userRepo, ISpeedRunRepository speedRunRepo, IGameRepository gameRepo, ICacheService cacheService)
        {
            _userRepo = userRepo;
            _speedRunRepo = speedRunRepo;
            _gameRepo = gameRepo;
            _cacheService = cacheService;
        }

        public UserDetailsViewModel GetUserDetails(string userAbbr, int? speedRunID)
        {
            var userVM = GetUser(userAbbr);
            var userDetailsVM = new UserDetailsViewModel(userVM, speedRunID);

            return userDetailsVM;
        }

        public UserViewModel GetUser(string userAbbr)
        {
            var user = _userRepo.GetUserViews(i => i.Abbr == userAbbr).FirstOrDefault();
            var userVM = user != null ? new UserViewModel(user) : null;

            return userVM;
        }

        public UserViewModel GetUser(int userID)
        {
            var user = _userRepo.GetUserViews(i => i.ID == userID).FirstOrDefault();
            var userVM = user != null ? new UserViewModel(user) : null;

            return userVM;
        }

        public IEnumerable<SearchResult> SearchUsers(string searchText)
        {
            return _userRepo.SearchUsers(searchText);
        }
    }
}
