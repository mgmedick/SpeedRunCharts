using SpeedRunApp.Client;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
//using SpeedRunApp.Interfaces.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class GamesService : IGamesService
    {
        private readonly ICacheHelper _cacheHelper = null;

        public GamesService(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        public GameDetailsViewModel GetGameDetails(string gameID)
        {
            var game = GetGame(gameID);
            var gameVM = new GameDetailsViewModel(game);


            return gameVM;
        }

        public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, string variableID, string variableValues)
        {
            Leaderboard leaderboard = null;
            ClientContainer clientContainer = new ClientContainer();
            var leaderboardEmbeds = new LeaderboardEmbeds { EmbedGame = false, EmbedCategory = false, EmbedLevel = false, EmbedPlayers = true, EmbedRegions = false, EmbedPlatforms = false, EmbedVariables = true };
            IEnumerable<VariableValue> variableFilters = null;

            if (!string.IsNullOrWhiteSpace(variableValues))
            {
                variableFilters = variableValues.Split(",").Select(i => new VariableValue { VariableID = i.Split("|")[0], ID = i.Split("|")[1] });
            }

            if (categoryType == CategoryType.PerGame)
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID, variableFilters: variableFilters, embeds: leaderboardEmbeds);
            }
            else
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForLevel(gameId: gameID, categoryId: categoryID, variableFilters: variableFilters, levelId: levelID, embeds: leaderboardEmbeds);
            }

            var platforms = _cacheHelper.GetPlatforms();
            foreach (var record in leaderboard.Records)
            {
                if (!string.IsNullOrWhiteSpace(record.System?.PlatformID))
                {
                    record.System.Platform = platforms.FirstOrDefault(i => i.ID == record.System.PlatformID);
                }
            }

            var recordVMs = leaderboard.Records.Select(i => new SpeedRunRecordViewModel(i)).ToList();

            return recordVMs.OrderBy(i => i.PrimaryTimeMilliseconds);
        }

        public Game GetGame(string gameID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var gameEmbeds = new GameEmbeds { EmbedCategories = true, EmbedLevels = true, EmbedModerators = true, EmbedPlatforms = true, EmbedVariables = true };
            var game = clientContainer.Games.GetGame(gameID, gameEmbeds);

            return game;
        }
    }
}
