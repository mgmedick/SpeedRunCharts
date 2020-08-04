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

        public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, IEnumerable<IDNamePair> moderators)
        {
            Leaderboard leaderboard = null;
            ClientContainer clientContainer = new ClientContainer();
            var leaderboardEmbeds = new LeaderboardEmbeds { EmbedGame = false, EmbedCategory = false, EmbedLevel = false, EmbedPlayers = true, EmbedRegions = false, EmbedPlatforms = false, EmbedVariables = false };

            if (categoryType == CategoryType.PerGame)
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID, embeds: leaderboardEmbeds);
            }
            else
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForLevel(gameId: gameID, categoryId: categoryID, levelId: levelID, embeds: leaderboardEmbeds);
            }

            var recordVMs = leaderboard.Records.Select(i => new SpeedRunRecordViewModel(i)).ToList();
            var platforms = _cacheHelper.GetPlatforms();
            var examiners = moderators?.ToDictionary(t => t.ID, t => t.Name);

            foreach (var record in recordVMs)
            {
                if (!string.IsNullOrWhiteSpace(record.ExaminerUserID))
                {
                    record.ExaminerName = GetExaminerName(record.ExaminerUserID, examiners);
                }

                record.PlatformName = platforms.Where(i => i.ID == record.PlatformID).Select(i => i.Name).FirstOrDefault();
            }

            return recordVMs.OrderBy(i => i.PrimaryRunTimeMilliseconds);
        }

        private string GetExaminerName(string examinerUserID, Dictionary<string, string> examiners)
        {
            string examinerName = null;
            ClientContainer clientContainer = new ClientContainer();

            if (examiners.ContainsKey(examinerUserID))
            {
                examinerName = examiners[examinerUserID];
            }
            else
            {
                var examiner = clientContainer.Users.GetUser(examinerUserID);
                if (examiner != null)
                {
                    examiners.Add(examiner.ID, examiner.Name);
                    examinerName = examiner.Name;
                }
            }

            return examinerName;
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
