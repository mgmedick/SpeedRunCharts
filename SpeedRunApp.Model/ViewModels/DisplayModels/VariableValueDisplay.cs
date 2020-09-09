using System.Collections.Generic;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class VariableValueDisplay : IDNamePair
    {
        public IEnumerable<VariableDisplay> Variables { get; set; }
    }
}


