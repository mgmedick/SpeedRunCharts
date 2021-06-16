using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserAccountService
    {
        Tuple<UserAccount, IEnumerable<string>> ValidateLogin(LoginViewModel loginVM);
        IEnumerable<string> ValidateSignUp(SignUpViewModel signUpVM);
        void LockUserAccount(int userID);
        void SendActivationEmail(string email);
        ActivateViewModel GetActivateUserAccount(string email, long expirationTime, string token);
        IEnumerable<string> ValidateActivateUserAccount(ActivateViewModel activateUserAcctVM);
        void CreateUserAccount(string username, string email, string pass);
        IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate);
    }
}
