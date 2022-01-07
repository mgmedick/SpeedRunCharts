using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserAccountService
    {
        Task SendActivationEmail(string email);
        ActivateViewModel GetActivateUserAccount(string email, long expirationTime, string token);
        void CreateUserAccount(string username, string email, string pass);
        IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate);
        Task SendResetPasswordEmail(string username);
        ChangePasswordViewModel GetChangePassword(string username, string email, long expirationTime, string token);
        void ChangeUserAcctPassword(string username, string pass);
        bool EmailExists(string email);
        bool PasswordMatches(string password, string username);
        bool UsernameExists(string username, bool activeFilter);
        UserAccountViewModel GetUserAccount(int userID);
        void SaveUserAccount(UserAccountViewModel userAcctVM, int currUserAcctID);
        Task SendConfirmRegistrationEmail(string email, string username);
    }
}
