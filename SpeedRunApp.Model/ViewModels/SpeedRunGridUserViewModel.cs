using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridUserViewModel : SpeedRunGridViewModel
    {
        public SpeedRunGridUserViewModel(SpeedRunGridUserView run) : base((SpeedRunGridView)run)
        {
            SpeedRunComID = run.SpeedRunComID;
            SubCategoryVariableValues = run.SubCategoryVariableValues;
            CategoryName = run.CategoryName;
            LevelName = run.LevelName;
            IsTimerAscending = run.IsTimerAscending;
            IsMiscellaneous = run.IsMiscellaneous;
        }
        
        public string SpeedRunComID { get; set; }
        public string SubCategoryVariableValues { get; set; }
        public string CategoryName { get; set; }
        public string LevelName { get; set; }
        public bool IsTimerAscending { get; set; }
        public bool IsMiscellaneous { get; set; }
        public bool IsPersonalBest { get; set; }
    }
}
