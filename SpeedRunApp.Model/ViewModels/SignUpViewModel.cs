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
        [EmailAddress]
        [RemoteWithServerSideAttribute(action: "EmailNotExists", controller: "SpeedRun", ErrorMessage = "Email already exists")]
        public string Email { get; set; }

        public bool Success { get; set; }
    }
}

