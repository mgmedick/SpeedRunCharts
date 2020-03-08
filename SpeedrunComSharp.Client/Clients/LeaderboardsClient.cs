using System;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
{

    public class LeaderboardsClient : BaseClient
    {
        public const string Name = "leaderboards";

        public LeaderboardsClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetLeaderboardsUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        private Leaderboard getLeaderboard(
            string uri, int? top = null,
            string platformId = null, string regionId = null,
            EmulatorsFilter emulatorsFilter = EmulatorsFilter.NotSet, bool filterOutRunsWithoutVideo = false,
            TimingMethod? orderBy = null, DateTime? filterOutRunsAfter = null,
            IEnumerable<VariableValue> variableFilters = null,
            LeaderboardEmbeds embeds = default(LeaderboardEmbeds))
        {
            var parameters = new List<string>() { embeds.ToString() };

            if (top.HasValue)
                parameters.Add(string.Format("top={0}", top.Value));
            if (!string.IsNullOrEmpty(platformId))
                parameters.Add(string.Format("platform={0}", Uri.EscapeDataString(platformId)));
            if (!string.IsNullOrEmpty(regionId))
                parameters.Add(string.Format("region={0}", Uri.EscapeDataString(regionId)));
            if (emulatorsFilter != EmulatorsFilter.NotSet)
                parameters.Add(string.Format("emulators={0}",
                    emulatorsFilter == EmulatorsFilter.OnlyEmulators ? "true" : "false"));
            if (filterOutRunsWithoutVideo)
                parameters.Add("video-only=true");
            if (orderBy.HasValue)
            {
                parameters.Add(orderBy.Value.ToParameters());
            }
            if (filterOutRunsAfter.HasValue)
            {
                var date = filterOutRunsAfter.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                parameters.Add(string.Format("date={0}", 
                    Uri.EscapeDataString(date)));
            }
            if (variableFilters != null)
            {
                foreach (var variableValue in variableFilters)
                {
                    parameters.Add(string.Format("var-{0}={1}",
                        Uri.EscapeDataString(variableValue.VariableID),
                        Uri.EscapeDataString(variableValue.ID)));
                }
            }

            var innerUri = GetLeaderboardsUri(string.Format("{0}{1}",
                uri,
                parameters.ToParameters()));

            var result = DoRequest(innerUri);
            return Parse(result.data);
        }

        public Leaderboard GetLeaderboardForFullGameCategory(
            string gameId, string categoryId,
            int? top = null,
            string platformId = null, string regionId = null,
            EmulatorsFilter emulatorsFilter = EmulatorsFilter.NotSet, bool filterOutRunsWithoutVideo = false,
            TimingMethod? orderBy = null, DateTime? filterOutRunsAfter = null,
            IEnumerable<VariableValue> variableFilters = null,
            LeaderboardEmbeds embeds = default(LeaderboardEmbeds))
        {
            var uri = string.Format("/{0}/category/{1}",
                Uri.EscapeDataString(gameId),
                Uri.EscapeDataString(categoryId));

            return getLeaderboard(uri,
                top,
                platformId, regionId,
                emulatorsFilter, filterOutRunsWithoutVideo,
                orderBy, filterOutRunsAfter,
                variableFilters,
                embeds);
        }

        public Leaderboard GetLeaderboardForLevel(
            string gameId, string levelId, string categoryId,
            int? top = null,
            string platformId = null, string regionId = null,
            EmulatorsFilter emulatorsFilter = EmulatorsFilter.NotSet, bool filterOutRunsWithoutVideo = false,
            TimingMethod? orderBy = null, DateTime? filterOutRunsAfter = null,
            IEnumerable<VariableValue> variableFilters = null,
            LeaderboardEmbeds embeds = default(LeaderboardEmbeds))
        {
            var uri = string.Format("/{0}/level/{1}/{2}",
                Uri.EscapeDataString(gameId),
                Uri.EscapeDataString(levelId),
                Uri.EscapeDataString(categoryId));

            return getLeaderboard(uri,
                top,
                platformId, regionId,
                emulatorsFilter, filterOutRunsWithoutVideo,
                orderBy, filterOutRunsAfter,
                variableFilters,
                embeds);
        }

        public Leaderboard Parse(dynamic leaderboardElement)
        {
            var leaderboard = new Leaderboard();

            var properties = leaderboardElement.Properties as IDictionary<string, dynamic>;

            //Parse Attributes

            leaderboard.WebLink = new Uri(leaderboardElement.weblink as string);

            var emulators = leaderboardElement.emulators as string;
            if (emulators == "true")
                leaderboard.EmulatorFilter = EmulatorsFilter.OnlyEmulators;
            else if (emulators == "false")
                leaderboard.EmulatorFilter = EmulatorsFilter.NoEmulators;
            else
                leaderboard.EmulatorFilter = EmulatorsFilter.NotSet;

            leaderboard.AreRunsWithoutVideoFilteredOut = properties["video-only"];

            //TODO Not actually optional
            if (leaderboardElement.timing != null)
                leaderboard.OrderedBy = (leaderboardElement.timing as string).ToTimingMethod();

            if (leaderboardElement.values is DynamicJsonObject)
            {
                var valueProperties = leaderboardElement.values.Properties as IDictionary<string, dynamic>;
                leaderboard.VariableFilters = valueProperties.Select(x => Client.Variables.ParseValueDescriptor(x) as VariableValue).ToList().AsReadOnly();
            }
            else
            {
                leaderboard.VariableFilters = new List<VariableValue>().AsReadOnly();
            }

            Func<dynamic, Record> recordParser = x => Client.Common.ParseRecord(x) as Record;
            leaderboard.Records = ParseCollection(leaderboardElement.runs, recordParser);

            //Parse Links

            if (properties["game"] is string)
            {
                leaderboard.GameID = leaderboardElement.game as string;
                leaderboard.game = new Lazy<Game>(() => Client.Games.GetGame(leaderboard.GameID));
            }
            else
            {
                var game = Client.Games.Parse(properties["game"].data) as Game;
                leaderboard.game = new Lazy<Game>(() => game);
                leaderboard.GameID = game.ID;
            }

            if (properties["category"] is string)
            {
                leaderboard.CategoryID = leaderboardElement.category as string;
                leaderboard.category = new Lazy<Category>(() => Client.Categories.GetCategory(leaderboard.CategoryID));
            }
            else
            {
                var category = Client.Categories.Parse(properties["category"].data) as Category;
                leaderboard.category = new Lazy<Category>(() => category);
                if (category != null)
                    leaderboard.CategoryID = category.ID;
            }

            if (properties["level"] == null)
            {
                leaderboard.level = new Lazy<Level>(() => null);
            }
            else if (properties["level"] is string)
            {
                leaderboard.LevelID = leaderboardElement.level as string;
                leaderboard.level = new Lazy<Level>(() => Client.Levels.GetLevel(leaderboard.LevelID));
            }
            else
            {
                var level = Client.Levels.Parse(properties["level"].data) as Level;
                leaderboard.level = new Lazy<Level>(() => level);
                if (level != null)
                    leaderboard.LevelID = level.ID;
            }

            if (properties["platform"] == null)
            {
                leaderboard.platformFilter = new Lazy<Platform>(() => null);
            }
            else if (properties["platform"] is string)
            {
                leaderboard.PlatformIDOfFilter = properties["platform"] as string;
                leaderboard.platformFilter = new Lazy<Platform>(() => Client.Platforms.GetPlatform(leaderboard.PlatformIDOfFilter));
            }
            else
            {
                var platform = Client.Platforms.Parse(properties["platform"].data) as Platform;
                leaderboard.platformFilter = new Lazy<Platform>(() => platform);
                if (platform != null)
                    leaderboard.PlatformIDOfFilter = platform.ID;
            }

            if (properties["region"] == null)
            {
                leaderboard.regionFilter = new Lazy<Region>(() => null);
            }
            else if (properties["region"] is string)
            {
                leaderboard.RegionIDOfFilter = properties["region"] as string;
                leaderboard.regionFilter = new Lazy<Region>(() => Client.Regions.GetRegion(leaderboard.RegionIDOfFilter));
            }
            else
            {
                var region = Client.Regions.Parse(properties["region"].data) as Region;
                leaderboard.regionFilter = new Lazy<Region>(() => region);
                if (region != null)
                    leaderboard.RegionIDOfFilter = region.ID;
            }

            //Parse Embeds

            if (properties.ContainsKey("players"))
            {
                Func<dynamic, Player> playerParser = x => Client.Common.ParsePlayer(x) as Player;
                var players = ParseCollection(leaderboardElement.players.data, playerParser) as ReadOnlyCollection<Player>;

                foreach (var record in leaderboard.Records)
                {
                    record.Players = record.Players.Select(x => players.FirstOrDefault(y => x.Equals(y))).ToList().AsReadOnly();
                }

                leaderboard.players = new Lazy<ReadOnlyCollection<Player>>(() => players);
            }
            else
            {
                leaderboard.players = new Lazy<ReadOnlyCollection<Player>>(() => leaderboard.Records.SelectMany(x => x.Players).ToList().Distinct().ToList().AsReadOnly());
            }

            if (properties.ContainsKey("regions"))
            {
                Func<dynamic, Region> regionParser = x => Client.Regions.Parse(x) as Region;
                var regions = ParseCollection(leaderboardElement.regions.data, regionParser) as ReadOnlyCollection<Region>;

                foreach (var record in leaderboard.Records)
                {
                    record.System.region = new Lazy<Region>(() => regions.FirstOrDefault(x => x.ID == record.System.RegionID));
                }

                leaderboard.usedRegions = new Lazy<ReadOnlyCollection<Region>>(() => regions);
            }
            else
            {
                leaderboard.usedRegions = new Lazy<ReadOnlyCollection<Region>>(() => leaderboard.Records.Select(x => x.Region).Distinct().Where(x => x != null).ToList().AsReadOnly());
            }

            if (properties.ContainsKey("platforms"))
            {
                Func<dynamic, Platform> platformParser = x => Client.Platforms.Parse(x) as Platform;
                var platforms = ParseCollection(leaderboardElement.platforms.data, platformParser) as ReadOnlyCollection<Platform>;

                foreach (var record in leaderboard.Records)
                {
                    record.System.platform = new Lazy<Platform>(() => platforms.FirstOrDefault(x => x.ID == record.System.PlatformID));
                }

                leaderboard.usedPlatforms = new Lazy<ReadOnlyCollection<Platform>>(() => platforms);
            }
            else
            {
                leaderboard.usedPlatforms = new Lazy<ReadOnlyCollection<Platform>>(() => leaderboard.Records.Select(x => x.Platform).Distinct().Where(x => x != null).ToList().AsReadOnly());
            }

            Action<ReadOnlyCollection<Variable>> patchVariablesOfRecords = variables =>
            {
                foreach (var record in leaderboard.Records)
                {
                    foreach (var value in record.VariableValues)
                    {
                        value.variable = new Lazy<Variable>(() => variables.FirstOrDefault(x => x.ID == value.VariableID));
                    }
                }
            };

            if (properties.ContainsKey("variables"))
            {
                Func<dynamic, Variable> variableParser = x => Client.Variables.Parse(x) as Variable;
                var variables = ParseCollection(leaderboardElement.variables.data, variableParser) as ReadOnlyCollection<Variable>;

                patchVariablesOfRecords(variables);

                leaderboard.applicableVariables = new Lazy<ReadOnlyCollection<Variable>>(() => variables);
            }
            else if (string.IsNullOrEmpty(leaderboard.LevelID))
            {
                leaderboard.applicableVariables = new Lazy<ReadOnlyCollection<Variable>>(() =>
                {
                    var variables = leaderboard.Category.Variables;

                    patchVariablesOfRecords(variables);

                    return variables;
                });
            }
            else
            {
                leaderboard.applicableVariables = new Lazy<ReadOnlyCollection<Variable>>(() =>
                {
                    var variables = leaderboard.Category.Variables.Concat(leaderboard.Level.Variables).ToList().Distinct().ToList().AsReadOnly();

                    patchVariablesOfRecords(variables);

                    return variables;
                });
            }

            return leaderboard;
        }
    }
}
