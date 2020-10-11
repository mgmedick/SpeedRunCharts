using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;

namespace SpeedRunCommon
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent CommaSeparated(this IHtmlHelper html, IEnumerable<HtmlString> list)
        {

            if(list != null)
            {
                return new HtmlString(string.Join(", ", list));
            }
            else
            {
                return new HtmlString(string.Empty);
            }
        }

        public static HtmlString ToHtmlString(this IHtmlContent htmlContent)
        {
            if (htmlContent is HtmlString htmlString)
            {
                return htmlString;
            }

            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return new HtmlString(writer.ToString());
            }
        }
    }
}
