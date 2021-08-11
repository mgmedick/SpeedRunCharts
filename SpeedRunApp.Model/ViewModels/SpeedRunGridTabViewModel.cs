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

        public SpeedRunGridTabViewModel(IEnumerable<GameViewModel> tabItems, int gameID, int categoryTypeID, int categoryID, int? levelID, Dictionary<int, int> variableValueIDs)
        {
            TabItems = tabItems;
            GameID = gameID;
            CategoryTypeID = categoryTypeID;
            CategoryID = categoryID;
            LevelID = levelID;
            VariableValueIDs = variableValueIDs;
        }

        public IEnumerable<GameViewModel> TabItems { get; set; }
        public int? GameID { get; set; }
        public int? CategoryTypeID { get; set; }
        public int? CategoryID { get; set; }
        public int? LevelID { get; set; }
        public Dictionary<int, int> VariableValueIDs { get; set; }
    }
}
