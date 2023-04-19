using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;

namespace SpeedRunApp.Model.ViewModels
{
    public class WorldRecordGridViewModel : SpeedRunGridViewModel
    {
        public WorldRecordGridViewModel(WorldRecordGridView run) : base(run)
        {
            CategoryName = run.CategoryName;
            CategoryTypeID = run.CategoryTypeID;
            IsMiscellaneous = run.IsMiscellaneous;
            LevelName = run.LevelName;
        }
         
        public string CategoryName { get; set; }
        public int CategoryTypeID { get; set; }
        public bool IsMiscellaneous { get; set; }
        public string LevelName { get; set; }      
    }      
}
