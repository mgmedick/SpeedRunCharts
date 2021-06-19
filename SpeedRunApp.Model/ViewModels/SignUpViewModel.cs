using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Model;

namespace SpeedRunApp.Model.ViewModels
{
    public class SignUpViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        [Remote(action: "EmailNotExists", controller: "SpeedRun", ErrorMessage = "Email already exists for another user")]
        public string Email { get; set; }
    }
}

