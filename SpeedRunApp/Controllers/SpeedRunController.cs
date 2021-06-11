using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace SpeedRunApp.WebUI.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunService _speedRunService = null;
        private readonly IGameService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly IUserAccountService _userAcctService = null;
        private readonly IConfiguration _config = null;

        public SpeedRunController(ISpeedRunService speedRunService, IGameService gamesService, IUserService userService, IUserAccountService userAcctService, IConfiguration config)
        {
            _speedRunService = speedRunService;
            _gamesService = gamesService;
            _userService = userService;
            _userAcctService = userAcctService;
            _config = config;
        }

        public ViewResult SpeedRunList()
        {
            var runListVM = _speedRunService.GetSpeedRunList();

            return View(runListVM);
        }

        [HttpGet]
        public JsonResult GetLatestSpeedRuns(SpeedRunListCategory category, int topAmount, int? orderValueOffset)
        {
            var results = _speedRunService.GetLatestSpeedRuns(category, topAmount, orderValueOffset);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetEditSpeedRun(int gameID, int? speedRunID = null, bool isReadOnly = false)
        {
            var results = _speedRunService.GetEditSpeedRun(gameID, speedRunID, isReadOnly);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetSpeedRunSummary(int speedRunID)
        {
            var results = _speedRunService.GetSpeedRunSummary(speedRunID);

            return Json(results);
        }

        [HttpGet]
        public ActionResult Login()
        {
            var loginVM = new LoginViewModel();

            return PartialView("_Login", loginVM);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var userAcct = _userAcctService.GetUserAccountForLogin(loginVM.Username);

                if (userAcct == null)
                {
                    ModelState.AddModelError("BadLogin", "Invalid username/password.");
                }
                else if (userAcct.Locked)
                {
                    ModelState.AddModelError("BadLogin", "The account has been locked due to too many failed login attempts. Please contact support to unlock the account.");
                }
                else
                {
                    string encodedPassword = loginVM.Password.EncodeString(userAcct.Salt);

                    if (encodedPassword != userAcct.Password)
                    {
                        var attemptCount = HttpContext.Session.Get<int>("PasswordAttempts");
                        var maxPasswordAttempts = Convert.ToInt32(_config.GetSection("SiteSettings").GetSection("MaxPasswordAttempts").Value);

                        attemptCount++;
                        HttpContext.Session.Set<int>("PasswordAttempts", attemptCount);

                        if (attemptCount > maxPasswordAttempts)
                        {
                            _userAcctService.LockUserAccount(userAcct.ID);
                            ModelState.AddModelError("BadLogin", "The account has been locked due to too many failed login attempts. Please contact support to unlock the account.");
                        }
                        else
                        {
                            ModelState.AddModelError("BadLogin", "Invalid username/password.");
                        }
                    }
                    else 
                    {
                        if (userAcct.PromptToChange)
                        {
                            HttpContext.Session.Set<string>("LoginUserName", userAcct.Username);
                            return RedirectToAction("ChangePassword");
                        }
                        else 
                        {
                            var claims = new List<Claim>
                            {
                                new Claim("UserID", userAcct.ID.ToString()),
                                new Claim("Email", userAcct.Email),
                                new Claim("Username", userAcct.Username)
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(identity));

                            return Json(new { success = true });
                        }
                    }
                }
            }

            return PartialView("_Login", loginVM);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            var signUpVM = new SignUpViewModel();

            return PartialView("_SignUp", signUpVM);
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
