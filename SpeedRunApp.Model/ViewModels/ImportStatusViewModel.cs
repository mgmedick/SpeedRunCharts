using System;
using System.Linq;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;

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
        public string ImportLastRunDateString
        {
            get
            {
                return ImportLastRunDate?.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            }
        }     
        public string ImportLastUpdateSpeedRunsDateString
        {
            get
            {
                return ImportLastUpdateSpeedRunsDate?.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            }
        }       
        public string ImportLastBulkReloadDateString
        {
            get
            {
                return ImportLastBulkReloadDate?.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            }
        }                 
    }
}


