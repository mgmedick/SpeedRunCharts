using System;
using System.Collections.Generic;
using SpeedRunApp.Model;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        UserDTO GetUser(string userID);
        IEnumerable<SpeedRunDTO> GetUserSpeedRuns(string userID);
    }
}
