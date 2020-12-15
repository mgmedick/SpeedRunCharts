using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGamesService
    {
        GameViewModel GetGame(string gameID);
        IEnumerable<SearchResult> SearchGames(string searchText);
        SpeedRunGridViewModel GetSpeedRunGridModel(string gameID);
    }
}
