using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace WeatherApp.Models
{
    public class WeatherListViewModel
    {
        public IEnumerable<Weather> Weathers { get; set; }
        public SelectList Months { get; set; }
        public SelectList Years { get; set; }
    }
}
