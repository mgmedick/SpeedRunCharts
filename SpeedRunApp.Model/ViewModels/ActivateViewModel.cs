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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }
        public string EmailToken { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        [RegularExpression(@"^[._()-\/#&$@+\w\s]{3,30}$", ErrorMessage = @"Username must be between 3 - 30 alphanumeric/special characters")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [Compare(otherProperty: nameof(ConfirmPassword))]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[._()-\/#&$@+\w\s]{8,30}$", ErrorMessage = @"Password must be between 8 - 30 alphanumeric/special characters with 1 number and letter")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password required")]
        public string ConfirmPassword { get; set; }

        public bool IsValid { get; set; }
    }
}

