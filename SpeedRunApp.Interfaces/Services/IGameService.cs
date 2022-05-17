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
        SpeedRunGridTabViewModel GetWorldRecordGridTabs(int gameID);
        SpeedRunGridTabViewModel GetSpeedRunGridTabs(int gameID, int? speedRunID = null);
        SpeedRunGridTabViewModel GetSpeedRunGridTabsForUser(int userID, int? speedRunID = null);
        //IEnumerable<IDNamePair> GetPersonalBestGridTabs(int userID);
    }
}
