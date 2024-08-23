using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;
using SpeedRunApp.Model.JSON;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridViewModel
    {
        public SpeedRunGridViewModel(SpeedRunGridView run)
        {
            ID = run.ID;
            Code = run.Code;
            GameID = run.GameID;
            CategoryTypeID = run.CategoryTypeID;
            CategoryID = run.CategoryID;
            CategoryName = run.CategoryName;
            LevelID = run.LevelID;
            LevelName = run.LevelName;
            SubCategoryVariableValueIDs = run.SubCategoryVariableValueIDs;
            SubCategoryVariableValues = run.SubCategoryVariableValues;
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;  
            PrimaryTime = TimeSpan.FromMilliseconds(run.PrimaryTime);      
            VariableValues = run.VariableValues;
            VideoLinks = run.Videos;
            Players = run.Players;
            PlatformName = run.PlatformName;
        }

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
        public string PlatformName { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
        public string SubCategoryVariableValues { get; set; }
        public Dictionary<int, int> VariableValues { get; set; }
        public List<PlayerResult> Players { get; set; }
        public List<string> VideoLinks { get; set; }
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public string Comment { get; set; }
        public string SpeedRunComLink { get; set; }
        public string SplitsLink { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public bool IsPersonalBest { get; set; }

        public string PlayerNames
        {
            get
            {
                return Players != null ? string.Join(", ", Players.Select(i => i.Name)) : string.Empty;
            }
        }
        
        public string VerifyDateString
        {
            get
            {
                return VerifyDate?.ToString("MM/dd/yyyy");
            }
        }

        public string RelativeVerifyDateString
        {
            get
            {
                return VerifyDate?.ToRealtiveDateString();
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
