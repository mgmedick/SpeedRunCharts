using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameChartTabViewModel
    {
        public GameChartTabViewModel(IEnumerable<GameTabViewModel> tabItems)
        {
            TabItems = tabItems;
        }

        public IEnumerable<GameTabViewModel> TabItems { get; set; }
    }
}
