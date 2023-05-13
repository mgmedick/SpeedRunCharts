using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserChartTabViewModel
    {
        public UserChartTabViewModel(IEnumerable<GameTabViewModel> tabItems, IEnumerable<IDNamePair> categoryTypes, IEnumerable<SpeedRunChartViewModel> tableData)
        {
            TabItems = tabItems;
            CategoryTypes = categoryTypes;
            TableData = tableData;
        }

        public IEnumerable<GameTabViewModel> TabItems { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }               
        public IEnumerable<SpeedRunChartViewModel> TableData { get; set; }
    }
}
