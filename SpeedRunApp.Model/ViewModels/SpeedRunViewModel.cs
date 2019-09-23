using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeedRunCommon;
using SpeedRunApp.Model;

namespace SpeedRunApp.Model
{
    public class SpeedRunViewModel
    {
        public SpeedRunViewModel(SpeedRunDTO run)
        {
            ID = run.ID;
            PlayerID = run.PlayerID;
            PlayerName = run.PlayerName;
            GameID = run.GameID;
            GameName = run.GameName;
            GameCoverImageLinkString = run.GameCoverImageLink.ToString();
            CategoryID = run.CategoryID;
            CategoryName = run.CategoryName;
            PlatformID = run.PlatformID;
            PlatformName = run.PlatformName;
            LevelID = run.LevelID;
            LevelName = run.LevelName;
            RelativeDateSubmittedString = run.DateSubmitted?.ToRealtiveDateString();
            VideoLinkString = run.VideoLink?.ToString();
            VideoLinkEmbeddedString = run.VideoLink?.ToEmbeddedURIString();
            PrimaryRunTimeString = run.PrimaryRunTime.ToShortString();
        }

        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string GameID { get; set; }
        public string GameName { get; set; }
        public string GameCoverImageLinkString { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string LevelID { get; set; }
        public string LevelName { get; set; }
        public string DateSubmittedString { get; set; }
        public string RelativeDateSubmittedString { get; set; }
        public string VideoLinkString { get; set; }
        public string VideoLinkEmbeddedString { get; set; }
        public string PrimaryRunTimeString { get; set; }
    }
}
