using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunListViewModel
    {
        public SpeedRunListViewModel(int defaultTopAmount)
        {
            DefaultTopAmount = defaultTopAmount;
        }

        public int DefaultTopAmount { get; set; }
    }
}
