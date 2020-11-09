﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Entity;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRun
    {
        //public SpeedRun()
        //{
        //}

        public string ID { get; set; }
        public Uri WebLink { get; set; }
        public string GameID { get; set; }
        public string LevelID { get; set; }
        public string CategoryID { get; set; }
        public SpeedRunVideos Videos { get; set; }
        public string Comment { get; set; }
        public SpeedRunStatus Status { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public SpeedRunTimes Times { get; set; }
        public SpeedRunSystem System { get; set; }
        public Uri SplitsUri { get; set; }
        public bool SplitsAvailable { get { return SplitsUri != null; } }
        public IEnumerable<VariableValueMapping> VariableValueMappings { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public Player Player { get { return Players.FirstOrDefault(); } }

        //public SpeedRunDTO(Run run)
        //{
        //    ID = run.ID;
        //    DateSubmitted = run.DateSubmitted;
        //    Player = new PlayerDTO(run.Player);
        //    GameID = run.GameID;
        //    CategoryID = run.CategoryID;
        //    LevelID = run.LevelID;
        //    PlatformID = run.Platform?.ID;
        //    Status = new SpeedRunStatusDTO(run.Status);
        //    VideoLink = run.Videos?.Links.FirstOrDefault();

        //    if (run.Times.Primary.HasValue)
        //    {
        //        PrimaryRunTime = run.Times.Primary.Value;
        //    }

        //    //links
        //    _game = run.game;
        //    _category = run.category;
        //    _level = run.level;
        //    _platform = run.System.platform;
        //}

        //public string ID { get; set; }
        //public DateTime? DateSubmitted { get; set; }
        //public string GameID { get; set; }
        //public string LevelID { get; set; }
        //public string CategoryID { get; set; }
        //public string PlatformID { get; set; }
        //public SpeedRunStatusDTO Status { get; set; }
        //public Uri VideoLink { get; set; }
        //public TimeSpan PrimaryRunTime { get; set; }
        //public PlayerDTO Player { get; set; }

        //embeds
        public IEnumerable<User> PlayerUsers { get; set; }
        public User PlayerUser { get { return PlayerUsers?.FirstOrDefault(); } }
        public IEnumerable<Guest> PlayerGuests { get; set; }
        public Guest PlayerGuest { get { return PlayerGuests?.FirstOrDefault(); } }
        public Game Game { get; set; }
        public Category Category { get; set; }
        public Level Level { get; set; }
        public Platform Platform { get { return System.Platform; } }
        public Region Region { get { return System.Region; } }
        public IEnumerable<Variable> Variables { get; set; }
        public IEnumerable<VariableValue> VariableValues { get; set; }

        public SpeedRunEntity ConvertToEntity(DateTime? modifiedDate = null)
        {
            return new SpeedRunEntity
            {
                ID = this.ID,
                StatusTypeID = (int)this.Status.Type,
                GameID = this.GameID,
                CategoryID = this.CategoryID,
                LevelID = this.LevelID,
                PlatformID = this.System.PlatformID,
                RegionID = this.System.RegionID,
                IsEmulated = this.System.IsEmulated,
                PrimaryTime = this.Times.Primary?.Ticks,
                RealTime = this.Times.RealTime?.Ticks,
                RealTimeWithoutLoads = this.Times.RealTimeWithoutLoads?.Ticks,
                GameTime = this.Times.GameTime?.Ticks,
                Comment = this.Comment,
                ExaminerUserID = this.Status.ExaminerUserID,
                RejectReason = this.Status.Reason,
                SpeedRunComUrl = this.WebLink.ToString(),
                SplitsUrl = this.SplitsUri?.ToString(),
                RunDate = this.Date,
                DateSubmitted = this.DateSubmitted,
                VerifyDate = this.VerifyDate,
                ModifiedDate = modifiedDate
            };
        }
    }
}
