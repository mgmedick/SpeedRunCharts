using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;
using SpeedRunApp.Model.JSON;
using Org.BouncyCastle.Asn1.Icao;
using System.Linq.Expressions;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunDetailsViewModel
    {
        public SpeedRunDetailsViewModel()
        {
        }
        
        public SpeedRunDetailsViewModel(SpeedRunDetailView run)
        {
            ID = run.ID;
            Code = run.Code;
            GameID = run.GameID;
            GameName = run.GameName;
            GameAbbr = run.GameAbbr;
            GameCoverImageLink = run.GameCoverImageUrl;
            CategoryTypeID = run.CategoryTypeID;
            CategoryID = run.CategoryID;
            CategoryName = run.CategoryName;
            LevelID = run.LevelID;
            LevelName = run.LevelName;
            SubCategoryVariableValueIDs = run.SubCategoryVariableValueIDs;
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;  
            PrimaryTime = TimeSpan.FromMilliseconds(run.PrimaryTime); 
            ShowMilliseconds = run.ShowMilliseconds;      
            VariableValues = run.VariableValues;
            VideoLinks = run.Videos;
            Players = run.Players;
            PlatformName = run.PlatformName;
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public int GameID { get; set; }
        public string GameName { get; set; }
        public string GameAbbr { get; set; }
        public string GameCoverImageLink { get; set; }
        public int CategoryTypeID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsTimerAscending { get; set; }
        public bool IsMiscellaneous { get; set; }        
        public int? LevelID { get; set; }
        public string LevelName { get; set; }
        public string PlatformName { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
        public List<VariableValueResult> VariableValues { get; set; }
        public List<PlayerResult> Players { get; set; }
        public List<VideoResult> VideoLinks { get; set; }
        public bool ShowMilliseconds { get; set; }
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public string Comment { get; set; }
        public string SpeedRunComLink { get; set; }
        public string SplitsLink { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public long? ViewCount { get; set; }


         public List<string> SubCategoryVariableValueNames
        {
            get
            {
                return VariableValues?.Select(x => x.Name).ToList();
            }
        }

        public string EmbeddedVideoLink
        {
            get
            {
                return VideoLinks?.Select(x => x.EmbeddedVideoLinkUrl).FirstOrDefault();
            }
        }

        public string EmbeddedVideoLinkAutoplay
        {
            get
            {
                var result = EmbeddedVideoLink;

                if (!string.IsNullOrWhiteSpace(EmbeddedVideoLink)) {
                    result = new Uri(EmbeddedVideoLink).ToParameterizedURI(true, true, true).ToString();
                }

                return result;
            }
        }

        public string ViewCountString
        {
            get
            {
                var viewCount = VideoLinks?.Select(x => x.ViewCount).FirstOrDefault();
                return viewCount > 0 ? viewCount.Value.ToShortString() : string.Empty;
            }
        }                
        
        public string RelativeVerifyDateString
        {
            get
            {
                return VerifyDate?.ToRealtiveDateString(true);
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

        public double PrimaryTimeSeconds
        {
            get
            {
                return PrimaryTime.TotalSeconds;
            }
        }

        public string PrimaryTimeMillisecondsString
        {
            get
            {
                return PrimaryTime.ToShortString();
            }
        }

        public string PrimaryTimeSecondsString
        {
            get
            {
                return PrimaryTime.ToShortString(true);
            }
        }        

        public string PrimaryTimeString
        {
            get
            {
                return PrimaryTime.ToShortString(!ShowMilliseconds);
            }
        }        

        public string RelativeDateSubmittedString
        {
            get
            {
                return DateSubmitted?.ToRealtiveDateString();
            }
        }
                
        public string RelativeDateSubmittedStringShort
        {
            get
            {
                return DateSubmitted?.ToRealtiveDateString(true);
            }
        }                    
    }
}
