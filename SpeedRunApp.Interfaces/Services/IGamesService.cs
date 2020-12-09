using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGamesService
    {
        GameViewModel GetGameDetails(string gameID);
        IEnumerable<SearchResult> SearchGames(string searchText);
        SpeedRunGridViewModel1 GetSpeedRunGridModel(string gameID);
    }
}
