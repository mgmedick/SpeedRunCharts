using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel(GameDTO game)
        {
            ID = game.ID;
            Name = game.Name;
            Abbreviation = game.Abbreviation;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.CoverImageUri;
            Categories = game.Categories;
            Levels = game.Levels;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public Uri CoverImageUri { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public IEnumerable<LevelDTO> Levels { get; set; }
        public IEnumerable<CategoryType> CategoryTypes { get { return Categories.Select(i => i.Type).OrderBy(i=>i).Distinct(); } }
    }
}
