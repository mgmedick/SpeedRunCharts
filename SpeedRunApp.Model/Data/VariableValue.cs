using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.Data
{
    public class VariableValue : TabItem
    {
        public IEnumerable<Variable> SubVariables { get; set; }
    }
}



