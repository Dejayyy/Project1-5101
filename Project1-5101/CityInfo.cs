using System;

namespace Project1_5101
{
    public class CityInfo
    {
        //Properties
        public int CityID;
        public string CityName;
        public string CityAscii;
        public int Population;
        public string Province;
        public double Latitude;
        public double Longitude;

        public CityInfo() { }

        //Parameterized Constructor
        public CityInfo(int CityID, string CityName, string CityAscii, int Population, string Province, double Latitude, double Longitude)
        {
            this.CityID = CityID;
            this.CityName = CityName;
            this.CityAscii = CityAscii;
            this.Population = Population;
            this.Province = Province;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
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
