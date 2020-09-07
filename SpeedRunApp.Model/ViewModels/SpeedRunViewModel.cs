using SpeedRunApp.Model.Data;
using SpeedRunCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunViewModel
    {
        public SpeedRunViewModel(SpeedRun run)
        {
            ID = run.ID;
            PlayerUsers = run.PlayerUsers;
            PlayerGuests = run.PlayerGuests;
            IsEmulated = run.System.IsEmulated;
            DateSubmitted = run.DateSubmitted;
            VideoLinkEmbeded = run.Videos.EmbededLinks?.FirstOrDefault(i => i != null);
            PrimaryTime = run.Times.Primary.Value;
            RealTime = run.Times.RealTime;
            RealTimeWithoutLoads = run.Times.RealTimeWithoutLoads;
            GameTime = run.Times.GameTime;
            StatusType = new IDNamePair { ID = ((int)run.Status.Type).ToString(), Name = run.Status.Type.ToString() };
            VerifyDate = run.Status.VerifyDate;
            RejectedReason = run.Status.Reason;
            Comment = run.Comment;
            SplitsLink = run.SplitsUri;
            ExaminerUserID = run.Status.ExaminerUserID;

            if (run.PlayerUser != null)
            {
                Player = new IDNamePair { ID = run.PlayerUser.ID, Name = run.PlayerUser.Name };
            }
            if (run.Game != null)
            {
                Game = new IDNamePair { ID = run.GameID, Name = run.Game.Name };
                GameCoverImageLink = run.Game.Assets?.CoverLarge?.Uri;
            }
            if (run.Category != null)
            {
                Category = new IDNamePair { ID = run.CategoryID, Name = run.Category.Name };
                CategoryType = new IDNamePair { ID = ((int)run.Category?.Type).ToString(), Name = run.Category?.Type.ToString() };
            }
            if (run.Platform != null)
            {
                Platform = new IDNamePair { ID = run.Platform.ID, Name = run.Platform.Name };
            }
            if (run.Level != null)
            {
                Level = new IDNamePair { ID = run.Level.ID, Name = run.Level.Name };
            }
            if (run.Variables != null)
            {
                Variables = run.Variables.Where(i => i.IsSubCategory).Select(i => new VariableDisplay { ID = i.ID, Name = i.Name, GameID = this.Game.ID, CategoryID = this.Category.ID, VariableValues = i.Values.Select(g => new VariableValueDisplay { ID = g.ID, Name = g.Value }) });
            }
        }

        public string ID { get; set; }
        public IDNamePair Player { get; set; }
        public IEnumerable<User> PlayerUsers { get; set; }
        public IEnumerable<Guest> PlayerGuests { get; set; }
        public IDNamePair Game { get; set; }
        public Uri GameCoverImageLink { get; set; }
        public IDNamePair Category { get; set; }
        public IDNamePair CategoryType { get; set; }
        public IDNamePair Platform { get; set; }
        public IDNamePair Level { get; set; }
        public bool IsEmulated { get; set; }
        public string ModeratorName { get; set; }
        public string ExaminerUserID { get; set; }
        public string ExaminerName { get; set; }
        public IDNamePair StatusType { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string RejectedReason { get; set; }
        public string Comment { get; set; }
        public Uri VideoLinkEmbeded { get; set; }
        public Uri SplitsLink { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public TimeSpan? RealTime { get; set; }
        public TimeSpan? RealTimeWithoutLoads { get; set; }
        public TimeSpan? GameTime { get; set; }
        public IEnumerable<VariableDisplay> Variables { get; set; }
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
            }
        }

        public string SplitsLinkString
        {
            get
            {
                return SplitsLink?.AbsoluteUri;
            }
        }

        public string PrimaryTimeString
        {
            get
            {
                return PrimaryTime.ToShortString();
            }
        }

        public double PrimaryTimeSeconds
        {
            get
            {
                return PrimaryTime.TotalSeconds;
            }
        }

        public double PrimaryTimeMilliseconds
        {
            get
            {
                return PrimaryTime.TotalMilliseconds;
            }
        }

        public string RealTimeString
        {
            get
            {
                return RealTime?.ToShortString();
            }
        }

        public string RealTimeWithoutLoadsString
        {
            get
            {
                return RealTimeWithoutLoads?.ToShortString();
            }
        }

        public string GameTimeString
        {
            get
            {
                return GameTime?.ToShortString();
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
