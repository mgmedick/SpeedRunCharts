namespace SpeedRunApp.Model.Data
{
    public class Platform
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }

        //#region Links
        //public IEnumerable<Game> Games { get; set; }
        //public Lazy<IEnumerable<SpeedRun>> runs { get; set; }
        //public IEnumerable<SpeedRun> Runs { get; set; }
        //#endregion

        //public Platform() { }

        /*
        public static Platform Parse(SpeedrunComClient client, dynamic platformElement)
        {
            if (platformElement is ArrayList)
                return null;

            var platform = new Platform();

            //Parse Attributes

            platform.ID = platformElement.id as string;
            platform.Name = platformElement.name as string;
            platform.YearOfRelease = (int)platformElement.released;

            //Parse Links

            platform.Games = client.Games.GetGames(platformId: platform.ID);
            //platform.Runs = client.Runs.GetRuns(platformId: platform.ID);

            return platform;
        }
        */
        //public override int GetHashCode()
        //{
        //    return (ID ?? string.Empty).GetHashCode();
        //}

        //public override bool Equals(object obj)
        //{
        //    var other = obj as Platform;

        //    if (other == null)
        //        return false;

        //    return ID == other.ID;
        //}

        public override string ToString()
        {
            return Name;
        }
    }
}
