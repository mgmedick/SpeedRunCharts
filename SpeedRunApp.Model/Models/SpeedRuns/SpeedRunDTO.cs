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
        public SpeedRunDTO()
        {
        }

        public string ID { get; set; }
        public Uri WebLink { get; set; }
        public string GameID { get; set; }
        public string LevelID { get; set; }
        public string CategoryID { get; set; }
        public SpeedRunVideosDTO Videos { get; set; }
        public string Comment { get; set; }
        public SpeedRunStatusDTO Status { get; set; }
        public Player Player { get { return Players.FirstOrDefault(); } }
        public IEnumerable<Player> Players { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public SpeedRunTimesDTO Times { get; set; }
        public SpeedRunSystemDTO System { get; set; }
        public Uri SplitsUri { get; set; }
        public bool SplitsAvailable { get { return SplitsUri != null; } }
        public IEnumerable<VariableValue> VariableValues { get; set; }

        //public SpeedRunDTO(Run run)
        //{
        //    ID = run.ID;
        //    DateSubmitted = run.DateSubmitted;
        //    Player = new PlayerDTO(run.Player);
        //    GameID = run.GameID;
        //    CategoryID = run.CategoryID;
        //    LevelID = run.LevelID;
        //    PlatformID = run.Platform?.ID;
        //    Status = new SpeedRunStatusDTO(run.Status);
        //    VideoLink = run.Videos?.Links.FirstOrDefault();

        //    if (run.Times.Primary.HasValue)
        //    {
        //        PrimaryRunTime = run.Times.Primary.Value;
        //    }

        //    //links
        //    _game = run.game;
        //    _category = run.category;
        //    _level = run.level;
        //    _platform = run.System.platform;
        //}

        //public string ID { get; set; }
        //public DateTime? DateSubmitted { get; set; }
        //public string GameID { get; set; }
        //public string LevelID { get; set; }
        //public string CategoryID { get; set; }
        //public string PlatformID { get; set; }
        //public SpeedRunStatusDTO Status { get; set; }
        //public Uri VideoLink { get; set; }
        //public TimeSpan PrimaryRunTime { get; set; }
        //public PlayerDTO Player { get; set; }

        //embeds
        public GameDTO Game { get; set; }
        public CategoryDTO Category { get; set; }
        public LevelDTO Level { get; set; }
        public PlatformDTO Platform { get; set; }
    }
}
