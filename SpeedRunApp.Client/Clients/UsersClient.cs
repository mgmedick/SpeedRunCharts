using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SpeedRunCommon;

namespace SpeedRunApp.Client
{
    public class UsersClient : BaseClient
    {

        public const string Name = "users";

        public UsersClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetUsersUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public static Uri GetUsersSiteUri(string subUri)
        {
            return GetSiteUri(string.Format("{0}{1}", Name, subUri));
        }

        //public static Uri GetUserProfileImageUri(string subUri)
        //{
        //    var profileImageUri = new UriBuilder(BaseUri);
        //    var subUri = string.Format("/themes/user/{0}/image.png", subUri);
        //    var uri = GetUsersSiteUri(subUri);

        //    try
        //    {
        //        var request = (HttpWebRequest)WebRequest.Create(uri);
        //        request.GetResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        uri = null;
        //    }

        //    return uri;
        //}

        //public User GetUserFromSiteUri(string siteUri)
        //{
        //    var id = GetUserIDFromSiteUri(siteUri);

        //    if (string.IsNullOrEmpty(id))
        //        return null;

        //    return GetUser(id);
        //}

        //public string GetUserIDFromSiteUri(string siteUri)
        //{
        //    var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

        //    if (elementDescription == null
        //        || elementDescription.Type != ElementType.User)
        //        return null;

        //    return elementDescription.ID;
        //}

        public IEnumerable<User> GetUsers(
            string name = null, 
            string twitch = null, string hitbox = null, 
            string twitter = null, string speedrunslive = null, 
            int? elementsPerPage = null,
            UsersOrdering orderBy = default(UsersOrdering))
        {
            var parameters = new List<string>();

            if (!string.IsNullOrEmpty(name))
                parameters.Add(string.Format("name={0}", 
                    Uri.EscapeDataString(name)));

            if (!string.IsNullOrEmpty(twitch))
                parameters.Add(string.Format("twitch={0}",
                    Uri.EscapeDataString(twitch)));

            if (!string.IsNullOrEmpty(hitbox))
                parameters.Add(string.Format("hitbox={0}",
                    Uri.EscapeDataString(hitbox)));

            if (!string.IsNullOrEmpty(twitter))
                parameters.Add(string.Format("twitter={0}",
                    Uri.EscapeDataString(twitter)));

            if (!string.IsNullOrEmpty(speedrunslive))
                parameters.Add(string.Format("speedrunslive={0}",
                    Uri.EscapeDataString(speedrunslive)));
            
            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage));

            parameters.AddRange(orderBy.ToParameters());

            var uri = GetUsersUri(parameters.ToParameters());
            return DoRequest(uri, x => Parse(x) as User);
        }

        //public IEnumerable<User> GetUsersFuzzy(
        //    string fuzzyName = null,
        //    int? elementsPerPage = null,
        //    UsersOrdering orderBy = default(UsersOrdering))
        //{
        //    var parameters = new List<string>();

        //    if (!string.IsNullOrEmpty(fuzzyName))
        //        parameters.Add(string.Format("lookup={0}",
        //            Uri.EscapeDataString(fuzzyName)));

        //    if (elementsPerPage.HasValue)
        //        parameters.Add(string.Format("max={0}", elementsPerPage));

        //    parameters.AddRange(orderBy.ToParameters());

        //    var uri = GetUsersUri(parameters.ToParameters());
        //    return DoPaginatedRequest(uri,
        //        x => Parse(x) as User);
        //}

        public User GetUser(string userId, IEnumerable<Embeds> embeds = null)
        {
            User user = null;
            var uri = GetUsersUri(string.Format("/{0}",
                Uri.EscapeDataString(userId)));

            var result = DoRequest(uri);

            if(result != null)
            {
                user = Parse(result.data, embeds);
            }

            return user;
        }

        public IEnumerable<SpeedRunRecord> GetPersonalBests(
            string userId, int? top = null,
            string seriesId = null, string gameId = null,
            SpeedRunEmbeds embeds = null)
        {
            var parameters = new List<string>() { embeds?.ToString() };

            if (top.HasValue)
                parameters.Add(string.Format("top={0}", top.Value));
            if (!string.IsNullOrEmpty(seriesId))
                parameters.Add(string.Format("series={0}", Uri.EscapeDataString(seriesId)));
            if (!string.IsNullOrEmpty(gameId))
                parameters.Add(string.Format("game={0}", Uri.EscapeDataString(gameId)));

            var uri = GetUsersUri(string.Format("/{0}/personal-bests{1}",
                Uri.EscapeDataString(userId),
                parameters.ToParameters()));

            return DoRequest(uri, x => Client.Common.ParseRecord(x) as SpeedRunRecord);
        }

        public Uri GetUserProfileImageUri(string userName)
        {
            return Task.Run(async () => await ParseProfileImageUri(userName)).Result;
        }

        public User Parse(dynamic userElement, IEnumerable<Embeds> embeds = null)
        {
            var user = new User();
            var properties = userElement.Properties as IDictionary<string, dynamic>;

            //Parse Attributes
            user.ID = userElement.id as string;
            user.Name = userElement.names.international as string;
            user.JapaneseName = userElement.names.japanese as string;
            user.WebLink = new Uri(userElement.weblink as string);
            user.NameStyle = ParseUserNameStyle(properties["name-style"]) as UserNameStyle;
            user.Role = parseUserRole(userElement.role as string);

            var signUpDate = userElement.signup as string;
            if (!string.IsNullOrEmpty(signUpDate))
                user.SignUpDate = DateTime.Parse(signUpDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            user.Location = ParseLocation(userElement.location) as Location;

            var twitchLink = userElement.twitch;
            if (twitchLink != null)
                user.TwitchProfile = new Uri(twitchLink.uri as string);

            var hitboxLink = userElement.hitbox;
            if (hitboxLink != null)
                user.HitboxProfile = new Uri(hitboxLink.uri as string);

            var youtubeLink = userElement.youtube;
            if (youtubeLink != null)
                user.YoutubeProfile = new Uri(youtubeLink.uri as string);

            var twitterLink = userElement.twitter;
            if (twitterLink != null)
                user.TwitterProfile = new Uri(twitterLink.uri as string);

            var speedRunsLiveLink = userElement.speedrunslive;
            if (speedRunsLiveLink != null)
                user.SpeedRunsLiveProfile = new Uri(speedRunsLiveLink.uri as string);

            return user;
        }

        private static UserRole parseUserRole(string role)
        {
            switch (role)
            {
                case "banned":
                    return UserRole.Banned;
                case "user":
                    return UserRole.User;
                case "trusted":
                    return UserRole.Trusted;
                case "moderator":
                    return UserRole.Moderator;
                case "admin":
                    return UserRole.Admin;
                case "programmer":
                    return UserRole.Programmer;
                case "contentmoderator":
                    return UserRole.ContentModerator;
            }

            throw new ArgumentException("role");
        }

        private static UserNameStyle ParseUserNameStyle(dynamic styleElement)
        {
            var style = new UserNameStyle();

            style.IsGradient = styleElement.style == "gradient";

            if (style.IsGradient)
            {
                var properties = styleElement.Properties as IDictionary<string, dynamic>;
                var colorFrom = properties["color-from"];
                var colorTo = properties["color-to"];

                style.LightGradientStartColorCode = colorFrom.light as string;
                style.LightGradientEndColorCode = colorTo.light as string;
                style.DarkGradientStartColorCode = colorFrom.dark as string;
                style.DarkGradientEndColorCode = colorTo.dark as string;
            }
            else
            {
                style.LightSolidColorCode = styleElement.color.light as string;
                style.DarkSolidColorCode = styleElement.color.dark as string;
            }

            return style;
        }

        private Location ParseLocation(dynamic locationElement)
        {
            var location = new Location();

            if (locationElement != null)
            {
                location.Country = ParseCountry(locationElement.country) as Country;

                if (locationElement.region != null)
                    location.Region = ParseCountryRegion(locationElement.region) as CountryRegion;
            }

            return location;
        }

        private Country ParseCountry(dynamic countryElement)
        {
            var country = new Country();

            country.Code = countryElement.code as string;
            country.Name = countryElement.names.international as string;
            country.JapaneseName = countryElement.names.japanese as string;

            return country;
        }

        private CountryRegion ParseCountryRegion(dynamic regionElement)
        {
            var region = new CountryRegion();

            region.Code = regionElement.code as string;
            region.Name = regionElement.names.international as string;
            region.JapaneseName = regionElement.names.japanese as string;

            return region;
        }

        private async Task<Uri> ParseProfileImageUri(string userName)
        {
            var subUri = string.Format("/themes/user/{0}/image.png", userName);
            var uri = GetSiteUri(subUri);

            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        uri = null;
                    }
                }
            }

            return uri;
        }
    }
}
