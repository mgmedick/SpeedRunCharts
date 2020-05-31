using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
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

        public Run GetRunFromSiteUri(string siteUri, RunEmbeds embeds = default(RunEmbeds))
        {
            var id = GetRunIDFromSiteUri(siteUri);

            if (string.IsNullOrEmpty(id))
                return null;

            return GetRun(id, embeds);
        }

        public string GetRunIDFromSiteUri(string siteUri)
        {
            var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

            if (elementDescription == null 
                || elementDescription.Type != ElementType.Run)
                return null;

            return elementDescription.ID;
        }

        public IEnumerable<Run> GetRuns(
            string userId = null, string guestName = null,
            string examerUserId = null, string gameId = null,
            string levelId = null, string categoryId = null,
            string platformId = null, string regionId = null,
            bool onlyEmulatedRuns = false, RunStatusType? status = null,
            int? elementsPerPage = null,
            RunEmbeds embeds = null,
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
            //return DoPaginatedRequest(uri, x => Parse(x) as Run);
            return DoPaginatedRequest(uri, x => Parse(x) as Run);
        }

        public Run GetRun(string runId,
            RunEmbeds embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            var uri = GetRunsUri(string.Format("/{0}{1}",
                Uri.EscapeDataString(runId),
                parameters.ToParameters()));

            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public Run Submit(string categoryId,
            string platformId,
            string levelId = null,
            DateTime? date = null,
            string regionId = null,
            TimeSpan? realTime = null,
            TimeSpan? realTimeWithoutLoads = null,
            TimeSpan? gameTime = null,
            bool? emulated = null,
            Uri videoUri = null,
            string comment = null,
            Uri splitsIOUri = null,
            IEnumerable<VariableValue> variables = null,
            bool? verify = null,
            bool simulateSubmitting = false)
        {
            var parameters = new List<string>();

            if (simulateSubmitting)
                parameters.Add("dry=yes");

            var uri = GetRunsUri(parameters.ToParameters());

            dynamic postBody = new DynamicJsonObject();
            dynamic runElement = new DynamicJsonObject();

            runElement.category = categoryId;
            runElement.platform = platformId;

            if (!string.IsNullOrEmpty(levelId))
                runElement.level = levelId;

            if (date.HasValue)
                runElement.date = date.Value.ToUniversalTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(regionId))
                runElement.region = regionId;

            if (verify.HasValue)
                runElement.verified = verify;

            dynamic timesElement = new DynamicJsonObject();

            if (!realTime.HasValue
                && !realTimeWithoutLoads.HasValue
                && !gameTime.HasValue)
            {
                throw new APIException("You need to provide at least one time.");
            }

            if (realTime.HasValue)
                timesElement.realtime = realTime.Value.TotalSeconds;

            if (realTimeWithoutLoads.HasValue)
                timesElement.realtime_noloads = realTimeWithoutLoads.Value.TotalSeconds;

            if (gameTime.HasValue)
                timesElement.ingame = gameTime.Value.TotalSeconds;

            runElement.times = timesElement;

            if (emulated.HasValue)
                runElement.emulated = emulated.Value;

            if (videoUri != null)
                runElement.video = videoUri.AbsoluteUri;

            if (!string.IsNullOrEmpty(comment))
                runElement.comment = comment;

            if (splitsIOUri != null)
                runElement.splitsio = splitsIOUri.PathAndQuery.Substring(splitsIOUri.PathAndQuery.LastIndexOf('/') + 1);

            if (variables != null)
            {
                var variablesList = variables.ToList();

                if (variablesList.Any())
                {
                    var variablesElement = new Dictionary<string, dynamic>();

                    foreach (var variable in variablesList)
                    {
                        var key = variable.VariableID;
                        dynamic value = new DynamicJsonObject();

                        if (variable.IsCustomValue)
                        {
                            value.type = "user-defined";
                            value.value = variable.Value;
                        }
                        else
                        {
                            value.type = "pre-defined";
                            value.value = variable.ID;
                        }

                        variablesElement.Add(key, value);
                    }

                    runElement.variables = variablesElement;
                }
            }

            postBody.run = runElement;

            var result = DoPostRequest(uri, postBody.ToString());

            return Parse(result.data);
        }

        //public Record Parse(dynamic runElement)
        //{
        //    Run record = new Record();
        //    Parse(runElement, record);

        //    return (Record)record;
        //}

        //public Run Parse(dynamic runElement)
        //{
        //    var run = new Run();
        //    Parse(runElement, run);

        //    return run;
        //}

        public Run Parse(dynamic runElement, Run run = null)
        {
            if (run == null)
            {
                run = new Run();
            }

            //Parse Attributes
            run.ID = runElement.id as string;
            run.WebLink = new Uri(runElement.weblink as string);
            run.Videos = ParseRunVideos(runElement.videos) as RunVideos;
            run.Comment = runElement.comment as string;
            run.Status = ParseRunStatus(runElement.status) as RunStatus;

            Func<dynamic, Player> parsePlayer = x => Client.Common.ParsePlayer(x) as Player;

            if (runElement.players is IEnumerable<dynamic>)
            {
                run.Players = ParseCollection(runElement.players, parsePlayer);
            }
            else
            {
                run.Players = ParseCollection(runElement.players.data, parsePlayer);
            }

            var runDate = runElement.date;
            if (!string.IsNullOrEmpty(runDate))
                run.Date = DateTime.Parse(runDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            //var dateSubmitted = runElement.submitted as string;
            //if (!string.IsNullOrEmpty(dateSubmitted))
            //DateTime.Parse(dateSubmitted, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            if (runElement.submitted != null)
            {
                run.DateSubmitted = runElement.submitted;
            }

            run.Times = ParseRunTimes(runElement.times) as RunTimes;
            run.System = ParseRunSystem(runElement.system) as RunSystem;

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
                run.game = new Lazy<Game>(() => Client.Games.GetGame(run.GameID));
            }
            else
            {
                var game = Client.Games.Parse(properties["game"].data) as Game;
                run.game = new Lazy<Game>(() => game);
                run.GameID = game.ID;
            }

            if (properties["category"] == null)
            {
                run.category = new Lazy<Category>(() => null);
            }
            else if (properties["category"] is string)
            {
                run.CategoryID = runElement.category as string;
                run.category = new Lazy<Category>(() => Client.Categories.GetCategory(run.CategoryID));
            }
            else
            {
                var category = Client.Categories.Parse(properties["category"].data) as Category;
                run.category = new Lazy<Category>(() => category);
                if (category != null)
                    run.CategoryID = category.ID;
            }

            if (properties["level"] == null)
            {
                run.level = new Lazy<Level>(() => null);
            }
            else if (properties["level"] is string)
            {
                run.LevelID = runElement.level as string;
                run.level = new Lazy<Level>(() => Client.Levels.GetLevel(run.LevelID));
            }
            else
            {
                var level = Client.Levels.Parse(properties["level"].data) as Level;
                run.level = new Lazy<Level>(() => level);
                if (level != null)
                    run.LevelID = level.ID;
            }

            if (properties.ContainsKey("platform"))
            {
                var platform = Client.Platforms.Parse(properties["platform"].data) as Platform;
                run.System.platform = new Lazy<Platform>(() => platform);
            }

            if (properties.ContainsKey("region"))
            {
                var region = Client.Regions.Parse(properties["region"].data) as Region;
                run.System.region = new Lazy<Region>(() => region);
            }

            if (!string.IsNullOrEmpty(run.Status.ExaminerUserID))
            {
                run.examiner = new Lazy<User>(() => Client.Users.GetUser(run.Status.ExaminerUserID));
            }
            else
            {
                run.examiner = new Lazy<User>(() => null);
            }

            return run;
        }

        private RunStatus ParseRunStatus(dynamic statusElement)
        {
            var status = new RunStatus();

            var properties = statusElement.Properties as IDictionary<string, dynamic>;

            status.Type = ParseRunStatusType(statusElement.status as string);

            if (status.Type == RunStatusType.Rejected
                || status.Type == RunStatusType.Verified)
            {
                status.ExaminerUserID = statusElement.examiner as string;
                if (!string.IsNullOrWhiteSpace(status.ExaminerUserID))
                {
                    status.examiner = new Lazy<User>(() => Client.Users.GetUser(status.ExaminerUserID));
                }

                if (status.Type == RunStatusType.Verified)
                {
                    var date = properties["verify-date"] as string;
                    if (!string.IsNullOrEmpty(date))
                        status.VerifyDate = DateTime.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
            }
            else
            {
                status.examiner = new Lazy<User>(() => null);
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

        private RunSystem ParseRunSystem(dynamic systemElement)
        {
            var system = new RunSystem();

            system.IsEmulated = (bool)systemElement.emulated;

            if (!string.IsNullOrEmpty(systemElement.platform as string))
            {
                system.PlatformID = systemElement.platform as string;
                system.platform = new Lazy<Platform>(() => Client.Platforms.GetPlatform(system.PlatformID));
            }
            else
            {
                system.platform = new Lazy<Platform>(() => null);
            }

            if (!string.IsNullOrEmpty(systemElement.region as string))
            {
                system.RegionID = systemElement.region as string;
                system.region = new Lazy<Region>(() => Client.Regions.GetRegion(system.RegionID));
            }
            else
            {
                system.region = new Lazy<Region>(() => null);
            }

            return system;
        }

        private RunTimes ParseRunTimes(dynamic timesElement)
        {
            var times = new RunTimes();

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

        private RunVideos ParseRunVideos(dynamic videosElement)
        {
            if (videosElement == null)
                return null;

            var videos = new RunVideos();

            videos.Text = videosElement.text as string;

            videos.Links = ParseCollection(videosElement.links, new Func<dynamic, Uri>(parseVideoLink));

            return videos;
        }

        private Uri parseVideoLink(dynamic element)
        {
            var videoUri = element.uri as string;
            if (!string.IsNullOrEmpty(videoUri))
            {
                if (!videoUri.StartsWith("http"))
                    videoUri = "http://" + videoUri;

                if (Uri.IsWellFormedUriString(videoUri, UriKind.Absolute))
                    return new Uri(videoUri);
            }

            return null;
        }
    }
}
