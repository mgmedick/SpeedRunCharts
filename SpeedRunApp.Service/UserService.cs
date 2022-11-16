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
        private readonly ISettingRepository _settingRepo = null;

        public UserService(IUserRepository userRepo, ISpeedRunRepository speedRunRepo, IGameRepository gameRepo, ICacheService cacheService, ISettingRepository settingRepo)
        {
            _userRepo = userRepo;
            _speedRunRepo = speedRunRepo;
            _gameRepo = gameRepo;
            _cacheService = cacheService;
            _settingRepo = settingRepo;
        }

        public UserDetailsViewModel GetUserDetails(string userAbbr, string speedRunComID)
        {
            var userVM = GetUser(userAbbr);
            var speedRunID = string.IsNullOrWhiteSpace(speedRunComID) ? (int?)null : _speedRunRepo.GetSpeedRunID(speedRunComID);
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

         public List<string> SetUserIsChanged(int userID)
        {
            var errorMessages = new List<string>();
            var isBulkReloadRunning = _settingRepo.GetSetting("IsBulkReloadRunning")?.Num == 1;
            
            if (isBulkReloadRunning)
            {
                errorMessages.Add("Import is running Bulk Reload, Games cannot be updated until complete");
            }
            else
            {
                var user = _userRepo.GetUsers(i => i.ID == userID).FirstOrDefault();
                if (user != null) {
                    if (user.IsChanged.HasValue && user.IsChanged.Value){
                        errorMessages.Add("User is alreay updating");
                    } else {
                        user.IsChanged = true;
                        _userRepo.UpdateUserIsChanged(user);
                    }
                }
            }

            return errorMessages;
        }        
    }
}
