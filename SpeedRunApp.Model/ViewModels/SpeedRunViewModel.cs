﻿using SpeedRunApp.Model.Data;
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
            PlayerID = run.PlayerUser?.ID;
            PlayerName = run.PlayerUser != null ? run.PlayerUser.Name : run.PlayerGuest?.Name;
            PlayerUsers = run.PlayerUsers;
            PlayerGuests = run.PlayerGuests;
            GameID = run.GameID;
            GameName = run.Game?.Name;
            GameCoverImageLink = run.Game?.Assets?.CoverLarge?.Uri;
            CategoryID = run.CategoryID;
            CategoryName = run.Category?.Name;
            PlatformID = run.System.PlatformID;
            PlatformName = run.System.Platform?.Name;
            LevelID = run.LevelID;
            LevelName = run.Level?.Name;
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
            Variables = run.Variables?.Where(i => i.IsSubCategory).Select(i => new VariableDisplay { ID = i.ID, Name = i.Name, GameID = this.GameID, CategoryID = this.CategoryID, VariableValues = i.Values.Select(g => new VariableValueDisplay { ID = g.ID, Name = g.Value }) });

            if (run.Category != null)
            {
                CategoryType = new IDNamePair { ID = ((int)run.Category?.Type).ToString(), Name = run.Category?.Type.ToString() };
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
