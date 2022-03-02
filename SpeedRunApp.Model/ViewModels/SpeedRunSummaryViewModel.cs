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
            Game = new IDNameAbbrPair { ID = run.GameID, Name = run.GameName, Abbr = run.GameAbbr };
            GameCoverImageLink = run.GameCoverImageUrl;
            CategoryType = new IDNamePair { ID = run.CategoryTypeID, Name = run.CategoryTypeName };
            Category = new IDNamePair { ID = run.CategoryID, Name = run.CategoryName };
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;

            if(run.LevelID.HasValue) {
                Level = new IDNamePair { ID = run.LevelID.Value, Name = run.LevelName };                            
            }

            if (!string.IsNullOrWhiteSpace(run.SubCategoryVariableValues))
            {
                SubCategoryVariableValues = new List<Tuple<string, string>>();
                SubCategoryVariableValueNames = new List<string>();
                foreach (var value in run.SubCategoryVariableValues.Split("^^"))
                {
                    var variableValue = value.Split("¦", 3);
                    SubCategoryVariableValues.Add(new Tuple<string, string>(variableValue[0], variableValue[1]));
                    SubCategoryVariableValueNames.Add(variableValue[2]);
                }
            }

            if (!string.IsNullOrWhiteSpace(run.Players))
            {
                Players = new List<IDNameAbbrPair>();
                foreach (var player in run.Players.Split("^^"))
                {
                    var playerValue = player.Split("¦", 3);
                    int playerID;
                    int.TryParse(playerValue[0], out playerID);
                    Players.Add(new IDNameAbbrPair { ID = playerID, Name = playerValue[1], Abbr = playerValue[2] });
                }
            }

            if (!string.IsNullOrWhiteSpace(run.EmbeddedVideoLinks))
            {
                EmbeddedVideoLinks = new List<string>();
                VideoThumbnailLinks = new List<string>();
                foreach (var embeddedVideoLink in run.EmbeddedVideoLinks.Split(","))
                {
                    var embeddedVideoLinkValue = embeddedVideoLink.Split("|", 2);
                    if (!string.IsNullOrWhiteSpace(embeddedVideoLinkValue[0]))
                    {
                        EmbeddedVideoLinks.Add(embeddedVideoLinkValue[0].Replace("&amp;", "&"));

                        if (!string.IsNullOrWhiteSpace(embeddedVideoLinkValue[1]))
                        {
                            VideoThumbnailLinks.Add(embeddedVideoLinkValue[1].Replace("&amp;", "&"));
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
        public IDNameAbbrPair Game { get; set; }
        public string GameCoverImageLink { get; set; }
        public IDNamePair CategoryType { get; set; }
        public IDNamePair Category { get; set; }
        public IDNamePair Level { get; set; } 
        public List<Tuple<string, string>> SubCategoryVariableValues { get; set; }
        public List<string> SubCategoryVariableValueNames { get; set; }
        public List<IDNameAbbrPair> Players { get; set; }
        //public List<string> VideoLinks { get; set; }
        public List<string> EmbeddedVideoLinks { get; set; }
        public List<string> VideoThumbnailLinks { get; set; }        
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }

        string _subCategoryVariableValuesString = null;
        public string SubCategoryVariableValuesString
        {
            get
            {
                if (SubCategoryVariableValueNames != null)
                {
                    _subCategoryVariableValuesString = string.Join(" - ", SubCategoryVariableValueNames);
                }

                return _subCategoryVariableValuesString;
            }
        }

        // public List<string> EmbeddedVideoLinks { 
        //     get
        //     {
        //         return VideoLinks?.Where(i => !string.IsNullOrWhiteSpace(i)).Select(i=>new Uri(i).ToEmbeddedURIString()).ToList();
        //     } 
        // }

        public string VideoLink
        {
            get
            {
                return EmbeddedVideoLinks?.FirstOrDefault();
            }
        }

        public string VideoThumbnailLink
        {
            get
            {
                return VideoThumbnailLinks?.FirstOrDefault();
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
                return PrimaryTime.ToShortString();
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
    }
}
