using System;
using System.Collections.Generic;
using SpeedrunComSharp.Model;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
{
    public class SeriesClient : BaseClient
    {

        public const string Name = "series";

        public SeriesClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetSeriesUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public Series GetSeriesFromSiteUri(string siteUri, SeriesEmbeds embeds = default(SeriesEmbeds))
        {
            var id = GetSeriesIDFromSiteUri(siteUri);

            if (string.IsNullOrEmpty(id))
                return null;

            return GetSingleSeries(id, embeds);
        }

        public string GetSeriesIDFromSiteUri(string siteUri)
        {
            var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

            if (elementDescription == null
                || elementDescription.Type != ElementType.Series)
                return null;

            return elementDescription.ID;
        }

        public IEnumerable<Series> GetMultipleSeries(
           string name = null, string abbreviation = null,
           string moderatorId = null, int? elementsPerPage = null,
           SeriesEmbeds embeds = null,
           SeriesOrdering orderBy = default(SeriesOrdering))
        {
            var parameters = new List<string>() { embeds?.ToString() };

            parameters.AddRange(orderBy.ToParameters());

            if (!string.IsNullOrEmpty(name))
                parameters.Add(string.Format("name={0}", Uri.EscapeDataString(name)));

            if (!string.IsNullOrEmpty(abbreviation))
                parameters.Add(string.Format("abbreviation={0}", Uri.EscapeDataString(abbreviation)));

            if (!string.IsNullOrEmpty(moderatorId))
                parameters.Add(string.Format("moderator={0}", Uri.EscapeDataString(moderatorId)));

            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            var uri = GetSeriesUri(parameters.ToParameters());
            return DoPaginatedRequest(uri,
                x => Parse(x) as Series);
        }

        public Series GetSingleSeries(string seriesId, SeriesEmbeds embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            var uri = GetSeriesUri(string.Format("/{0}{1}",
                Uri.EscapeDataString(seriesId),
                parameters.ToParameters()));

            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public IEnumerable<Game> GetGames(
            string seriesId,
            string name = null, int? yearOfRelease = null,
            string platformId = null, string regionId = null,
            string moderatorId = null, int? elementsPerPage = null,
            GameEmbeds embeds = null,
            GamesOrdering orderBy = default(GamesOrdering))
        {
            var parameters = new List<string>() { embeds?.ToString() };

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

            var uri = GetSeriesUri(string.Format("/{0}/games{1}",
                Uri.EscapeDataString(seriesId),
                parameters.ToParameters()));

            return DoPaginatedRequest(uri,
                x => Client.Games.Parse(x) as Game);
        }

        public Series Parse(dynamic seriesElement)
        {
            var series = new Series();

            //Parse Attributes

            series.ID = seriesElement.id as string;
            series.Name = seriesElement.names.international as string;
            series.JapaneseName = seriesElement.names.japanese as string;
            series.WebLink = new Uri(seriesElement.weblink as string);
            series.Abbreviation = seriesElement.abbreviation as string;

            var created = seriesElement.created as string;
            if (!string.IsNullOrEmpty(created))
                series.CreationDate = DateTime.Parse(created, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            series.Assets = Client.Common.ParseAssets(seriesElement.assets);

            //Parse Embeds

            if (seriesElement.moderators is DynamicJsonObject && seriesElement.moderators.Properties.ContainsKey("data"))
            {
                Func<dynamic, User> userParser = x => Client.Users.Parse(x) as User;
                ReadOnlyCollection<User> users = ParseCollection(seriesElement.moderators.data, userParser);
                series.moderatorUsers = new Lazy<ReadOnlyCollection<User>>(() => users);
            }
            else if (seriesElement.moderators is DynamicJsonObject)
            {
                var moderatorsProperties = seriesElement.moderators.Properties as IDictionary<string, dynamic>;
                series.Moderators = moderatorsProperties.Select(x => Client.Common.ParseModerator(x)).ToList().AsReadOnly();

                series.moderatorUsers = new Lazy<ReadOnlyCollection<User>>(
                    () =>
                    {
                        ReadOnlyCollection<User> users;

                        if (series.Moderators.Count(x => !x.user.IsValueCreated) > 1)
                        {
                            users = Client.Games.GetGame(series.ID, embeds: new GameEmbeds(embedModerators: true)).ModeratorUsers;

                            foreach (var user in users)
                            {
                                var moderator = series.Moderators.FirstOrDefault(x => x.UserID == user.ID);
                                if (moderator != null)
                                {
                                    moderator.user = new Lazy<User>(() => user);
                                }
                            }
                        }
                        else
                        {
                            users = series.Moderators.Select(x => x.User).ToList().AsReadOnly();
                        }

                        return users;
                    });
            }
            else
            {
                series.Moderators = new ReadOnlyCollection<Moderator>(new Moderator[0]);
                series.moderatorUsers = new Lazy<ReadOnlyCollection<User>>(() => new List<User>().AsReadOnly());
            }

            //Parse Links

            series.Games = Client.Series.GetGames(series.ID).Select(game =>
            {
                game.series = new Lazy<Series>(() => series);
                return game;
            }).Cache();

            return series;
        }
    }
}
