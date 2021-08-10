using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridTabViewModel
    {
        public SpeedRunGridTabViewModel(IEnumerable<GameViewModel> tabItems, int gameID, int categoryTypeID, int categoryID, int? levelID)
        {
            TabItems = tabItems;
            GameID = gameID;
            CategoryTypeID = categoryTypeID;
            CategoryID = categoryID;
            LevelID = levelID;
        }

        public IEnumerable<GameViewModel> TabItems { get; set; }
        public int GameID { get; set; }
        public int CategoryTypeID { get; set; }
        public int CategoryID { get; set; }
        public int? LevelID { get; set; }
    }
}
