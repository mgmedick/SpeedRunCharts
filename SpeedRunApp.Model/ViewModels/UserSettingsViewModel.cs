using System;
using System.Linq;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserSettingsViewModel
    {
        public UserSettingsViewModel(UserView userVW, List<SpeedRunListCategory> speedRunListCategories)
        {
            UserID = userVW.UserID;
            Username = userVW.Username;
            Email = userVW.Email;
            IsDarkTheme = userVW.IsDarkTheme;
            SpeedRunListCategoryIDs = userVW.SpeedRunListCategoryIDs?.Split(',').Select(i=>Convert.ToInt32(i)).ToList() ?? new List<int>();
            SpeedRunListCategories = speedRunListCategories;
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsDarkTheme { get; set; }
        public List<int> SpeedRunListCategoryIDs { get; set; }
        public List<SpeedRunListCategory> SpeedRunListCategories { get; set; }
    }
}


