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
        GameViewModel GetGame(string gameAbbr);
        GameViewModel GetGame(int gameID);
        EditSpeedRunViewModel GetEditSpeedRun(int gamID, int? speedRunID);
        IEnumerable<SearchResult> SearchGames(string searchText);
        LeaderboardTabViewModel GetLeaderboardTabs(int gameID, int? speedRunID = null);
        WorldRecordTabViewModel GetWorldRecordTabs(int gameID);
        GameChartTabViewModel GetGameChartTabs(int gameID);
        UserSpeedRunTabViewModel GetUserSpeedRunTabsAndData(int userID, int? speedRunID = null);   
        UserChartTabViewModel GetUserChartTabsAndData(int userID);     
        List<string> SetGameIsChanged(int gameID);
    }
}
