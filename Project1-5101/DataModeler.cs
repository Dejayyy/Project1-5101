﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using System.Formats.Asn1;

namespace Project1_5101
{
    public class DataModeler
    {
        public delegate void ParseDelegate(string fileName);

        public void ParseXML(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
        }
        public void ParseJSON(string fileName) 
        {
            string jsonText = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<List<CityInfo>>(jsonText);
        }
        public void ParseCSV(string fileName) {
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CityInfo>();
            }
        }


        //ParceFile is a placeholder for the variable in statistics
        public Dictionary<string, CityInfo> ParseFile(string fileName, string fileType)
        {
            Dictionary<string, CityInfo> cityData = new Dictionary<string, CityInfo>();

            ParseDelegate parser = fileType switch
            {
                "xml" => ParseXML,
                "json" => ParseJSON,
                "csv" => ParseCSV,
                _ => throw new ArgumentException("Invalid file type")
            };

            parser.Invoke(fileName);
            return cityData;
        }
    }
}
