using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class SearchSpeedRunGridViewModel
    {
        public SearchSpeedRunGridViewModel(string sender, IEnumerable<IDNamePair> categoryTypes, IEnumerable<GameDisplay> games, IEnumerable<CategoryDisplay> categories, IEnumerable<LevelDisplay> levels)
        {
            Sender = sender;
            CategoryTypes = categoryTypes;
            Games = games;
            Categories = categories;
            Levels = levels;
        }

        public string Sender { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }
        public IEnumerable<GameDisplay> Games { get; set; }
        public IEnumerable<CategoryDisplay> Categories { get; set; }
        public IEnumerable<LevelDisplay> Levels { get; set; }
    }
}
