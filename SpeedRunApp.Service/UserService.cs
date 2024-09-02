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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo = null;
        private readonly IEmailService _emailService = null;
        private readonly IHttpContextAccessor _context = null;
        private readonly IConfiguration _config = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public UserService(IUserRepository userRepo, IEmailService emailService, IHttpContextAccessor context, IConfiguration config, ISpeedRunRepository speedRunRepo)
        {
            _userRepo = userRepo;
            _emailService = emailService;
            _context = context;
            _config = config;
            _speedRunRepo = speedRunRepo;
        }

        public UserSettingsViewModel GetUserSettings(int userID)
        {
            var userView = _userRepo.GetUserViews(i => i.UserID == userID).FirstOrDefault();
            var userSettingsVM = new UserSettingsViewModel(userView, _speedRunRepo.GetSummaryLists().ToList());

            return userSettingsVM;
        }

        public void SaveUserSettings(UserSettingsViewModel userSettingsVM, int currUserID)
        {
            var user = _userRepo.GetUsers(i => i.ID == userSettingsVM.UserID).FirstOrDefault();
            var userSetting = _userRepo.GetUserSettings(i=> i.UserID == user.ID).FirstOrDefault();

            if (userSetting.ID == 0) 
            {            
                userSetting = new UserSetting()
                {
                    UserID = userSettingsVM.UserID,
                    IsDarkTheme = userSettingsVM.IsDarkTheme
                };
            }
            else
            {
                userSetting.UserID = userSettingsVM.UserID;
                userSetting.IsDarkTheme = userSettingsVM.IsDarkTheme;              
            }

            _userRepo.SaveUserSetting(userSetting);

            var userSummaryLists = userSettingsVM.SummaryListIDs?.Select(i => new UserSummaryList() { UserID = user.ID, SummaryListID = i });
            SaveUserSummaryLists(user.ID, userSummaryLists);

            user.ModifiedDate = DateTime.UtcNow;
            user.ModifiedBy = currUserID;
            _userRepo.SaveUser(user);
        }

         public void SaveUserSummaryLists(int userID, List<int> summaryListIDs)
        {
            var user = _userRepo.GetUsers(i => i.ID == userID).FirstOrDefault();
            var userSummaryLists = summaryListIDs?.Select(i => new UserSummaryList() { UserID = user.ID, SummaryListID = i });
            SaveUserSummaryLists(user.ID, userSummaryLists);

            user.ModifiedDate = DateTime.UtcNow;
            user.ModifiedBy = userID;
            _userRepo.SaveUser(user);
        }       

        public async Task SendActivationEmail(string email)
        {
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var baseUrl = string.Format("{0}://{1}{2}", _context.HttpContext.Request.Scheme, _context.HttpContext.Request.Host, _context.HttpContext.Request.PathBase);
            var queryParams = string.Format("email={0}&expirationTime={1}", email, DateTime.UtcNow.AddHours(48).Ticks);
            var token = queryParams.GetHMACSHA256Hash(hashKey);

            var activateUser = new
            {
                Email = email,
                ActivateLink = string.Format("{0}/Home/Activate?{1}&token={2}", baseUrl, queryParams, token)
            };

            await _emailService.SendEmailTemplate(email, "Create your speedruncharts.com account", Template.ActivateEmail.ToString(), activateUser);
        }

        public ActivateViewModel GetActivateUser(string email, long expirationTime, string token)
        {
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var strToHash = string.Format("email={0}&expirationTime={1}", email, expirationTime);
            var hash = strToHash.GetHMACSHA256Hash(hashKey);
            var expireDate = new DateTime(expirationTime);
            var emailExists = _userRepo.GetUsers(i => i.Email == email).Any();
            var isValid = (hash == token) && expireDate > DateTime.UtcNow && !emailExists;
            var emailToken = email.GetHMACSHA256Hash(hashKey);
            var activateUserVM = new ActivateViewModel() { Email = email, EmailToken = emailToken, IsValid = isValid };

            return activateUserVM;
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
            var user = _userRepo.GetUsers(i => i.Username == username).FirstOrDefault();
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var baseUrl = string.Format("{0}://{1}{2}", _context.HttpContext.Request.Scheme, _context.HttpContext.Request.Host, _context.HttpContext.Request.PathBase);
            var queryParams = string.Format("username={0}&email={1}&expirationTime={2}", user.Username, user.Email, DateTime.UtcNow.AddHours(48).Ticks);
            var token = string.Format("{0}&password={1}", queryParams, user.Password).GetHMACSHA256Hash(hashKey);

            var passwordReset = new
            {
                Username = user.Username,
                ResetPassLink = string.Format("{0}/Home/ChangePassword?{1}&token={2}", baseUrl, queryParams, token)
            };

            await _emailService.SendEmailTemplate(user.Email, "Reset your speedruncharts.com password", Template.ResetPasswordEmail.ToString(), passwordReset);
        }
        
        public ChangePasswordViewModel GetChangePassword(string username, string email, long expirationTime, string token)
        {
            var user = _userRepo.GetUsers(i => i.Username == username).FirstOrDefault();
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var strToHash = string.Format("username={0}&email={1}&expirationTime={2}&password={3}", username, email, expirationTime, user.Password);
            var hash = strToHash.GetHMACSHA256Hash(hashKey);
            var expirationDate = new DateTime(expirationTime);
            var isValid = (hash == token) && expirationDate > DateTime.UtcNow;
            var changePassVM = new ChangePasswordViewModel() { IsValid = isValid };

            return changePassVM;
        }

        public IEnumerable<UserView> GetUserViews(Expression<Func<UserView, bool>> predicate)
        {
            return _userRepo.GetUserViews(predicate);
        }

        public int CreateUser(string email, string username, string pass)
        {
            var user = new User()
            {
                Email = email,
                Username = username,
                Password = pass.HashString(),
                Active = true,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            };

            _userRepo.SaveUser(user);

            var isdarktheme = (_context.HttpContext.Request.Cookies["theme"] ?? _config.GetSection("SiteSettings").GetSection("DefaultTheme").Value) == "theme-dark";
            var userSetting = new UserSetting() {
                UserID = user.ID,
                IsDarkTheme = isdarktheme
            };

            _userRepo.SaveUserSetting(userSetting);      
   
            return user.ID;  
        }

        public void ChangeUserPassword(string username, string pass)
        {
            var user = _userRepo.GetUsers(i => i.Username == username).FirstOrDefault();
            user.Password = pass.HashString();
            user.ModifiedBy = user.ID;
            user.ModifiedDate = DateTime.UtcNow;

            _userRepo.SaveUser(user);
        }

        public void SaveUserSummaryLists(int userID, IEnumerable<UserSummaryList> userSummaryLists)
        {
            _userRepo.DeleteUserSummaryLists(i => i.UserID == userID);

            if (userSummaryLists != null && userSummaryLists.Any())
            {
                _userRepo.SaveUserSummaryLists(userSummaryLists);
            }
        }

        public void UpdateIsDarkTheme(int currUserID, bool isDarkTheme)
        {
            var user = _userRepo.GetUsers(i => i.ID == currUserID).FirstOrDefault();

            if (user != null)
            {
                var userSetting = new UserSetting()
                {
                    UserID = user.ID,
                    IsDarkTheme = isDarkTheme
                };

                _userRepo.SaveUserSetting(userSetting);
            }
        }

        //jqvalidate
        public bool EmailExists(string email, bool activeFilter)
        {
            var result = _userRepo.GetUsers(i => i.Email == email && (i.Active || i.Active == activeFilter)).Any();

            return result;
        }   

        public bool PasswordMatches(string password, string username)
        {
            var result = false;
            var user = _userRepo.GetUsers(i => i.Username == username && i.Active).FirstOrDefault();

            if (user != null)
            {
                result = password.VerifyHash(user.Password);
            }

            return result;
        }

        public bool UsernameExists(string username, bool activeFilter)
        {
            var result = _userRepo.GetUsers(i => i.Username == username && (i.Active || i.Active == activeFilter)).Any();

            return result;
        }
    }
}
