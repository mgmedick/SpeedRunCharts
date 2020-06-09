using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

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
            Assets = new AssetsDTO(game.Assets);

            //links
            _categories = game.categories;
            _levels = game.levels;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public AssetsDTO Assets { get; set; }

        //links
        private Lazy<ReadOnlyCollection<Category>> _categories { get; set; }
        public IEnumerable<CategoryDTO> Categories { get { return _categories.Value?.Select(i => new CategoryDTO(i)); } }
        private Lazy<ReadOnlyCollection<Level>> _levels { get; set; }
        public IEnumerable<LevelDTO> Levels { get { return _levels.Value?.Select(i => new LevelDTO(i)); } }
        public IEnumerable<CategoryType> CategoryTypes { get { return Categories.Select(i => i.Type).OrderBy(i => i).Distinct(); } }
    }
}
