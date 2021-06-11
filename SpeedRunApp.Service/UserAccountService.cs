using System;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SpeedRunApp.Service
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAcctRepo = null;

        public UserAccountService(IUserAccountRepository userAcctRepo)
        {
            _userAcctRepo = userAcctRepo;
        }

        public UserAccount GetUserAccountForLogin(string username)
        {
            return _userAcctRepo.GetUserAccounts(i => i.Username == username && i.Active).FirstOrDefault();
        }

        public void LockUserAccount(int userID)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.ID == userID).FirstOrDefault();
            if (userAcct != null)
            {
                userAcct.Locked = true;
                _userAcctRepo.SaveUserAccount(userAcct);
            }
        }
    }
}
