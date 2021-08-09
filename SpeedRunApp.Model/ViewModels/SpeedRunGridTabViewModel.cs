using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridTabViewModel
    {
        public SpeedRunGridTabViewModel(IEnumerable<GameViewModel> tabItems)
        {
            TabItems = tabItems;
        }

        public IEnumerable<GameViewModel> TabItems { get; set; }
    }
}
