﻿using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        UserDTO GetUser(string userID, bool cacheSpeedRuns);
        IEnumerable<SpeedRunDTO> GetUserSpeedRuns(string userID);
        IEnumerable<SpeedRunDTO> GetUserSpeedRuns(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID);
    }
}
