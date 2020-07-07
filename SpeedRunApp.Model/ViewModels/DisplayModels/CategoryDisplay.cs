using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class CategoryDisplay : IDNamePair
    {
        public string GameID { get; set; }
        public string CategoryTypeID { get; set; }
    }
}
