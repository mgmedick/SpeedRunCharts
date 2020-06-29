using System.Collections.Generic;

namespace SpeedRunApp.Model.Data
{
    public class UserNameStyle
    {
        public bool IsGradient { get; set; }
        public string LightSolidColorCode { get; set; }
        public string LightGradientStartColorCode
        {
            get { return LightSolidColorCode; }
            set { LightSolidColorCode = value; }
        }
        public string LightGradientEndColorCode { get; set; }
        public string DarkSolidColorCode { get; set; }
        public string DarkGradientStartColorCode
        {
            get { return DarkSolidColorCode; }
            set { DarkSolidColorCode = value; }
        }
        public string DarkGradientEndColorCode { get; set; }

        //public UserNameStyle() { }

        /*
        public static UserNameStyle Parse(SpeedrunComClient client, dynamic styleElement)
        {
            var style = new UserNameStyle();

            style.IsGradient = styleElement.style == "gradient";

            if (style.IsGradient)
            {
                var properties = styleElement.Properties as IDictionary<string, dynamic>;
                var colorFrom = properties["color-from"];
                var colorTo = properties["color-to"];

                style.LightGradientStartColorCode = colorFrom.light as string;
                style.LightGradientEndColorCode = colorTo.light as string;
                style.DarkGradientStartColorCode = colorFrom.dark as string;
                style.DarkGradientEndColorCode = colorTo.dark as string;
            }
            else
            {
                style.LightSolidColorCode = styleElement.color.light as string;
                style.DarkSolidColorCode = styleElement.color.dark as string;
            }

            return style;
        }
        */
    }
}
