using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class PlayerDTO
    {
        public PlayerDTO(Player player)
        {
            ID = player.UserID;
            Name = player.Name;
        }

        public string ID { get; set; }
        public string Name { get; set; }
    }
}
