using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;
using System.Net;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunSummaryViewModel
    {
        public SpeedRunSummaryViewModel(SpeedRunSummaryView run)
        {
            ID = run.ID;
            SpeedRunComID = run.SpeedRunComID;
            Game = new IDNameAbbrPair { ID = run.GameID, Name = run.GameName, Abbr = run.GameAbbr };
            GameCoverImageLink = run.GameCoverImageUrl;
            ShowMilliseconds = run.ShowMilliseconds;
            CategoryType = new IDNamePair { ID = run.CategoryTypeID, Name = run.CategoryTypeName };
            Category = new IDNamePair { ID = run.CategoryID, Name = run.CategoryName };
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            ImportedDate = run.ImportedDate;
            Rank = run.Rank;

            if(run.LevelID.HasValue) {
                Level = new IDNamePair { ID = run.LevelID.Value, Name = run.LevelName };                            
            }

            if (!string.IsNullOrWhiteSpace(run.SubCategoryVariableValues))
            {
                SubCategoryVariableValueNames = new List<string>();
                foreach (var value in run.SubCategoryVariableValues.Split("^^"))
                {
                    SubCategoryVariableValueNames.Add(value);
                }
            }

            if (!string.IsNullOrWhiteSpace(run.Players))
            {
                Players = new List<UserNameViewModel>();
                foreach (var player in run.Players.Split("^^"))
                {
                    var playerValue = player.Split("¦", 7);
                    int playerID;
                    int.TryParse(playerValue[0], out playerID);
                    Players.Add(new UserNameViewModel { ID = playerID, Name = playerValue[1], Abbr = playerValue[2], ColorLight = playerValue[3], ColorToLight = playerValue[4], ColorDark = playerValue[5], ColorToDark = playerValue[6] });
                }
            }

            if (!string.IsNullOrWhiteSpace(run.EmbeddedVideoLinks))
            {
                EmbeddedVideoLinks = new List<string>();
                VideoThumbnailLinks = new List<string>();
                ViewCountStrings = new List<string>();

                foreach (var embeddedVideoLink in run.EmbeddedVideoLinks.Split("^^"))
                {
                    var embeddedVideoLinkValue = embeddedVideoLink.Split("|", 3);
                    if (!string.IsNullOrWhiteSpace(embeddedVideoLinkValue[0]))
                    {
                        EmbeddedVideoLinks.Add(embeddedVideoLinkValue[0]);

                        if (!string.IsNullOrWhiteSpace(embeddedVideoLinkValue[1]))
                        {
                            VideoThumbnailLinks.Add(embeddedVideoLinkValue[1]);
                        }

                        int viewCount;
                        if (int.TryParse(embeddedVideoLinkValue[2], out viewCount) && viewCount > 0)
                        {                     
                            ViewCountStrings.Add(viewCount.ToShortString());
                        }                        
                    }                    
                }
            }

            if (run.PrimaryTime.HasValue)
            {
                PrimaryTime = new TimeSpan(run.PrimaryTime.Value);
            }
        }

        public int ID { get; set; }
        public string SpeedRunComID { get; set; }
        public IDNameAbbrPair Game { get; set; }
        public string GameCoverImageLink { get; set; }
        public bool ShowMilliseconds { get; set; }
        public IDNamePair CategoryType { get; set; }
        public IDNamePair Category { get; set; }
        public IDNamePair Level { get; set; } 
        public List<string> SubCategoryVariableValueNames { get; set; }
        public List<UserNameViewModel> Players { get; set; }
        public List<string> EmbeddedVideoLinks { get; set; }
        public List<string> VideoThumbnailLinks { get; set; }    
        public List<string> ViewCountStrings { get; set; }
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public DateTime ImportedDate { get; set; }        
        public string VideoLink
        {
            get
            {
                return EmbeddedVideoLinks?.FirstOrDefault();
            }
        }

        public string VideoLinkAutoplay
        {
            get
            {
                return EmbeddedVideoLinks?.FirstOrDefault()?.Replace("autoplay=false","autoplay=true").Replace("autoplay=0","autoplay=1");
            }
        }

        public string VideoThumbnailLink
        {
            get
            {
                return VideoThumbnailLinks?.FirstOrDefault();
            }
        }

        public bool IsVideoThumbnailLowRes
        {
            get
            {
                return (VideoThumbnailLinks.FirstOrDefault() ?? string.Empty).EndsWith("hqdefault.jpg");
            }
        }          

        public string ViewCountString
        {
            get
            {
                return ViewCountStrings?.FirstOrDefault();
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

        public string RelativeVerifyDateString
        {
            get
            {
                return VerifyDate?.ToRealtiveDateString();
            }
        }

        public string RelativeVerifyDateStringShort
        {
            get
            {
                return VerifyDate?.ToRealtiveDateString(true);
            }
        }

        public string RelativeImportedDateStringShort
        {
            get
            {
                return ImportedDate.ToRealtiveDateString(true);
            }
        }                
    }
}
