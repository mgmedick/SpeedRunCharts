using System;
using System.Linq;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserSettingsViewModel
    {
        public UserSettingsViewModel(UserView userVW, List<SpeedRunSummaryList> speedRunSummaryLists)
        {
            UserID = userVW.UserID;
            Username = userVW.Username;
            Email = userVW.Email;
            IsDarkTheme = userVW.IsDarkTheme;
            SpeedRunSummaryListIDs = userVW.SpeedRunSummaryListIDs?.Split(',').Select(i=>Convert.ToInt32(i)).ToList() ?? new List<int>();
            SpeedRunSummaryLists = speedRunSummaryLists;
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsDarkTheme { get; set; }
        public List<int> SpeedRunSummaryListIDs { get; set; }
        public List<SpeedRunSummaryList> SpeedRunSummaryLists { get; set; }
    }
}


