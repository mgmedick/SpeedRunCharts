using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGameService
    {
        GameViewModel GetGame(string gameID);
        IEnumerable<SearchResult> SearchGames(string searchText);
        Tuple<SpeedRunGridViewModel, IEnumerable<SpeedRunViewModel>> GetSpeedRunGrid(string gameID);
    }
}
