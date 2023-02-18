using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel(GameViewModel gameVM, int? speedRunID)
        {
            GameVM = gameVM;
            SpeedRunID = speedRunID;
        }
                
        public GameViewModel GameVM { get; set; }
        public int? SpeedRunID { get; set; }
    }
}


