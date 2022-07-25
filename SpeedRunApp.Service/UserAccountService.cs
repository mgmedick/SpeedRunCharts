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
using System.Linq.Expressions;
using SpeedRunCommon.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace SpeedRunApp.Service
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAcctRepo = null;
        private readonly IEmailService _emailService = null;
        private readonly IHttpContextAccessor _context = null;
        private readonly IConfiguration _config = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public UserAccountService(IUserAccountRepository userAcctRepo, IEmailService emailService, IHttpContextAccessor context, IConfiguration config, ISpeedRunRepository speedRunRepo)
        {
            _userAcctRepo = userAcctRepo;
            _emailService = emailService;
            _context = context;
            _config = config;
            _speedRunRepo = speedRunRepo;
        }

        public async Task SendActivationEmail(string email)
        {
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var baseUrl = string.Format("{0}://{1}{2}", _context.HttpContext.Request.Scheme, _context.HttpContext.Request.Host, _context.HttpContext.Request.PathBase);
            var queryParams = string.Format("email={0}&expirationTime={1}", email, DateTime.UtcNow.AddHours(48).Ticks);
            var token = queryParams.GetHMACSHA256Hash(hashKey);

            var activateUserAcct = new
            {
                Email = email,
                ActivateLink = string.Format("{0}/SpeedRun/Activate?{1}&token={2}", baseUrl, queryParams, token)
            };

            await _emailService.SendEmailTemplate(email, "Create your speedruncharts.com account", Template.ActivateEmail.ToString(), activateUserAcct);
        }

        public ActivateViewModel GetActivateUserAccount(string email, long expirationTime, string token)
        {
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var strToHash = string.Format("email={0}&expirationTime={1}", email, expirationTime);
            var hash = strToHash.GetHMACSHA256Hash(hashKey);
            var expirationDate = new DateTime(expirationTime);
            var emailExists = _userAcctRepo.GetUserAccounts(i => i.Email == email).Any();
            var isValid = (hash == token) && expirationDate > DateTime.UtcNow && !emailExists;
            var activateUserAcctVM = new ActivateViewModel() { IsValid = isValid };

            return activateUserAcctVM;
        }

        public async Task SendConfirmRegistrationEmail(string email, string username)
        {
            var confirmRegistration = new
            {
                Username = username,
                SupportEmail = _config.GetSection("SiteSettings").GetSection("FromEmail").Value
            };

            await _emailService.SendEmailTemplate(email, "Thanks for registering at speedruncharts.com", Template.ConfirmRegistration.ToString(), confirmRegistration);
        }

        public async Task SendResetPasswordEmail(string username)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == username).FirstOrDefault();
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var baseUrl = string.Format("{0}://{1}{2}", _context.HttpContext.Request.Scheme, _context.HttpContext.Request.Host, _context.HttpContext.Request.PathBase);
            var queryParams = string.Format("username={0}&email={1}&expirationTime={2}", userAcct.Username, userAcct.Email, DateTime.UtcNow.AddHours(48).Ticks);
            var token = string.Format("{0}&password={1}", queryParams, userAcct.Password).GetHMACSHA256Hash(hashKey);

            var passwordReset = new
            {
                Username = userAcct.Username,
                ResetPassLink = string.Format("{0}/SpeedRun/ChangePassword?{1}&token={2}", baseUrl, queryParams, token)
            };

            await _emailService.SendEmailTemplate(userAcct.Email, "Reset your speedruncharts.com password", Template.ResetPasswordEmail.ToString(), passwordReset);
        }
        
        public ChangePasswordViewModel GetChangePassword(string username, string email, long expirationTime, string token)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == username).FirstOrDefault();
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var strToHash = string.Format("username={0}&email={1}&expirationTime={2}&password={3}", username, email, expirationTime, userAcct.Password);
            var hash = strToHash.GetHMACSHA256Hash(hashKey);
            var expirationDate = new DateTime(expirationTime);
            var isValid = (hash == token) && expirationDate > DateTime.UtcNow;
            var changePassVM = new ChangePasswordViewModel() { IsValid = isValid };

            return changePassVM;
        }

        public IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate)
        {
            return _userAcctRepo.GetUserAccounts(predicate);
        }

        public IEnumerable<UserAccountView> GetUserAccountViews(Expression<Func<UserAccountView, bool>> predicate)
        {
            return _userAcctRepo.GetUserAccountViews(predicate);
        }

        public void CreateUserAccount(string username, string pass)
        {
            var email = _context.HttpContext.Session.Get<string>("Email");
            var isdarktheme = (_context.HttpContext.Request.Cookies["theme"] ?? _config.GetSection("SiteSettings").GetSection("DefaultTheme").Value) == "theme-dark";

            var userAcct = new UserAccount()
            {
                Username = username,
                Password = pass.HashString(),
                Email = email,
                Active = true,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            };

            _userAcctRepo.SaveUserAccount(userAcct);

            var userAcctSetting = new UserAccountSetting() {
                UserAccountID = userAcct.ID,
                IsDarkTheme = isdarktheme
            };

            _userAcctRepo.SaveUserAccountSetting(userAcctSetting);
        }

        public void ChangeUserAcctPassword(string username, string pass)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == username).FirstOrDefault();
            userAcct.Password = pass.HashString();
            userAcct.ModifiedBy = userAcct.ID;
            userAcct.ModifiedDate = DateTime.UtcNow;

            _userAcctRepo.SaveUserAccount(userAcct);
        }

        public UserAccountViewModel GetUserAccount(int userAccountID)
        {
            var userAcctView = _userAcctRepo.GetUserAccountViews(i => i.UserAccountID == userAccountID).FirstOrDefault();
            var userAcctVM = new UserAccountViewModel(userAcctView);
            userAcctVM.SpeedRunListCategories = _speedRunRepo.SpeedRunListCategories().ToList();

            return userAcctVM;
        }

        public void SaveUserAccount(UserAccountViewModel userAcctVM, int currUserAccountID)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.ID == userAcctVM.UserAccountID).FirstOrDefault();

            if (userAcct != null)
            {
                var userAcctSetting = new UserAccountSetting()
                {
                    UserAccountID = userAcctVM.UserAccountID,
                    IsDarkTheme = userAcctVM.IsDarkTheme
                };

                var userAcctSpeedRunListCategories = userAcctVM.SpeedRunListCategoryIDs?.Select(i => new UserAccountSpeedRunListCategory() { UserAccountID = userAcct.ID, SpeedRunListCategoryID = i });

                _userAcctRepo.SaveUserAccountSetting(userAcctSetting);
                SaveUserAccountSpeedRunListCategories(userAcct.ID, userAcctSpeedRunListCategories);

                userAcct.ModifiedDate = DateTime.UtcNow;
                userAcct.ModifiedBy = currUserAccountID;
                _userAcctRepo.SaveUserAccount(userAcct);
            }
        }

        public void SaveUserAccountSpeedRunListCategories(int userAccountID, IEnumerable<UserAccountSpeedRunListCategory> userAcctSpeedRunListCategories)
        {
            _userAcctRepo.DeleteUserAccountSpeedRunListCategories(i => i.UserAccountID == userAccountID);

            if (userAcctSpeedRunListCategories != null && userAcctSpeedRunListCategories.Any())
            {
                _userAcctRepo.SaveUserAccountSpeedRunListCategories(userAcctSpeedRunListCategories);
            }
        }

        public void UpdateIsDarkTheme(int currUserAccountID, bool isDarkTheme)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.ID == currUserAccountID).FirstOrDefault();

            if (userAcct != null)
            {
                var userAcctSetting = new UserAccountSetting()
                {
                    UserAccountID = userAcct.ID,
                    IsDarkTheme = isDarkTheme
                };

                _userAcctRepo.SaveUserAccountSetting(userAcctSetting);
            }
        }

        //jqvalidate
        public bool EmailExists(string email)
        {
            var result = _userAcctRepo.GetUserAccounts(i => i.Email == email).Any();

            return result;
        }

        public bool PasswordMatches(string password, string username)
        {
            var result = false;
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == username && i.Active).FirstOrDefault();

            if (userAcct != null)
            {
                result = password.VerifyHash(userAcct.Password);
            }

            return result;
        }

        public bool UsernameExists(string username, bool activeFilter)
        {
            var result = _userAcctRepo.GetUserAccounts(i => i.Username == username && (i.Active || i.Active == activeFilter)).Any();

            return result;
        }
    }
}
