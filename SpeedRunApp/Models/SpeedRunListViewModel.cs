using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeedrunComSharp;

namespace SpeedRunApp.WebUI.Models
{
    public class SpeedRunListViewModel
    {
        public IEnumerable<SpeedrunComSharp.Run> SpeedRuns
        {
            get;
            set;
        }
    }
}
