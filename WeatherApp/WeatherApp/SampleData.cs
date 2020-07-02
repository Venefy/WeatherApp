using System;
using System.Linq;
using WeatherApp.Models;

namespace WeatherApp
{
    public static class SampleData
    {
        public static void Initialize(WeatherContext context)
        {
            if (!context.Weathers.Any())
            {
                context.Weathers.AddRange(
                    new Weather
                    {
                        Date = new DateTime(2008, 6, 1, 7, 47, 0),
                        T = 20
                    },
                    new Weather
                    {
                        Date = new DateTime(2007, 6, 1, 7, 47, 0),
                        T = 10
                    },
                    new Weather
                    {
                        Date = new DateTime(2006, 6, 1, 7, 47, 0),
                        T = 0
                    }
                ); ;
                context.SaveChanges();
            }
        }
    }
}