using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel()
        {
        }

        public GameDetailsViewModel(GameView gameVW, string speedRunCode = null)
        {
            ID = gameVW.ID;
            Name = gameVW.Name;
            Abbr = gameVW.Abbr;
            ReleaseDate = gameVW.ReleaseDate;
            CoverImageUri = gameVW.CoverImageUrl;
            SrcUrl = gameVW.SrcUrl;
            SpeedRunCode = speedRunCode;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string CoverImageUri { get; set; }
        public string SrcUrl { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string SpeedRunCode { get; set; }
        public string DisplayName
        {
            get
            {
                return Name + (ReleaseDate != null ? " (" + ReleaseDate.Value.Year + ")" : string.Empty);
            }
        }
    }       
}


