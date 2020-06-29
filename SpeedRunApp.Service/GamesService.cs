using SpeedRunApp.Model;
using SpeedRunApp.Client;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Interfaces.Services;
using SpeedRunCommon;
using System.Collections.Generic;

namespace SpeedRunApp.Service
{
    public class GamesService : IGamesService
    {
        public GameDetailsViewModel GetGame(string gameID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var gameEmbeds = new GameEmbeds { EmbedCategories = true, EmbedLevels = true, EmbedModerators = true, EmbedPlatforms = true };
            var game = clientContainer.Games.GetGame(gameID, gameEmbeds);
            var gameVM = new GameDetailsViewModel(game);

            return gameVM;
        }

        public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            List<SpeedRunRecordViewModel> recordVMs = new List<SpeedRunRecordViewModel>();
            Leaderboard leaderboard = null;
            ClientContainer clientContainer = new ClientContainer();
            var leaderboardEmbeds = new LeaderboardEmbeds { EmbedGame = true, EmbedCategory = true, EmbedLevel = true, EmbedPlayers = true, EmbedRegions = false, EmbedPlatforms = true, EmbedVariables = false };

            if (categoryType == CategoryType.PerGame)
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID, embeds: leaderboardEmbeds);
            }
            else
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForLevel(gameId: gameID, levelId: levelID, categoryId: categoryID, embeds: leaderboardEmbeds);
            }

            foreach (var record in leaderboard.Records)
            {
                var recordVM = new SpeedRunRecordViewModel(record);
                var examiner = clientContainer.Users.GetUser(record.Status.ExaminerUserID);
                recordVM.ExaminerName = examiner.Name;
                recordVMs.Add(recordVM);
            }

            return recordVMs;
        }
    }
}
