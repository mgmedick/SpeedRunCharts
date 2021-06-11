using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunCommon.Extensions
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

        public static string HashPassword(this string pass)
        {
            string result = BCrypt.Net.BCrypt.HashPassword(pass);

            return result;
        }

        public static bool VerifyPassword(this string pass, string passHash)
        {
            bool result = BCrypt.Net.BCrypt.Verify(pass, passHash);

            return result;
        }
    }
}
