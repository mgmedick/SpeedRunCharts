using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class PlayerSpeedRunTabViewModel
    {
        public PlayerSpeedRunTabViewModel(IEnumerable<GameTabViewModel> tabItems, IEnumerable<IDNamePair> categoryTypes, IEnumerable<SpeedRunGridPlayerViewModel> tableData)
        {
            TabItems = tabItems;
            CategoryTypes = categoryTypes;
            TableData = tableData;
        }

        public IEnumerable<GameTabViewModel> TabItems { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }               
        public IEnumerable<SpeedRunGridPlayerViewModel> TableData { get; set; }
    }
}
