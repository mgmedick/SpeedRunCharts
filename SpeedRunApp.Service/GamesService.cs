using SpeedRunApp.Model;
using SpeedRunApp.Client;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Interfaces.Services;
using SpeedRunCommon;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SpeedRunApp.Service
{
    public class GamesService : IGamesService
    {
        public GameDetailsViewModel GetGameDetails(string gameID)
        {
            var game = GetGame(gameID);
            var gameVM = new GameDetailsViewModel(game);
            gameVM.SpeedRunGridVM = GetGameSpeedRunGrid(game);

            return gameVM;
        }

        public Game GetGame(string gameID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var gameEmbeds = new GameEmbeds { EmbedCategories = true, EmbedLevels = true, EmbedModerators = true, EmbedPlatforms = true };
            var game = clientContainer.Games.GetGame(gameID, gameEmbeds);

            return game;
        }

        public SpeedRunGridViewModel GetGameSpeedRunGrid(Game game)
        {
            var categoryTypes = game.CategoryTypes
                    .Select(i => new IDNamePair { ID = ((int)i).ToString(), Name = i.ToString() })
                    .ToList();

            var games = new List<GameDisplay>() { new GameDisplay { ID = game.ID, Name = game.Name, CategoryTypeIDs = game.Categories.Select(i => ((int)i.Type).ToString()).Distinct() } };

            var categories = game.Categories
                            .Select(i => new CategoryDisplay { ID = i.ID, Name = i.Name, CategoryTypeID = ((int)i.Type).ToString(), GameID = i.GameID })
                            .ToList();

            var levels = game.Levels
                         .Select(i => new LevelDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID })
                         .ToList();

            return new SpeedRunGridViewModel("Game", categoryTypes, games, categories, levels);
        }

        public SpeedRunGridViewModel SearchGameSpeedRunGrid(string gameID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels)
        {
            var game = GetGame(gameID);
            var gameSpeedRunGridVM = GetGameSpeedRunGrid(game);

            if (drpCategoryTypes.Any())
            {
                gameSpeedRunGridVM.CategoryTypes = gameSpeedRunGridVM.CategoryTypes.Where(i => drpCategoryTypes.Contains(i.ID)).ToList();
            }

            if (drpCategories.Any())
            {
                gameSpeedRunGridVM.Categories = gameSpeedRunGridVM.Categories.Where(i => drpCategories.Contains(i.ID)).ToList();
            }

            if (drpLevels.Any())
            {
                gameSpeedRunGridVM.Levels = gameSpeedRunGridVM.Levels.Where(i => drpLevels.Contains(i.ID)).ToList();
            }

            return gameSpeedRunGridVM;
        }

        public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, DateTime? startDate, DateTime? endDate)
        {
            Leaderboard leaderboard = null;
            ClientContainer clientContainer = new ClientContainer();
            var leaderboardEmbeds = new LeaderboardEmbeds { EmbedGame = false, EmbedCategory = false, EmbedLevel = true, EmbedPlayers = true, EmbedRegions = false, EmbedPlatforms = true, EmbedVariables = false };

            if (categoryType == CategoryType.PerGame)
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID, filterOutRunsAfter: endDate, embeds: leaderboardEmbeds);
            }
            else
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForLevel(gameId: gameID, categoryId: categoryID, levelId: levelID, filterOutRunsAfter: endDate, embeds: leaderboardEmbeds);
            }

            if (startDate.HasValue)
            {
                leaderboard.Records = leaderboard.Records.Where(i => i.DateSubmitted.HasValue && i.DateSubmitted.Value >= startDate.Value);
            }

            List<SpeedRunRecordViewModel> recordVMs = new List<SpeedRunRecordViewModel>();
            var game = GetGame(gameID);

            foreach (var record in leaderboard.Records)
            {
                var recordVM = new SpeedRunRecordViewModel(record);
                if (!string.IsNullOrWhiteSpace(record.Status.ExaminerUserID))
                {
                    var user = game.ModeratorUsers?.FirstOrDefault(i => i.ID == record.Status.ExaminerUserID);
                    if (user != null)
                    {
                        recordVM.ExaminerName = user.Name;
                    }
                    else
                    {
                        var examiner = clientContainer.Users.GetUser(record.Status.ExaminerUserID);
                        if (examiner != null)
                        {
                            recordVM.ExaminerName = examiner.Name;
                        }
                    }
                }

                recordVMs.Add(recordVM);
            }

            return recordVMs.OrderBy(i => i.PrimaryRunTimeSeconds);
        }
    }
}
