using System.Collections.Generic;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridViewModel
    {
        public SpeedRunGridViewModel()
        {
        }
        /*
        public SpeedRunGridViewModel(string sender, IEnumerable<IDNamePair> categoryTypes, IEnumerable<GameDisplay> games, IEnumerable<CategoryDisplay> categories, IEnumerable<LevelDisplay> levels, IEnumerable<SpeedRunRecordViewModel> speedRunRecordVMs)
        {
            Sender = sender;
            CategoryTypes = categoryTypes;
            Games = games;
            Categories = categories;
            Levels = levels;
        }
        */
        public SpeedRunGridViewModel(string sender, IEnumerable<IDNamePair> categoryTypes, IEnumerable<GameDisplay> games, IEnumerable<CategoryDisplay> categories, IEnumerable<LevelDisplay> levels, IEnumerable<SpeedRunViewModel> speedRunVMs)
        {
            Sender = sender;
            CategoryTypes = categoryTypes;
            Games = games;
            Categories = categories;
            Levels = levels;
            SpeedRunVMs = speedRunVMs;
        }

        public string Sender { get; set; }
        public IEnumerable<SpeedRunViewModel> SpeedRunVMs { get; set; }
        //public IEnumerable<SpeedRunRecordViewModel> SpeedRunRecordVMs { get; set; }
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }
        public IEnumerable<GameDisplay> Games { get; set; }
        public IEnumerable<CategoryDisplay> Categories { get; set; }
        public IEnumerable<LevelDisplay> Levels { get; set; }
    }
}
