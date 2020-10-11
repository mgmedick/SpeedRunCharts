using SpeedRunApp.Client;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace SpeedRunApp.Service
{
    public class SpeedRunsService : ISpeedRunsService
    {
        private readonly IConfiguration _config = null;
        private readonly ICacheHelper _cacheHelper = null;
        private readonly IGamesService _gamesService = null;

        public SpeedRunsService(IConfiguration Configuration, ICacheHelper cacheHelper, IGamesService gamesService)
        {
            _config = Configuration;
            _cacheHelper = cacheHelper;
            _gamesService = gamesService;
        }

        public SpeedRunListViewModel GetSpeedRunList()
        {
            int elementsPerPage = Convert.ToInt32(_config.GetSection("ApiSettings").GetSection("SpeedRunListElementsPerPage").Value);
            var runVMs = GetLatestSpeedRuns(SpeedRunListCategory.New, elementsPerPage, null);
            var statusTypes = new List<IDNamePair> { new IDNamePair { ID = ((int)RunStatusType.New).ToString(), Name = RunStatusType.New.ToString() },
                                                    new IDNamePair { ID = ((int)RunStatusType.Verified).ToString(), Name = RunStatusType.Verified.ToString()},
                                                    new IDNamePair { ID = ((int)RunStatusType.Rejected).ToString(), Name = RunStatusType.Rejected.ToString()} };
            var categoryTypes = new List<IDNamePair> { new IDNamePair { ID = ((int)CategoryType.PerGame).ToString(), Name = CategoryType.PerGame.ToString() },
                                                        new IDNamePair { ID = ((int)CategoryType.PerLevel).ToString(), Name = CategoryType.PerLevel.ToString() } };
            var platforms = _cacheHelper.GetPlatforms().Select(i => new IDNamePair { ID = i.ID, Name = i.Name });
            var runListVM = new SpeedRunListViewModel(runVMs, statusTypes, categoryTypes, platforms);

            return runListVM;
        }

        public IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory category, int elementsPerPage, int? elementsOffset)
        {
            IEnumerable<SpeedRunViewModel> runVMs = null;
            switch (category)
            {
                case SpeedRunListCategory.New:
                    runVMs = GetSpeedRuns(RunStatusType.New, RunsOrdering.DateSubmittedDescending, elementsPerPage, elementsOffset);
                    break;
                case SpeedRunListCategory.Verified:
                    runVMs = GetSpeedRuns(RunStatusType.Verified, RunsOrdering.VerifyDateDescending, elementsPerPage, elementsOffset);
                    break;
                case SpeedRunListCategory.Rejected:
                    runVMs = GetSpeedRuns(RunStatusType.Rejected, RunsOrdering.DateSubmittedDescending, elementsPerPage, elementsOffset);
                    break;
            }

            return runVMs;
        }

        public IEnumerable<SpeedRunViewModel> GetSpeedRuns(RunStatusType status, RunsOrdering orderBy, int elementsPerPage, int? elementsOffset)
        {
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = false, EmbedPlatform = false };
            ClientContainer clientContainer = new ClientContainer();

            var runs = clientContainer.Runs.GetRuns(status: status, orderBy: orderBy, elementsPerPage: elementsPerPage, embeds: runEmbeds, elementsOffset: elementsOffset);
            var runVMs = runs.Where(i => i.Videos.EmbededLinks != null && i.Videos.EmbededLinks.Any(g => g != null)).Select(i => new SpeedRunViewModel(i));

            return runVMs;
        }

        public SpeedRunViewModel GetSpeedRun(string runID)
        {
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = false, EmbedPlatform = false };
            ClientContainer clientContainer = new ClientContainer();
            var run = clientContainer.Runs.GetRun(runID, runEmbeds);
            var runVM = new SpeedRunViewModel(run);

            return runVM;
        }

        public EditSpeedRunViewModel GetEditSpeedRun(string runID, bool isReadOnly)
        {
            ClientContainer clientContainer = new ClientContainer();
         
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = false, EmbedPlatform = false };
            var run = clientContainer.Runs.GetRun(runID, runEmbeds);
            var runVM = new SpeedRunViewModel(run);

            var gameDetails = _gamesService.GetGameDetails(runVM.Game.ID);
            var statusTypes = new List<IDNamePair> { new IDNamePair { ID = ((int)RunStatusType.New).ToString(), Name = RunStatusType.New.ToString() },
                                                    new IDNamePair { ID = ((int)RunStatusType.Verified).ToString(), Name = RunStatusType.Verified.ToString()},
                                                    new IDNamePair { ID = ((int)RunStatusType.Rejected).ToString(), Name = RunStatusType.Rejected.ToString()} };
            var editSpeedRunVM = new EditSpeedRunViewModel(statusTypes, gameDetails.CategoryTypes, gameDetails.Categories, gameDetails.Levels, gameDetails.Platforms, gameDetails.Variables, runVM, isReadOnly);

            return editSpeedRunVM;
        }

        public IEnumerable<SearchResult> SearchGamesAndUsers(string term)
        {
            var games = SearchGames(term);
            var users = SearchUsers(term);
            var results = games.Concat(users);

            return results;
        }

        public IEnumerable<SearchResult> SearchGames(string term)
        {
            ClientContainer clientContainer = new ClientContainer();
            var games = clientContainer.Games.GetGames(term).Select(i => new SearchResult { Value = i.ID, Label = i.Name, Category = "Games" });

            return games;
        }

        public IEnumerable<SearchResult> SearchUsers(string term)
        {
            ClientContainer clientContainer = new ClientContainer();
            var users = clientContainer.Users.GetUsers(term).Select(i => new SearchResult { Value = i.ID, Label = i.Name, Category = "Users" });

            return users;
        }
    }
}
