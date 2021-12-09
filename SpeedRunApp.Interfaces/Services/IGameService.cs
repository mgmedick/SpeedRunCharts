using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGameService
    {
        GameViewModel GetGame(int gameID);
        IEnumerable<SearchResult> SearchGames(string searchText);
        SpeedRunGridTabViewModel GetWorldRecordGridTabs(int gameID);
        SpeedRunGridTabViewModel GetSpeedRunGridTabs(int gameID);
        SpeedRunGridTabViewModel GetSpeedRunGridTabsForUser(int userID);
        //IEnumerable<IDNamePair> GetPersonalBestGridTabs(int userID);
    }
}
