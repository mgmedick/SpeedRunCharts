using System;
using System.Linq;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserSettingsViewModel
    {
        public UserSettingsViewModel(UserView userVW, List<SummaryList> summaryLists)
        {
            UserID = userVW.UserID;
            Username = userVW.Username;
            Email = userVW.Email;
            IsDarkTheme = userVW.IsDarkTheme;
            SummaryListIDs = userVW.SummaryListIDs?.Split(',').Select(i=>Convert.ToInt32(i)).ToList() ?? new List<int>();
            SummaryLists = summaryLists;
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsDarkTheme { get; set; }
        public List<int> SummaryListIDs { get; set; }
        public List<SummaryList> SummaryLists { get; set; }
    }
}


