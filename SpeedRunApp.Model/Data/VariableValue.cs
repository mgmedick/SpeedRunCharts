using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.Data
{
    public class VariableValue
    {       
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int GameID { get; set; }
        public int VariableID { get; set; }
        public bool IsMiscellaneous { get; set; }
        public bool Deleted { get; set; }

        //Transient
        public bool HasData { get; set; }
        public IEnumerable<Variable> SubVariables { get; set; }
        public object Clone()
        {
            VariableValue variableValue = (VariableValue)this.MemberwiseClone();

            return variableValue;
        }            
    }
}



