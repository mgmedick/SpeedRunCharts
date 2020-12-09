//using SpeedRunApp.Interfaces.Helpers;

namespace SpeedRunApp.Service
{
    public class GamesService_Old
    {
        /*
        private readonly ICacheHelper _cacheHelper = null;

        public GamesService_Old(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        public GameDetailsViewModel GetGameDetails(string gameID)
        {
            var game = GetGame(gameID);
            var gameVM = new GameDetailsViewModel(game);

            return gameVM;
        }

        public IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, string variableValues)
        {
            Leaderboard leaderboard = null;
            ClientContainer clientContainer = new ClientContainer();
            var leaderboardEmbeds = new LeaderboardEmbeds { EmbedGame = true, EmbedCategory = true, EmbedLevel = false, EmbedPlayers = true, EmbedRegions = false, EmbedPlatforms = false, EmbedVariables = true };
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
            Dictionary<string, IEnumerable<Variable>> gameVariables = new Dictionary<string, IEnumerable<Variable>>();
            foreach (var record in leaderboard.Records)
            {
                if (!string.IsNullOrWhiteSpace(record.System?.PlatformID))
                {
                    record.System.Platform = platforms.FirstOrDefault(i => i.ID == record.System.PlatformID);
                }

                var values = new List<VariableValue>();
                foreach (var variableValueMapping in record.VariableValueMappings)
                {
                    var variable = GetGameVariable(record.GameID, variableValueMapping.VariableID, gameVariables);
                    var variableValue = variable.Values.FirstOrDefault(i => i.ID == variableValueMapping.VariableValueID);
                    values.Add(variableValue);
                }
                record.VariableValues = values;
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

        public Variable GetGameVariable(string gameID, string variableID, Dictionary<string, IEnumerable<Variable>> gameVariables)
        {
            Variable variable = null;
            ClientContainer clientContainer = new ClientContainer();

            if (gameVariables.ContainsKey(gameID))
            {
                variable = gameVariables[gameID].FirstOrDefault(i => i.ID == variableID);
            }
            else
            {
                var variables = clientContainer.Games.GetVariables(gameID);
                if (variables != null && variables.Any())
                {
                    gameVariables.Add(gameID, variables);
                    variable = variables.FirstOrDefault(i => i.ID == variableID);
                }
            }

            return variable;
        }
        */
    }
}
