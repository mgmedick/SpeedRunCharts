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
        /*
        public int? GameID { get; set; }
        public int? CategoryTypeID { get; set; }
        public int? CategoryID { get; set; }
        public int? LevelID { get; set; }
        public Dictionary<string, string> SubCategoryVariableValueIDs { get; set; }
        public bool? ShowAllData { get; set; }
        public bool? ShowMisc { get; set; }
        */
    }
}
