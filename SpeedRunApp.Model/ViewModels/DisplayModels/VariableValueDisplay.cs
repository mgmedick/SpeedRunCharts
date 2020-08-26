using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class VariableValueDisplay : IDNamePair
    {
        public IEnumerable<VariableDisplay> Variables { get; set; }
        //public string CategoryID { get; set; }
        //public IDNamePair Variable { get; set; }
        //public IEnumerable<VariableValueDisplay> childVariableValues { get; set; }
    }
}


