using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserAccountService
    {
        UserAccount GetUserAccountForLogin(string username);
        void LockUserAccount(int userID);
    }
}
