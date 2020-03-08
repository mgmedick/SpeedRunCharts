using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedRunCommon
{
    public static class DateTimeHelper
    {
        #region DateTime
        public static string ToRealtiveDateString(this DateTime DateSubmitted)
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
                submittedTimeAgo = ts.Seconds == 1 ? "1 second ago" : ts.Seconds + " seconds ago";
            }
            else if (delta < 2 * minute)
            {
                return "1 minute ago";
            }
            else if (delta < 45 * minute)
            {
                submittedTimeAgo = ts.Minutes + " minutes ago";
            }
            else if (delta < 90 * minute)
            {
                submittedTimeAgo = "1 hour ago";
            }
            else if (delta < 24 * hour)
            {
                submittedTimeAgo = ts.Hours + " hours ago";
            }
            else if (delta < 48 * hour)
            {
                submittedTimeAgo = "1 day ago";
            }
            else if (delta < 30 * day)
            {
                submittedTimeAgo = ts.Days + " days ago";
            }
            else if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                submittedTimeAgo = months <= 1 ? "1 month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                submittedTimeAgo = years <= 1 ? "1 year ago" : years + " years ago";
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
                return Ts.ToString("fffffff'ms'");

            return Ts.ToString();
        }
        #endregion
    }
}
