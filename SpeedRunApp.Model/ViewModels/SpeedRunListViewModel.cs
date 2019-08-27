using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedRunApp.Model
{
    public class SpeedRunListViewModel
    {
        public IEnumerable<SpeedRunViewModel> SpeedRuns
        {
            get;
            set;
        }
    }
}
