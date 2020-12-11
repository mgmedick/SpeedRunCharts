using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface IGameRepository
    {
        GameView GetGameView(string gameID);
        IEnumerable<SearchResult> SearchGames(string searchText);
        SpeedRunGridItem GetSpeedRunGridItemByGameID(string gameID);
    }
}






