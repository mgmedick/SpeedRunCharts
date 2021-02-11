using SpeedRunApp.Model.Data;
using SpeedRunCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Helpers;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunViewModel
    {
        public SpeedRunViewModel(SpeedRunView run)
        {
            ID = run.ID;
            Game = new IDNamePair { ID = run.GameID, Name = run.GameName };
            CategoryType = new IDNamePair { ID = run.CategoryTypeID, Name = run.CategoryTypeName };
            Category = new IDNamePair { ID = run.CategoryID, Name = run.CategoryName };
            IsEmulated = run.IsEmulated;
            SplitsLink = run.SplitsUrl;
            RunDate = run.RunDate;
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;
            Comment = run.Comment;

            if (run.LevelID.HasValue)
            {
                Level = new IDNamePair { ID = run.LevelID.Value, Name = run.LevelName };
            }

            if (run.PlatformID.HasValue)
            {
                Platform = new IDNamePair { ID = run.PlatformID.Value, Name = run.PlatformName };
            }

            if (!string.IsNullOrWhiteSpace(run.VariableValues))
            {
                VariableValues = new List<Tuple<string, string>>();
                foreach (var value in run.VariableValues.Split(","))
                {
                    var variableValue = value.Split("|", 2);
                    VariableValues.Add(new Tuple<string, string>(variableValue[0], variableValue[1]));
                }
            }

            if (!string.IsNullOrWhiteSpace(run.SubCategoryVariableValues))
            {
                SubCategoryVariableValues = new List<Tuple<string, string>>();
                foreach (var value in run.SubCategoryVariableValues.Split(","))
                {
                    var variableValue = value.Split("|", 2);
                    SubCategoryVariableValues.Add(new Tuple<string, string>(variableValue[0], variableValue[1]));
                }
            }

            if (!string.IsNullOrWhiteSpace(run.Players))
            {
                Players = new List<IDNamePair>();
                foreach (var player in run.Players.Split("^^"))
                {
                    var playerValue = player.Split("|", 2);
                    Players.Add(new IDNamePair { ID = Convert.ToInt32(playerValue[0]), Name = playerValue[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(run.VideoLinks))
            {
                VideoLinks = new List<string>();
                foreach (var videoLink in run.VideoLinks.Split(","))
                {
                    if (!string.IsNullOrWhiteSpace(videoLink))
                    {
                        VideoLinks.Add(new Uri(videoLink).ToEmbeddedURIString());
                    }
                }
            }

            if (run.PrimaryTime.HasValue)
            {
                PrimaryTime = new TimeSpan(run.PrimaryTime.Value);
            }

            if (run.RealTime.HasValue)
            {
                RealTime = new TimeSpan(run.RealTime.Value);
            }

            if (run.RealTimeWithoutLoads.HasValue)
            {
                RealTimeWithoutLoads = new TimeSpan(run.RealTimeWithoutLoads.Value);
            }

            if (run.GameTime.HasValue)
            {
                GameTime = new TimeSpan(run.GameTime.Value);
            }

            if (run.ExaminerUserID.HasValue)
            {
                Examiner = new IDNamePair { ID = run.ExaminerUserID.Value, Name = run.ExaminerUserName };
            }
        }

        public int ID { get; set; }
        public IDNamePair Game { get; set; }
        public IDNamePair CategoryType { get; set; }
        public IDNamePair Category { get; set; }
        public IDNamePair Level { get; set; }
        public IDNamePair Platform { get; set; }
        public List<Tuple<string, string>> VariableValues { get; set; }
        public List<Tuple<string, string>> SubCategoryVariableValues { get; set; }
        public List<IDNamePair> Players { get; set; }
        public List<string> VideoLinks { get; set; }
        public bool IsEmulated { get; set; }
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public TimeSpan? RealTime { get; set; }
        public TimeSpan? RealTimeWithoutLoads { get; set; }
        public TimeSpan? GameTime { get; set; }
        public string Comment { get; set; }
        public IDNamePair Examiner { get; set; }
        public string SplitsLink { get; set; }
        public DateTime? RunDate { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }

        public IDNamePair Player
        {
            get
            {
                return Players?.FirstOrDefault();
            }
        }

        string _subCategoryVariableValuesString = null;
        public string SubCategoryVariableValuesString
        {
            get
            {
                if (SubCategoryVariableValues != null)
                {
                    _subCategoryVariableValuesString = string.Join(" - ", SubCategoryVariableValues.Select(i => i.Item2));
                }

                return _subCategoryVariableValuesString;
            }
        }

        public string VideoLink
        {
            get
            {
                return VideoLinks?.FirstOrDefault();
            }
        }

        public string VerifyDateString
        {
            get
            {
                return VerifyDate?.ToString("MM/dd/yyyy");
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

        public string RankString
        {
            get
            {
                return Rank?.ToOrdinalString();
            }
        }

        public double PrimaryTimeMilliseconds
        {
            get
            {
                return PrimaryTime.TotalMilliseconds;
            }
        }

        public string PrimaryTimeString
        {
            get
            {
                return PrimaryTime.ToShortString();
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
    }
}
