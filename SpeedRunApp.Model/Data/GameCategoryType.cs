using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class GameCategoryType
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int CategoryTypeID { get; set; }
        public bool Deleted { get; set; }
    }
} 
