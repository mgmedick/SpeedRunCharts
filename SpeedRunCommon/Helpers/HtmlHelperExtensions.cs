using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace SpeedRunApp.WebUI.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent CommaSeparated(this IHtmlHelper html, IEnumerable<HtmlString> list)
        {
            return new HtmlString(string.Join(", ", list));
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
