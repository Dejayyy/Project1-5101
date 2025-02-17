using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;

namespace Project1_5101
{
    public class DataModeler
    {
        public delegate Dictionary<string, CityInfo> ParseDelegate(string fileName);

        //Parsing XML and storing into Dictionary
        public Dictionary<string, CityInfo> ParseXML(string fileName)
        {
            Dictionary<string, CityInfo> cityData = new Dictionary<string, CityInfo>();
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNodeList cities = doc.SelectNodes("//City");

            foreach (XmlNode node in cities)
            {
                CityInfo city = new CityInfo
                {
                    CityID = int.Parse(node["CityID"].InnerText),
                    CityName = node["CityName"].InnerText,
                    CityAscii = node["CityAscii"].InnerText,
                    Population = int.Parse(node["Population"].InnerText),
                    Province = node["Province"].InnerText,
                    Latitude = double.Parse(node["Latitude"].InnerText),
                    Longitude = double.Parse(node["Longitude"].InnerText)
                };

                if (!cityData.ContainsKey(city.CityName))
                    cityData.Add(city.CityName, city);
            }

            return cityData;
        }

        //Parsing JSON and storing into Dictionary
        public Dictionary<string, CityInfo> ParseJSON(string fileName)
        {
            Dictionary<string, CityInfo> cityData = new Dictionary<string, CityInfo>();
            string jsonText = File.ReadAllText(fileName);
            List<CityInfo> cities = JsonConvert.DeserializeObject<List<CityInfo>>(jsonText);

            foreach (var city in cities)
            {
                if (!cityData.ContainsKey(city.CityName))
                    cityData.Add(city.CityName, city);
            }

            return cityData;
        }

        //Parsing CSV and storing into Dictionary
        public Dictionary<string, CityInfo> ParseCSV(string fileName)
        {
            Dictionary<string, CityInfo> cityData = new Dictionary<string, CityInfo>();

            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CityInfo>();

                foreach (var city in records)
                {
                    if (!cityData.ContainsKey(city.CityName))
                        cityData.Add(city.CityName, city);
                }
            }

            return cityData;
        }

        //Parses a file based on the provided type
        public Dictionary<string, CityInfo> ParseFile(string fileName, string fileType)
        {
            ParseDelegate parser = fileType switch
            {
                "xml" => ParseXML,
                "json" => ParseJSON,
                "csv" => ParseCSV,
                _ => throw new ArgumentException("Invalid file type")
            };

            return parser.Invoke(fileName);
        }
    }
}
