﻿using SpeedRunApp.Model.Data;
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
            GameID = run.GameID;
            CategoryID = run.CategoryID;
            LevelID = run.LevelID;
            SubCategoryVariableValueIDs = run.SubCategoryVariableValueIDs;
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;
            Comment = run.Comment;

            if (run.PlatformID.HasValue)
            {
                Platform = new IDNamePair { ID = run.PlatformID.Value, Name = run.PlatformName };
                PlatformName = run.PlatformName;
            }

            if (!string.IsNullOrWhiteSpace(run.VariableValues))
            {
                VariableValues = new Dictionary<int, int>();
                foreach (var variableValue in run.VariableValues.Split(","))
                {
                    var values = variableValue.Split("|", 2);
                    VariableValues.Add(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]));
                }
            }

            if (!string.IsNullOrWhiteSpace(run.Players) || !string.IsNullOrWhiteSpace(run.Guests))
            {
                Players = new List<UserNameViewModel>();

                if (!string.IsNullOrWhiteSpace(run.Players))
                {
                    foreach (var player in run.Players.Split("^^"))
                    {
                        var playerValue = player.Split("¦", 7);
                        int playerID;
                        int.TryParse(playerValue[0], out playerID);                               
                        Players.Add(new UserNameViewModel { ID = playerID, Name = playerValue[1], Abbr = playerValue[2], ColorLight = playerValue[3], ColorToLight = playerValue[4], ColorDark = playerValue[5], ColorToDark = playerValue[6] });
                    }
                }

                if (!string.IsNullOrWhiteSpace(run.Guests))
                {
                    foreach (var guest in run.Guests.Split("^^"))
                    {
                        var guestValue = guest.Split("¦", 3);
                        int guestID;
                        int.TryParse(guestValue[0], out guestID);
                        Players.Add(new UserNameViewModel { ID = 0, Name = guestValue[1], Abbr = guestValue[2] });
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(run.VideoLinks))
            {
                VideoLinks = new List<string>();

                foreach (var videoLink in run.VideoLinks.Split("^^"))
                {
                    if (!string.IsNullOrWhiteSpace(videoLink))
                    {
                        VideoLinks.Add(videoLink);                      
                    }                    
                }
            }

            if (run.PrimaryTime.HasValue)
            {
                PrimaryTime = new TimeSpan(run.PrimaryTime.Value);
            }
        }

        public int ID { get; set; }
        public int GameID { get; set; }
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
