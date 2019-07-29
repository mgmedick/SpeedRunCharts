using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeedrunComSharp;

namespace SpeedRunApp.WebUI.Models
{
    public class SpeedRunViewModel
    {
        public SpeedRunViewModel(Run speedRun)
        {
            SpeedRun = speedRun;
        }

        public Run SpeedRun
        {
            get;
            set;
        }
    }
}
