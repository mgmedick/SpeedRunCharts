using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridViewModel
    {
        public SpeedRunGridViewModel(string sender, IEnumerable<SpeedRunGridItem> gridItems)
        {
            Sender = sender;
            GridItems = gridItems;
        }

        public string Sender { get; set; }
        public IEnumerable<SpeedRunGridItem> GridItems { get; set; }
    }
}
