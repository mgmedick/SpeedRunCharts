using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunListViewModel
    {
        public SpeedRunListViewModel(IEnumerable<SpeedRunViewModel> speedRuns, int elementsPerPage)
        {
            SpeedRuns = speedRuns;
            ElementsPerPage = elementsPerPage;
        }

        public IEnumerable<SpeedRunViewModel> SpeedRuns
        {
            get;
            set;
        }

        public int ElementsPerPage
        {
            get;
            set;
        }
    }
}
