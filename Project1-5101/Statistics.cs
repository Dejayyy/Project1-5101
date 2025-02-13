using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Project1_5101
{
    public class Statistics
    {
        private Dictionary<string, CityInfo> CityCatalogue { get; set; }

        //You may get the value of the CityCatalogue here in thisconstructor by calling the DataModeler.Parse method.
        public Statistics(string fileName, string fileType)
        {

        }
        public CityInfo DisplayCityInformation(string cityName)
        {
            // TODO: Implement logic to display city information
            // Handle cases where multiple cities share the same name
            throw new NotImplementedException();
        }
        public CityInfo DisplayLargestPopulationCity(string province)
        {
            // TODO: Implement logic to find largest population city in province
            throw new NotImplementedException();
        }
        public CityInfo DisplaySmallestPopulationCity(string province)
        {
            // TODO: Implement logic to find smallest population city in province
            throw new NotImplementedException();
        }

        public (CityInfo largerCity, int city1Population, int city2Population) CompareCitiesPopluation(string city1, string city2)
        {
            // TODO: Implement logic to compare populations of two cities
            throw new NotImplementedException();
        }

        public void ShowCityOnMap(string cityName, string province)
        {
            // TODO: Implement logic to show city on map using Lat/Long
            throw new NotImplementedException();
        }

        public double CalculateDistanceBetweenCities(string city1, string city2)
        {
            // TODO: Implement logic to calculate distance between cities using Google API or other
            throw new NotImplementedException();
        }

        // Province Methods
        public int DisplayProvincePopulation(string provinceName)
        {
            // TODO: Implement logic to calculate total population of province
            throw new NotImplementedException();
        }

        public List<CityInfo> DisplayProvinceCities(string provinceName)
        {
            // TODO: Implement logic to list all cities in province
            throw new NotImplementedException();
        }

        public List<(string province, int population)> RankProvincesByPopulation()
        {
            // TODO: Implement logic to rank provinces by population 
            throw new NotImplementedException();
        }

        public List<(string province, int cityCount)> RankProvincesByCities()
        {
            // TODO: Implement logic to rank provinces by number of cities 
            throw new NotImplementedException();
        }

        public (string capital, double latitude, double longitude) GetCapital(string province)
    {
            // TODO: Implement logic to get province capital with Lat/Long
            throw new NotImplementedException();
        }
    }
}
