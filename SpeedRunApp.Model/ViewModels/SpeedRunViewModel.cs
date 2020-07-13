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
            PlayerName = run.PlayerUser != null ? run.PlayerUser.Name : run.PlayerGuest?.Name;
            PlayerUsers = run.PlayerUsers;
            PlayerGuests = run.PlayerGuests;
            GameID = run.GameID;
            GameName = run.Game?.Name;
            GameCoverImageLink = run.Game?.Assets?.CoverLarge?.Uri;
            CategoryID = run.CategoryID;
            CategoryName = run.Category?.Name;
            PlatformID = run.Platform?.ID;
            PlatformName = run.Platform?.Name;
            LevelID = run.LevelID;
            LevelName = run.Level?.Name;
            IsEmulated = run.System.IsEmulated;
            DateSubmitted = run.DateSubmitted;
            VideoLinkEmbeded = run.Videos?.EmbededLinks?.FirstOrDefault(i => i != null);
            PrimaryRunTime = run.Times.Primary.Value;
            StatusType = new IDNamePair { ID = ((int)run.Status.Type).ToString(), Name = run.Status.Type.ToString() };
            VerifyDate = run.Status.VerifyDate;
            RejectedReason = run.Status.Reason;
            Comment = run.Comment;

            if (run.Category != null)
            {
                CategoryType = new IDNamePair { ID = ((int)run.Category.Type).ToString(), Name = run.Category.Type.ToString() };
            }
        }

        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public IEnumerable<User> PlayerUsers { get; set; }
        public IEnumerable<Guest> PlayerGuests { get; set; }
        public string GameID { get; set; }
        public string GameName { get; set; }
        public Uri GameCoverImageLink { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public IDNamePair CategoryType { get; set; }
        public string PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string LevelID { get; set; }
        public string LevelName { get; set; }
        public bool IsEmulated { get; set; }
        public string ModeratorName { get; set; }
        public string ExaminerName { get; set; }
        public IDNamePair StatusType { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string RejectedReason { get; set; }
        public string Comment { get; set; }
        public Uri VideoLinkEmbeded { get; set; }
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

        public string VideoLinkEmbeddedString
        {
            get
            {
                return VideoLinkEmbeded?.AbsoluteUri;
                //VideoLinkEmbeded?.AbsoluteUri;
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

        public string StatusTypeString
        {
            get
            {
                return StatusType.Name;
            }
        }

        public string RelativeVerifyDateString
        {
            get
            {
                return VerifyDate?.ToRealtiveDateString();
            }
        }
    }
}
