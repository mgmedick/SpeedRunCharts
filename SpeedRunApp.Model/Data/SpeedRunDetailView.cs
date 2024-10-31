using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SpeedRunApp.Model.JSON;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunDetailView
    {
        public int ID { get; set; }
        public string Code { get; set; }       
        public int GameID { get; set; }
        public string GameName { get; set; }
        public string GameAbbr { get; set; }
        public string GameCoverImageUrl { get; set; }
        public bool ShowMilliseconds { get; set; }
        public int CategoryTypeID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsTimerAscending { get; set; }
        public bool IsMiscellaneous { get; set; }
        public int? LevelID { get; set; }
        public string LevelName { get; set; }       
        public string SubCategoryVariableValueIDs { get; set; }
        public int? PlatformID { get; set; }  
        public string PlatformName { get; set; }      
        public int? Rank { get; set; }
        public long PrimaryTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string SrcUrl { get; set; }
        public string PlayersJson { get; set; }
        public string VariableValuesJson { get; set; }
        public string VideosJson { get; set; }

        private List<PlayerResult> _players = null;
        public List<PlayerResult> Players
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(PlayersJson))
                {
                    _players = JsonSerializer.Deserialize<List<PlayerResult>>(PlayersJson);
                }

                return _players;
            }
        }           
        private List<VariableValueResult> _variableValues = null;
        public List<VariableValueResult> VariableValues
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(VariableValuesJson))
                {
                    _variableValues = JsonSerializer.Deserialize<List<VariableValueResult>>(VariableValuesJson);
                }

                return _variableValues;
            }
        }

        private List<VideoResult> _videos = null;
        public List<VideoResult> Videos
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(VideosJson))
                {
                    _videos = JsonSerializer.Deserialize<List<VideoResult>>(VideosJson);
                }

                return _videos;
            }
        }           
    }
} 
