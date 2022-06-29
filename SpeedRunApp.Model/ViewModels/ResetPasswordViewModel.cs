using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SpeedRunApp.Model.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Username")]
        //[Remote(action: "ActiveUsernameExists", controller: "SpeedRun", ErrorMessage = "Username not found")]
        public string Username { get; set; }
    }
}

