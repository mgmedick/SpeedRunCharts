using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SpeedRunApp.Common
{
    public static class ExtensionMethods
    {
        #region Uri
        public static string ToEmbeddedURIString(this Uri uri)
        {
            return uri.ToEmbeddedURI()?.ToString();
        }

        public static Uri ToEmbeddedURI(this Uri uri)
        {
            Uri embededURI = null;

            if (uri != null)
            {
                string domain = uri.GetLeftPart(UriPartial.Authority);
                string path = uri.AbsolutePath;
                string query = uri.Query;
                string videoIDString = null;
                string uriString = null;

                if (domain.Contains("twitch.tv"))
                {
                    if (path.StartsWith(@"/videos/"))
                    {
                        videoIDString = uri.Segments.Last();
                        uriString = string.Format(@"https://player.twitch.tv/?video={0}&autoplay=false&muted=true", videoIDString);
                    }
                }
                else if (domain.Contains("youtube.com") || domain.Contains("youtu.be"))
                {
                    var queryDictionary = QueryHelpers.ParseQuery(query);
                    videoIDString = queryDictionary.ContainsKey("v") ? queryDictionary["v"].ToString() : uri.Segments.Last();
                    uriString = string.Format(@"https://www.youtube.com/embed/{0}", videoIDString);
                }


                if (!string.IsNullOrWhiteSpace(uriString))
                {
                    embededURI = new Uri(uriString);
                }
            }

            return embededURI;
        }
        #endregion

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

        #region Session
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        #endregion

    }
}
