using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpeedRunApp.Client
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
            LeaderboardEmbeds embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

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

            leaderboard.VideoOnlyFilter = properties["video-only"];

            leaderboard.TimingMethodFilter = (leaderboardElement.timing as string).ToTimingMethod();

            if (leaderboardElement.values is DynamicJsonObject)
            {
                var valueProperties = leaderboardElement.values.Properties as IDictionary<string, dynamic>;
                leaderboard.VariableValueMappingFilters = valueProperties.Select(x => Client.Variables.ParseVariableValueMapping(x) as VariableValueMapping).ToList();
            }

            Func<dynamic, SpeedRunRecord> recordParser = x => Client.Common.ParseRecord(x) as SpeedRunRecord;
            leaderboard.Records = ParseCollection(leaderboardElement.runs, recordParser);

            //Parse Embeds
            if (properties["game"] is string)
            {
                leaderboard.GameID = leaderboardElement.game as string;
            }
            else
            {
                var game = Client.Games.Parse(properties["game"].data) as Game;
                leaderboard.Game = game;
                leaderboard.GameID = game.ID;

                foreach (var record in leaderboard.Records)
                {
                    record.Game = leaderboard.Game;
                }
            }

            if (properties["category"] is string)
            {
                leaderboard.CategoryID = leaderboardElement.category as string;
            }
            else
            {
                var category = Client.Categories.Parse(properties["category"].data) as Category;
                leaderboard.Category = category;
                leaderboard.CategoryID = category.ID;

                foreach (var record in leaderboard.Records)
                {
                    record.Category = leaderboard.Category;
                }
            }

            if (properties["level"] == null)
            {
                leaderboard.LevelID = null;
            }
            else if (properties["level"] is string)
            {
                leaderboard.LevelID = leaderboardElement.level as string;
            }
            else
            {
                var level = Client.Levels.Parse(properties["level"].data) as Level;
                if (level != null)
                {
                    leaderboard.Level = level;
                    leaderboard.LevelID = level.ID;

                    foreach (var record in leaderboard.Records)
                    {
                        record.Level = leaderboard.Level;
                    }
                }
            }

            if (properties["platform"] == null)
            {
                leaderboard.PlatformFilterID = null;
            }
            else if (properties["platform"] is string)
            {
                leaderboard.PlatformFilterID = properties["platform"] as string;
            }
            else
            {
                var platform = Client.Platforms.Parse(properties["platform"].data) as Platform;
                if (platform != null)
                {
                    leaderboard.PlatformFilter = platform;
                    leaderboard.PlatformFilterID = platform.ID;
                }
            }

            if (properties["region"] == null)
            {
                leaderboard.RegionFilterID = null;
            }
            else if (properties["region"] is string)
            {
                leaderboard.RegionFilterID = properties["region"] as string;
            }
            else
            {
                var region = Client.Regions.Parse(properties["region"].data) as Region;
                if (region != null)
                {
                    leaderboard.RegionFilter = region;
                    leaderboard.RegionFilterID = region.ID;
                }
            }

            if (properties.ContainsKey("players"))
            {
                var data = (leaderboardElement.players.data as IEnumerable<dynamic>);
                if (data != null && data.Any())
                {
                    Func<dynamic, User> userParser = x => Client.Users.Parse(x) as User;
                    var userData = data.Where(i => i.Properties["rel"] == "user");
                    IEnumerable<User> users = ParseCollection(userData, userParser);

                    Func<dynamic, Guest> guestParser = x => Client.Guests.Parse(x) as Guest;
                    var guestData = (leaderboardElement.players.data as IEnumerable<dynamic>).Where(i => i.Properties["rel"] == "guest");
                    IEnumerable<Guest> guests = ParseCollection(guestData, guestParser);

                    foreach (var record in leaderboard.Records)
                    {
                        record.PlayerUsers = users.Where(x => record.Players.Any(i => i.IsUser && i.UserID == x.ID))
                                                  .GroupBy(g => new { g.ID })
                                                  .Select(i => i.First())
                                                  .OrderBy(i => i.Name);

                        record.PlayerGuests = guests.Where(x => record.Players.Any(i => !i.IsUser && i.GuestName == x.Name))
                                                    .GroupBy(g => new { g.Name })
                                                    .Select(i => i.First())
                                                    .OrderBy(i => i.Name);
                    }

                    leaderboard.PlayerUsers = users.GroupBy(g => new { g.ID })
                                                   .Select(i => i.First())
                                                   .OrderBy(i => i.Name);

                    leaderboard.PlayerGuests = guests.GroupBy(g => new { g.Name })
                                                     .Select(i => i.First())
                                                     .OrderBy(i => i.Name);
                }
            }
            else
            {
                leaderboard.Players = leaderboard.Records.SelectMany(x => x.Players).ToList().Distinct().ToList();
            }

            if (properties.ContainsKey("regions"))
            {
                Func<dynamic, Region> regionParser = x => Client.Regions.Parse(x) as Region;
                var regions = ParseCollection(leaderboardElement.regions.data, regionParser) as IEnumerable<Region>;

                foreach (var record in leaderboard.Records)
                {
                    record.System.Region = regions.FirstOrDefault(x => x.ID == record.System.RegionID);
                }

                leaderboard.UsedRegions = regions;
            }
            else
            {
                leaderboard.UsedRegions = leaderboard.Records.Select(x => x.Region).Distinct().Where(x => x != null).ToList();
            }

            if (properties.ContainsKey("platforms"))
            {
                Func<dynamic, Platform> platformParser = x => Client.Platforms.Parse(x) as Platform;
                var platforms = ParseCollection(leaderboardElement.platforms.data, platformParser) as IEnumerable<Platform>;

                foreach (var record in leaderboard.Records)
                {
                    record.System.Platform = platforms.FirstOrDefault(x => x.ID == record.System.PlatformID);
                }

                leaderboard.UsedPlatforms = platforms;
            }
            else
            {
                leaderboard.UsedPlatforms = leaderboard.Records.Select(x => x.Platform).Distinct().Where(x => x != null).ToList();
            }

            Action<IEnumerable<Variable>> patchVariablesOfRecords = variables =>
            {
                foreach (var record in leaderboard.Records)
                {
                    foreach (var value in record.VariableValueMappings)
                    {
                        value.Variable = variables.FirstOrDefault(x => x.ID == value.VariableID);
                    }
                }
            };

            if (properties.ContainsKey("variables"))
            {
                Func<dynamic, Variable> variableParser = x => Client.Variables.Parse(x) as Variable;
                var variables = ParseCollection(leaderboardElement.variables.data, variableParser) as IEnumerable<Variable>;

                patchVariablesOfRecords(variables);
                leaderboard.ApplicableVariables = variables;
            }
            else if (!string.IsNullOrWhiteSpace(leaderboard.LevelID))
            {
                var variables = (leaderboard.Category?.Variables ?? new List<Variable>()).Concat(leaderboard.Level?.Variables ?? new List<Variable>());

                patchVariablesOfRecords(variables);
                leaderboard.ApplicableVariables = variables;
            }
            else if (leaderboard.Category != null)
            {
                var variables = leaderboard.Category.Variables;

                patchVariablesOfRecords(variables);
                leaderboard.ApplicableVariables = variables;
            }

            return leaderboard;
        }
    }
}
