using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SpeedRunCommon;

namespace SpeedrunComSharp.Model
{
    public class Leaderboard
    {
        public Uri WebLink { get; set; }
        public EmulatorsFilter EmulatorFilter { get; set; }
        public bool AreRunsWithoutVideoFilteredOut { get; set; }
        public TimingMethod? OrderedBy { get; set; }
        public ReadOnlyCollection<VariableValue> VariableFilters { get; set; }
        public ReadOnlyCollection<Record> Records { get; set; }

        #region Embeds

        public Lazy<ReadOnlyCollection<Player>> players { get; set; }
        public ReadOnlyCollection<Player> Players { get { return players.Value; } }
        public Lazy<ReadOnlyCollection<Region>> usedRegions { get; set; }
        public ReadOnlyCollection<Region> UsedRegions { get { return usedRegions.Value; } }
        public Lazy<ReadOnlyCollection<Platform>> usedPlatforms { get; set; }
        public ReadOnlyCollection<Platform> UsedPlatforms { get { return usedPlatforms.Value; } }
        public Lazy<ReadOnlyCollection<Variable>> applicableVariables { get; set; }
        public ReadOnlyCollection<Variable> ApplicableVariables { get { return applicableVariables.Value; } }

        #endregion

        #region Links


        public string GameID { get; set; }
        public Lazy<Game> game { get; set; }
        public Game Game { get { return game.Value; } }
        public string CategoryID { get; set; }
        public Lazy<Category> category { get; set; }
        public Category Category { get { return category.Value; } }
        public string LevelID { get; set; }
        public Lazy<Level> level { get; set; }
        public Level Level { get { return level.Value; } }
        public string PlatformIDOfFilter { get; set; }
        public Lazy<Platform> platformFilter { get; set; }
        public Platform PlatformFilter { get { return platformFilter.Value; } }
        public string RegionIDOfFilter { get; set; }
        public Lazy<Region> regionFilter { get; set; }
        public Region RegionFilter { get { return regionFilter.Value; } }

        #endregion

        public Leaderboard() { }

        /*
        public static Leaderboard Parse(SpeedrunComClient client, dynamic leaderboardElement)
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
                leaderboard.OrderedBy = TimingMethodHelpers.FromString(leaderboardElement.timing as string);

            if (leaderboardElement.values is DynamicJsonObject)
            {
                var valueProperties = leaderboardElement.values.Properties as IDictionary<string, dynamic>;
                leaderboard.VariableFilters = valueProperties.Select(x => VariableValue.ParseValueDescriptor(client, x) as VariableValue).ToList().AsReadOnly();
            }
            else
            {
                leaderboard.VariableFilters = new List<VariableValue>().AsReadOnly();
            }

            Func<dynamic, Record> recordParser = x => Record.Parse(client, x) as Record;
            leaderboard.Records = client.ParseCollection(leaderboardElement.runs, recordParser);

            //Parse Links

            if (properties["game"] is string)
            {
                leaderboard.GameID = leaderboardElement.game as string;
                leaderboard.game = new Lazy<Game>(() => client.Games.GetGame(leaderboard.GameID));
            }
            else
            {
                var game = Game.Parse(client, properties["game"].data) as Game;
                leaderboard.game = new Lazy<Game>(() => game);
                leaderboard.GameID = game.ID;
            }

            if (properties["category"] is string)
            {
                leaderboard.CategoryID = leaderboardElement.category as string;
                leaderboard.category = new Lazy<Category>(() => client.Categories.GetCategory(leaderboard.CategoryID));
            }
            else
            {
                var category = Category.Parse(client, properties["category"].data) as Category;
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
                leaderboard.level = new Lazy<Level>(() => client.Levels.GetLevel(leaderboard.LevelID));
            }
            else
            {
                var level = Level.Parse(client, properties["level"].data) as Level;
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
                leaderboard.platformFilter = new Lazy<Platform>(() => client.Platforms.GetPlatform(leaderboard.PlatformIDOfFilter));
            }
            else
            {
                var platform = Platform.Parse(client, properties["platform"].data) as Platform;
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
                leaderboard.regionFilter = new Lazy<Region>(() => client.Regions.GetRegion(leaderboard.RegionIDOfFilter));
            }
            else
            {
                var region = Region.Parse(client, properties["region"].data) as Region;
                leaderboard.regionFilter = new Lazy<Region>(() => region);
                if (region != null)
                    leaderboard.RegionIDOfFilter = region.ID;
            }

            //Parse Embeds

            if (properties.ContainsKey("players"))
            {
                Func<dynamic, Player> playerParser = x => Player.Parse(client, x) as Player;
                var players = client.ParseCollection(leaderboardElement.players.data, playerParser) as ReadOnlyCollection<Player>;

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
                Func<dynamic, Region> regionParser = x => Region.Parse(client, x) as Region;
                var regions = client.ParseCollection(leaderboardElement.regions.data, regionParser) as ReadOnlyCollection<Region>;
                
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
                Func<dynamic, Platform> platformParser = x => Platform.Parse(client, x) as Platform;
                var platforms = client.ParseCollection(leaderboardElement.platforms.data, platformParser) as ReadOnlyCollection<Platform>;

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
                Func<dynamic, Variable> variableParser = x => Variable.Parse(client, x) as Variable;
                var variables = client.ParseCollection(leaderboardElement.variables.data, variableParser) as ReadOnlyCollection<Variable>;

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
        */
    }
}
