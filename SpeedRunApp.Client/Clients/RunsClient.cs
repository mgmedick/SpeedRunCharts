using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public IEnumerable<SpeedRunDTO> GetRuns(
            string userId = null, string guestName = null,
            string examerUserId = null, string gameId = null,
            string levelId = null, string categoryId = null,
            string platformId = null, string regionId = null,
            bool onlyEmulatedRuns = false, RunStatusType? status = null,
            int? elementsPerPage = null,
            SpeedRunEmbedsDTO embeds = null,
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

            return DoRequest(uri, x => Parse(x) as SpeedRunDTO);
        }

        public SpeedRunDTO GetRun(string runId,
            SpeedRunEmbedsDTO embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            var uri = GetRunsUri(string.Format("/{0}{1}",
                Uri.EscapeDataString(runId),
                parameters.ToParameters()));

            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public SpeedRunRecordDTO Parse(dynamic runElement)
        {
            SpeedRunRecordDTO record = new SpeedRunRecordDTO();
            Parse(runElement, record);

            return (SpeedRunRecordDTO)record;
        }

        public SpeedRunDTO Parse(dynamic runElement, SpeedRunDTO run = null)
        {
            if (run == null)
            {
                run = new SpeedRunDTO();
            }

            //Parse Attributes
            run.ID = runElement.id as string;
            run.WebLink = new Uri(runElement.weblink as string);
            run.Videos = ParseRunVideos(runElement.videos) as SpeedRunVideosDTO;
            run.Comment = runElement.comment as string;
            run.Status = ParseRunStatus(runElement.status) as SpeedRunStatusDTO;

            Func<dynamic, PlayerDTO> parsePlayer = x => Client.Common.ParsePlayer(x) as PlayerDTO;

            if (runElement.players is IEnumerable<dynamic>)
            {
                run.Players = ParseCollection(runElement.players, parsePlayer);
            }
            else
            {
                run.Players = ParseCollection(runElement.players.data, parsePlayer);
            }

            var runDate = runElement.date;
            if (!string.IsNullOrWhiteSpace(runDate))
                run.Date = DateTime.Parse(runDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            if (runElement.submitted != null)
            {
                run.DateSubmitted = runElement.submitted;
            }

            run.Times = ParseRunTimes(runElement.times) as SpeedRunTimesDTO;
            run.System = ParseRunSystem(runElement.system) as SpeedRunSystemDTO;

            var splits = runElement.splits;
            if (splits != null)
            {
                run.SplitsUri = new Uri(splits.uri as string);
            }

            if (runElement.values is DynamicJsonObject)
            {
                var valueProperties = runElement.values.Properties as IDictionary<string, dynamic>;
                run.VariableValues = valueProperties.Select(x => Client.Variables.ParseValueDescriptor(x) as VariableValue).ToList().AsReadOnly();
            }
            else
            {
                run.VariableValues = new List<VariableValue>().AsReadOnly();
            }

            //Parse Links
            var properties = runElement.Properties as IDictionary<string, dynamic>;

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
                if (category != null)
                {
                    run.Category = category;
                    run.CategoryID = category.ID;
                }
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

            if (properties.ContainsKey("platform"))
            {
                var platform = Client.Platforms.Parse(properties["platform"].data) as Platform;
                run.System.Platform = platform;
            }

            if (properties.ContainsKey("region"))
            {
                var region = Client.Regions.Parse(properties["region"].data) as Region;
                run.System.Region = region;
            }

            return run;
        }

        private SpeedRunStatusDTO ParseRunStatus(dynamic statusElement)
        {
            var status = new SpeedRunStatusDTO();
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

        private SpeedRunSystemDTO ParseRunSystem(dynamic systemElement)
        {
            var system = new SpeedRunSystemDTO();

            system.IsEmulated = (bool)systemElement.emulated;
            system.PlatformID = systemElement.platform as string;
            system.RegionID = systemElement.region as string;

            return system;
        }

        private SpeedRunTimesDTO ParseRunTimes(dynamic timesElement)
        {
            var times = new SpeedRunTimesDTO();

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

        private SpeedRunVideosDTO ParseRunVideos(dynamic videosElement)
        {
            SpeedRunVideosDTO videos = null;

            if (videosElement != null)
            {
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
