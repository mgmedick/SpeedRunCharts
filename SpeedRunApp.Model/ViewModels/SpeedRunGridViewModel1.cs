using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridViewModel1
    {
        public SpeedRunGridViewModel1()
        {
        }

        public SpeedRunGridViewModel1(string sender, IEnumerable<SpeedRunGridItemViewModel> gridItems)
        {
            Sender = sender;
            GridItems = gridItems;
        }

        public string Sender { get; set; }
        public IEnumerable<SpeedRunGridItemViewModel> GridItems { get; set; }
    }
}
