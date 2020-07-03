using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class Weather 
    {
        public string getMonth()
        {
            return Date.ToString("MMMM", new CultureInfo("ru-RU")) ;
         }
        public string getYear()
        {
            return Date.Year.ToString();
        }
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public double T { get; set; }
        public double Humidity { get; set; }
        public double Td { get; set; }
        public double AtmoPress { get; set; }
        public string Wind { get; set; }
        public double WindSpeed { get; set; }
        public double Clouds { get; set; }
        public double h { get; set; }
        public double VV { get; set; }
        public string Other { get; set; }
    }
    
}
