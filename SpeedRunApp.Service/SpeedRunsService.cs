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
        IConfiguration _config = null;

        public SpeedRunsService(IConfiguration Configuration)
        {
            _config = Configuration;
        }

        public SpeedRunListViewModel GetSpeedRunList()
        {
            int elementsPerPage = Convert.ToInt32(_config.GetSection("ApiSettings").GetSection("SpeedRunListElementsPerPage").Value);
            var runVMs = GetLatestSpeedRuns(SpeedRunListCategory.New, elementsPerPage, null);
            var runListVM = new SpeedRunListViewModel(runVMs, elementsPerPage);

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
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = false, EmbedLevel = false, EmbedPlatform = false };
            ClientContainer clientContainer = new ClientContainer();
            var run = clientContainer.Runs.GetRun(runID, runEmbeds);
            var runVM = new SpeedRunViewModel(run);

            return runVM;
        }

        public IEnumerable<SearchResult> SearchGamesAndUsers(string term)
        {
            ClientContainer clientContainer = new ClientContainer();
            var games = clientContainer.Games.GetGames(term).Select(i => new SearchResult { Value = i.ID, Label = i.Name, Category = "Games" });
            var users = clientContainer.Users.GetUsers(term).Select(i => new SearchResult { Value = i.ID, Label = i.Name, Category = "Users" });
            var results = games.Concat(users);

            return results;
        }
    }
}
