using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class Weather
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double T { get; set; }
    }
}
