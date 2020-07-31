using SpeedRunApp.Model;
using SpeedRunApp.Client;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Interfaces.Services;
//using SpeedRunApp.Interfaces.Helpers;
using SpeedRunCommon;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace SpeedRunApp.Service
{
    public class GamesService : IGamesService
    {
        private readonly ICacheHelper _cacheHelper = null;
        private readonly IConfiguration _appConfig = null;

        public GamesService(ICacheHelper cacheHelper, IConfiguration appConfig)
        {
            _cacheHelper = cacheHelper;
            _appConfig = appConfig;
        }

        public GameDetailsViewModel GetGameDetails(string gameID)
        {
            var game = GetGame(gameID);
            var gameVM = new GameDetailsViewModel(game);

            return gameVM;
        }

        public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, int elementsPerPage, int elementsOffset)
        {
            var runs = GetGameSpeedRuns(gameID, elementsPerPage, elementsOffset);

            var ranks = runs.Where(i=>i.Status.Type == RunStatusType.Verified)
                            .OrderBy(x => x.Times.Primary)
                            .Select((g, i) => new { Key = g.ID, Rank = i + 1 })
                            .ToDictionary(x => x.Key, x => x.Rank);

            var platforms = _cacheHelper.GetPlatforms();

            var records = runs.Select(i => (SpeedRunRecord)i).ToList();
            records.ForEach(i => { if (ranks.ContainsKey(i.ID)) { i.Rank = ranks[i.ID]; } i.System.Platform = platforms.FirstOrDefault(g => g.ID == i.System.PlatformID); });
            var recordVMs = records.Select(i => new SpeedRunRecordViewModel(i));

            //var platforms = _cacheHelper.GetPlatforms();
            //foreach (var run in runs)
            //{
            //    var runVM = new SpeedRunViewModel(run);
            //    //if (includeExaminer)
            //    //{
            //    //    var examiner = clientContainer.Users.GetUser(run.Status.ExaminerUserID);
            //    //    runVM.ExaminerID = examiner?.ID;
            //    //    runVM.ExaminerName = examiner?.Name;
            //    //}

            //    runVM.PlatformName = platforms.Where(i => i.ID == runVM.PlatformID).Select(i => i.Name).FirstOrDefault();
            //    runVMs.Add(runVM);
            //}

            return recordVMs;
        }

        public Game GetGame(string gameID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var gameEmbeds = new GameEmbeds { EmbedCategories = true, EmbedLevels = true, EmbedModerators = true, EmbedPlatforms = true };
            var game = clientContainer.Games.GetGame(gameID, gameEmbeds);

            return game;
        }

        public IEnumerable<SpeedRun> GetGameSpeedRuns(string gameID, int elementsPerPage, int elementsOffset)
        {
            ClientContainer clientContainer = new ClientContainer();
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = false, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = false, EmbedPlatform = false };
            var runs = clientContainer.Runs.GetRuns(gameId: gameID, elementsPerPage: elementsPerPage, elementsOffset: elementsOffset, embeds: runEmbeds);

            return runs;
        }
    }
}
