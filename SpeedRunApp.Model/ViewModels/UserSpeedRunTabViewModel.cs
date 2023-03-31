using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserSpeedRunTabViewModel
    {
        public UserSpeedRunTabViewModel(IEnumerable<GameTabViewModel> tabItems, IEnumerable<IDNamePair> categoryTypes, IEnumerable<SpeedRunGridViewModel> tableData)
        {
            TabItems = tabItems;
            CategoryTypes = categoryTypes;
            TableData = tableData;
        }

        public IEnumerable<GameTabViewModel> TabItems { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }               
        public IEnumerable<SpeedRunGridViewModel> TableData { get; set; }
    }
}
