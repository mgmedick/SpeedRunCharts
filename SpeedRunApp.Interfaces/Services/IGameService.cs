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
        List<IDNamePair> GetWorldRecordGridTabs(int gameID);
        SpeedRunGridTabViewModel GetSpeedRunGridTabs(int gameID);
        SpeedRunGridTabViewModel GetSpeedRunGridTabsForUser(int userID);
    }
}
