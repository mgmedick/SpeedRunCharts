using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunCommon
{
    public static class StringHelpers
    {
        public static string[] SplitString(this string param, string delimiter, int? count = null)
        {
            string[] result = null;

            if (count.HasValue)
            {
                result = param.Split(new string[] { delimiter }, count.Value, StringSplitOptions.None);
            }
            else
            {
                result = param.Split(new string[] { delimiter }, StringSplitOptions.None);
            }
            return param.Split(new string[] { delimiter }, StringSplitOptions.None);
        }
    }
}
