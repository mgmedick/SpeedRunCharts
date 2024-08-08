using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class PlayerDetailsViewModel
    {
        public PlayerDetailsViewModel(PlayerView playerVW, PlayerSpeedRunCountResult playerRunCounts, string speedRunCode)
        {
            ID = playerVW.ID;
            Name = playerVW.Name;
            Code = playerVW.Code;
            SrcUrl = playerVW.SrcUrl;
            TwitchUrl = playerVW.TwitchUrl;
            HitboxUrl = playerVW.HitboxUrl;
            YoutubeUrl = playerVW.YoutubeUrl;
            TwitchUrl = playerVW.TwitchUrl;
            SpeedRunCode = speedRunCode;
            TotalSpeedRuns = playerRunCounts.TotalSpeedRuns;
            TotalWorldRecords = playerRunCounts.TotalWorldRecords;
            TotalPersonalBests = playerRunCounts.TotalPersonalBests;
        }
                
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }    
        public string SrcUrl { get; set; }
        public string TwitchUrl { get; set; }
        public string HitboxUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string SpeedRunCode { get; set; }
        public int TotalSpeedRuns { get; set; }
        public int TotalWorldRecords { get; set; }
        public int TotalPersonalBests { get; set; }        
    }
}


