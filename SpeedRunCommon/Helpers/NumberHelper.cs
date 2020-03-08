﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedRunCommon
{
    public static class NumberHelper
    {
        public static string ToOrdinalString(this int num)
        {

            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }
    }
}
