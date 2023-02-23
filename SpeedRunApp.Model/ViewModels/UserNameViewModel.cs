using SpeedRunApp.Model.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserNameViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string ColorLight { get; set; }
        public string ColorToLight { get; set; }
        public string ColorDark { get; set; }
        public string ColorToDark { get; set; }
    }
}


