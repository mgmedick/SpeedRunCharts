using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Client
{
    public class RunsClient : BaseClient
    {

        public const string Name = "runs";

        public RunsClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetRunsUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public IEnumerable<SpeedRun> GetRuns(
            string userId = null, string guestName = null,
            string examerUserId = null, string gameId = null,
            string levelId = null, string categoryId = null,
            string platformId = null, string regionId = null,
            bool onlyEmulatedRuns = false, RunStatusType? status = null,
            int? elementsPerPage = null,
            SpeedRunEmbeds embeds = null,
            RunsOrdering orderBy = default(RunsOrdering),
            int? elementsOffset = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            if (!string.IsNullOrEmpty(userId))
                parameters.Add(string.Format("user={0}", Uri.EscapeDataString(userId)));
            if (!string.IsNullOrEmpty(guestName))
                parameters.Add(string.Format("guest={0}", Uri.EscapeDataString(guestName)));
            if (!string.IsNullOrEmpty(examerUserId))
                parameters.Add(string.Format("examiner={0}", Uri.EscapeDataString(examerUserId)));
            if (!string.IsNullOrEmpty(gameId))
                parameters.Add(string.Format("game={0}", Uri.EscapeDataString(gameId)));
            if (!string.IsNullOrEmpty(levelId))
                parameters.Add(string.Format("level={0}", Uri.EscapeDataString(levelId)));
            if (!string.IsNullOrEmpty(categoryId))
                parameters.Add(string.Format("category={0}", Uri.EscapeDataString(categoryId)));
            if (!string.IsNullOrEmpty(platformId))
                parameters.Add(string.Format("platform={0}", Uri.EscapeDataString(platformId)));
            if (!string.IsNullOrEmpty(regionId))
                parameters.Add(string.Format("region={0}", Uri.EscapeDataString(regionId)));
            if (onlyEmulatedRuns)
                parameters.Add("emulated=yes");
            if (status.HasValue)
            {
                switch (status.Value)
                {
                    case RunStatusType.New:
                        parameters.Add("status=new"); break;
                    case RunStatusType.Rejected:
                        parameters.Add("status=rejected"); break;
                    case RunStatusType.Verified:
                        parameters.Add("status=verified"); break;
                }
            }
            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage));
            if(elementsOffset.HasValue)
                parameters.Add(string.Format("offset={0}", elementsOffset));

            parameters.AddRange(orderBy.ToParameters());

            var uri = GetRunsUri(parameters.ToParameters());

            return DoRequest(uri, x => Parse(x) as SpeedRun);
        }

        public SpeedRun GetRun(string runId,
            SpeedRunEmbeds embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            var uri = GetRunsUri(string.Format("/{0}{1}",
                Uri.EscapeDataString(runId),
                parameters.ToParameters()));

            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public SpeedRunRecord Parse(dynamic runElement)
        {
            SpeedRunRecord record = new SpeedRunRecord();
            Parse(runElement, record);

            return (SpeedRunRecord)record;
        }

        public SpeedRun Parse(dynamic runElement, SpeedRun run = null)
        {
            if (run == null)
            {
                run = new SpeedRun();
            }
            var properties = runElement.Properties as IDictionary<string, dynamic>;

            //Parse Attributes
            run.ID = runElement.id as string;
            run.WebLink = new Uri(runElement.weblink as string);
            run.Videos = ParseRunVideos(runElement.videos) as SpeedRunVideos;
            run.Comment = runElement.comment as string;
            run.Status = ParseRunStatus(runElement.status) as SpeedRunStatus;

            //Func<dynamic, Player> parsePlayer = x => Client.Common.ParsePlayer(x) as Player;

            //if (runElement.players is IEnumerable<dynamic>)
            //{
            //    run.Players = ParseCollection(runElement.players, parsePlayer);
            //}
            //else
            //{
            //    run.Players = ParseCollection(runElement.players.data, parsePlayer);
            //}

            var runDate = runElement.date;
            if (!string.IsNullOrWhiteSpace(runDate))
                run.Date = DateTime.Parse(runDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            if (runElement.submitted != null)
            {
                run.DateSubmitted = runElement.submitted;
            }

            run.Times = ParseRunTimes(runElement.times) as SpeedRunTimes;
            run.System = ParseRunSystem(runElement.system, properties) as SpeedRunSystem;

            var splits = runElement.splits;
            if (splits != null)
            {
                run.SplitsUri = new Uri(splits.uri as string);
            }

            if (runElement.values is DynamicJsonObject)
            {
                var valueProperties = runElement.values.Properties as IDictionary<string, dynamic>;
                run.VariableValueMappings = valueProperties.Select(x => Client.Variables.ParseVariableValueMapping(x) as VariableValueMapping).ToList().AsReadOnly();
            }

            //Parse embeds
            if (runElement.players is IEnumerable<dynamic>)
            {
                Func<dynamic, Player> parsePlayer = x => Client.Common.ParsePlayer(x) as Player;
                run.Players = ParseCollection(runElement.players, parsePlayer);
            }
            else
            {
                Func<dynamic, User> userParser = x => Client.Users.Parse(x) as User;
                IEnumerable<User> users = ParseCollection(runElement.players.data, userParser);
                run.PlayerUsers = users;
            }

            if (properties["game"] is string)
            {
                run.GameID = runElement.game as string;
            }
            else
            {
                var game = Client.Games.Parse(properties["game"].data) as Game;
                run.Game = game;
                run.GameID = game.ID;
            }

            if (properties["category"] is string)
            {
                run.CategoryID = runElement.category as string;
            }
            else
            {
                var category = Client.Categories.Parse(properties["category"].data) as Category;
                run.Category = category;
                run.CategoryID = category.ID;
            }

            if (properties["level"] is string)
            {
                run.LevelID = runElement.level as string;
            }
            else
            {
                var level = Client.Levels.Parse(properties["level"].data) as Level;
                if (level != null)
                {
                    run.Level = level;
                    run.LevelID = level.ID;
                }
            }



            return run;
        }

        private SpeedRunStatus ParseRunStatus(dynamic statusElement)
        {
            var status = new SpeedRunStatus();
            var properties = statusElement.Properties as IDictionary<string, dynamic>;

            status.Type = ParseRunStatusType(statusElement.status as string);

            if (status.Type == RunStatusType.Rejected || status.Type == RunStatusType.Verified)
            {
                status.ExaminerUserID = statusElement.examiner as string;
            }

            if (status.Type == RunStatusType.Verified)
            {
                var date = properties["verify-date"] as string;
                if (!string.IsNullOrEmpty(date))
                    status.VerifyDate = DateTime.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }

            if (status.Type == RunStatusType.Rejected)
            {
                status.Reason = statusElement.reason as string;
            }

            return status;
        }

        private RunStatusType ParseRunStatusType(string type)
        {
            switch (type)
            {
                case "new":
                    return RunStatusType.New;
                case "verified":
                    return RunStatusType.Verified;
                case "rejected":
                    return RunStatusType.Rejected;
            }

            throw new ArgumentException("type");
        }

        private SpeedRunSystem ParseRunSystem(dynamic systemElement, IDictionary<string, dynamic> properties)
        {
            var system = new SpeedRunSystem();

            system.IsEmulated = (bool)systemElement.emulated;
            system.PlatformID = systemElement.platform as string;
            system.RegionID = systemElement.region as string;

            if (properties.ContainsKey("platform"))
            {
                var platform = Client.Platforms.Parse(properties["platform"].data) as Platform;
                system.Platform = platform;
            }

            if (properties.ContainsKey("region"))
            {
                var region = Client.Regions.Parse(properties["region"].data) as Region;
                system.Region = region;
            }

            return system;
        }

        private SpeedRunTimes ParseRunTimes(dynamic timesElement)
        {
            var times = new SpeedRunTimes();

            if (timesElement.primary != null)
                times.Primary = TimeSpan.FromSeconds((double)timesElement.primary_t);

            if (timesElement.realtime != null)
                times.RealTime = TimeSpan.FromSeconds((double)timesElement.realtime_t);

            if (timesElement.realtime_noloads != null)
                times.RealTimeWithoutLoads = TimeSpan.FromSeconds((double)timesElement.realtime_noloads_t);

            if (timesElement.ingame != null)
                times.GameTime = TimeSpan.FromSeconds((double)timesElement.ingame_t);

            return times;
        }

        private SpeedRunVideos ParseRunVideos(dynamic videosElement)
        {
            SpeedRunVideos videos = new SpeedRunVideos();

            if (videosElement != null)
            {
                //if (videosElement.text != null)
                //{
                //    videos.Text = videosElement.text as string;
                //}
                videos.Text = videosElement.text as string;
                videos.Links = ParseCollection(videosElement.links, new Func<dynamic, Uri>(parseVideoLink));
            }

            return videos;
        }

        private Uri parseVideoLink(dynamic element)
        {
            Uri videoUri = null;

            var videoUriString = element.uri as string;
            if (!string.IsNullOrWhiteSpace(videoUriString))
            {
                if (!videoUriString.StartsWith("http"))
                {
                    videoUriString = "http://" + videoUri;
                }

                if (Uri.IsWellFormedUriString(videoUriString, UriKind.Absolute))
                {
                    videoUri = new Uri(videoUriString);
                }
            }

            return videoUri;
        }
    }
}
