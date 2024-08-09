using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGameService
    {
        GameDetailsViewModel GetGameDetails(string gameAbbr, string speedRunComID);
        // EditSpeedRunViewModel GetEditSpeedRun(int gamID, int? speedRunID);
        IEnumerable<SearchResult> SearchGames(string searchText);
        LeaderboardTabViewModel GetLeaderboardTabs(int gameID, string speedRunCode = null);
        LeaderboardTabViewModel GetWorldRecordTabs(int gameID);
        LeaderboardTabViewModel GetGameChartTabs(int gameID);
        PlayerSpeedRunTabViewModel GetPlayerSpeedRunTabsAndData(int playerID);
    }
}
