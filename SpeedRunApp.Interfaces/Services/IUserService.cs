using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        UserDetailsViewModel GetUserDetails(string userAbbr, string speedRunComID);        
        UserViewModel GetUser(string userAbbr);
        UserViewModel GetUser(int userID);
        IEnumerable<SearchResult> SearchUsers(string searchText);
        List<string> SetUserIsChanged(int userID);
    }
}
