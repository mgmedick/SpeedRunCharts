using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class WorldRecordGridView : SpeedRunGridView
    {
        public string CategoryName { get; set; }
        public int CategoryTypeID { get; set; }
        public bool IsMiscellaneous { get; set; }
        public string LevelName { get; set; }

    }
} 
