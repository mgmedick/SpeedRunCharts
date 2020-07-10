using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel(Game game)
        {
            ID = game.ID;
            Name = game.Name;
            Abbreviation = game.Abbreviation;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.Assets?.CoverLarge?.Uri;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public Uri CoverImageUri { get; set; }
        public SpeedRunGridViewModel SpeedRunGridVM { get; set; }
    }
}
