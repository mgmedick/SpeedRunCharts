using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Model
{
    public class SpeedRunDTO
    {
        public SpeedRunDTO(Run run)
        {
            ID = run.ID;
            DateSubmitted = run.DateSubmitted;
            PlayerID = run.Player?.UserID;
            PlayerName = run.Player?.Name;
            GameID = run.GameID;
            GameName = run.Game.Name;
            GameCoverImageLink = run.Game.Assets?.CoverLarge?.Uri;
            CategoryID = run.CategoryID;
            CategoryName = run.Category.Name;
            LevelID = run.LevelID;
            LevelName = run.Level?.Name;
            PlatformID = run.Platform?.ID;
            PlatformName = run.Platform?.Name;
            Status = new SpeedRunStatusDTO(run.Status);
            VideoLink = run.Videos?.Links.FirstOrDefault();

            if (run.Times.Primary.HasValue)
            {
                PrimaryRunTime = run.Times.Primary.Value;
            }
        }

        public string ID { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string GameID { get; set; }
        public string GameName { get; set; }
        public Uri GameCoverImageLink { get; set; }
        public string LevelID { get; set; }
        public string LevelName { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public SpeedRunStatusDTO Status { get; set; }
        public Uri VideoLink { get; set; }
        public TimeSpan PrimaryRunTime { get; set; }
    }
}
