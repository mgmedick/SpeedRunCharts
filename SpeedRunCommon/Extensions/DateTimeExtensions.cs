using System;
using System.Collections.Generic;

namespace SpeedRunCommon.Extensions
{
    public static class DateTimeExtensions
    {
        #region DateTime
        public static string ToRealtiveDateString(this DateTime DateSubmitted, bool shortFormat = false)
        {           
            string submittedTimeAgo = null;

            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - DateSubmitted.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
            {
                if(ts.Seconds == 1) {
                    submittedTimeAgo = shortFormat ? "1s" : "1 second";
                } else {
                    submittedTimeAgo = shortFormat ? ts.Seconds + "s" : ts.Seconds + " seconds";
                }
            }
            else if (delta < 2 * minute)
            {
                return shortFormat ? "1m" : "1 minute";
            }
            else if (delta < 45 * minute)
            {
                submittedTimeAgo = shortFormat ? ts.Minutes + "m" : ts.Minutes + " minutes";
            }
            else if (delta < 90 * minute)
            {
                submittedTimeAgo = shortFormat ? "1h" : "1 hour";
            }
            else if (delta < 24 * hour)
            {
                submittedTimeAgo = shortFormat ? ts.Hours + "h" : ts.Hours + " hours";
            }
            else if (delta < 48 * hour)
            {
                submittedTimeAgo = shortFormat ? "1d" : "1 day";
            }
            else if (delta < 30 * day)
            {
                submittedTimeAgo = shortFormat ? ts.Days + "d" : ts.Days + " days";

            }
            else if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                if (months <= 1) {
                   submittedTimeAgo = shortFormat ? "1M" : "1 month";
                } else {
                    submittedTimeAgo = shortFormat ? months + "M" : months + " months";
                }
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                if (years <= 1) {
                    submittedTimeAgo = shortFormat ? "1y" : "1 year";
                } else {
                    submittedTimeAgo = shortFormat ? years + "y" : years + " years";
                }
            }

            if (!string.IsNullOrWhiteSpace(submittedTimeAgo))
            {
                submittedTimeAgo += " ago";
            }

            return submittedTimeAgo;
        } 
        #endregion

        #region TimeSpan
        public static string ToShortString(this TimeSpan Ts, bool excludeMilliseconds = false)
        {
            var result = string.Empty;

            if (Ts.TotalDays > 1d)
            {
                result = Ts.ToString("d'd 'h'h 'm'm 's's'");
            }
            else if (Ts.TotalHours > 1d || Ts.TotalMinutes == 60)
            {
                result = Ts.ToString("h'h 'm'm 's's'");
            }
            else if (Ts.TotalMinutes > 1d || Ts.TotalSeconds == 60)
            {
                result = Ts.ToString("m'm 's's'");
            }
            else if (Ts.TotalSeconds > 1d)
            {
                result = Ts.ToString("s's'");
            }

            if (Ts.Milliseconds > 0d && !excludeMilliseconds)
            {
                result = (string.Format("{0} {1}ms", result, Ts.Milliseconds).Trim());
            }

            return result;
        }
        #endregion
    }
}
