namespace SpeedRunApp.Model.Data
{
    public class Moderator
    {
        public string UserID { get; set; }
        public ModeratorType Type { get; set; }


        //public Moderator() { }

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
        //public override string ToString()
        //{
        //    return Name;
        //}
    }
}
