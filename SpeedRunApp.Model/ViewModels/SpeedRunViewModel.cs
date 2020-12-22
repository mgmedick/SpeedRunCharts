﻿using SpeedRunApp.Model.Data;
using SpeedRunCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunViewModel
    {
        public SpeedRunViewModel(SpeedRunView run)
        {
            OrderValue = run.OrderValue;
            ID = run.ID;
            StatusType = new IDNamePair { ID = run.StatusTypeID.ToString(), Name = run.StatusTypeName.ToString() };
            Game = new IDNamePair { ID = run.GameID, Name = run.GameName };
            GameCoverImageLink = run.GameCoverImageUrl;
            CategoryType = new IDNamePair { ID = run.CategoryTypeID.ToString(), Name = run.CategoryTypeName };
            Category = new IDNamePair { ID = run.CategoryID, Name = run.CategoryName };
            IsEmulated = run.IsEmulated;
            SplitsLink = run.SplitsUrl;
            RunDate = run.RunDate;
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;

            if (!string.IsNullOrWhiteSpace(run.LevelID))
            {
                Level = new IDNamePair { ID = run.LevelID, Name = run.LevelName };
            }

            if (!string.IsNullOrWhiteSpace(run.PlatformID))
            {
                Platform = new IDNamePair { ID = run.PlatformID, Name = run.PlatformName };
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
                    Players.Add(new IDNamePair { ID = playerValue[0], Name = playerValue[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(run.VideoLinks))
            {
                VideoLinks = new List<string>();
                foreach (var videoLink in run.VideoLinks.Split(","))
                {
                    VideoLinks.Add(videoLink);
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

            if (!string.IsNullOrWhiteSpace(run.ExaminerUserID))
            {
                Examiner = new IDNamePair { ID = run.ExaminerUserID, Name = run.ExaminerUserName };
            }
        }

        public int OrderValue { get; set; }
        public string ID { get; set; }
        public IDNamePair StatusType { get; set; }
        public IDNamePair Game { get; set; }
        public string GameCoverImageLink { get; set; }
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
        public string RejectReason { get; set; }
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
