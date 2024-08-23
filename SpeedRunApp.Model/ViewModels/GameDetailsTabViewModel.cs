using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsTabViewModel
    {
        public GameDetailsTabViewModel(IEnumerable<GameTabViewModel> tabItems)
        {
            TabItems = tabItems;
        }

        public GameDetailsTabViewModel(IEnumerable<GameTabViewModel> tabItems, IEnumerable<IDNamePair> exportTypes)
        {
            TabItems = tabItems;
            ExportTypes = exportTypes;
        }

        public IEnumerable<GameTabViewModel> TabItems { get; set; }
        public IEnumerable<IDNamePair> ExportTypes { get; set; }  
        public SpeedRunGridView RunVW { get; set; }
    }
}
