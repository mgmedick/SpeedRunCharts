using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpeedRunApp.Client
{
    public static class StringHelpers
    {

        public static string ToParameters(this string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
                return "";
            else
                return "?" + parameters;
        }

        public static string ToParameters(this IEnumerable<string> parameters)
        {
            var list = parameters.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (list.Any())
                return "?" + string.Join("&", list);//.Aggregate("&");
            else
                return "";
        }

        //public static string Aggregate(this IEnumerable<string> list, string combiner)
        //{
        //    var builder = new StringBuilder();

        //    foreach (var element in list)
        //    {
        //        builder.Append(element);
        //        builder.Append(combiner);
        //    }

        //    builder.Length -= combiner.Length;

        //    return builder.ToString();
        //}
    }
}
