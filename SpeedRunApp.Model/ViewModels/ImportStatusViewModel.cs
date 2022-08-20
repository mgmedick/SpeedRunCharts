using System;

namespace SpeedRunApp.Model.ViewModels
{
    public class ImportStatusViewModel
    {
        public ImportStatusViewModel(DateTime? importLastRunDate, DateTime? importLastUpdateSpeedRunsDate, DateTime? importLastBulkReloadDate)
        {
            ImportLastRunDate = importLastRunDate;
            ImportLastUpdateSpeedRunsDate = importLastUpdateSpeedRunsDate;
            ImportLastBulkReloadDate = importLastBulkReloadDate;
        }
        
        public DateTime? ImportLastRunDate { get; set; }
        public DateTime? ImportLastUpdateSpeedRunsDate { get; set; }
        public DateTime? ImportLastBulkReloadDate { get; set; }                        
    }
}


