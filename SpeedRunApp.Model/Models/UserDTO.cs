using System;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class UserDTO
    {
        public UserDTO(User user)
        {
            ID = user.ID;
            Name = user.Name;
            JapaneseName = user.JapaneseName;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
    }
}
