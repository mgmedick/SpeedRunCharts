﻿using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        UserDetailsViewModel GetUserDetails(string userAbbr, int? speedRunID);        
        UserViewModel GetUser(string userAbbr);
        UserViewModel GetUser(int userID);
        IEnumerable<SearchResult> SearchUsers(string searchText);
    }
}
