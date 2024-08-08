using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;

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
            LevelID = run.LevelID;
            SubCategoryVariableValueIDs = run.SubCategoryVariableValueIDs;
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;  
            PrimaryTime = new TimeSpan(run.PrimaryTime);      
            VariableValues = run.VariableValues;
            VideoLinks = run.Videos;

            if (run.PlatformID.HasValue)
            {
                Platform = new IDNamePair { ID = run.PlatformID.Value, Name = run.PlatformName };
                PlatformName = run.PlatformName;
            }

            if (run.Players != null) {
                Players = run.Players.Select(i => new UserNameViewModel() {ID = i.ID,
                                                                            Name = i.Name,
                                                                            ColorLight = i.ColorLight,
                                                                            ColorToLight = i.ColorToLight,
                                                                            ColorDark = i.ColorDark,
                                                                            ColorToDark = i.ColorToDark}).ToList();
            }
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public int GameID { get; set; }
        public int CategoryTypeID { get; set; }
        public int CategoryID { get; set; }
        public int? LevelID { get; set; }
        public IDNamePair Platform { get; set; }
        public string PlatformName { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
        public Dictionary<int, int> VariableValues { get; set; }        
        public List<UserNameViewModel> Players { get; set; }
        public List<string> VideoLinks { get; set; }
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public string Comment { get; set; }
        public string SpeedRunComLink { get; set; }
        public string SplitsLink { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public UserNameViewModel Player
        {
            get
            {
                return Players?.FirstOrDefault();
            }
        }

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

        public double PrimaryTimeTicks
        {
            get
            {
                return PrimaryTime.Ticks;
            }
        }        

        public double PrimaryTimeSeconds
        {
            get
            {
                return PrimaryTime.TotalSeconds;
            }
        }

        public string PrimaryTimeString
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

        public string DateSubmittedString
        {
            get
            {
                return DateSubmitted?.ToString("MM/dd/yyyy");
            }
        }

        public string MonthYearSubmitted
        {
            get
            {
                return DateSubmitted?.ToString("MM/yyyy");
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
