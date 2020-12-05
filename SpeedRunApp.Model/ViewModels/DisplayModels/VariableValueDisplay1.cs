using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class VariableValueDisplay1 : IDNamePair
    {
        public IEnumerable<VariableDisplay1> SubVariables { get; set; }
    }
}


