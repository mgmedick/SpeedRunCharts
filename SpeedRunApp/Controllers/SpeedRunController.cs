using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon.Extensions;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Linq;

namespace SpeedRunApp.WebUI.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunService _speedRunService = null;
        private readonly IGameService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly IUserAccountService _userAcctService = null;

        public SpeedRunController(ISpeedRunService speedRunService, IGameService gamesService, IUserService userService, IUserAccountService userAcctService)
        {
            _speedRunService = speedRunService;
            _gamesService = gamesService;
            _userService = userService;
            _userAcctService = userAcctService;
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
                var login = _userAcctService.ValidateLogin(loginVM);
                var userAcct = login.Item1;
                var errorList = login.Item2;

                if (errorList.Any())
                {
                    foreach (var error in errorList)
                    {
                        ModelState.AddModelError("BadLogin", error);
                    }
                }
                else if (userAcct.PromptToChange)
                {
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

            return PartialView("_Login", loginVM);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            var signUpVM = new SignUpViewModel();

            return PartialView("_SignUp", signUpVM);
        }

        [HttpPost]
        public ActionResult SignUp(SignUpViewModel signUpVM)
        {
            if (ModelState.IsValid)
            {
                var errorList = _userAcctService.SignUp(signUpVM);

                if (errorList.Any())
                {
                    foreach (var error in errorList)
                    {
                        ModelState.AddModelError("BadSignUp", error);
                    }
                }
                else
                {
                    return Json(new { success = true });
                }
            }

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
