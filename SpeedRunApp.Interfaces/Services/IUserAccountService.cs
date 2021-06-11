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
        IEnumerable<string> SignUp(SignUpViewModel signUpVM);
        void LockUserAccount(int userID);
    }
}
