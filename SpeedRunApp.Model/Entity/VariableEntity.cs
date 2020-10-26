using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class VariableEntity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string GameID { get; set; }
        public int VariableScopeTypeID { get; set; }
        public string CategoryID { get; set; }
        public string LevelID { get; set; }
    }
} 
