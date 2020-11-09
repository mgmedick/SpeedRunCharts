using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class SpeedRunVariableValueEntity
    {
        public int ID { get; set; }
        public string SpeedRunID { get; set; }
        public string VariableID { get; set; }
        public string VariableValueID { get; set; }
    }
} 
