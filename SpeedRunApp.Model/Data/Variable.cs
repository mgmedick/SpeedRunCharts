using System;
using System.Collections.Generic;

namespace SpeedRunApp.Model.Data
{
    public class Variable : ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsSubCategory { get; set; }
        public int ScopeTypeID { get; set; }
        public int? CategoryID { get; set; }
        public int? LevelID { get; set; }
        public IEnumerable<VariableValue> VariableValues { get; set; }

        public object Clone()
        {
            Variable variable = (Variable)this.MemberwiseClone();
            //variable.VariableValues = new List<VariableValue>(this.VariableValues);

            return variable;
        }
    }
}


