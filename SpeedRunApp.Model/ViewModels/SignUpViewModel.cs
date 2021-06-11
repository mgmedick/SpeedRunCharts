using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;
using System.ComponentModel.DataAnnotations;

namespace SpeedRunApp.Model.ViewModels
{
    public class SignUpViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

