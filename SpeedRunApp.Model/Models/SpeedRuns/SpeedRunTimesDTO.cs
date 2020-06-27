using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedRunApp.Model
{
    public class SpeedRunTimesDTO
    {
        public TimeSpan? Primary { get; set; }
        public TimeSpan? RealTime { get; set; }
        public TimeSpan? RealTimeWithoutLoads { get; set; }
        public TimeSpan? GameTime { get; set; }
    }
}
