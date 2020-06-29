namespace SpeedRunApp.Model.Data
{
    public class Location
    {
        public Country Country { get; set; }
        public CountryRegion Region { get; set; }

        //public Location() { }

        /*
        public static Location Parse(SpeedrunComClient client, dynamic locationElement)
        {
            var location = new Location();
            
            if (locationElement != null)
            {
                location.Country = Country.Parse(client, locationElement.country) as Country;

                if (locationElement.region != null)
                    location.Region = CountryRegion.Parse(client, locationElement.region) as CountryRegion;
            }
            
            return location;
        }
        */

        public override string ToString()
        {
            return (Country?.Name + " " + Region?.Name).Trim();
        }
    }
}
