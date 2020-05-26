using SpeedRunApp.Model;
using SpeedrunComSharp.Client;
using SpeedrunComSharp.Model;
using SpeedRunApp.Interfaces.Services;

namespace SpeedRunApp.Service
{
    public class GamesService : IGamesService
    {
        public GameDTO GetGame(string gameID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var gameEmbeds = new GameEmbeds { EmbedCategories = true, EmbedLevels = true, EmbedModerators = true, EmbedPlatforms = true };
            var clientGame = clientContainer.Games.GetGame(gameID, gameEmbeds);

            return new GameDTO(clientGame);
        }
    }
}
