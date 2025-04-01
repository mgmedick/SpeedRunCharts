using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;

namespace SpeedRunCommon.Extensions
{
    public static class UriExtensions
    {
        public static Uri ToParameterizedURI(this Uri uri, bool autoplay, bool muted, bool controls)
        {
            Uri embededURI = null;

            if (uri != null)
            {
                var domain = uri.GetLeftPart(UriPartial.Authority);
                var uriString = uri.ToString();

                if (domain.Contains("twitch.tv"))
                {
                    uriString = string.Format("{0}&autoplay={1}&muted={2}&controls={3}", uri, autoplay, muted, controls);
                }
                else if (domain.Contains("youtube.com"))
                {
                    uriString = string.Format("{0}?autoplay={1}&mute={2}&controls={3}", uri, autoplay ? 1 : 0, muted ? 1 : 0, controls ? 1 : 0); 
                }                
                else if (domain.Contains("vimeo.com") || domain.Contains("medal.tv"))
                {
                    uriString = string.Format("{0}?autoplay={1}&muted={2}", uri, autoplay ? 1 : 0, muted ? 1 : 0); 
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
