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
            GameCoverImageLink = run.GameCoverImageLink;
            CategoryID = run.CategoryID;
            CategoryName = run.CategoryName;
            PlatformID = run.PlatformID;
            PlatformName = run.PlatformName;
            LevelID = run.LevelID;
            LevelName = run.LevelName;
            ExaminerName = run.Status.Examiner?.Name;
            DateSubmitted = run.DateSubmitted;
            VideoLink = run.VideoLink;
            PrimaryRunTime = run.PrimaryRunTime;
        }

        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string GameID { get; set; }
        public string GameName { get; set; }
        public Uri GameCoverImageLink { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
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

        string _primaryRunTimeString;
        public string PrimaryRunTimeString
        {
            get
            {
                if (PrimaryRunTime.Days > 0)
                {
                    _primaryRunTimeString = PrimaryRunTime.ToString(@"dd\hh\:mm\:ss");
                }
                else if (PrimaryRunTime.Hours > 0)
                {
                    _primaryRunTimeString = PrimaryRunTime.ToString(@"hh\:mm\:ss");
                }
                else
                {
                    _primaryRunTimeString = PrimaryRunTime.ToString(@"mm\:ss");
                }

                return _primaryRunTimeString;
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
