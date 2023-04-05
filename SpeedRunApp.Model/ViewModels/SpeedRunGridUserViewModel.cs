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
            SubCategoryVariableValues = run.SubCategoryVariableValues;
        }
        public string SubCategoryVariableValues { get; set; }
        public bool IsPersonalBest { get; set; }
    }
}
