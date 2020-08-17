using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDisplay : IDNamePair
    {
        public IEnumerable<string> CategoryTypeIDs { get; set; }
    }
}
