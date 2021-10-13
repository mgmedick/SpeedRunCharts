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
            Game = new IDNamePair { ID = run.GameID, Name = run.GameName };
            GameCoverImageLink = run.GameCoverImageUrl;
            Category = new IDNamePair { ID = run.CategoryID, Name = run.CategoryName };
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;

            if (!string.IsNullOrWhiteSpace(run.SubCategoryVariableValues))
            {
                SubCategoryVariableValues = new List<Tuple<string, string>>();
                foreach (var value in run.SubCategoryVariableValues.Split(","))
                {
                    var variableValue = value.Split("|", 2);
                    SubCategoryVariableValues.Add(new Tuple<string, string>(variableValue[0], variableValue[1]));
                }
            }

            if (!string.IsNullOrWhiteSpace(run.SubCategoryVariableValueNames))
            {
                SubCategoryVariableValueNames = run.SubCategoryVariableValueNames.Split(",").ToList();
            }

            if (!string.IsNullOrWhiteSpace(run.Players))
            {
                Players = new List<IDNamePair>();
                foreach (var player in run.Players.Split("^^"))
                {
                    var playerValue = player.Split("|", 2);
                    int playerID;
                    int.TryParse(playerValue[0], out playerID);
                    Players.Add(new IDNamePair { ID = playerID, Name = playerValue[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(run.VideoLinks))
            {
                VideoLinks = new List<string>();
                foreach (var videoLink in run.VideoLinks.Split(","))
                {
                    VideoLinks.Add(videoLink.Replace("&amp;", "&"));
                }
            }

            if (!string.IsNullOrWhiteSpace(run.EmbeddedVideoLinks))
            {
                EmbeddedVideoLinks = new List<string>();
                foreach (var embeddedVideoLink in run.EmbeddedVideoLinks.Split(","))
                {
                    if (!string.IsNullOrWhiteSpace(embeddedVideoLink))
                    {
                        EmbeddedVideoLinks.Add(embeddedVideoLink.Replace("&amp;", "&"));
                    }                    
                }
            }

            if (run.PrimaryTime.HasValue)
            {
                PrimaryTime = new TimeSpan(run.PrimaryTime.Value);
            }
        }

        public int ID { get; set; }
        public IDNamePair Game { get; set; }
        public string GameCoverImageLink { get; set; }
        public IDNamePair Level { get; set; }
        public IDNamePair Category { get; set; }
        public List<Tuple<string, string>> SubCategoryVariableValues { get; set; }
        public List<string> SubCategoryVariableValueNames { get; set; }
        public List<IDNamePair> Players { get; set; }
        public List<string> VideoLinks { get; set; }
        public List<string> EmbeddedVideoLinks { get; set; }
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

        public string VideoLink
        {
            get
            {
                return EmbeddedVideoLinks?.FirstOrDefault();
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
    }
}
