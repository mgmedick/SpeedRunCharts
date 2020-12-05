using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunListViewModel
    {
        public SpeedRunListViewModel(IEnumerable<IDNamePair> statusTypes, IEnumerable<IDNamePair> categoryTypes, IEnumerable<IDNamePair> platforms)
        {
            StatusTypes = statusTypes;
            CategoryTypes = categoryTypes;
            Platforms = platforms;
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
