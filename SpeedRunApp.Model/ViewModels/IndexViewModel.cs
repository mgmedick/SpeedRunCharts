using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel(int defaultTopAmount, List<SummaryList> summaryLists)
        {
            DefaultTopAmount = defaultTopAmount;
            SummaryLists = summaryLists;
        }

        public int DefaultTopAmount { get; set; }
        public List<SummaryList> SummaryLists { get; set; }
    }
}
