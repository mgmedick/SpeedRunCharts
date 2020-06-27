using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class SpeedRunVideosDTO
    {
        public SpeedRunVideosDTO()
        {
        }

        public string Text { get; set; }
        public IEnumerable<Uri> Links { get; set; }
    }
}
