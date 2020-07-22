using System;
using System.Collections;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;

namespace SpeedRunApp.Client
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

        public IEnumerable<Region> GetRegions(int? elementsPerPage = null,
            RegionsOrdering orderBy = default(RegionsOrdering))
        {
            var parameters = new List<string>();

            parameters.AddRange(orderBy.ToParameters());

            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            var uri = GetRegionsUri(parameters.ToParameters());

            return DoRequest(uri, x => Parse(x) as Region);
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

            return region;
        }
    }
}
