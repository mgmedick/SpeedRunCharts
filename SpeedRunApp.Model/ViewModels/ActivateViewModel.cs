using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SpeedRunApp.Model.ViewModels
{
    public class ActivateViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        [Remote(action: "UsernameExists", controller: "SpeedRun")]
        [RegularExpression(@"^(?=.{3,15}$)([A-Za-z0-9][._()\[\]-]?)*$", ErrorMessage = @"Username must be between 3-15 characters and any combo of letters, numbers and special characters (._[]()-\)")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [Compare(otherProperty: nameof(ConfirmPassword))]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password required")]
        public string ConfirmPassword { get; set; }

        public bool IsValid { get; set; }
    }
}

