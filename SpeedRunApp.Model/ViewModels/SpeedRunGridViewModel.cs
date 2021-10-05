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
            GameID = run.GameID;
            CategoryTypeID = run.CategoryTypeID;
            CategoryID = run.CategoryID;
            CategoryName = run.CategoryName;
            LevelID = run.LevelID;
            LevelName = run.LevelName;
            //SpeedRunComLink = run.SpeedRunComUrl;
            //SplitsLink = run.SplitsUrl;
            VariableValueIDs = run.VariableValueIDs;
            DateSubmitted = run.DateSubmitted;
            VerifyDate = run.VerifyDate;
            Rank = run.Rank;
            Comment = run.Comment;

            if (run.PlatformID.HasValue)
            {
                Platform = new IDNamePair { ID = run.PlatformID.Value, Name = run.PlatformName };
                PlatformName = run.PlatformName;
            }

            if (!string.IsNullOrWhiteSpace(run.Variables))
            {
                Variables = new List<IDNamePair>();
                foreach (var variable in run.Variables.Split("^^"))
                {
                    var values = variable.Split("|", 2);
                    Variables.Add(new IDNamePair { ID = Convert.ToInt32(values[0]), Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(run.VariableValues))
            {
                VariableValues = new Dictionary<int, IDNamePair>();
                foreach (var variableValue in run.VariableValues.Split("^^"))
                {
                    var values = variableValue.Split("|", 3);
                    VariableValues.Add(Convert.ToInt32(values[0]), new IDNamePair { ID = Convert.ToInt32(values[1]), Name = values[2]});
                }
            }

            if (!string.IsNullOrWhiteSpace(run.Players) || !string.IsNullOrWhiteSpace(run.Guests))
            {
                Players = new List<IDNamePair>();

                if (!string.IsNullOrWhiteSpace(run.Players))
                {
                    foreach (var player in run.Players.Split("^^"))
                    {
                        var playerValue = player.Split("|", 2);
                        int playerID;
                        int.TryParse(playerValue[0], out playerID);
                        Players.Add(new IDNamePair { ID = playerID, Name = playerValue[1] });
                    }
                }

                if (!string.IsNullOrWhiteSpace(run.Guests))
                {
                    foreach (var guest in run.Guests.Split("^^"))
                    {
                        var guestValue = guest.Split("|", 2);
                        int guestID;
                        int.TryParse(guestValue[0], out guestID);
                        Players.Add(new IDNamePair { ID = 0, Name = guestValue[1] });
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
        public int CategoryTypeID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? LevelID { get; set; }
        public string LevelName { get; set; }
        public IDNamePair Platform { get; set; }
        public string PlatformName { get; set; }
        public string VariableValueIDs { get; set; }
        public List<IDNamePair> Variables { get; set; }
        public Dictionary<int, IDNamePair> VariableValues { get; set; }
        public List<IDNamePair> Players { get; set; }
        public List<string> VideoLinks { get; set; }
        public bool IsEmulated { get; set; }
        public int? Rank { get; set; }
        public TimeSpan PrimaryTime { get; set; }
        public string Comment { get; set; }
        public string SpeedRunComLink { get; set; }
        public string SplitsLink { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }

        public IDNamePair Player
        {
            get
            {
                return Players?.FirstOrDefault();
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

        public string IsEmulatedString
        {
            get
            {
                return IsEmulated.ToString();
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
    }
}
