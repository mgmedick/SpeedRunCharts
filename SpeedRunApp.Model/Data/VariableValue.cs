using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.Data
{
    public class VariableValue : ICloneable
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<Variable> SubVariables { get; set; }
        public object Clone()
        {
            return (VariableValue)this.MemberwiseClone();
        }
    }
}



