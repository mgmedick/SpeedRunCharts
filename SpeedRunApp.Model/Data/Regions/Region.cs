using System;
using System.Collections.Generic;

namespace SpeedRunApp.Model.Data
{
    public class Region
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string Abbreviation
        {
            get
            {
                switch (Name)
                {
                    case "USA / NTSC": return "NTSC-U";
                    case "EUR / PAL": return "PAL";
                    case "JPN / NTSC": return "NTSC-J";
                    case "CHN / iQue": return "CHN";
                    case "KOR / NTSC": return "KOR";
                }

                return Name;
            }
        }

        //#region Links
        //public IEnumerable<Game> Games { get; set; }
        //public Lazy<IEnumerable<Run>> runs { get; set; }
        //public IEnumerable<Run> Runs { get { return runs.Value; } }
        //#endregion

        //public Region() { }

        /*
        public static Region Parse(SpeedrunComClient client, dynamic regionElement)
        {
            if (regionElement is ArrayList)
                return null;

            var region = new Region();

            //Parse Attributes

            region.ID = regionElement.id as string;
            region.Name = regionElement.name as string;

            //Parse Links

            region.Games = client.Games.GetGames(regionId: region.ID);
            //region.Runs = client.Runs.GetRuns(regionId: region.ID);

            return region;
        }
        */
        //public override int GetHashCode()
        //{
        //    return (ID ?? string.Empty).GetHashCode();
        //}

        //public override bool Equals(object obj)
        //{
        //    var region = obj as Region;

        //    if (region == null)
        //        return false;

        //    return ID == region.ID;
        //}

        public override string ToString()
        {
            return Name;
        }
    }
}
