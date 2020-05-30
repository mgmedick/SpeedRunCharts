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
            var gameEmbeds = new GameEmbeds { EmbedCategories = true, EmbedLevels = true, EmbedModerators = true, EmbedPlatforms = true };
            var user = clientContainer.Users.GetUser(userID);

            return new UserDTO(user);
        }

        public IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID)
        {
            ClientContainer clientContainer = new ClientContainer();
            var gameEmbeds = new GameEmbeds { EmbedCategories = true, EmbedLevels = true, EmbedModerators = true, EmbedPlatforms = true };
            var user = clientContainer.Users.GetUser(userID);

            var dtoRecords = user.Runs.Select(i => new SpeedRunDTO(i));
            var runVMs = dtoRecords.Select(i => new SpeedRunViewModel(i)).OrderBy(i => i.PrimaryRunTime);

            return runVMs;
        }
    }
}
