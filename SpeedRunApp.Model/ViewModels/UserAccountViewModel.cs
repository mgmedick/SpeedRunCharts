using System;
using System.Linq;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserAccountViewModel
    {
        public UserAccountViewModel()
        {
        }

        public UserAccountViewModel(UserAccountView userAcctView)
        {
            UserAccountID = userAcctView.UserAccountID;
            Username = userAcctView.Username;
            IsDarkTheme = userAcctView.IsDarkTheme;
            SpeedRunListCategoryIDs = string.IsNullOrWhiteSpace(userAcctView.SpeedRunListCategoryIDs) ? new List<int>() : userAcctView.SpeedRunListCategoryIDs.Split(",").Select(i => Convert.ToInt32(i)).ToList();
        }

        public int UserAccountID { get; set; }
        public string Username { get; set; }
        public bool IsDarkTheme { get; set; }
        public List<int> SpeedRunListCategoryIDs { get; set; }
        public List<SpeedRunListCategory> SpeedRunListCategories { get; set; }
    }
}


