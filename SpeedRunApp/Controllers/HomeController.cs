using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon.Extensions;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Linq;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using System.Data.SqlTypes;

namespace SpeedRunApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpeedRunService _speedRunService = null;
        private readonly IUserService _userService = null;
        private readonly IAuthService _authService = null;
        private readonly ISettingService _settingService = null;
        private readonly ICacheService _cacheService = null;
        private readonly IConfiguration _config = null;
        private readonly ILogger _logger = null;

        public HomeController(ISpeedRunService speedRunService, IUserService userService, IAuthService authService, ISettingService settingService, ICacheService cacheService, IConfiguration config, ILogger logger)
        {
            _speedRunService = speedRunService;
            _userService = userService;
            _authService = authService;
            _settingService = settingService;
            _cacheService = cacheService;
            _config = config;
            _logger = logger;
        }

        public ViewResult Index()
        {
            var defaultTopAmount = Convert.ToInt32(_config.GetSection("SiteSettings").GetSection("DefaultTopAmount").Value);
            var currUserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var summaryLists = _speedRunService.GetSummaryLists(currUserID).ToList();
            var indexVM = new IndexViewModel(defaultTopAmount, summaryLists);

            return View(indexVM);
        }

        [HttpGet]
        public JsonResult GetSummaryListResults(int summaryListID, int topAmount, int? orderValueOffset, int? categoryTypeID)
        {
            var results = _speedRunService.GetSummaryListResults(summaryListID, topAmount, orderValueOffset, categoryTypeID);

            return Json(results);
        }
                
        public ViewResult Error()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult RefreshCache()
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {       
                var token = (string)HttpContext.Request.Form["token"];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    token = HttpUtility.UrlDecode(token);
                    var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value; 
                    var lastImportDateUtcString = (_settingService.GetSetting("LastImportDate")?.Dte ?? (DateTime)SqlDateTime.MinValue).ToString();
                    
                    if (lastImportDateUtcString.GetHMACSHA256Hash(hashKey) == token)
                    {
                        _cacheService.RefreshCache();
                        success = true;
                    }              
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "RefreshCache");
                success = false;
                errorMessages = new List<string>() { "Error refreshing cache" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }        

        [HttpGet]
        public ActionResult Login()
        {
            var loginVM = new LoginViewModel() {
                GClientID = _config.GetSection("Auth").GetSection("Google").GetSection("ClientID").Value,
                FBClientID = _config.GetSection("Auth").GetSection("Facebook").GetSection("ClientID").Value,
                FBApiVer = _config.GetSection("Auth").GetSection("Facebook").GetSection("ApiVersion").Value,
                RecaptchaKey = _config.GetSection("Auth").GetSection("Google").GetSection("RecaptchaKey").Value
            };

            return View(loginVM);
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel loginVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                var result = await _authService.ValidateGoogleRecaptcha(loginVM.Token);
                if (!result)
                {
                     ModelState.AddModelError("Login", "Invalid recaptcha");                   
                }

                if (!_userService.EmailExists(loginVM.Email, true))
                {
                    ModelState.AddModelError("Login", "Email not found");
                }

                if (!_userService.PasswordMatches(loginVM.Password, loginVM.Email))
                {
                    ModelState.AddModelError("Login", "Invalid password");
                }

                if (ModelState.IsValid)
                {
                    var userVW = _userService.GetUserViews(i => i.Email == loginVM.Email).FirstOrDefault();
                    LoginUser(userVW);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Login");
                success = false;
                errorMessages = new List<string>() { "Error logging user in." };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            var baseUrl = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
            var refUrl = Request.Headers["Referer"].ToString();
            var url = !string.IsNullOrWhiteSpace(refUrl) ? refUrl : baseUrl;
            
            return Redirect(url);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            var signUpVM = new SignUpViewModel() {
                GClientID = _config.GetSection("Auth").GetSection("Google").GetSection("ClientID").Value,
                FBClientID = _config.GetSection("Auth").GetSection("Facebook").GetSection("ClientID").Value,
                FBApiVer = _config.GetSection("Auth").GetSection("Facebook").GetSection("ApiVersion").Value,
                RecaptchaKey = _config.GetSection("Auth").GetSection("Google").GetSection("RecaptchaKey").Value
            };

            return View(signUpVM);
        }

        [HttpPost]
        public async Task<JsonResult> SignUp(SignUpViewModel signUpVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                var result = await _authService.ValidateGoogleRecaptcha(signUpVM.Token);
                if (!result)
                {
                     ModelState.AddModelError("SignUp", "Invalid recaptcha");                   
                }

                if (_userService.EmailExists(signUpVM.Email, false))
                {
                    ModelState.AddModelError("SignUp", "Email already exists for another user");
                }

                if (ModelState.IsValid)
                {
                    _ = _userService.SendActivationEmail(signUpVM.Email).ContinueWith(t => _logger.Error(t.Exception, "SendActivationEmail"), TaskContinuationOptions.OnlyOnFaulted);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SignUp");
                success = false;
                errorMessages = new List<string>() { "Error signing up user" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpPost]
        public async Task<JsonResult> LoginOrSignUpWithSocial(string accessToken, int socialAccountTypeID)
        {
            var success = false;
            var isNewUser = false;
            List<string> errorMessages = null;
 
            try
            {
                var result = await _authService.ValidateSocialToken(accessToken, socialAccountTypeID);

                if (result == null)
                {
                    ModelState.AddModelError("LoginOrSignUpWithSocial", "Invalid Token");
                }

                var userVW = _userService.GetUserViews(i => i.Email == result.Email && i.Active).FirstOrDefault();              
                if (_userService.EmailExists(result.Email, false) && userVW == null)
                {
                    ModelState.AddModelError("LoginOrSignUpWithSocial", "Email not found");
                }

                if (ModelState.IsValid)           
                {
                    if (userVW != null)
                    {
                        LoginUser(userVW);
                        success = true;
                    }
                    else
                    {
                        var username = result.Email.Split('@')[0];
                        if (_userService.UsernameExists(username, false))
                        {
                            username += '_' + ((String)result.Email).GetHashCode();
                        }
                        var pass = StringExtensions.GeneratePassword(15, 2);
                        var userID = _userService.CreateUser(result.Email, username, pass);
                        userVW = _userService.GetUserViews(i => i.UserID == userID).FirstOrDefault();
                        LoginUser(userVW);
                        _ = _userService.SendConfirmRegistrationEmail(userVW.Email, userVW.Username).ContinueWith(t => _logger.Error(t.Exception, "SendConfirmRegistrationEmail"), TaskContinuationOptions.OnlyOnFaulted);
                        isNewUser = true;
                        success = true;
                        TempData.Add("ShowWelcome", true);                    
                    }
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }                
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "LoginOrSignUpByGoogle");
                success = false;
                errorMessages = new List<string>() { "Error logging in with Google" };
            }

            return Json(new { success = success, isnewuser = isNewUser, errorMessages = errorMessages });
        }

        [HttpGet]
        public ViewResult Activate(string email, long expirationTime, string token)
        {
            var activateUserVM = _userService.GetActivateUser(email, expirationTime, token);
            HttpContext.Session.Set<string>("Email", email);

            return View(activateUserVM);
        }

        [HttpPost]
        public JsonResult Activate(ActivateViewModel activateUserVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
                if (activateUserVM.Email.GetHMACSHA256Hash(hashKey) != activateUserVM.EmailToken)
                {
                    ModelState.AddModelError("Activate", "Email does not match registration");
                }

                if (_userService.EmailExists(activateUserVM.Email, false))
                {
                    ModelState.AddModelError("Activate", "Email already exists for another user");
                }

                if (_userService.UsernameExists(activateUserVM.Username, false))
                {
                    ModelState.AddModelError("Activate", "Username already exists for another user");
                }

                if (ModelState.IsValid)
                {
                    var userID = _userService.CreateUser(activateUserVM.Email, activateUserVM.Username, activateUserVM.Password);
                    var userVW = _userService.GetUserViews(i => i.UserID == userID).FirstOrDefault();
                    LoginUser(userVW);
                    _ = _userService.SendConfirmRegistrationEmail(userVW.Email, userVW.Username).ContinueWith(t => _logger.Error(t.Exception, "SendConfirmRegistrationEmail"), TaskContinuationOptions.OnlyOnFaulted);
                    success = true;
                    TempData.Add("ShowWelcome", true);                    
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Activate");
                success = false;
                errorMessages = new List<string>() { "Error creating user" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public ActionResult ResetPassword(string email)
        {
            var resetPassVM = new ResetPasswordViewModel() { Email = email };
            return View(resetPassVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ResetPassword(ResetPasswordViewModel resetPassVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                if (!_userService.EmailExists(resetPassVM.Email, true))
                {
                    ModelState.AddModelError("ResetPassword", "Email not found");
                }

                if (ModelState.IsValid)
                {
                    _ = _userService.SendResetPasswordEmail(resetPassVM.Email).ContinueWith(t => _logger.Error(t.Exception, "SendResetPasswordEmail"), TaskContinuationOptions.OnlyOnFaulted);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ResetPassword");
                success = false;
                errorMessages = new List<string>() { "Error resetting password" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public ViewResult ChangePassword(string username, string email, long expirationTime, string token)
        {
            var changePassVM = _userService.GetChangePassword(username, email, expirationTime, token);
            HttpContext.Session.Set<string>("Username", username);

            return View(changePassVM);
        }

        [HttpPost]
        public JsonResult ChangePassword(ChangePasswordViewModel changePassVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                var username = HttpContext.Session.Get<string>("Username");
                if (_userService.PasswordMatches(changePassVM.Password, username))
                {
                    ModelState.AddModelError("ChangePassword", "Password must differ from previous password");
                }

                if (ModelState.IsValid)
                {
                    _userService.ChangeUserPassword(username, changePassVM.Password);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ChangePassword");
                success = false;
                errorMessages = new List<string>() { "Error changing password" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpPost]
        public JsonResult UpdateIsDarkTheme(bool isDarkTheme)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                var userID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _userService.UpdateIsDarkTheme(userID, isDarkTheme);

                var userVW = _userService.GetUserViews(i => i.UserID == userID).FirstOrDefault();
                LoginUser(userVW);
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "UpdateIsDarkTheme");
                success = false;
                errorMessages = new List<string>() { "Error updating isDarkTheme" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        private async void LoginUser(UserView userVW)
        {
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, userVW.UserID.ToString()),
                            new Claim(ClaimTypes.Email, userVW.Email),
                            new Claim(ClaimTypes.Name, userVW.Username),
                            new Claim("theme", userVW.IsDarkTheme ? "dark" : "light")
                        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ActiveEmailExists(string email)
        {
            var result = _userService.EmailExists(email, true);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult UsernameNotExists(string username)
        {
            var result = !_userService.UsernameExists(username, false);

            return Json(result);
        }
     
        [AllowAnonymous]
        [HttpGet]
        public IActionResult PasswordNotMatches(string password, string email)
        {
            var result = !_userService.PasswordMatches(password, email);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult EmailNotExists(string email)
        {
            var result = !_userService.EmailExists(email, false);

            return Json(result);
        } 
    }
}
