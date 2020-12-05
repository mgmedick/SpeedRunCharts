using System;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class VariableDisplay1 : ICloneable
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsSubCategory { get; set; }
        public int ScopeTypeID { get; set; }
        public string CategoryID { get; set; }
        public string LevelID { get; set; }
        public IEnumerable<VariableValueDisplay1> VariableValues { get; set; }

        public object Clone()
        {
            VariableDisplay1 variable = (VariableDisplay1)this.MemberwiseClone();
            variable.VariableValues = new List<VariableValueDisplay1>(this.VariableValues);

            return variable;
        }
    }
}


