using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGameService
    {
        GameDetailsViewModel GetGameDetails(string gameAbbr, string code);
        IEnumerable<SearchResult> SearchGames(string searchText);
        GameDetailsTabViewModel GetLeaderboardTabs(int gameID, string speedRunCode = null);
        GameDetailsTabViewModel GetWorldRecordTabs(int gameID);
        GameDetailsTabViewModel GetGameChartTabs(int gameID);
        IEnumerable<GameTabViewModel> GetGameTabs(IEnumerable<GameView> games, IEnumerable<SpeedRun> runs = null, bool hasDataOnly = false);
    }
}
