using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class LevelDTO
    {
        public LevelDTO(Level level)
        {
            ID = level.ID;
            Name = level.Name;
        }

        public string ID { get; set; }
        public string Name { get; set; }
    }
}
