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
        public bool IsSingleCategory { get; set; }
        public object Clone()
        {
            Variable variable = (Variable)this.MemberwiseClone();

            if (VariableValues != null) {
                var list = new List<VariableValue>();
                foreach (var variableValue in VariableValues)
                {
                    var variableValueClone = (VariableValue)variableValue.Clone();
                    list.Add(variableValueClone);
                }
                variable.VariableValues = list;
            }
            
            return variable;
        }
    }
}


