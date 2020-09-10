using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class VariableValueDisplay : IDNamePair
    {
        public VariableDisplay Variable { get; set; }
        public IEnumerable<VariableDisplay> SubVariables { get; set; }
    }
}


