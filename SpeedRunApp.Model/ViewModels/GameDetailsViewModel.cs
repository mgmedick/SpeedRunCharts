using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeedRunApp.Common;
using SpeedRunApp.Model;

namespace SpeedRunApp.Model
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel()
        {
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public IEnumerable<RunDTO> Runs { get; set; }
    }
}
