using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WeatherApp.Models
{
    public class WeatherListViewModel
    {
        public IEnumerable<Weather> Weathers { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public SelectList Months { get; set; }
        public void SetMonths()
        {
            var dateFormatInfo = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
            string[] monnames = dateFormatInfo.MonthNames;
            List<string> months = new List<string>();
            months.AddRange(monnames);
            months.Remove("");
            Months = new SelectList(months);
        }
        public SelectList Years { get; set; }
        public void SetYears()
        {
            List<string> years = new List<string>();
            years.Add("2010");
            years.Add("2011");
            years.Add("2012");
            years.Add("2013");
            Years = new SelectList(years);
        }
    }
    
}
