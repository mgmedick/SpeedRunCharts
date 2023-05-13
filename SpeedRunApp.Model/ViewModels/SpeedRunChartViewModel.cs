using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon.Extensions;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunChartViewModel
    {
        public SpeedRunChartViewModel(SpeedRunChartView run)
        {
            ID = run.ID;
            GameID = run.GameID;
            CategoryID = run.CategoryID;
            LevelID = run.LevelID;
            SubCategoryVariableValueIDs = run.SubCategoryVariableValueIDs;            
            Rank = run.Rank;
            DateSubmitted = run.DateSubmitted;
            
            if (run.PrimaryTime.HasValue)
            {
                PrimaryTime = new TimeSpan(run.PrimaryTime.Value);
            }

            if (!string.IsNullOrWhiteSpace(run.Players) || !string.IsNullOrWhiteSpace(run.Guests))
            {
                Players = new List<IDNameAbbrPair>();

                if (!string.IsNullOrWhiteSpace(run.Players))
                {
                    foreach (var player in run.Players.Split("^^"))
                    {
                        var playerValue = player.Split("¦", 3);
                        int playerID;
                        int.TryParse(playerValue[0], out playerID);
                        Players.Add(new IDNameAbbrPair { ID = playerID, Name = playerValue[1], Abbr = playerValue[2] });
                    }
                }

                if (!string.IsNullOrWhiteSpace(run.Guests))
                {
                    foreach (var guest in run.Guests.Split("^^"))
                    {
                        var guestValue = guest.Split("¦", 3);
                        int guestID;
                        int.TryParse(guestValue[0], out guestID);
                        Players.Add(new IDNameAbbrPair { ID = 0, Name = guestValue[1], Abbr = guestValue[2] });
                    }
                }
            }                 
        }
        
        public int ID { get; set; }
        public int GameID { get; set; }
        public int CategoryID { get; set; }
        public int? LevelID { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
        public List<IDNameAbbrPair> Players { get; set; }
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
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
    }
}
