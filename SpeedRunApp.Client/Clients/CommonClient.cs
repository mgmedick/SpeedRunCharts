using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunApp.Model;
using SpeedRunCommon;
using System.IO;

namespace SpeedRunApp.Client
{
    public class CommonClient : BaseClient
    {

        public const string Name = "common";

        public CommonClient(ClientContainer client) : base(client)
        {
        }

        public ReadOnlyCollection<T> ParseCollection<T>(dynamic collection, Func<dynamic, T> parser)
        {
            var enumerable = collection as IEnumerable<dynamic>;
            if (enumerable == null)
                return new List<T>(new T[0]).AsReadOnly();

            return enumerable.Select(parser).ToList().AsReadOnly();
        }

        public ReadOnlyCollection<T> ParseCollection<T>(dynamic collection)
        {
            var enumerable = collection as IEnumerable<dynamic>;
            if (enumerable == null)
                return new List<T>(new T[0]).AsReadOnly();

            return enumerable.OfType<T>().ToList().AsReadOnly();
        }

        public Assets ParseAssets(dynamic assetsElement)
        {
            var assets = new Assets();

            var properties = assetsElement.Properties as IDictionary<string, dynamic>;

            assets.Logo = ParseImageAsset(assetsElement.logo) as ImageAsset;
            assets.CoverTiny = ParseImageAsset(properties["cover-tiny"]) as ImageAsset;
            assets.CoverSmall = ParseImageAsset(properties["cover-small"]) as ImageAsset;
            assets.CoverMedium = ParseImageAsset(properties["cover-medium"]) as ImageAsset;
            assets.CoverLarge = ParseImageAsset(properties["cover-large"]) as ImageAsset;
            assets.Icon = ParseImageAsset(assetsElement.icon) as ImageAsset;
            assets.TrophyFirstPlace = ParseImageAsset(properties["trophy-1st"]) as ImageAsset;
            assets.TrophySecondPlace = ParseImageAsset(properties["trophy-2nd"]) as ImageAsset;
            assets.TrophyThirdPlace = ParseImageAsset(properties["trophy-3rd"]) as ImageAsset;
            assets.TrophyFourthPlace = ParseImageAsset(properties["trophy-4th"]) as ImageAsset;
            assets.BackgroundImage = ParseImageAsset(assetsElement.background) as ImageAsset;
            assets.ForegroundImage = ParseImageAsset(assetsElement.foreground) as ImageAsset;

            return assets;
        }

        private ImageAsset ParseImageAsset(dynamic imageElement)
        {
            if (imageElement == null)
                return null;

            var image = new ImageAsset();

            var uri = imageElement.uri as string;
            image.Uri = new Uri(uri);
            image.Width = (int)imageElement.width;
            image.Height = (int)imageElement.height;

            return image;
        }

        public Moderator ParseModerator(KeyValuePair<string, dynamic> moderatorElement)
        {
            var moderator = new Moderator();

            moderator.UserID = moderatorElement.Key;
            moderator.Type = moderatorElement.Value as string == "moderator"
                ? ModeratorType.Moderator
                : ModeratorType.SuperModerator;

            moderator.user = new Lazy<User>(() => Client.Users.GetUser(moderator.UserID));

            return moderator;
        }

        public PlayerDTO ParsePlayer(dynamic playerElement)
        {
            var player = new PlayerDTO();

            var properties = playerElement.Properties as IDictionary<string, dynamic>;

            if (playerElement.rel as string == "user")
            {
                player.UserID = playerElement.id as string;
            }
            else
            {
                player.GuestName = playerElement.name as string;
            }

            return player;
        }

        public Record ParseRecord(dynamic recordElement)
        {
            Record record = new Record();

            record.Rank = (int)recordElement.place;

            //Parse potential embeds
            var properties = recordElement.Properties as IDictionary<string, dynamic>;

            if (properties.ContainsKey("game"))
                recordElement.run.game = recordElement.game;
            if (properties.ContainsKey("category"))
                recordElement.run.category = recordElement.category;
            if (properties.ContainsKey("level"))
                recordElement.run.level = recordElement.level;
            if (properties.ContainsKey("players"))
                recordElement.run.players = recordElement.players;
            if (properties.ContainsKey("region"))
                recordElement.run.region = recordElement.region;
            if (properties.ContainsKey("platform"))
                recordElement.run.platform = recordElement.platform;

            Client.Runs.Parse(recordElement.run, record);

            return record;
        }

        //BaseService objects
        public ElementDescription ParseElementDescription(string uri)
        {
            var splits = uri.Split('/');

            if (splits.Length < 2)
                return null;

            var id = splits[splits.Length - 1];
            var uriTypeString = splits[splits.Length - 2];

            try
            {
                var uriType = parseUriType(uriTypeString);
                return new ElementDescription(id, uriType);
            }
            catch
            {
                return null;
            }
        }

        private ElementType parseUriType(string type)
        {
            switch (type)
            {
                case CategoriesClient.Name:
                    return ElementType.Category;
                case GamesClient.Name:
                    return ElementType.Game;
                case GuestsClient.Name:
                    return ElementType.Guest;
                case LevelsClient.Name:
                    return ElementType.Level;
                case NotificationsClient.Name:
                    return ElementType.Notification;
                case PlatformsClient.Name:
                    return ElementType.Platform;
                case RegionsClient.Name:
                    return ElementType.Region;
                case RunsClient.Name:
                    return ElementType.Run;
                case SeriesClient.Name:
                    return ElementType.Series;
                case UsersClient.Name:
                    return ElementType.User;
                case VariablesClient.Name:
                    return ElementType.Variable;
            }
            throw new ArgumentException("type");
        }

        public ReadOnlyCollection<HttpWebLink> ParseLinks(string linksString)
        {
            return (linksString ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => ParseLink(x.Trim(' ')))
                .ToList()
                .AsReadOnly();
        }

        private HttpWebLink ParseLink(string linkString)
        {
            var link = new HttpWebLink();

            var leftAngledParenthesis = linkString.IndexOf('<');
            var rightAngledParenthesis = linkString.IndexOf('>');

            if (leftAngledParenthesis >= 0 && rightAngledParenthesis >= 0)
            {
                link.Uri = linkString.Substring(leftAngledParenthesis + 1, rightAngledParenthesis - leftAngledParenthesis - 1);
            }

            linkString = linkString.Substring(rightAngledParenthesis + 1);
            var parameters = linkString.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var parameter in parameters)
            {
                var splits = parameter.Split(new[] { '=' }, 2);
                if (splits.Length == 2)
                {
                    var parameterType = splits[0];
                    var parameterValue = splits[1].Trim('"');

                    switch (parameterType)
                    {
                        case "rel":
                            link.Relation = parameterValue;
                            break;
                        case "anchor":
                            link.Anchor = parameterValue;
                            break;
                        case "rev":
                            link.RelationTypes = parameterValue;
                            break;
                        case "hreflang":
                            link.Language = parameterValue;
                            break;
                        case "media":
                            link.Media = parameterValue;
                            break;
                        case "title":
                            link.Title = parameterValue;
                            break;
                        case "title*":
                            link.Titles = parameterValue;
                            break;
                        case "type":
                            link.Type = parameterValue;
                            break;
                    }
                }
            }

            return link;
        }

        public APIException ParseAPIException(Stream stream)
        {
            var json = JSONHelper.FromStream(stream);
            var properties = json.Properties as IDictionary<string, dynamic>;
            if (properties.ContainsKey("errors"))
            {
                var errors = json.errors as IList<dynamic>;
                return new APIException(json.message as string, errors.Select(x => x as string));
            }
            else
                return new APIException(json.message as string);
        }
    }
}
