using Newtonsoft.Json;

namespace Domain.Models
{
    /// <summary>
    /// A simple model that encapsulates the data returned by the external API.
    /// </summary>
    public class IpstackApiResult
    {
        public string Ip { get; set; }

        [JsonProperty(PropertyName = "continent_name")]
        public string ContinentName { get; set; }


        [JsonProperty(PropertyName = "country_name")]
        public string CountryName { get; set; }


        [JsonProperty(PropertyName = "region_name")]
        public string RegionName { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"Ip: {Ip} | Continent: {ContinentName} | Country: {CountryName} | Region: {RegionName} | City: {City} | Coords: {Latitude} : {Longitude}";
        }
    }
}
