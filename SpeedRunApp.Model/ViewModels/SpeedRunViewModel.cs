using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeedRunCommon;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunViewModel
    {
        public SpeedRunViewModel(SpeedRun run)
        {
            ID = run.ID;
            PlayerID = run.PlayerUser?.ID;
            PlayerName = run.PlayerUser?.Name;
            GameID = run.GameID;
            GameName = run.Game.Name;
            GameCoverImageLink = run.Game.Assets?.CoverLarge?.Uri;
            CategoryID = run.CategoryID;
            CategoryName = run.Category.Name;
            CategoryType = run.Category.Type;
            PlatformID = run.Platform?.ID;
            PlatformName = run.Platform?.Name;
            LevelID = run.LevelID;
            LevelName = run.Level?.Name;
            DateSubmitted = run.DateSubmitted;
            VideoLink = run.Videos?.Links?.FirstOrDefault(i => i != null);
            PrimaryRunTime = run.Times.Primary.Value;
        }

        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string GameID { get; set; }
        public string GameName { get; set; }
        public Uri GameCoverImageLink { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public CategoryType CategoryType { get; set; }
        public string PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string LevelID { get; set; }
        public string LevelName { get; set; }
        public string ExaminerName { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public Uri VideoLink { get; set; }
        public TimeSpan PrimaryRunTime { get; set; }
        public string GameCoverImageLinkString
        {
            get
            {
                return GameCoverImageLink?.AbsoluteUri;
            }
        }

        public string DateSubmittedString
        {
            get
            {
                return DateSubmitted?.ToString("MM/dd/yyyy");
            }
        }

        public string MonthYearSubmitted
        {
            get
            {
                return DateSubmitted?.ToString("MM/yyyy");
            }
        }

        public string RelativeDateSubmittedString
        {
            get
            {
                return DateSubmitted?.ToRealtiveDateString();
            }
        }

        public string VideoLinkString
        {
            get
            {
                return VideoLink?.AbsoluteUri;
            }
        }

        public string VideoLinkEmbeddedString
        {
            get
            {
                return VideoLink?.ToEmbeddedURIString();
            }
        }

        public string PrimaryRunTimeString
        {
            get
            {
                return PrimaryRunTime.ToShortString();
            }
        }

        public double PrimaryRunTimeMinutes
        {
            get
            {
                return PrimaryRunTime.TotalMinutes;
            }
        }

        public double PrimaryRunTimeSeconds
        {
            get
            {
                return PrimaryRunTime.TotalSeconds;
            }
        }
    }
}
