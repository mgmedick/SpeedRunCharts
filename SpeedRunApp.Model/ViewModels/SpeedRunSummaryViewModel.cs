using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;
using System.Net;
using System.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunSummaryViewModel
    {
        public SpeedRunSummaryViewModel(SpeedRunSummaryView run)
        {
            ID = run.ID;
            Code = run.Code;
            SortOrder = run.SortOrder;
            GameName = run.GameName;
            GameAbbr = run.GameAbbr;
            GameCoverImageLink = run.GameCoverImageUrl;
            CategoryTypeName = run.CategoryTypeName;
            CategoryName = run.CategoryName;
            LevelName = run.LevelName;
            SubCategoryVariableValueNames = run.SubCategoryVariableValueNames;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;
            PrimaryTime = TimeSpan.FromMilliseconds(run.PrimaryTime);
            ShowMilliseconds = run.ShowMilliseconds;
            Players = run.Players;
            VideoLinks = run.Videos;
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public int SortOrder { get; set; }
        public string GameName { get; set; }
        public string GameAbbr { get; set; }
        public string GameCoverImageLink { get; set; }
        public string CategoryTypeName { get; set; }    
        public string CategoryName { get; set; }    
        public string LevelName { get; set; }
        public List<string> SubCategoryVariableValueNames { get; set; }
        public List<PlayerResult> Players { get; set; }
        public List<VideoResult> VideoLinks { get; set; }
        public bool ShowMilliseconds { get; set; }
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public DateTime? VerifyDate { get; set; }

        public string VideoLink
        {
            get
            {
                return VideoLinks?.Select(x => x.EmbeddedVideoLinkUrl).FirstOrDefault();
            }
        }

        public string VideoThumbnailLink
        {
            get
            {
                return VideoLinks?.Select(x => x.ThumbnailLinkUrl).FirstOrDefault();
            }
        }

        public string VideoLinkAutoplay
        {
            get
            {
                return VideoLink?.Replace("autoplay=false","autoplay=true").Replace("autoplay=0","autoplay=1");
            }
        }

        public bool IsVideoThumbnailLowRes
        {
            get
            {
                return (VideoThumbnailLink ?? string.Empty).EndsWith("hqdefault.jpg");
            }
        }          

        public string RankString
        {
            get
            {
                return Rank?.ToOrdinalString();
            }
        }

        public string PrimaryTimeString
        {
            get
            {
                return PrimaryTime.ToShortString(!ShowMilliseconds);
            }
        }

        public string RelativeVerifyDateString
        {
            get
            {
                return VerifyDate?.ToRealtiveDateString(true);
            }
        }             
    }
}
