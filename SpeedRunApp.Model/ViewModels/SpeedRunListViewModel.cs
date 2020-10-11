using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunListViewModel
    {
        public SpeedRunListViewModel(IEnumerable<SpeedRunViewModel> speedRuns, IEnumerable<IDNamePair> statusTypes, IEnumerable<IDNamePair> categoryTypes, IEnumerable<IDNamePair> platforms)
        {
            SpeedRuns = speedRuns;
            StatusTypes = statusTypes;
            CategoryTypes = categoryTypes;
            Platforms = platforms;
        }

        public IEnumerable<SpeedRunViewModel> SpeedRuns
        {
            get;
            set;
        }

        public IEnumerable<IDNamePair> StatusTypes
        {
            get;
            set;
        }

        public IEnumerable<IDNamePair> CategoryTypes
        {
            get;
            set;
        }

        public IEnumerable<IDNamePair> Platforms
        {
            get;
            set;
        }
    }
}
