﻿using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SpeedRunApp.Model.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

