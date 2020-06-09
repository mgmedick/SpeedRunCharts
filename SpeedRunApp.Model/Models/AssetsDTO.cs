using System;
using System.Linq;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Model
{
    public class AssetsDTO
    {
        public AssetsDTO(Assets assets)
        {
            Logo = assets.Logo;
            CoverTiny = assets.CoverTiny;
            CoverSmall = assets.CoverSmall;
            CoverMedium = assets.CoverMedium;
            CoverLarge = assets.CoverLarge;
            Icon = assets.Icon;
            TrophyFirstPlace = assets.TrophyFirstPlace;
            TrophySecondPlace = assets.TrophySecondPlace;
            TrophyThirdPlace = assets.TrophyThirdPlace;
            TrophyFourthPlace = assets.TrophyFourthPlace;
            BackgroundImage = assets.BackgroundImage;
            ForegroundImage = assets.ForegroundImage;
        }

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
    }
}
