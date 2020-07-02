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
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Http;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        static WeatherContext db;
        ApplicationContext _context;
        IHostEnvironment _appEnvironment;
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


        public static void CreateWew(IRow curRow)
        {

            string date = curRow.GetCell(0).StringCellValue;
            date += " " + curRow.GetCell(1).StringCellValue;

            Weather wew1 = new Weather
            {
                Date = Convert.ToDateTime(date)
            };

            wew1.T = Convert.ToDouble(curRow.GetCell(2).NumericCellValue, CultureInfo.InvariantCulture);
            db.Weathers.Add(wew1);
            db.SaveChanges();
        }


        [HttpPost]
        public IActionResult AddNew(IFormFileCollection excel)
        {
            foreach (var uploadedFile in excel)
            {
                if (uploadedFile != null)
                {
                    //Set Key Name
                    string excelName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);

                    //Get url To Save
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", excelName);

                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        uploadedFile.CopyTo(stream);
                        try
                        {
                            Stream file = uploadedFile.OpenReadStream();
                            XSSFWorkbook xssfwb;
                            {
                                xssfwb = new XSSFWorkbook(file);
                            }

                            for (int i = 0; i < 12; i++)
                            {
                                ISheet sheet = xssfwb.GetSheetAt(i);
                                for (int row = 4; row <= sheet.LastRowNum; row++)
                                {
                                    var curRow = sheet.GetRow(row);
                                    CreateWew(curRow);
                                }

                            }
                            MessageBox.Show("\t Таблицы из файла " + uploadedFile.FileName + " успешно загружены.   \n");
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }


                }
            }
            return RedirectToAction("LoadTable");
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
