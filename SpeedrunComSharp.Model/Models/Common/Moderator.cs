using System;
using System.Collections.Generic;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedrunComSharp.Model
{
    public class Moderator
    {
        public string UserID { get; set; }
        public ModeratorType Type { get; set; }

        #region Links

        public Lazy<User> user { get; set; }
        public User User { get { return user.Value; } }
        public string Name { get { return User.Name; } }

        #endregion

        public Moderator() { }

        /*
        public static Moderator Parse(SpeedrunComClient client, KeyValuePair<string, dynamic> moderatorElement)
        {
            var moderator = new Moderator();

            moderator.UserID = moderatorElement.Key;
            moderator.Type = moderatorElement.Value as string == "moderator" 
                ? ModeratorType.Moderator 
                : ModeratorType.SuperModerator;

            moderator.user = new Lazy<User>(() => client.Users.GetUser(moderator.UserID));

            return moderator;
        }
        */
        public override string ToString()
        {
            return Name;
        }
    }
}
