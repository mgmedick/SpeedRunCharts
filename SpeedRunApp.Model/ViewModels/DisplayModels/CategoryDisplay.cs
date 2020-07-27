﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class CategoryDisplay : IDNamePair
    {
        public string GameID { get; set; }
        public string CategoryTypeID { get; set; }
    }
}
