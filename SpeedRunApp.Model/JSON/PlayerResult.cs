using SpeedRunApp.Model.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpeedRunApp.Model.JSON
{
    public class PlayerResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ColorLight { get; set; }
        public string ColorToLight { get; set; }
        public string ColorDark { get; set; }
        public string ColorToDark { get; set; }
    }
}


