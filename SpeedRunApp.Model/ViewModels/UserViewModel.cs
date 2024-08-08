using System;
using System.Linq;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(UserView userView)
        {
            UserID = userView.UserID;
            Username = userView.Username;
            IsDarkTheme = userView.IsDarkTheme;
            SpeedRunListCategoryIDs = string.IsNullOrWhiteSpace(userView.SpeedRunListCategoryIDs) ? new List<int>() : userView.SpeedRunListCategoryIDs.Split(",").Select(i => Convert.ToInt32(i)).ToList();
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public bool IsDarkTheme { get; set; }
        public List<int> SpeedRunListCategoryIDs { get; set; }
        public List<SpeedRunListCategory> SpeedRunListCategories { get; set; }
    }
}


