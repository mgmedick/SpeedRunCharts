using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunGridTabView
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public string GameName { get; set; }
        public int CategoryTypeID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? LevelID { get; set; }
        public string LevelName { get; set; }
        public string VariableValues { get; set; }

    }
} 
