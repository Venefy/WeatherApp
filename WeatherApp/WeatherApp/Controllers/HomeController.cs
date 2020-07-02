using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.OracleClient;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        WeatherContext db;
        public HomeController(WeatherContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(db.Weathers.ToList());
        }
        public IActionResult LoadTable()
        {

            return View(db.Weathers.ToList());
        }

        public IActionResult ViewTable(string month, string year)
        {

            IQueryable<Weather> weathers = db.Weathers;
            if (month != null)
            {
                weathers = weathers.Where(p => p.getMonth() == month);
            }
            if (year != null)
            {
                weathers = weathers.Where(p => p.getYear() == year);
            }

            var dateFormatInfo = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
            string[] monnames = dateFormatInfo.MonthNames;
            List<string> months = new List<string>();
            months.Add("");
            months.AddRange(monnames);
            months.Remove("");
            List<string> years = new List<string>();
            years.Add("");
            years.Add("10");
            years.Add("07");
            years.Add("08");

            WeatherListViewModel viewModel = new WeatherListViewModel
            {
                Weathers = weathers.ToList(),
                Months = new SelectList(months),
                Years = new SelectList(years)
            };
            return View(viewModel);
        }
    


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
