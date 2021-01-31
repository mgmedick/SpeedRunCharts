using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridContainerViewModel
    {
        public SpeedRunGridContainerViewModel(SpeedRunGridTabViewModel gridModel, IEnumerable<SpeedRunGridViewModel> gridData)
        {
            GridModel = gridModel;
            GridData = gridData;
        }

        public SpeedRunGridTabViewModel GridModel { get; set; }
        public IEnumerable<SpeedRunGridViewModel> GridData { get; set; }
    }
}
