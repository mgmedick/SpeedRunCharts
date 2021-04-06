using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.Data
{
    public class VariableValue : TabItem, ICloneable
    {        
        public IEnumerable<Variable> SubVariables { get; set; }

        public object Clone()
        {
            VariableValue variableValue = (VariableValue)this.MemberwiseClone();

            return variableValue;
        }
    }
}



