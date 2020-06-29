using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunListViewModel
    {
        public SpeedRunListViewModel(IEnumerable<SpeedRunViewModel> speedRuns)
        {
            SpeedRuns = speedRuns;
        }

        public IEnumerable<SpeedRunViewModel> SpeedRuns
        {
            get;
            set;
        }
    }
}
