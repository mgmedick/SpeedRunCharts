using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunVideos
    {
        //public SpeedRunVideos()
        //{
        //}

        public string Text { get; set; }
        public IEnumerable<Uri> Links { get; set; }
        public IEnumerable<Uri> EmbededLinks { get; set; }
    }
}
