using Newtonsoft.Json;
using System;

/*
 * Authors: Ayden Nicholson, William Mouhtouris, Logan McCallum 
 */

namespace Project1_5101
{
    public class CityInfo
    {
        
        [JsonProperty("id")]
        public int CityID { get; set; }
        [JsonProperty("city")]
        public string CityName { get; set; }
        [JsonProperty("city_ascii")]
        public string CityAscii { get; set; }
        [JsonProperty("population")]
        public int Population { get; set; }
        [JsonProperty("admin_name")]
        public string Province { get; set; }
        [JsonProperty("lat")]
        public double Latitude { get; set; }
        [JsonProperty("lng")]
        public double Longitude { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("capital")]
        public string Capital { get; set; }

        public CityInfo() { }

        //Parameterized Constructor
        public CityInfo(int CityID, string CityName, string CityAscii, int Population, string Province, double Latitude, double Longitude, string country, string capital)
        {
            this.CityID = CityID;
            this.CityName = CityName;
            this.CityAscii = CityAscii;
            this.Population = Population;
            this.Province = Province;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Country = country;
            this.Capital = capital;
        }

        //Methods
        public string GetProvince()
        {
            return Province;
        }

        public int GetPopulation()
        {
            return Population;
        }

        public (double, double) GetLocation()
        {
            return (Latitude, Longitude);
        }
    }
}
