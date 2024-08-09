using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SpeedRunApp.Model.Data
{
    public class UserView
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsDarkTheme { get; set; }
        public string SpeedRunListCategoryIDs { get; set; }

        private List<int> _speedRunListCategoryIDList = new List<int>();
        public List<int> SpeedRunListCategoryIDList
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(SpeedRunListCategoryIDs))
                {
                    _speedRunListCategoryIDList = SpeedRunListCategoryIDs.Split(',').Select(i=>Convert.ToInt32(i)).ToList();
                }

                return _speedRunListCategoryIDList;
            }
        }       
    }
} 
