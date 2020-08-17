using System;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunTimes
    {
        public TimeSpan? Primary { get; set; }
        public TimeSpan? RealTime { get; set; }
        public TimeSpan? RealTimeWithoutLoads { get; set; }
        public TimeSpan? GameTime { get; set; }
    }
}
