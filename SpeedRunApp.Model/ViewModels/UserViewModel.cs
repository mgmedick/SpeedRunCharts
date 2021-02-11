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
            SignUpDate = user.SignUpDate;
            Location = user.Location;
            SpeedRunComLink = user.SpeedRunComUrl;
            ProfileImage = user.ProfileImageUrl;
            TwitchProfile = user.TwitchProfileUrl;
            HitboxProfile = user.HitboxProfileUrl;
            YoutubeProfile = user.YoutubeProfileUrl;
            TwitterProfile = user.TwitterProfileUrl;
            SpeedRunsLiveProfile = user.SpeedRunsLiveProfileUrl;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public string SpeedRunComLink { get; set; }
        public string ProfileImage { get; set; }
        public string TwitchProfile { get; set; }
        public string HitboxProfile { get; set; }
        public string YoutubeProfile { get; set; }
        public string TwitterProfile { get; set; }
        public string SpeedRunsLiveProfile { get; set; }
    }
}


