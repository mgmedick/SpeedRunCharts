using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;

namespace SpeedRunCommon
{
    public static class UriHelper
    {

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
                        uriString = string.Format(@"https://player.twitch.tv/?video={0}&parent=localhost&autoplay=false&muted=true", videoIDString);
                    }
                }
                else if (domain.Contains("youtube.com") || domain.Contains("youtu.be"))
                {
                    var queryDictionary = QueryHelpers.ParseQuery(query);
                    videoIDString = queryDictionary.ContainsKey("v") ? queryDictionary["v"].ToString() : uri.Segments.Last();
                    uriString = string.Format(@"https://www.youtube.com/embed/{0}", videoIDString);
                }
                else if (domain.Contains("vimeo.com"))
                {
                    if (path.StartsWith(@"/video/"))
                    {
                        videoIDString = uri.Segments.Last();
                        uriString = string.Format(@"https://player.vimeo.com/video/{0}?autoplay=0&muted=1", videoIDString);
                    }
                }

                if (!string.IsNullOrWhiteSpace(uriString))
                {
                    embededURI = new Uri(uriString);
                }
            }

            return embededURI;
        }
    }
}
