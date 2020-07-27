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

        public SpeedRunListViewModel GetLatestSpeedRuns(int? elementsOffset = null)
        {
            //SpeedRunListViewModel runListVM = null;
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true, EmbedPlatform = true };
            ClientContainer clientContainer = new ClientContainer();
            var runs = clientContainer.Runs.GetRuns(status: RunStatusType.New, orderBy: RunsOrdering.DateSubmittedDescending, elementsPerPage: 10, embeds: runEmbeds, elementsOffset: elementsOffset);
            var runVMs = runs.Where(i => i.Videos.EmbededLinks != null && i.Videos.EmbededLinks.Any(g => g != null)).Select(i => new SpeedRunViewModel(i));
            var runListVM = new SpeedRunListViewModel(runVMs);
            //foreach (var run in runs)
            //{
            //    var runVM = new SpeedRunViewModel(run);
            //    if (!string.IsNullOrWhiteSpace(run.Status.ExaminerUserID))
            //    {
            //        var examiner = clientContainer.Users.GetUser(run.Status.ExaminerUserID);
            //        runVM.ExaminerName = examiner.Name;
            //    }

            //    runVMs.Add(runVM);
            //}

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
    }
}
