namespace SpeedRunApp.Model.Data
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }

        //public Country() { }

        /*
        public static Country Parse(SpeedrunComClient client, dynamic countryElement)
        {
            var country = new Country();

            country.Code = countryElement.code as string;
            country.Name = countryElement.names.international as string;
            country.JapaneseName = countryElement.names.japanese as string;

            return country;
        }
        */

        public override string ToString()
        {
            return Name;
        }
    }
}
