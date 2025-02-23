using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

/*
 * Authors: Ayden Nicholson, William Mouhtouris, Logan McCallum 
 */

namespace Project1_5101
{
    public class Statistics
    {
        private Dictionary<string, CityInfo>? CityCatalogue { get; set; }

        //Get the value of the CityCatalogue here in thisconstructor by calling the DataModeler.Parse method.
        public Statistics(string fileName, string fileType)
        {
            DataModeler dataModeler = new DataModeler();
            CityCatalogue = dataModeler.ParseFile(fileName, fileType);
        }

        public CityInfo DisplayCityInformation(string cityName)
        {
            //Handle cases where multiple cities share the same name
            var matchingCities = CityCatalogue?.Values.Where(city => city.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!matchingCities.Any())
                throw new Exception($"City '{cityName}' not found.");
            

            if (matchingCities.Count() > 1)
            {
                Console.WriteLine($"\nMultiple cities found with the name {cityName}: ");
                for (int i=0; i <matchingCities.Count(); i++)
                {
                    Console.WriteLine($"{i+1}) {matchingCities[i].CityName}, {matchingCities[i].Province}");
                }
                Console.WriteLine("\nPlease select a city (number): ");
                if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= matchingCities.Count) 
                    return matchingCities[selection-1];
                
                throw new Exception("Invalid selection.");
            }
            return matchingCities.First();
        }

        //Takes the population of both cities and compares it and outputs the largest
        public CityInfo DisplayLargestPopulationCity(string province)
        {
            var city = CityCatalogue.Values
               .Where(c => c.Province.Equals(province, StringComparison.OrdinalIgnoreCase))
               .MaxBy(c => c.Population);

            if (city == null)          
                throw new Exception($"No cities found in province {province}. ");
            
            return city;
        }

        //Takes the population of both cities and compares it and outputs the smallest
        public CityInfo DisplaySmallestPopulationCity(string province)
        {
            var city = CityCatalogue.Values
               .Where(c => c.Province.Equals(province, StringComparison.OrdinalIgnoreCase))
               .MinBy(c => c.Population);

            if (city == null)
                throw new Exception($"No cities found in province {province}. ");
            
            return city;
        }

        //Compares cities populations
        public (CityInfo largerCity, int city1Population, int city2Population) CompareCitiesPopluation(string city1, string city2)
        {
            var firstCity = DisplayCityInformation(city1);
            var secondCity = DisplayCityInformation(city2);

            return firstCity.Population > secondCity.Population
                ? (firstCity, firstCity.Population, secondCity.Population)
                : (secondCity, firstCity.Population, secondCity.Population);
        }

        //Shows the city on the map
        public void ShowCityOnMap(string cityName, string province)
        {
            var city = CityCatalogue.Values
        .FirstOrDefault(c => c.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase) &&
                             c.Province.Equals(province, StringComparison.OrdinalIgnoreCase));

            if (city == null)
            {
                Console.WriteLine($"City '{cityName}' not found in province '{province}'.");
                return;
            }

            string googleMapsUrl = $"https://www.google.com/maps/search/?api=1&query={city.Latitude},{city.Longitude}";

            Console.WriteLine($"Opening Google Maps for {cityName}, {province}: {googleMapsUrl}");

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = googleMapsUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening map: {ex.Message}");
            }
        }

        //Calculates the distance between cities
        public double CalculateDistanceBetweenCities(string city1, string city2)
        {
            var firstCity = DisplayCityInformation(city1);
            var secondCity = DisplayCityInformation(city2);

            if (firstCity == null || secondCity == null)
                throw new Exception("One or both cities were not found.");
            

            double R = 6371; //Radius of the Earth in kilometers

            double lat1 = firstCity.Latitude * (Math.PI / 180);
            double lon1 = firstCity.Longitude * (Math.PI / 180);
            double lat2 = secondCity.Latitude * (Math.PI / 180);
            double lon2 = secondCity.Longitude * (Math.PI / 180);

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = R * c; //Kilometers

            return distance;
        }

        //Province Methods

        //Displays Province populations
        public int DisplayProvincePopulation(string provinceName)
        {
            var population = CityCatalogue.Values
               .Where(city => city.Province.Equals(provinceName, StringComparison.OrdinalIgnoreCase))
               .Sum(city => city.Population);

            if(population == 0)
                throw new Exception($"No cities found in province {provinceName}"       );
            
            return population;
        }

        //Displays the cities in the province
        public List<CityInfo> DisplayProvinceCities(string provinceName)
        {
            var cities = CityCatalogue.Values
                .Where(city => city.Province.Equals(provinceName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!cities.Any())
                throw new Exception($"No cities found in province");
            
            return cities;
        }

        //Ranked highest to lowest in population
        public List<(string province, int population)> RankProvincesByPopulation()
        {
            return CityCatalogue.Values
         .GroupBy(city => city.Province)
         .Select(group => (group.Key, group.Sum(city => city.Population)))
         .OrderByDescending(entry => entry.Item2)
         .ToList();
        }

        //Ranks the provinces by cities
        public List<(string province, int cityCount)> RankProvincesByCities()
        {
            return CityCatalogue.Values
                .GroupBy(city => city.Province)
                .Select(group => (group.Key, group.Count()))
                .OrderByDescending(entry => entry.Item2)
                .ToList();
        }

        //Gets the capitial of the province
        public (string capital, double latitude, double longitude) GetCapital(string province)
        {
            var capitalCity = CityCatalogue.Values
                .Where(city => city.Province.Equals(province, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (capitalCity == null)
                throw new Exception($"Capital not found for province '{province}'.");

            return (capitalCity.CityName, capitalCity.Latitude, capitalCity.Longitude);
        }
    }
}
