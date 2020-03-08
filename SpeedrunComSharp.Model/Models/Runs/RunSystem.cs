using System;

namespace SpeedrunComSharp.Model
{
    public class RunSystem
    {
        public string PlatformID { get; set; }
        public bool IsEmulated { get; set; }
        public string RegionID { get; set; }

        #region Links
        public Lazy<Platform> platform { get; set; }
        public Platform Platform { get { return platform.Value; } }
        public Lazy<Region> region { get; set; }
        public Region Region { get { return region.Value; } }
        #endregion

        public RunSystem() { }

        /*
        public static RunSystem Parse(SpeedrunComClient client, dynamic systemElement)
        {
            var system = new RunSystem();

            system.IsEmulated = (bool)systemElement.emulated;

            if (!string.IsNullOrEmpty(systemElement.platform as string))
            {
                system.PlatformID = systemElement.platform as string;
                system.platform = new Lazy<Platform>(() => client.Platforms.GetPlatform(system.PlatformID));
            }
            else
            {
                system.platform = new Lazy<Platform>(() => null);
            }

            if (!string.IsNullOrEmpty(systemElement.region as string))
            {
                system.RegionID = systemElement.region as string;
                system.region = new Lazy<Region>(() => client.Regions.GetRegion(system.RegionID));
            }
            else
            {
                system.region = new Lazy<Region>(() => null);
            }

            return system;
        }
        */
    }
}
