using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedRunApp.Model
{
    public class RegionDTO
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
    }
}
