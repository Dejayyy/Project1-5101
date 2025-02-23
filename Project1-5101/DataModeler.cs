using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;

/*
 * Authors: Ayden Nicholson, William Mouhtouris, Logan McCallum 
 */

namespace Project1_5101
{
    public class DataModeler
    {
        private Dictionary<string, CityInfo> cityData;
        public delegate void ParseDelegate(string fileName);

        public DataModeler()
        {
            cityData = new Dictionary<string, CityInfo>();
        }

        //Parsing XML and storing into Dictionary
        public void ParseXML(string fileName)
        {
            try
            {
                cityData = new Dictionary<string, CityInfo>();
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNodeList? cities = doc.SelectNodes("//CanadaCities/CanadaCity");

                foreach (XmlNode node in cities)
                {
                    try
                    {

                        //Skip entries with missing city name
                        var cityNameNode = node.SelectSingleNode("city");
                        if (cityNameNode == null || string.IsNullOrWhiteSpace(cityNameNode.InnerText))
                        {
                            continue;
                        }

                        CityInfo city = new CityInfo
                        {
                            CityID = int.Parse(node.SelectSingleNode("id")?.InnerText ?? "0"),
                            CityName = cityNameNode.InnerText.Trim(),
                            CityAscii = node.SelectSingleNode("city_ascii")?.InnerText?.Trim() ?? "",
                            Population = int.Parse(node.SelectSingleNode("population")?.InnerText ?? "0"),
                            Province = node.SelectSingleNode("admin_name")?.InnerText?.Trim() ?? "",
                            Latitude = double.Parse(node.SelectSingleNode("lat")?.InnerText ?? "0"),
                            Longitude = double.Parse(node.SelectSingleNode("lng")?.InnerText ?? "0"),
                            Country = node.SelectSingleNode("country")?.InnerText?.Trim() ?? "",
                            Capital = node.SelectSingleNode("capital")?.InnerText?.Trim() ?? ""
                        };

                        if (!cityData.ContainsKey(city.CityName))
                            cityData.Add(city.CityName, city);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing city node: {ex.Message}");
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing XML file: {ex.Message}");
            }

        }

        //Parsing JSON and storing into Dictionary
        public void ParseJSON(string fileName)
        {
            cityData = new Dictionary<string, CityInfo>();
            string jsonText = File.ReadAllText(fileName);
            List<Dictionary<string, string>>? cities = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonText);

            if (cities == null)
                throw new Exception("Failed to deserialize JSON data.");

            foreach (var cityDataEntry in cities)
            {
                try
                {
                    var city = new CityInfo
                    {
                        CityID = int.TryParse(cityDataEntry["id"], out int id) ? id : 0,
                        CityName = cityDataEntry["city"],
                        CityAscii = cityDataEntry["city_ascii"],
                        Population = int.TryParse(cityDataEntry["population"], out int population) ? population : 0,
                        Province = cityDataEntry["admin_name"],
                        Latitude = double.TryParse(cityDataEntry["lat"], out double lat) ? lat : 0.0,
                        Longitude = double.TryParse(cityDataEntry["lng"], out double lng) ? lng : 0.0,
                        Country = cityDataEntry["country"],
                        Capital = cityDataEntry["capital"]
                    };

                    if (!string.IsNullOrWhiteSpace(city.CityName))
                        cityData[city.CityName] = city;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing city data: {ex.Message}");
                }
            }

            Console.WriteLine($"JSON Parsing Complete. {cityData.Count} cities loaded.");
        }


        //Parsing CSV and storing into Dictionary
        public void ParseCSV(string fileName)
        {
            cityData.Clear();
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var city = new CityInfo
                    {
                        CityName = csv.GetField("city"),
                        CityAscii = csv.GetField("city_ascii"),
                        Latitude = csv.GetField<double>("lat"),
                        Longitude = csv.GetField<double>("lng"),
                        Country = csv.GetField("country"),
                        Province = csv.GetField("admin_name"),
                        Capital = csv.GetField("capital"),
                        Population = csv.GetField<int>("population"),
                        CityID = csv.GetField<int>("id")
                    };

                    if (!string.IsNullOrWhiteSpace(city.CityName))
                    {
                        cityData[city.CityName] = city; 
                    }
                }
               
            }

            
        }

        //Parses a file based on the provided type
        public Dictionary<string, CityInfo>? ParseFile(string fileName, string fileType)
        {
            ParseDelegate parser;

            
            switch (fileType.ToLower())
            {
                case "csv":
                    parser = ParseCSV;
                    break;
                case "json":
                    parser = ParseJSON;
                    break;
                case "xml":
                    parser = ParseXML;
                    break;
                default:
                    throw new ArgumentException("Unsupported file type");
            }

            parser.Invoke(fileName);
            
            return cityData;
        }
    }
}
