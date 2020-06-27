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
        public PlayerDTO()
        {

        }

        //public PlayerDTO(Player player)
        //{
        //    ID = player.UserID;
        //    Name = player.Name;
        //}

        public bool IsUser { get { return string.IsNullOrEmpty(GuestName); } }
        public string UserID { get; set; }
        public string GuestName { get; set; }
    }
}
