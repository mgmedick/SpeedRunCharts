using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel(GameViewModel gameVM, DateTime gameSpeedRunLastImportDate, int? speedRunID)
        {
            GameVM = gameVM;
            GameSpeedRunLastImportDate = gameSpeedRunLastImportDate;
            SpeedRunID = speedRunID;
        }
                
        public GameViewModel GameVM { get; set; }
        public DateTime GameSpeedRunLastImportDate { get; set; }
        public int? SpeedRunID { get; set; }
    }
}


