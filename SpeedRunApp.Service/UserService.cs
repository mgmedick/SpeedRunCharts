using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;
using SpeedrunComSharp.Client;
using SpeedrunComSharp.Model;
using SpeedRunApp.Interfaces.Services;

namespace SpeedRunApp.Service
{
    public class UserService : IUserService
    {
        public UserDTO GetUser(string userID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var runEmbeds = new RunEmbeds { EmbedCategory = true, EmbedLevel = true, EmbedGame = true, EmbedPlatform = true, EmbedPlayers = true };
            var user = clientContainer.Users.GetUser(userID, new List<Embeds> { runEmbeds });

            return new UserDTO(user);
        }

        public IEnumerable<SpeedRunDTO> GetUserSpeedRuns(string userID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var runEmbeds = new RunEmbeds { EmbedCategory = true, EmbedLevel = true, EmbedGame = true, EmbedPlatform = true, EmbedPlayers = true} ;
            var runs = clientContainer.Runs.GetRuns(userId: userID, embeds: runEmbeds);
            var dtoRecords = runs.Select(i => new SpeedRunDTO(i));
            //var recordVMs = dtoRecords.Select(i => new SpeedRunViewModel(i)).OrderBy(i => i.PrimaryRunTime);

            return dtoRecords;
        }
    }
}
