using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;

namespace SpeedRunCommon.Extensions
{
    public static class UriExtensions
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
                        uriString = string.Format(@"https://player.twitch.tv/?video={0}&parent=localhost&parent=speedruncharts.com&parent=www.speedruncharts.com&autoplay=false&muted=true", videoIDString);
                    }
                }
                else if (domain.Contains("youtube.com") || domain.Contains("youtu.be"))
                {
                    var queryDictionary = QueryHelpers.ParseQuery(query);
                    videoIDString = queryDictionary.ContainsKey("v") ? queryDictionary["v"].ToString() : uri.Segments.Last();
                    uriString = string.Format(@"https://www.youtube.com/embed/{0}?autoplay=0&muted=1", videoIDString);
                }
                else if (domain.Contains("vimeo.com"))
                {
                    if (path.StartsWith(@"/video/"))
                    {
                        videoIDString = uri.Segments.Last();
                        uriString = string.Format(@"https://player.vimeo.com/video/{0}?autoplay=0&muted=1", videoIDString);
                    }
                }
                else if (domain.Contains("streamable.com"))
                {
                    videoIDString = uri.Segments.Last();
                    uriString = string.Format(@"https://streamable.com/o/{0}", videoIDString);
                }
                else if (domain.Contains("medal.tv"))
                {
                    videoIDString = string.Join(string.Empty, uri.Segments.Reverse().Take(2).Reverse().ToList());
                    uriString = string.Format(@"https://medal.tv/clip/{0}?autoplay=0&muted=1&loop=0", videoIDString);
                }

                if (!string.IsNullOrWhiteSpace(uriString))
                {
                    embededURI = new Uri(uriString);
                }
            }

            return embededURI;
        }

        public static string ToThumbnailURIString(this Uri uri)
        {
            return uri.ToThumbnailURI()?.ToString();
        }

        public static Uri ToThumbnailURI(this Uri uri)
        {
            Uri embededURI = null;

            if (uri != null)
            {
                string domain = uri.GetLeftPart(UriPartial.Authority);
                string path = uri.AbsolutePath;
                string query = uri.Query;
                string videoIDString = null;
                string uriString = null;

                //if (domain.Contains("twitch.tv"))
                //{
                //    if (path.StartsWith(@"/videos/"))
                //    {
                //        videoIDString = uri.Segments.Last();
                //        uriString = string.Format(@"https://player.twitch.tv/?video={0}&parent=localhost&parent=speedruncharts.com&parent=www.speedruncharts.com&autoplay=false&muted=true", videoIDString);
                //    }
                //}
                //else 
                if (domain.Contains("youtube.com") || domain.Contains("youtu.be"))
                {
                    var queryDictionary = QueryHelpers.ParseQuery(query);
                    videoIDString = queryDictionary.ContainsKey("v") ? queryDictionary["v"].ToString() : uri.Segments.Last();
                    uriString = string.Format(@"https://img.youtube.com/vi/{0}/1.jpg", videoIDString);
                }
                //else if (domain.Contains("vimeo.com"))
                //{
                //    if (path.StartsWith(@"/video/"))
                //    {
                //        videoIDString = uri.Segments.Last();
                //        uriString = string.Format(@"https://player.vimeo.com/video/{0}?autoplay=0&muted=1", videoIDString);
                //    }
                //}
                //else if (domain.Contains("streamable.com"))
                //{
                //    videoIDString = uri.Segments.Last();
                //    uriString = string.Format(@"https://streamable.com/o/{0}", videoIDString);
                //}
                //else if (domain.Contains("medal.tv"))
                //{
                //    videoIDString = string.Join(string.Empty, uri.Segments.Reverse().Take(2).Reverse().ToList());
                //    uriString = string.Format(@"https://medal.tv/clip/{0}?autoplay=0&muted=1&loop=0", videoIDString);
                //}

                if (!string.IsNullOrWhiteSpace(uriString))
                {
                    embededURI = new Uri(uriString);
                }
            }

            return embededURI;
        }
    }
}
