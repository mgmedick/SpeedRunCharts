using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel(UserViewModel userVM, int? speedRunID)
        {
            UserVM = userVM;
            SpeedRunID = speedRunID;
        }
                
        public UserViewModel UserVM { get; set; }
        public int? SpeedRunID { get; set; }
    }
}


