using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
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

        public Game GetGameFromSiteUri(string siteUri, GameEmbeds embeds = default(GameEmbeds))
        {
            var id = GetGameIDFromSiteUri(siteUri);

            if (string.IsNullOrEmpty(id))
                return null;

            return GetGame(id, embeds);
        }

        public string GetGameIDFromSiteUri(string siteUri)
        {
            var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

            if (elementDescription == null
                || elementDescription.Type != ElementType.Game)
                return null;

            return elementDescription.ID;
        }

        public IEnumerable<Game> GetGames(
            string name = null, int? yearOfRelease = null, 
            string platformId = null, string regionId = null, 
            string moderatorId = null, int? elementsPerPage = null,
            GameEmbeds embeds = default(GameEmbeds),
            GamesOrdering orderBy = default(GamesOrdering))
        {
            var parameters = new List<string>() { embeds.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            if (!string.IsNullOrEmpty(name))
                parameters.Add(string.Format("name={0}", Uri.EscapeDataString(name)));

            if (yearOfRelease.HasValue)
                parameters.Add(string.Format("released={0}", yearOfRelease.Value));

            if (!string.IsNullOrEmpty(platformId))
                parameters.Add(string.Format("platform={0}", Uri.EscapeDataString(platformId)));

            if (!string.IsNullOrEmpty(regionId))
                parameters.Add(string.Format("region={0}", Uri.EscapeDataString(regionId)));

            if (!string.IsNullOrEmpty(moderatorId))
                parameters.Add(string.Format("moderator={0}", Uri.EscapeDataString(moderatorId)));

            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            var uri = GetGamesUri(parameters.ToParameters());
            return DoPaginatedRequest(uri, 
                x => Parse(x) as Game);
        }

        public IEnumerable<GameHeader> GetGameHeaders(int elementsPerPage = 1000,
            GamesOrdering orderBy = default(GamesOrdering))
        {
            var parameters = new List<string>() { "_bulk=yes" };

            parameters.AddRange(orderBy.ToParameters());
            parameters.Add(string.Format("max={0}", elementsPerPage));

            var uri = GetGamesUri(parameters.ToParameters());

            return DoPaginatedRequest(uri,
                x => ParseGameHeader(x) as GameHeader);
        }

        public Game GetGame(string gameId, GameEmbeds embeds = default(GameEmbeds))
        {
            var parameters = new List<string>() { embeds.ToString() };

            var uri = GetGamesUri(string.Format("/{0}{1}", 
                Uri.EscapeDataString(gameId), 
                parameters.ToParameters()));

            var result = DoRequest(uri);

            return Parse(result.data);
        }
        
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

        public ReadOnlyCollection<Category> GetCategories(
            string gameId, bool miscellaneous = true,
            CategoryEmbeds embeds = default(CategoryEmbeds),
            CategoriesOrdering orderBy = default(CategoriesOrdering))
        {
            var parameters = new List<string>() { embeds.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            if (!miscellaneous)
                parameters.Add("miscellaneous=no");

            var uri = GetGamesUri(string.Format("/{0}/categories{1}", 
                Uri.EscapeDataString(gameId), 
                parameters.ToParameters()));

            return DoDataCollectionRequest(uri,
                x => Client.Categories.Parse(x) as Category);
        }

        public ReadOnlyCollection<Level> GetLevels(string gameId,
            LevelEmbeds embeds = default(LevelEmbeds),
            LevelsOrdering orderBy = default(LevelsOrdering))
        {
            var parameters = new List<string>() { embeds.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            var uri = GetGamesUri(string.Format("/{0}/levels{1}", 
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            return DoDataCollectionRequest(uri,
                 x => Client.Levels.Parse(x) as Level);
        }

        public ReadOnlyCollection<Variable> GetVariables(string gameId,
            VariablesOrdering orderBy = default(VariablesOrdering))
        {
            var parameters = new List<string>(orderBy.ToParameters());

            var uri = GetGamesUri(string.Format("/{0}/variables{1}", 
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            return DoDataCollectionRequest(uri,
                x => Client.Variables.Parse(x) as Variable);
        }

        public ReadOnlyCollection<Game> GetRomHacks(string gameId,
            GameEmbeds embeds = default(GameEmbeds),
            GamesOrdering orderBy = default(GamesOrdering))
        {
            var parameters = new List<string>() { embeds.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            var uri = GetGamesUri(string.Format("/{0}/romhacks{1}", 
                Uri.EscapeDataString(gameId),
                parameters.ToParameters()));

            return DoDataCollectionRequest(uri, 
                x => Parse(x) as Game);
        }

        public IEnumerable<Leaderboard> GetRecords(string gameId,
            int? top = null, LeaderboardScope scope = LeaderboardScope.All,
            bool includeMiscellaneousCategories = true, bool skipEmptyLeaderboards = false,
            int? elementsPerPage = null,
            LeaderboardEmbeds embeds = default(LeaderboardEmbeds))
        {
            var parameters = new List<string>() { embeds.ToString() };

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

            return DoPaginatedRequest<Leaderboard>(uri,
                x => Client.Leaderboards.Parse(x));
        }

        public Game Parse(dynamic gameElement)
        {
            var game = new Game();

            //Parse Attributes

            game.Header = ParseGameHeader(gameElement);
            game.YearOfRelease = (int?)gameElement.released;
            game.Ruleset = ParseRuleset(gameElement.ruleset);

            game.IsRomHack = gameElement.romhack;

            var created = gameElement.created as string;
            if (!string.IsNullOrEmpty(created))
                game.CreationDate = DateTime.Parse(created, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            game.Assets = Client.Common.ParseAssets(gameElement.assets);

            //Parse Embeds

            var properties = gameElement.Properties as IDictionary<string, dynamic>;

            if (gameElement.moderators is DynamicJsonObject && gameElement.moderators.Properties.ContainsKey("data"))
            {
                Func<dynamic, User> userParser = x => Client.Users.Parse(x) as User;
                ReadOnlyCollection<User> users = ParseCollection(gameElement.moderators.data, userParser);
                game.moderatorUsers = new Lazy<ReadOnlyCollection<User>>(() => users);
            }
            else if (gameElement.moderators is DynamicJsonObject)
            {
                var moderatorsProperties = gameElement.moderators.Properties as IDictionary<string, dynamic>;
                game.Moderators = moderatorsProperties.Select(x => Client.Common.ParseModerator(x)).ToList().AsReadOnly();

                game.moderatorUsers = new Lazy<ReadOnlyCollection<User>>(
                    () =>
                    {
                        ReadOnlyCollection<User> users;

                        if (game.Moderators.Count(x => !x.user.IsValueCreated) > 1)
                        {
                            users = Client.Games.GetGame(game.ID, embeds: new GameEmbeds(embedModerators: true)).ModeratorUsers;

                            foreach (var user in users)
                            {
                                var moderator = game.Moderators.FirstOrDefault(x => x.UserID == user.ID);
                                if (moderator != null)
                                {
                                    moderator.user = new Lazy<User>(() => user);
                                }
                            }
                        }
                        else
                        {
                            users = game.Moderators.Select(x => x.User).ToList().AsReadOnly();
                        }

                        return users;
                    });
            }
            else
            {
                game.Moderators = new ReadOnlyCollection<Moderator>(new Moderator[0]);
                game.moderatorUsers = new Lazy<ReadOnlyCollection<User>>(() => new List<User>().AsReadOnly());
            }

            if (properties["platforms"] is IList)
            {
                game.PlatformIDs = ParseCollection<string>(gameElement.platforms);

                if (game.PlatformIDs.Count > 1)
                {
                    game.platforms = new Lazy<ReadOnlyCollection<Platform>>(
                        () => Client.Games.GetGame(game.ID, embeds: new GameEmbeds(embedPlatforms: true)).Platforms);
                }
                else
                {
                    game.platforms = new Lazy<ReadOnlyCollection<Platform>>(
                        () => game.PlatformIDs.Select(x => Client.Platforms.GetPlatform(x)).ToList().AsReadOnly());
                }
            }
            else
            {
                Func<dynamic, Platform> platformParser = x => Client.Platforms.Parse(x) as Platform;
                ReadOnlyCollection<Platform> platforms = ParseCollection(gameElement.platforms.data, platformParser);
                game.platforms = new Lazy<ReadOnlyCollection<Platform>>(() => platforms);
                game.PlatformIDs = platforms.Select(x => x.ID).ToList().AsReadOnly();
            }

            if (properties["regions"] is IList)
            {
                game.RegionIDs = ParseCollection<string>(gameElement.regions);

                if (game.RegionIDs.Count > 1)
                {
                    game.regions = new Lazy<ReadOnlyCollection<Region>>(
                        () => Client.Games.GetGame(game.ID, embeds: new GameEmbeds(embedRegions: true)).Regions);
                }
                else
                {
                    game.regions = new Lazy<ReadOnlyCollection<Region>>(
                        () => game.RegionIDs.Select(x => Client.Regions.GetRegion(x)).ToList().AsReadOnly());
                }
            }
            else
            {
                Func<dynamic, Region> regionParser = x => Client.Regions.Parse(x) as Region;
                ReadOnlyCollection<Region> regions = ParseCollection(gameElement.regions.data, regionParser);
                game.regions = new Lazy<ReadOnlyCollection<Region>>(() => regions);
                game.RegionIDs = regions.Select(x => x.ID).ToList().AsReadOnly();
            }

            //Parse Links

            game.runs = new Lazy<IEnumerable<Run>>(() => Client.Runs.GetRuns(gameId: game.ID));

            if (properties.ContainsKey("levels"))
            {
                Func<dynamic, Level> levelParser = x => Client.Levels.Parse(x) as Level;
                ReadOnlyCollection<Level> levels = ParseCollection(gameElement.levels.data, levelParser);
                game.levels = new Lazy<ReadOnlyCollection<Level>>(() => levels);
            }
            else
            {
                game.levels = new Lazy<ReadOnlyCollection<Level>>(() => Client.Games.GetLevels(game.ID));
            }

            if (properties.ContainsKey("categories"))
            {
                Func<dynamic, Category> categoryParser = x => Client.Categories.Parse(x) as Category;
                ReadOnlyCollection<Category> categories = ParseCollection(gameElement.categories.data, categoryParser);

                foreach (var category in categories)
                {
                    category.game = new Lazy<Game>(() => game);
                }

                game.categories = new Lazy<ReadOnlyCollection<Category>>(() => categories);
            }
            else
            {
                game.categories = new Lazy<ReadOnlyCollection<Category>>(() =>
                {
                    var categories = Client.Games.GetCategories(game.ID);

                    foreach (var category in categories)
                    {
                        category.game = new Lazy<Game>(() => game);
                    }

                    return categories;
                });
            }

            if (properties.ContainsKey("variables"))
            {
                Func<dynamic, Variable> variableParser = x => Client.Variables.Parse(x) as Variable;
                ReadOnlyCollection<Variable> variables = ParseCollection(gameElement.variables.data, variableParser);
                game.variables = new Lazy<ReadOnlyCollection<Variable>>(() => variables);
            }
            else
            {
                game.variables = new Lazy<ReadOnlyCollection<Variable>>(() => Client.Games.GetVariables(game.ID));
            }

            var links = properties["links"] as IEnumerable<dynamic>;
            var seriesLink = links.FirstOrDefault(x => x.rel == "series");
            if (seriesLink != null)
            {
                var parentUri = seriesLink.uri as string;
                game.SeriesID = parentUri.Substring(parentUri.LastIndexOf('/') + 1);
                game.series = new Lazy<Series>(() => Client.Series.GetSingleSeries(game.SeriesID));
            }
            else
            {
                game.series = new Lazy<Series>(() => null);
            }

            var originalGameLink = links.FirstOrDefault(x => x.rel == "game");
            if (originalGameLink != null)
            {
                var originalGameUri = originalGameLink.uri as string;
                game.OriginalGameID = originalGameUri.Substring(originalGameUri.LastIndexOf('/') + 1);
                game.originalGame = new Lazy<Game>(() => Client.Games.GetGame(game.OriginalGameID));
            }
            else
            {
                game.originalGame = new Lazy<Game>(() => null);
            }

            game.romHacks = new Lazy<ReadOnlyCollection<Game>>(() =>
            {
                var romHacks = Client.Games.GetRomHacks(game.ID);

                if (romHacks != null)
                {
                    foreach (var romHack in romHacks)
                    {
                        romHack.originalGame = new Lazy<Game>(() => game);
                    }
                }

                return romHacks;
            });

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
