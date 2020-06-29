using System;
using System.Collections;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;

namespace SpeedRunApp.Client
{
    public class PlatformsClient : BaseClient
    {

        public const string Name = "platforms";

        public PlatformsClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetPlatformsUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        //public Platform GetPlatformFromSiteUri(string siteUri)
        //{
        //    var id = GetPlatformIDFromSiteUri(siteUri);

        //    if (string.IsNullOrEmpty(id))
        //        return null;

        //    return GetPlatform(id);
        //}

        //public string GetPlatformIDFromSiteUri(string siteUri)
        //{
        //    var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

        //    if (elementDescription == null
        //        || elementDescription.Type != ElementType.Platform)
        //        return null;

        //    return elementDescription.ID;
        //}

        public IEnumerable<Platform> GetPlatforms(int? elementsPerPage = null,
            PlatformsOrdering orderBy = default(PlatformsOrdering))
        {
            var parameters = new List<string>();

            parameters.AddRange(orderBy.ToParameters());

            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            var uri = GetPlatformsUri(parameters.ToParameters());

            return DoRequest(uri, x => Parse(x) as Platform);
        }

        public Platform GetPlatform(string platformId)
        {
            var uri = GetPlatformsUri(string.Format("/{0}", Uri.EscapeDataString(platformId)));
            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public Platform Parse(dynamic platformElement)
        {
            if (platformElement is ArrayList)
                return null;

            var platform = new Platform();

            //Parse Attributes
            platform.ID = platformElement.id as string;
            platform.Name = platformElement.name as string;
            platform.YearOfRelease = (int)platformElement.released;

            return platform;
        }
    }
}
