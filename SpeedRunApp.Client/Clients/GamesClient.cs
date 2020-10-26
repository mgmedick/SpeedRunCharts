using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpeedRunApp.Client
{
    public class GamesClient : BaseClient
    {

        public const string Name = "games";

        public GamesClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetGamesUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public IEnumerable<Game> GetGames(
            string name = null, int? yearOfRelease = null,
            string platformId = null, string regionId = null,
            string moderatorId = null, int? elementsPerPage = null,
            GameEmbeds embeds = null,
            GamesOrdering orderBy = default(GamesOrdering),
            int? elementsOffset = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            if (!string.IsNullOrWhiteSpace(name))
                parameters.Add(string.Format("name={0}", Uri.EscapeDataString(name)));

            if (yearOfRelease.HasValue)
                parameters.Add(string.Format("released={0}", yearOfRelease.Value));

            if (!string.IsNullOrWhiteSpace(platformId))
                parameters.Add(string.Format("platform={0}", Uri.EscapeDataString(platformId)));

            if (!string.IsNullOrWhiteSpace(regionId))
                parameters.Add(string.Format("region={0}", Uri.EscapeDataString(regionId)));

            if (!string.IsNullOrWhiteSpace(moderatorId))
                parameters.Add(string.Format("moderator={0}", Uri.EscapeDataString(moderatorId)));

            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            if (elementsOffset.HasValue)
                parameters.Add(string.Format("offset={0}", elementsOffset));

            var uri = GetGamesUri(parameters.ToParameters());
            return DoRequest(uri, x => Parse(x) as Game);
        }

        public IEnumerable<GameHeader> GetGameHeaders(int elementsPerPage = 1000,
            GamesOrdering orderBy = default(GamesOrdering))
        {
            var parameters = new List<string>() { "_bulk=yes" };

            parameters.AddRange(orderBy.ToParameters());
            parameters.Add(string.Format("max={0}", elementsPerPage));

            var uri = GetGamesUri(parameters.ToParameters());

            return DoRequest(uri, x => ParseGameHeader(x) as GameHeader);
        }

        public Game GetGame(string gameId, GameEmbeds embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            var uri = GetGamesUri(string.Format("/{0}{1}",
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            var result = DoRequest(uri);

            return Parse(result.data);
        }

        /*
        public Game SearchGame(string name, GameEmbeds embeds = default(GameEmbeds))
        {
            var game = GetGames(name: name, embeds: embeds, elementsPerPage: 1).FirstOrDefault();
            
            return game;
        }

        public Game SearchGameExact(string name, GameEmbeds embeds = default(GameEmbeds))
        {
            var game = GetGames(name: name, embeds: embeds, elementsPerPage: 1).Take(1).FirstOrDefault(x => x.Name == name);

            return game;
        }
        */


        public IEnumerable<Category> GetCategories(
            string gameId, bool miscellaneous = true,
            CategoryEmbeds embeds = null,
            CategoriesOrdering orderBy = default(CategoriesOrdering))
        {
            var parameters = new List<string>() { embeds?.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            if (!miscellaneous)
                parameters.Add("miscellaneous=no");

            var uri = GetGamesUri(string.Format("/{0}/categories{1}",
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            return DoRequest(uri, x => Client.Categories.Parse(x) as Category);
        }

        public IEnumerable<Level> GetLevels(string gameId,
            LevelEmbeds embeds = null,
            LevelsOrdering orderBy = default(LevelsOrdering))
        {
            var parameters = new List<string>() { embeds?.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            var uri = GetGamesUri(string.Format("/{0}/levels{1}",
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            return DoRequest(uri, x => Client.Levels.Parse(x) as Level);
        }

        public IEnumerable<Variable> GetVariables(string gameId,
            VariablesOrdering orderBy = default(VariablesOrdering))
        {
            var parameters = new List<string>(orderBy.ToParameters());

            var uri = GetGamesUri(string.Format("/{0}/variables{1}",
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            return DoRequest(uri, x => Client.Variables.Parse(x) as Variable);
        }

        public IEnumerable<Game> GetRomHacks(string gameId,
            GameEmbeds embeds = null,
            GamesOrdering orderBy = default(GamesOrdering))
        {
            var parameters = new List<string>() { embeds?.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            var uri = GetGamesUri(string.Format("/{0}/romhacks{1}",
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            return DoRequest(uri, x => Parse(x) as Game);
        }

        public IEnumerable<Leaderboard> GetRecords(string gameId,
            int? top = null, LeaderboardScope scope = LeaderboardScope.All,
            bool includeMiscellaneousCategories = true, bool skipEmptyLeaderboards = false,
            int? elementsPerPage = null,
            LeaderboardEmbeds embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            if (top.HasValue)
                parameters.Add(string.Format("top={0}", top.Value));
            if (scope != LeaderboardScope.All)
                parameters.Add(scope.ToParameter());
            if (!includeMiscellaneousCategories)
                parameters.Add("miscellaneous=false");
            if (skipEmptyLeaderboards)
                parameters.Add("skip-empty=true");
            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            var uri = GetGamesUri(string.Format("/{0}/records{1}",
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            return DoRequest<Leaderboard>(uri, x => Client.Leaderboards.Parse(x));
        }

        public Game Parse(dynamic gameElement)
        {
            var game = new Game();
            var properties = gameElement.Properties as IDictionary<string, dynamic>;

            //Parse Attributes
            game.Header = ParseGameHeader(gameElement);
            game.YearOfRelease = (int?)gameElement.released;
            game.Ruleset = ParseRuleset(gameElement.ruleset);

            game.IsRomHack = gameElement.romhack;

            if (gameElement.created != null)
            {
                game.CreationDate = gameElement.created;
            }

            //game.CreationDate = gameElement.created as DateTime?;
            //var created = gameElement.created as string;
            //if (!string.IsNullOrWhiteSpace(created))
            //{
            //    game.CreationDate = DateTime.Parse(created, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            //}

            game.Assets = Client.Common.ParseAssets(gameElement.assets);

            //Parse Links
            var links = properties["links"] as IEnumerable<dynamic>;
            var seriesLink = links.FirstOrDefault(x => x.rel == "series");
            if (seriesLink != null)
            {
                var parentUri = seriesLink.uri as string;
                game.SeriesID = parentUri.Substring(parentUri.LastIndexOf('/') + 1);
            }

            var originalGameLink = links.FirstOrDefault(x => x.rel == "game");
            if (originalGameLink != null)
            {
                var originalGameUri = originalGameLink.uri as string;
                game.OriginalGameID = originalGameUri.Substring(originalGameUri.LastIndexOf('/') + 1);
            }

            //Parse Embeds
            if (!gameElement.moderators.Properties.ContainsKey("data"))
            {
                var moderatorsProperties = gameElement.moderators.Properties as IDictionary<string, dynamic>;
                game.Moderators = moderatorsProperties.Select(x => Client.Common.ParseModerator(x)).ToList();
            }
            else
            {
                Func<dynamic, User> userParser = x => Client.Users.Parse(x) as User;
                IEnumerable<User> users = ParseCollection(gameElement.moderators.data, userParser);
                game.ModeratorUsers = users;
            }

            if (properties["platforms"] is IList)
            {
                game.PlatformIDs = ParseCollection<string>(gameElement.platforms);
            }
            else
            {
                Func<dynamic, Platform> platformParser = x => Client.Platforms.Parse(x) as Platform;
                IEnumerable<Platform> platforms = ParseCollection(gameElement.platforms.data, platformParser);
                game.Platforms = platforms;
                game.PlatformIDs = platforms.Select(x => x.ID).ToList();
            }

            if (properties["regions"] is IList)
            {
                game.RegionIDs = ParseCollection<string>(gameElement.regions);
            }
            else
            {
                Func<dynamic, Region> regionParser = x => Client.Regions.Parse(x) as Region;
                IEnumerable<Region> regions = ParseCollection(gameElement.regions.data, regionParser);
                game.Regions = regions;
                game.RegionIDs = regions.Select(x => x.ID).ToList();
            }

            if (properties.ContainsKey("levels"))
            {
                Func<dynamic, Level> levelParser = x => Client.Levels.Parse(x) as Level;
                IEnumerable<Level> levels = ParseCollection(gameElement.levels.data, levelParser);
                game.Levels = levels;
            }

            if (properties.ContainsKey("categories"))
            {
                Func<dynamic, Category> categoryParser = x => Client.Categories.Parse(x) as Category;
                IEnumerable<Category> categories = ParseCollection(gameElement.categories.data, categoryParser);

                foreach (var category in categories)
                {
                    category.Game = game;
                }

                game.Categories = categories;
            }

            if (properties.ContainsKey("variables"))
            {
                Func<dynamic, Variable> variableParser = x => Client.Variables.Parse(x) as Variable;
                IEnumerable<Variable> variables = ParseCollection(gameElement.variables.data, variableParser);

                foreach (var category in game.Categories)
                {
                    category.Variables = variables.Where(x => x.CategoryID == category.ID);
                }

                foreach (var level in game.Levels)
                {
                    level.Variables = variables.Where(x => x.CategoryID == level.ID);
                }

                game.Variables = variables;
            }

            return game;
        }

        private static GameHeader ParseGameHeader(dynamic gameHeaderElement)
        {
            var gameHeader = new GameHeader();

            gameHeader.ID = gameHeaderElement.id as string;
            gameHeader.Name = gameHeaderElement.names.international as string;
            gameHeader.JapaneseName = gameHeaderElement.names.japanese as string;
            gameHeader.WebLink = new Uri(gameHeaderElement.weblink as string);
            gameHeader.Abbreviation = gameHeaderElement.abbreviation as string;

            return gameHeader;
        }

        private Ruleset ParseRuleset(dynamic rulesetElement)
        {
            var ruleset = new Ruleset();

            var properties = rulesetElement.Properties as IDictionary<string, dynamic>;

            ruleset.ShowMilliseconds = properties["show-milliseconds"];
            ruleset.RequiresVerification = properties["require-verification"];
            ruleset.RequiresVideo = properties["require-video"];

            Func<dynamic, TimingMethod> timingMethodParser = x => (x as string).ToTimingMethod();
            ruleset.TimingMethods = ParseCollection(properties["run-times"], timingMethodParser);
            ruleset.DefaultTimingMethod = (properties["default-time"] as string).ToTimingMethod();

            ruleset.EmulatorsAllowed = properties["emulators-allowed"];

            return ruleset;
        }
    }
}
