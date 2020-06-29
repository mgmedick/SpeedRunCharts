namespace SpeedRunApp.Model.Data
{
    public class CountryRegion
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }

        //public CountryRegion() { }

        /*
        public static CountryRegion Parse(SpeedrunComClient client, dynamic regionElement)
        {
            var region = new CountryRegion();

            region.Code = regionElement.code as string;
            region.Name = regionElement.names.international as string;
            region.JapaneseName = regionElement.names.japanese as string;

            return region;
        }
        */

        public override string ToString()
        {
            return Name;
        }
    }
}
