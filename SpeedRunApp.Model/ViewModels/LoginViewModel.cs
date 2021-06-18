﻿using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SpeedRunApp.Model.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        [Remote(action: "UsernameExists", controller: "SpeedRun")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [Remote(action: "PasswordMatches", controller: "SpeedRun", AdditionalFields = nameof(Username))]
        public string Password { get; set; }

        public bool Success { get; set; }
    }
}

