using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using SpeedRunCommon;

namespace SpeedrunComSharp.Model
{
    public class User : IElementWithID
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public Uri WebLink { get; set; }
        public UserNameStyle NameStyle { get; set; }
        public UserRole Role { get; set; }
        public DateTime? SignUpDate { get; set; }
        public Location Location { get; set; }

        public Uri TwitchProfile { get; set; }
        public Uri HitboxProfile { get; set; }
        public Uri YoutubeProfile { get; set; }
        public Uri TwitterProfile { get; set; }
        public Uri SpeedRunsLiveProfile { get; set; }

        #region Links
        public Lazy<IEnumerable<Run>> runs { get; set; }
        public IEnumerable<Run> Runs { get { return runs.Value; } }
        public IEnumerable<Game> ModeratedGames { get; set; }
        public Lazy<ReadOnlyCollection<Record>> personalBests { get; set; }
        public ReadOnlyCollection<Record> PersonalBests { get { return personalBests.Value; } }

        #endregion

        public User() { }

        /*
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

        public static User Parse(SpeedrunComClient client, dynamic userElement)
        {
            var user = new User();

            var properties = userElement.Properties as IDictionary<string, dynamic>;

            //Parse Attributes

            user.ID = userElement.id as string;
            user.Name = userElement.names.international as string;
            user.JapaneseName = userElement.names.japanese as string;
            user.WebLink = new Uri(userElement.weblink as string);
            user.NameStyle = UserNameStyle.Parse(client, properties["name-style"]) as UserNameStyle;
            user.Role = parseUserRole(userElement.role as string);

            var signUpDate = userElement.signup as string;
            if (!string.IsNullOrEmpty(signUpDate))
                user.SignUpDate = DateTime.Parse(signUpDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            user.Location = Location.Parse(client, userElement.location) as Location;

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

            //Parse Links

            //user.Runs = client.Runs.GetRuns(userId: user.ID);
            user.ModeratedGames = client.Games.GetGames(moderatorId: user.ID);
            user.personalBests = new Lazy<ReadOnlyCollection<Record>>(() =>
                {
                    var records = client.Users.GetPersonalBests(userId: user.ID);
                    var lazy = new Lazy<User>(() => user);

                    foreach (var record in records)
                    {
                        var player = record.Players.FirstOrDefault(x => x.UserID == user.ID);
                        if (player != null)
                        {
                            player.user = lazy;
                        }
                    }

                    return records;
                });

            return user;
        }
        */

        public override int GetHashCode()
        {
            return (ID ?? string.Empty).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as User;

            if (other == null)
                return false;

            return ID == other.ID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
