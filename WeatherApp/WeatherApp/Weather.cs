using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp.Models
{
        public class Weather
        {
            public int ID { get; set; }
            public DateTime Date { get; set; }
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
      
        
    public class WeatherContext : DbContext
    {
        public DbSet<Weather> Weathers { get; set; }
        public WeatherContext(DbContextOptions<WeatherContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }



}
