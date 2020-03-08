using System;
using System.Collections;
using System.Collections.Generic;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
{
    public class RegionsClient : BaseClient
    {

        public const string Name = "regions";

        public RegionsClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetRegionsUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public Region GetRegionFromSiteUri(string siteUri)
        {
            var id = GetRegionIDFromSiteUri(siteUri);

            if (string.IsNullOrEmpty(id))
                return null;

            return GetRegion(id);
        }

        public string GetRegionIDFromSiteUri(string siteUri)
        {
            var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

            if (elementDescription == null
                || elementDescription.Type != ElementType.Region)
                return null;

            return elementDescription.ID;
        }

        public IEnumerable<Region> GetRegions(int? elementsPerPage = null,
            RegionsOrdering orderBy = default(RegionsOrdering))
        {
            var parameters = new List<string>();

            parameters.AddRange(orderBy.ToParameters());

            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            var uri = GetRegionsUri(parameters.ToParameters());

            return DoPaginatedRequest(uri,
                x => Parse(x) as Region);
        }

        public Region GetRegion(string regionId)
        {
            var uri = GetRegionsUri(string.Format("/{0}", Uri.EscapeDataString(regionId)));
            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public Region Parse(dynamic regionElement)
        {
            if (regionElement is ArrayList)
                return null;

            var region = new Region();

            //Parse Attributes

            region.ID = regionElement.id as string;
            region.Name = regionElement.name as string;

            //Parse Links

            region.Games = Client.Games.GetGames(regionId: region.ID);
            region.runs = new Lazy<IEnumerable<Run>>(() => Client.Runs.GetRuns(regionId: region.ID));

            return region;
        }
    }
}
