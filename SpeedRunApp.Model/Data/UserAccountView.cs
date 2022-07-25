using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class UserAccountView
    {
        public int UserAccountID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsDarkTheme { get; set; }
        public string SpeedRunListCategoryIDs { get; set; }
    }
} 
