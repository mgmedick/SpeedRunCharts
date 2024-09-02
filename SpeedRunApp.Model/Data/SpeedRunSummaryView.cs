using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SpeedRunApp.Model.JSON;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunSummaryView
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int SortOrder { get; set; }       
        public int GameID { get; set; }
        public string GameName { get; set; }
        public string GameAbbr { get; set; }
        public string GameCoverImageUrl { get; set; }
        public bool ShowMilliseconds { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryTypeName { get; set; }            
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }        
        public int? LevelID { get; set; }
        public string LevelName { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
        public int? Rank { get; set; }
        public long PrimaryTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string SubCategoryVariableValueNamesJson { get; set; }       
        public string PlayersJson { get; set; }
        public string VideosJson { get; set; }

        private List<string> _subCategoryVariableValueNames = null;
        public List<string> SubCategoryVariableValueNames
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(SubCategoryVariableValueNamesJson))
                {
                    _subCategoryVariableValueNames = JsonSerializer.Deserialize<List<string>>(SubCategoryVariableValueNamesJson);
                }

                return _subCategoryVariableValueNames;
            }
        }  

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
