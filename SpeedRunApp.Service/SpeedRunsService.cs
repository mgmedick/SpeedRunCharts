using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Client;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon;
using SpeedRunApp.Interfaces.Services;

namespace SpeedRunApp.Service
{
    public class SpeedRunsService : ISpeedRunsService
    {
        public SpeedRunsService()
        {

        }

        public SpeedRunListViewModel GetLatestSpeedRuns(RunStatusType status, int? elementsOffset)
        {
            //SpeedRunListViewModel runListVM = null;
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = false, EmbedPlatform = false };
            ClientContainer clientContainer = new ClientContainer();

            var runs = clientContainer.Runs.GetRuns(status: status, orderBy: RunsOrdering.DateSubmittedDescending, elementsPerPage: 10, embeds: runEmbeds, elementsOffset: elementsOffset);
            var runVMs = runs.Where(i => i.Videos.EmbededLinks != null && i.Videos.EmbededLinks.Any(g => g != null)).Select(i => new SpeedRunViewModel(i));

            var runListVM = new SpeedRunListViewModel(runVMs, status);

            return runListVM;
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
