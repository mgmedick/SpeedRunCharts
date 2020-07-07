using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDisplay : IDNamePair
    {
        public IEnumerable<string> CategoryTypeIDs { get; set; }
    }
}
