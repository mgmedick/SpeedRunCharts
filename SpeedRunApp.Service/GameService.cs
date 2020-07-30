﻿using SpeedRunApp.Model;
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

        //public SpeedRunGridViewModel GetGameSpeedRunGrid(string gameID)
        //{
        //    var game = GetGame(gameID);
        //    var categoryTypes = game.CategoryTypes
        //            .Select(i => new IDNamePair { ID = ((int)i).ToString(), Name = i.ToString() })
        //            .ToList();

        //    var games = new List<GameDisplay>() { new GameDisplay { ID = game.ID, Name = game.Name, CategoryTypeIDs = game.Categories.Select(i => ((int)i.Type).ToString()).Distinct() } };

        //    var categories = game.Categories
        //                    .Select(i => new CategoryDisplay { ID = i.ID, Name = i.Name, CategoryTypeID = ((int)i.Type).ToString(), GameID = i.GameID })
        //                    .ToList();

        //    var levels = game.Levels
        //                 .Select(i => new LevelDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID })
        //                 .ToList();

        //    var records = GetGameSpeedRunRecords(game, false);

        //    return new SpeedRunGridViewModel("Game", categoryTypes, games, categories, levels, records);
        //}

        //public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, bool includeExaminer = false)
        //{
        //    var game = GetGame(gameID);

        //    return GetGameSpeedRunRecords(game, includeExaminer);
        //}

        //public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(Game game, bool includeExaminer = false)
        //{
        //    List<SpeedRunRecordViewModel> recordVMs = new List<SpeedRunRecordViewModel>();
        //    foreach (var category in game.Categories.Where(i => i.Type == CategoryType.PerGame))
        //    {
        //        recordVMs.AddRange(GetGameSpeedRunRecords(game.ID, CategoryType.PerGame, category.ID, null, includeExaminer));
        //    }

        //    foreach (var category in game.Categories.Where(i => i.Type == CategoryType.PerLevel))
        //    {
        //        foreach (var level in game.Levels)
        //        {
        //            recordVMs.AddRange(GetGameSpeedRunRecords(game.ID, CategoryType.PerLevel, category.ID, level.ID, includeExaminer));
        //        }
        //    }

        //    return recordVMs;
        //}

        public IEnumerable<SpeedRunViewModel> GetGameSpeedRuns(string gameID, int requestCount, bool includeExaminer)
        {
            List<SpeedRunViewModel> runVMs = new List<SpeedRunViewModel>();
            ClientContainer clientContainer = new ClientContainer();
            int pageCount = Convert.ToInt32(_appConfig["ApiSettings:MaxElementsPerPage"]);
            int offset = pageCount * requestCount;

            var runs = clientContainer.Runs.GetRuns(gameId: gameID, elementsPerPage: pageCount, elementsOffset: offset);

            var platforms = _cacheHelper.GetPlatforms();
            foreach (var run in runs)
            {
                var runVM = new SpeedRunViewModel(run);
                if (includeExaminer)
                {
                    var examiner = clientContainer.Users.GetUser(run.Status.ExaminerUserID);
                    runVM.ExaminerID = examiner?.ID;
                    runVM.ExaminerName = examiner?.Name;
                }

                runVM.PlatformName = platforms.Where(i => i.ID == runVM.PlatformID).Select(i => i.Name).FirstOrDefault();
                runVMs.Add(runVM);
            }

            return runVMs;
        }

        public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, bool includeExaminer = false)
        {
            List<SpeedRunRecordViewModel> recordVMs = new List<SpeedRunRecordViewModel>();
            ClientContainer clientContainer = new ClientContainer();
            Leaderboard leaderboard = null;
            var leaderboardEmbeds = new LeaderboardEmbeds { EmbedGame = false, EmbedCategory = false, EmbedLevel = false, EmbedPlayers = true, EmbedRegions = false, EmbedPlatforms = false, EmbedVariables = false };

            if (categoryType == CategoryType.PerGame)
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID, embeds: leaderboardEmbeds);
            }
            else
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForLevel(gameId: gameID, categoryId: categoryID, levelId: levelID, embeds: leaderboardEmbeds);
            }

            var platforms = _cacheHelper.GetPlatforms();
            foreach (var record in leaderboard.Records)
            {
                var recordVM = new SpeedRunRecordViewModel(record);
                if (includeExaminer)
                {
                    var examiner = clientContainer.Users.GetUser(record.Status.ExaminerUserID);
                    recordVM.ExaminerName = examiner?.Name;
                }

                recordVM.CategoryType = new IDNamePair { ID = ((int)categoryType).ToString(), Name = categoryType.ToString() };
                recordVM.PlatformName = platforms.Where(i => i.ID == recordVM.PlatformID).Select(i => i.Name).FirstOrDefault();
                recordVMs.Add(recordVM);
            }

            //if (includeExaminer)
            //{
            //    foreach (var record in leaderboard.Records)
            //    {
            //        var recordVM = new SpeedRunRecordViewModel(record);
            //        var examiner = clientContainer.Users.GetUser(record.Status.ExaminerUserID);
            //        recordVM.ExaminerName = examiner?.Name;
            //        recordVMs.Add(recordVM);
            //    }
            //}
            //else
            //{
            //    recordVMs = leaderboard.Records.Select(i => new SpeedRunRecordViewModel(i)).ToList();
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
    }
}
