﻿using SpeedRunApp.Model.Data;
using System;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserDetailsViewModel
    {
        //public UserDetailsViewModel(User user)
        public UserDetailsViewModel()
        {
            //ID = user.ID;
            //Name = user.Name;
            //JapaneseName = user.JapaneseName;
            //Role = new IDNamePair() { ID = ((int)user.Role).ToString(), Name = user.Role.ToString() };
            //SignUpDate = user.SignUpDate;
            //Location = user.Location.ToString();
            //ProfileImage = user.ProfileImage;
            //TwitchProfile = user.TwitchProfile;
            //HitboxProfile = user.HitboxProfile;
            //YoutubeProfile = user.YoutubeProfile;
            //TwitterProfile = user.TwitterProfile;
            //SpeedRunsLiveProfile = user.SpeedRunsLiveProfile;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public Uri ProfileImage { get; set; }
        public IDNamePair Role { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public Uri TwitchProfile { get; set; }
        public Uri HitboxProfile { get; set; }
        public Uri YoutubeProfile { get; set; }
        public Uri TwitterProfile { get; set; }
        public Uri SpeedRunsLiveProfile { get; set; }
    }
}


