using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class Variable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int GameID { get; set; }
        public int VariableScopeTypeID { get; set; }
        public int? CategoryID { get; set; }
        public int? LevelID { get; set; }
        public bool IsSubCategory { get; set; }
        public int SortOrder { get; set; }
        public bool Deleted { get; set; }

        //Transient
        public bool HasData {
            get
            {
                return !VariableValues.All(x => !x.HasData);
            }
        }        
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


