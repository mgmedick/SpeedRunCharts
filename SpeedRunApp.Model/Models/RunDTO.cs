﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Model
{
    public class RunDTO
    {
        public RunDTO(Run run)
        {
            ID = run.ID;
            WebLink = run.WebLink;
            DateSubmitted = run.DateSubmitted;
            GameID = run.GameID;
            GameName = run.Game.Name;
            LevelID = run.LevelID;
            LevelName = run.Level?.Name;
            CategoryID = run.CategoryID;
            CategoryName = run.Category.Name;
            PlayerID = run.Player.UserID;
            PlayerName = run.Player.Name;
            VideoLink = run.Videos?.Links.FirstOrDefault();

            if (run.Times.Primary.HasValue)
            {
                PrimaryRunTime = run.Times.Primary.Value;
            }
        }

        public string ID { get; set; }
        public Uri WebLink { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string GameID { get; set; }
        public string GameName { get; set; }
        public string LevelID { get; set; }
        public string LevelName { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public Uri VideoLink { get; set; }
        public TimeSpan PrimaryRunTime { get; set; }
    }
}
