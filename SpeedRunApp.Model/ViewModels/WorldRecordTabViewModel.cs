using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class WorldRecordTabViewModel
    {
        public WorldRecordTabViewModel(IEnumerable<GameTabViewModel> tabItems, IEnumerable<IDNamePair> exportTypes)
        {
            TabItems = tabItems;
            ExportTypes = exportTypes;
        }

        public IEnumerable<GameTabViewModel> TabItems { get; set; }
        public IEnumerable<IDNamePair> ExportTypes { get; set; }        
    }
}
