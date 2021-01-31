using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridTabViewModel
    {
        public SpeedRunGridTabViewModel(string sender, IEnumerable<GameViewModel> tabItems)
        {
            Sender = sender;
            TabItems = tabItems;
        }

        public string Sender { get; set; }
        public IEnumerable<GameViewModel> TabItems { get; set; }
    }
}
