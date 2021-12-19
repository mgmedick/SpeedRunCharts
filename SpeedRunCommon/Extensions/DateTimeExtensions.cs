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
                    submittedTimeAgo = shortFormat ? "1s" : "1 second ago";
                } else {
                    submittedTimeAgo = shortFormat ? ts.Seconds + "s" : ts.Seconds + " seconds ago";
                }
            }
            else if (delta < 2 * minute)
            {
                return shortFormat ? "1m" : "1 minute ago";
            }
            else if (delta < 45 * minute)
            {
                submittedTimeAgo = shortFormat ? ts.Minutes + "m" : ts.Minutes + " minutes ago";
            }
            else if (delta < 90 * minute)
            {
                submittedTimeAgo = shortFormat ? "1h" : "1 hour ago";
            }
            else if (delta < 24 * hour)
            {
                submittedTimeAgo = shortFormat ? ts.Hours + "h" : ts.Hours + " hours ago";
            }
            else if (delta < 48 * hour)
            {
                submittedTimeAgo = shortFormat ? "1d" : "1 day ago";
            }
            else if (delta < 30 * day)
            {
                submittedTimeAgo = shortFormat ? ts.Days + "d" : ts.Days + " days ago";

            }
            else if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                if (months <= 1) {
                   submittedTimeAgo = shortFormat ? "1M" : "1 month ago";
                } else {
                    submittedTimeAgo = shortFormat ? months + "M" : months + " months ago";
                }
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                if (years <= 1) {
                    submittedTimeAgo = shortFormat ? "1y" : "1 year ago";
                } else {
                    submittedTimeAgo = shortFormat ? years + "y" : years + " years ago";
                }
            }

            return submittedTimeAgo;
        }
        #endregion

        #region TimeSpan
        public static string ToShortString(this TimeSpan Ts)
        {
            if (Ts.TotalDays > 1d)
                return Ts.ToString("d'd 'h'h 'm'm 's's'");

            if (Ts.TotalHours > 1d)
                return Ts.ToString("h'h 'm'm 's's'");

            if (Ts.TotalMinutes > 1d)
                return Ts.ToString("m'm 's's'");

            if (Ts.TotalSeconds > 1d)
                return Ts.ToString("s's'");

            if (Ts.TotalMilliseconds > 1d)
                return Ts.ToString("fff'ms'");

            return Ts.ToString();
        }
        #endregion
    }
}
