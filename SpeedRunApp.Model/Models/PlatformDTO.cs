using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class PlatformDTO
    {
        public PlatformDTO(Platform platform)
        {
            ID = platform.ID;
            Name = platform.Name;
            YearOfRelease = platform.YearOfRelease;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
    }
}
