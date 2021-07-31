using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
//using SpeedRunApp.Interfaces.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class MenuService : IMenuService
    {
        private readonly IGameService _gamesService = null;
        private readonly IUserService _userService = null;

        public MenuService(IGameService gamesService, IUserService userService)
        {
            _gamesService = gamesService;
            _userService = userService;
        }

        public IEnumerable<SearchResult> Search(string searchText)
        {
            var results = new List<SearchResult>();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.Trim();
                var games = _gamesService.SearchGames(searchText).ToList();
                var gamesGroup = new SearchResult { Value = "0", Label = "Games", SubItems = games };
                var users = _userService.SearchUsers(searchText);
                var usersGroup = new SearchResult { Value = "0", Label = "Users", SubItems = users };

                results.Add(gamesGroup);
                results.Add(usersGroup);
            }

            return results;
        }
    }
}
