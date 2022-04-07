﻿using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridTabViewModel
    {
        public SpeedRunGridTabViewModel(IEnumerable<GameViewModel> tabItems, IEnumerable<IDNamePair> exportTypes)
        {
            TabItems = tabItems;
            ExportTypes = exportTypes;
        }

        public SpeedRunGridTabViewModel(IEnumerable<GameViewModel> tabItems, IEnumerable<IDNamePair> exportTypes, int gameID, int categoryTypeID, int categoryID, int? levelID, Dictionary<string, string> subCategoryVariableValueIDs, bool showAllData)
        {
            TabItems = tabItems;
            ExportTypes = exportTypes;
            GameID = gameID;
            CategoryTypeID = categoryTypeID;
            CategoryID = categoryID;
            LevelID = levelID;
            SubCategoryVariableValueIDs = subCategoryVariableValueIDs;
            ShowAllData = showAllData;
        }        

        public IEnumerable<GameViewModel> TabItems { get; set; }
        public IEnumerable<IDNamePair> ExportTypes { get; set; }
        public int? GameID { get; set; }
        public int? CategoryTypeID { get; set; }
        public int? CategoryID { get; set; }
        public int? LevelID { get; set; }
        public Dictionary<string, string> SubCategoryVariableValueIDs { get; set; }
        public bool? ShowAllData { get; set; }
    }
}
