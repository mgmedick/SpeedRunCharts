using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SpeedRunApp.WebUI
{
    public static class ExtensionMethods
    {
        public static string ToEmbeddedURIString(this string uriString)
        {
            string result = null;
            Uri parsedURI = null;

            if (!string.IsNullOrWhiteSpace(uriString) && Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out parsedURI))
            {
                result = parsedURI.ToEmbeddedURI().ToString();
            }

            return result;
        }

        public static Uri ToEmbeddedURI(this Uri uri)
        {
            Uri embededURI = null;

            if(uri != null)
            {
                string uriString = string.Format(@"{0}/{1}/{2}", uri.GetLeftPart(UriPartial.Authority), "embed", uri.PathAndQuery);
                embededURI = new Uri(uriString);
            }

            return embededURI;
        }
    }
}
