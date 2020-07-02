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
            return Date.ToString("YY");
        }
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double T { get; set; }
    }
    
}
