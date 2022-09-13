using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunGridTabView
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int CategoryID { get; set; }
        public int? LevelID { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
        public int? Rank { get; set; }
    }
} 
