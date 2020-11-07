using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Entity;

namespace SpeedRunApp.Model.Data
{
    public class Game
    {
        //public Game(Game game)
        //{
        //    ID = game.ID;
        //    Name = game.Name;
        //    Abbreviation = game.Abbreviation;
        //    YearOfRelease = game.YearOfRelease;
        //    Assets = new AssetsDTO(game.Assets);

        //    //links
        //    _categories = game.categories;
        //    _levels = game.levels;
        //}
        public GameHeader Header { get; set; }
        public string ID { get { return Header.ID; } }
        public string Name { get { return Header.Name; } }
        public string JapaneseName { get { return Header.JapaneseName; } }
        public string Abbreviation { get { return Header.Abbreviation; } }
        public Uri WebLink { get { return Header.WebLink; } }
        public int? YearOfRelease { get; set; }
        public Ruleset Ruleset { get; set; }
        public bool IsRomHack { get; set; }
        public DateTime? CreationDate { get; set; }
        public Assets Assets { get; set; }
        public IEnumerable<string> PlatformIDs { get; set; }
        public IEnumerable<string> RegionIDs { get; set; }
        public IEnumerable<Moderator> Moderators { get; set; }

        //linkIDs
        public string SeriesID { get; set; }
        public string OriginalGameID { get; set; }

        //embeds
        public IEnumerable<User> ModeratorUsers { get; set; }
        public IEnumerable<Platform> Platforms { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public IEnumerable<Level> Levels { get; set; }
        public IEnumerable<Variable> Variables { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<CategoryType> CategoryTypes { get { return Categories?.Select(i => i.Type).OrderBy(i => i).Distinct(); } }

        //private Lazy<ReadOnlyCollection<Category>> _categories { get; set; }
        //public IEnumerable<CategoryType> CategoryTypes { get { return Categories.Select(i => i.Type).OrderBy(i => i).Distinct(); } }

        public GameEntity ConvertToEntity()
        {
            return new GameEntity
            {
                ID = this.ID,
                Name = this.Name,
                JapaneseName = this.JapaneseName,
                Abbreviation = this.Abbreviation,
                YearOfRelease = this.YearOfRelease,
                IsRomHack = this.IsRomHack,
                SpeedRunComUrl = this.WebLink.ToString(),
                CoverImageUrl = this.Assets?.CoverLarge?.Uri.ToString(),
                CreatedDate = this.CreationDate
            };
        }
    }
}
