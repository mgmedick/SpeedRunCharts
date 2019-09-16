using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Model
{
    public class GameDTO
    {
        public GameDTO(Game game)
        {
            ID = game.ID;
            Name = game.Name;
            Abbreviation = game.Abbreviation;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.Assets?.CoverLarge?.Uri;
            Categories = game.Categories.Select(i => new CategoryDTO(i));
            Levels = game.Levels.Select(i => new LevelDTO(i));
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public Uri CoverImageUri { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public IEnumerable<LevelDTO> Levels { get; set; }
    }
}
