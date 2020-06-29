using System.Collections.Generic;
using SpeedRunCommon;

namespace SpeedRunApp.Model.Data
{
    public class Assets
    {
        public ImageAsset Logo { get; set; }
        public ImageAsset CoverTiny { get; set; }
        public ImageAsset CoverSmall { get; set; }
        public ImageAsset CoverMedium { get; set; }
        public ImageAsset CoverLarge { get; set; }
        public ImageAsset Icon { get; set; }
        public ImageAsset TrophyFirstPlace { get; set; }
        public ImageAsset TrophySecondPlace { get; set; }
        public ImageAsset TrophyThirdPlace { get; set; }
        public ImageAsset TrophyFourthPlace { get; set; }
        public ImageAsset BackgroundImage { get; set; }
        public ImageAsset ForegroundImage { get; set; }

        //public Assets() { }

        /*
        public static Assets Parse(SpeedrunComClient client, dynamic assetsElement)
        {
            var assets = new Assets();

            var properties = assetsElement.Properties as IDictionary<string, dynamic>;

            assets.Logo = ImageAsset.Parse(client, assetsElement.logo) as ImageAsset;
            assets.CoverTiny = ImageAsset.Parse(client, properties["cover-tiny"]) as ImageAsset;
            assets.CoverSmall = ImageAsset.Parse(client, properties["cover-small"]) as ImageAsset;
            assets.CoverMedium = ImageAsset.Parse(client, properties["cover-medium"]) as ImageAsset;
            assets.CoverLarge = ImageAsset.Parse(client, properties["cover-large"]) as ImageAsset;
            assets.Icon = ImageAsset.Parse(client, assetsElement.icon) as ImageAsset;
            assets.TrophyFirstPlace = ImageAsset.Parse(client, properties["trophy-1st"]) as ImageAsset;
            assets.TrophySecondPlace = ImageAsset.Parse(client, properties["trophy-2nd"]) as ImageAsset;
            assets.TrophyThirdPlace = ImageAsset.Parse(client, properties["trophy-3rd"]) as ImageAsset;
            assets.TrophyFourthPlace = ImageAsset.Parse(client, properties["trophy-4th"]) as ImageAsset;
            assets.BackgroundImage = ImageAsset.Parse(client, assetsElement.background) as ImageAsset;
            assets.ForegroundImage = ImageAsset.Parse(client, assetsElement.foreground) as ImageAsset;

            return assets;
        }
        */
    }
}
