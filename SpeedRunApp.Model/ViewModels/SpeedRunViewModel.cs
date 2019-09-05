using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeedRunApp.Common;
using SpeedRunApp.Model;

namespace SpeedRunApp.Model
{
    public class SpeedRunViewModel
    {
        public SpeedRunViewModel(RunDTO run)
        {
            ID = run.ID;
            PlayerID = run.PlayerID;
            PlayerName = run.PlayerName;
            GameID = run.GameID;
            GameName = run.GameName;
            GameCoverImageUri = run.GameCoverImageUri;
            CategoryID = run.CategoryID;
            CategoryName = run.CategoryName;
            RealtiveSubmittedDateString = run.DateSubmitted?.ToRealtiveDateString();
            VideoLink = run.VideoLink?.ToString();
            VideoLinkEmbedded = run.VideoLink?.ToEmbeddedURIString();
            PrimaryRunTimeString = run.PrimaryRunTime.ToString("h'h 'm'm 's's'");
        }

        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string GameID { get; set; }
        public string GameName { get; set; }
        public Uri GameCoverImageUri { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string RealtiveSubmittedDateString { get; set; }
        public string VideoLink { get; set; }
        public string VideoLinkEmbedded { get; set; }
        public string PrimaryRunTimeString { get; set; }
    }
}
