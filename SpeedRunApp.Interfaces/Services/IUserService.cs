using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        UserViewModel GetUser(string userID);
        IEnumerable<SearchResult> SearchUsers(string searchText);
        SpeedRunGridContainerViewModel GetSpeedRunGrid(int userID);
    }
}
