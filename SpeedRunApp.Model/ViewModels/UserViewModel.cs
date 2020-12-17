using SpeedRunApp.Model.Data;
using System;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(UserView user)
        {
            ID = user.ID;
            Name = user.Name;
            JapaneseName = user.JapaneseName;
            ProfileImage = user.ProfileImageUrl;
            Role = new IDNamePair { ID = user.Role.Split("|", 2)[0], Name = user.Role.Split("|", 2)[1] };
            SignUpDate = user.SignUpDate;
            Location = user.Location;
            TwitchProfile = user.TwitchProfileUrl;
            HitboxProfile = user.HitboxProfileUrl;
            YoutubeProfile = user.YoutubeProfileUrl;
            TwitterProfile = user.TwitterProfileUrl;
            SpeedRunsLiveProfile = user.SpeedRunsLiveProfileUrl;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string ProfileImage { get; set; }
        public IDNamePair Role { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public string TwitchProfile { get; set; }
        public string HitboxProfile { get; set; }
        public string YoutubeProfile { get; set; }
        public string TwitterProfile { get; set; }
        public string SpeedRunsLiveProfile { get; set; }
    }
}


