using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IPlayerService
    {
        PlayerDetailsViewModel GetPlayerDetails(string playerName, string speedRunCode); 
        PlayerDetailsTabViewModel GetPlayerSpeedRunTabsAndData(int playerID);       
        IEnumerable<SearchResult> SearchPlayers(string searchText);
    }
}
