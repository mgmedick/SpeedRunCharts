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
        IEnumerable<SearchResult> SearchGames(string searchText);
        LeaderboardTabViewModel GetLeaderboardTabs(int gameID, int? speedRunID = null);
        WorldRecordTabViewModel GetWorldRecordTabs(int gameID);
        UserSpeedRunTabViewModel GetUserSpeedRunTabs(int userID, int? speedRunID = null);        
        List<string> SetGameIsChanged(int gameID);
    }
}
