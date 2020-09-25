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
            StatusType = new IDNamePair { ID = ((int)run.Status.Type).ToString(), Name = run.Status.Type.ToString() };
            IsEmulated = run.System.IsEmulated;
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.Status.VerifyDate;
            VideoLinkEmbeded = run.Videos.EmbededLinks?.FirstOrDefault(i => i != null);
            Comment = run.Comment;
            RejectedReason = run.Status.Reason;
            SplitsLink = run.SplitsUri;
            PlayerUsers = run.PlayerUsers;
            PlayerGuests = run.PlayerGuests;

            //Times
            PrimaryTime = run.Times.Primary.Value;
            RealTime = run.Times.RealTime;
            RealTimeWithoutLoads = run.Times.RealTimeWithoutLoads;
            GameTime = run.Times.GameTime;

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

            //if (run.Variables != null)
            //{
            //    SubCategoryVariables = run.Variables.Where(i => i.IsSubCategory).Select(i => new VariableDisplay { ID = i.ID, Name = i.Name, GameID = this.Game.ID, CategoryID = this.Category.ID, VariableValues = i.Values.Select(g => new VariableValueDisplay { ID = g.ID, Name = g.Value }) });
            //}

            if (run.VariableValues != null)
            {
                SubCategoryVariableValues = run.VariableValues.Where(i => i.Variable.IsSubCategory).Select(i => new VariableValueDisplay { ID = i.ID, Name = i.Value, Variable = new VariableDisplay { ID = i.Variable.ID, Name = i.Variable.Name, GameID = this.Game.ID, CategoryID = this.Category.ID, LevelID = this.Level?.ID, ScopeTypeID = ((int)i.Variable.Scope.Type).ToString() } });
                VariableValues = run.VariableValues.Where(i => !i.Variable.IsSubCategory).Select(i => new VariableValueDisplay { ID = i.ID, Name = i.Value, Variable = new VariableDisplay { ID = i.Variable.ID, Name = i.Variable.Name, GameID = this.Game.ID, CategoryID = this.Category.ID } });
            }

            if (run.Status.Examiner != null)
            {
                Examiner = new IDNamePair { ID = run.Status.Examiner.ID, Name = run.Status.Examiner.Name };
            }
        }

        public string ID { get; set; }
        public IDNamePair Player { get; set; }
        public IDNamePair CategoryType { get; set; }
        public IDNamePair Game { get; set; }
        public IDNamePair Category { get; set; }
        public IDNamePair Level { get; set; }
        public IDNamePair Platform { get; set; }
        public IEnumerable<User> PlayerUsers { get; set; }
        public IEnumerable<Guest> PlayerGuests { get; set; }
        public Uri GameCoverImageLink { get; set; }
        public bool IsEmulated { get; set; }
        public IDNamePair Examiner { get; set; }
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
        public IEnumerable<VariableDisplay> SubCategoryVariables { get; set; }
        public IEnumerable<VariableValueDisplay> SubCategoryVariableValues { get; set; }
        public IEnumerable<VariableValueDisplay> VariableValues { get; set; }
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

        public string IsEmulatedString
        {
            get
            {
                return IsEmulated.ToString();
            }
        }
    }
}
