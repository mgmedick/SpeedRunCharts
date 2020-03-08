using System;
using System.Collections.Generic;

namespace SpeedRunCommon
{
    public static class EnumHelper
    {

        #region ToParameters
        //Cateogries
        public static IEnumerable<string> ToParameters(this CategoriesOrdering ordering)
        {

            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (CategoriesOrdering)((int)ordering - 1);

            var str = "";

            switch (ordering)
            {
                case CategoriesOrdering.Name:
                    str = "name"; break;
                case CategoriesOrdering.Miscellaneous:
                    str = "miscellaneous"; break;
            }

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }

        //Games
        public static IEnumerable<string> ToParameters(this GamesOrdering ordering)
        {
            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (GamesOrdering)((int)ordering - 1);

            var str = "";

            switch (ordering)
            {
                case GamesOrdering.Name:
                    str = "name.int"; break;
                case GamesOrdering.JapaneseName:
                    str = "name.jap"; break;
                case GamesOrdering.Abbreviation:
                    str = "abbreviation"; break;
                case GamesOrdering.YearOfRelease:
                    str = "released"; break;
                case GamesOrdering.CreationDate:
                    str = "created"; break;
            }

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }

        //LeaderboardSs
        public static string ToParameter(this LeaderboardScope scope)
        {
            switch (scope)
            {
                case LeaderboardScope.All:
                    return "all";
                case LeaderboardScope.FullGame:
                    return "full-game";
                case LeaderboardScope.Levels:
                    return "levels";
            }

            throw new ArgumentException("scope");
        }

        //Levels
        public static IEnumerable<string> ToParameters(this LevelsOrdering ordering)
        {
            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (LevelsOrdering)((int)ordering - 1);

            var str = "";

            switch (ordering)
            {
                case LevelsOrdering.Name:
                    str = "name"; break;
            }

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }

        //Notifications
        public static IEnumerable<string> ToParameters(this NotificationsOrdering ordering)
        {
            var list = new List<string>();

            if (ordering == NotificationsOrdering.OldestToNewest)
                list.Add("direction=asc");

            return list;
        }

        //Platforms
        public static IEnumerable<string> ToParameters(this PlatformsOrdering ordering)
        {
            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (PlatformsOrdering)((int)ordering - 1);

            var str = "";

            switch (ordering)
            {
                case PlatformsOrdering.YearOfRelease:
                    str = "released"; break;
            }

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }

        //Regions
        public static IEnumerable<string> ToParameters(this RegionsOrdering ordering)
        {
            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (RegionsOrdering)((int)ordering - 1);

            var str = "";

            /*switch (ordering)
            {
            }*/

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }

        //Runs
        public static IEnumerable<string> ToParameters(this RunsOrdering ordering)
        {
            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (RunsOrdering)((int)ordering - 1);

            var str = "";

            switch (ordering)
            {
                case RunsOrdering.Category:
                    str = "category"; break;
                case RunsOrdering.Level:
                    str = "level"; break;
                case RunsOrdering.Platform:
                    str = "platform"; break;
                case RunsOrdering.Region:
                    str = "region"; break;
                case RunsOrdering.Emulated:
                    str = "emulated"; break;
                case RunsOrdering.Date:
                    str = "date"; break;
                case RunsOrdering.DateSubmitted:
                    str = "submitted"; break;
                case RunsOrdering.Status:
                    str = "status"; break;
                case RunsOrdering.VerifyDate:
                    str = "verify-date"; break;
            }

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }

        //Series
        public static IEnumerable<string> ToParameters(this SeriesOrdering ordering)
        {
            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (SeriesOrdering)((int)ordering - 1);

            var str = "";

            switch (ordering)
            {
                case SeriesOrdering.JapaneseName:
                    str = "name.jap"; break;
                case SeriesOrdering.Abbreviation:
                    str = "abbreviation"; break;
                case SeriesOrdering.CreationDate:
                    str = "created"; break;
            }

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }


        public static string ToParameters(this TimingMethod timingMethod)
        {
            switch (timingMethod)
            {
                case TimingMethod.RealTime:
                    return "realtime";
                case TimingMethod.RealTimeWithoutLoads:
                    return "realtime_noloads";
                case TimingMethod.GameTime:
                    return "ingame";
            }
            throw new ArgumentException("timingMethod");
        }

        //Users
        public static IEnumerable<string> ToParameters(this UsersOrdering ordering)
        {
            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (UsersOrdering)((int)ordering - 1);

            var str = "";

            switch (ordering)
            {
                case UsersOrdering.JapaneseName:
                    str = "name.jap"; break;
                case UsersOrdering.SignUpDate:
                    str = "signup"; break;
                case UsersOrdering.Role:
                    str = "role"; break;
            }

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }

        //Variables
        public static IEnumerable<string> ToParameters(this VariablesOrdering ordering)
        {
            var isDescending = ((int)ordering & 1) == 1;
            if (isDescending)
                ordering = (VariablesOrdering)((int)ordering - 1);

            var str = "";

            switch (ordering)
            {
                case VariablesOrdering.Name:
                    str = "name"; break;
                case VariablesOrdering.Mandatory:
                    str = "mandatory"; break;
                case VariablesOrdering.UserDefined:
                    str = "user-defined"; break;
            }

            var list = new List<string>();

            if (!string.IsNullOrEmpty(str))
                list.Add(string.Format("orderby={0}", str));
            if (isDescending)
                list.Add("direction=desc");

            return list;
        }
        #endregion

        #region ToEnum
        public static NotificationStatus ToNotificationStatus(this string element)
        {
            switch (element)
            {
                case "read":
                    return NotificationStatus.Read;
                case "unread":
                    return NotificationStatus.Unread;
            }

            throw new ArgumentException("status");
        }

        public static NotificationType ToNotificationType(this string element)
        {
            switch (element)
            {
                case "post":
                    return NotificationType.Post;
                case "run":
                    return NotificationType.Run;
                case "game":
                    return NotificationType.Game;
                case "guide":
                    return NotificationType.Guide;
            }

            throw new ArgumentException("type");
        }

        public static TimingMethod ToTimingMethod(this string element)
        {
            switch (element)
            {
                case "realtime":
                    return TimingMethod.RealTime;
                case "realtime_noloads":
                    return TimingMethod.RealTimeWithoutLoads;
                case "ingame":
                    return TimingMethod.GameTime;
            }

            throw new ArgumentException("element");
        }
        #endregion

    }
}
