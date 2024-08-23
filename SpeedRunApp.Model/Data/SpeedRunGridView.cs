using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SpeedRunApp.Model.JSON;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunGridView
    {
        public int ID { get; set; }
        public string Code { get; set; }       
        public int GameID { get; set; }
        public int CategoryTypeID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsTimerAscending { get; set; }
        public bool IsMiscellaneous { get; set; }
        public int? LevelID { get; set; }
        public string LevelName { get; set; }       
        public string SubCategoryVariableValueIDs { get; set; }
        public string SubCategoryVariableValues { get; set; }
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
        public bool IsPersonalBest { get; set; }        

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
        private Dictionary<int, int> _variableValues = null;
        public Dictionary<int, int> VariableValues
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(VariableValuesJson))
                {
                    _variableValues = JsonSerializer.Deserialize<Dictionary<int, int>>(VariableValuesJson);
                }

                return _variableValues;
            }
        }         
        private List<string> _videos = null;
        public List<string> Videos
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(VideosJson))
                {
                    _videos = JsonSerializer.Deserialize<List<string>>(VideosJson);
                }

                return _videos;
            }
        }        
    }
} 
