using System;

namespace SpeedrunComSharp.Model
{
    public class RunTimes
    {
        public TimeSpan? Primary { get; set; }
        public TimeSpan? RealTime { get; set; }
        public TimeSpan? RealTimeWithoutLoads { get; set; }
        public TimeSpan? GameTime { get; set; }

        public RunTimes() { }

        /*
        public static RunTimes Parse(SpeedrunComClient client, dynamic timesElement)
        {
            var times = new RunTimes();

            if (timesElement.primary != null)
                times.Primary = TimeSpan.FromSeconds((double)timesElement.primary_t);

            if (timesElement.realtime != null)
                times.RealTime = TimeSpan.FromSeconds((double)timesElement.realtime_t);

            if (timesElement.realtime_noloads != null)
                times.RealTimeWithoutLoads = TimeSpan.FromSeconds((double)timesElement.realtime_noloads_t);

            if (timesElement.ingame != null)
                times.GameTime = TimeSpan.FromSeconds((double)timesElement.ingame_t);

            return times;
        
        }
        */

        public override string ToString()
        {
            if (Primary.HasValue)
                return Primary.Value.ToString();
            else
                return "-";
        }
    }
}
