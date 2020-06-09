using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class SpeedRunDTO
    {
        public SpeedRunDTO(Run run)
        {
            ID = run.ID;
            DateSubmitted = run.DateSubmitted;
            Player = new PlayerDTO(run.Player);
            GameID = run.GameID;
            CategoryID = run.CategoryID;
            LevelID = run.LevelID;
            PlatformID = run.Platform?.ID;
            Status = new SpeedRunStatusDTO(run.Status);
            VideoLink = run.Videos?.Links.FirstOrDefault();

            if (run.Times.Primary.HasValue)
            {
                PrimaryRunTime = run.Times.Primary.Value;
            }

            //links
            _game = run.game;
            _category = run.category;
            _level = run.level;
            _platform = run.System.platform;
        }

        public string ID { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string GameID { get; set; }
        public string LevelID { get; set; }
        public string CategoryID { get; set; }
        public string PlatformID { get; set; }
        public SpeedRunStatusDTO Status { get; set; }
        public Uri VideoLink { get; set; }
        public TimeSpan PrimaryRunTime { get; set; }
        public PlayerDTO Player { get; set; }

        //links
        private Lazy<Game> _game { get; set; }
        public GameDTO Game { get { return _game.Value != null ? new GameDTO(_game.Value) : null; } }
        private Lazy<Category> _category { get; set; }
        public CategoryDTO Category { get { return _category.Value != null ? new CategoryDTO(_category.Value) : null; } }
        private Lazy<Level> _level { get; set; }
        public LevelDTO Level { get { return _level.Value != null ? new LevelDTO(_level.Value) : null; } }
        private Lazy<Platform> _platform { get; set; }
        public PlatformDTO Platform { get { return _platform.Value != null ? new PlatformDTO(_platform.Value): null; } }
    }
}
