using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunGridUserView : SpeedRunGridView
    {    
        public string SpeedRunComID { get; set; }        
        public string SubCategoryVariableValues { get; set; }
        public string CategoryName { get; set; }
        public string LevelName { get; set; }
        public bool IsTimerAscending { get; set; }
        public bool IsMiscellaneous { get; set; }       
        public int UserID { get; set; }
    }
} 
