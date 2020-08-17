using SpeedRunApp.Model.Data;
using System;

namespace SpeedRunApp.Client
{
    public class GuestsClient : BaseClient
    {

        public const string Name = "guests";

        public GuestsClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetGuestsUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        //public Guest GetGuestFromSiteUri(string siteUri)
        //{
        //    var id = GetGuestIDFromSiteUri(siteUri);

        //    if (string.IsNullOrEmpty(id))
        //        return null;

        //    return GetGuest(id);
        //}

        //public string GetGuestIDFromSiteUri(string siteUri)
        //{
        //    var elementDescription = GetElementDescriptionFromSiteUri(siteUri);

        //    if (elementDescription == null
        //        || elementDescription.Type != ElementType.Guest)
        //        return null;

        //    return elementDescription.ID;
        //}

        public Guest GetGuest(string guestName)
        {
            var uri = GetGuestsUri(string.Format("/{0}", Uri.EscapeDataString(guestName)));
            var result = DoRequest(uri);

            return Parse(result.data);
        }

        public Guest Parse(dynamic guestElement)
        {
            var guest = new Guest();

            guest.Name = guestElement.name;

            return guest;
        }
    }
}
