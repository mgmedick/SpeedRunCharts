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
            var records = GetGameSpeedRuns(gameID, elementsPerPage, elementsOffset).Select(i => (SpeedRunRecord)i).ToList();
            var verifiedRecords = records.Where(i => i.Status.Type == RunStatusType.Verified && i.Times.Primary.HasValue);

            //var ranks = verifiedRecords.GroupBy(g=> new { g.GameID, g.CategoryID, g.LevelID })
            //                .OrderBy(i => i)
            //                .Select((g, i) => new { Key = g, Rank = i + 1 })
            //                .ToDictionary(x => x.Key, x => x.Rank);

            //var ranks4 = verifiedRecords.GroupBy(g => new { g.GameID, g.CategoryID, g.LevelID })
            //                        .SelectMany(g =>
            //                            g.OrderBy(i => i.Times.Primary).Select((j, i) => new { j.ID, j.GameID, j.CategoryID, j.LevelID, Rank = i + 1 })
            //                        ).OrderBy(i => i.GameID)
            //                        .ThenBy(i => i.CategoryID)
            //                        .ThenBy(i => i.LevelID)
            //                        .ThenBy(i => i.Rank);

            //var ranks2 = verifiedRecords.GroupBy(g => new { g.GameID, g.CategoryID, g.LevelID })
            //                        .Select(c => c.OrderBy(o => o.Times.Primary.Value).Select((v, i) => new { i, v }).ToList())
            //                        .SelectMany(c => c)
            //                        .Select(c => new { c.v.ID, c.v.GameID, c.v.CategoryID, c.v.LevelID, Rank = c.i + 1 })
            //                        .ToList().OrderBy(i => i.GameID)
            //                        .ThenBy(i => i.CategoryID)
            //                        .ThenBy(i => i.LevelID)
            //                        .ThenBy(i => i.Rank);

            //var ranks = verifiedRecords.GroupBy(g => new { g.GameID, g.CategoryID, g.LevelID })
            //                        .SelectMany(g =>
            //                            g.OrderBy(i=>i.Times.Primary).Select((j, i) => new { Key = j.ID, Rank = i + 1 })
            //                        ).ToDictionary(x => x.Key, x => x.Rank);

            //var categories = _cacheHelper.GetPlatforms();
            var platforms = _cacheHelper.GetPlatforms();

            foreach (var record in verifiedRecords)
            {
                //record.Rank = ranks4.Where(i => i.ID == record.ID).Select(i => i.Rank).FirstOrDefault();
                record.System.Platform = platforms.FirstOrDefault(g => g.ID == record.System.PlatformID);
            }

            var recordVMs = records.Select(i => new SpeedRunRecordViewModel(i)).OrderBy(i => i.PrimaryRunTime);

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
            var runEmbeds = new SpeedRunEmbeds { EmbedGame = false, EmbedPlayers = true, EmbedCategory = false, EmbedLevel = false, EmbedPlatform = false };
            var runs = clientContainer.Runs.GetRuns(gameId: gameID, status: RunStatusType.Verified, elementsPerPage: elementsPerPage, elementsOffset: elementsOffset,  embeds: runEmbeds);

            return runs;
        }
    }
}
