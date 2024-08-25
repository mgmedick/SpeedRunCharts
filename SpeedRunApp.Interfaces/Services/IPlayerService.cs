using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IPlayerService
    {
        PlayerDetailsViewModel GetPlayerDetails(string playerAbbr, string speedRunCode); 
        PlayerDetailsTabViewModel GetPlayerSpeedRunTabsAndData(int playerID);       
        IEnumerable<SearchResult> SearchPlayers(string searchText);
    }
}
