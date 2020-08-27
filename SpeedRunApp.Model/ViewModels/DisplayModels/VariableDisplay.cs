using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class VariableDisplay : IDNamePair
    {
        public string CategoryID { get; set; }
        public IEnumerable<VariableValueDisplay> VariableValues { get; set; }
    }
}


