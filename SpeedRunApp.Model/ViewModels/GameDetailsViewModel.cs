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
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public Uri CoverImageUri { get; set; }
        //public List<string> CategoryTypes { get { return Enum.GetNames(typeof(CategoryType)).ToList(); } }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        //public List<CategoryDTO> GameCategories { get { return Categories.Where(i => i.Type == CategoryType.PerGame).ToList(); } }
        //public List<CategoryDTO> LevelCategories { get { return Categories.Where(i => i.Type == CategoryType.PerLevel).ToList(); } }
    }
}
