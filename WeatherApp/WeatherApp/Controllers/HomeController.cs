using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Http;
using NPOI.XSSF.UserModel;
using System.IO;
using X.PagedList;
using System.Linq;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        static WeatherContext db;
        public HomeController(WeatherContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LoadTable()
        {

            return View();
        }

        protected IPagedList<Weather> GetPagedNames(int? page, List<Weather> WeathersQ)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand
            var listUnpaged = WeathersQ.AsEnumerable().ToList();

            // page the list
            const int pageSize = 7;
            var listPaged = listUnpaged.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

     
            [HttpGet]
        public IActionResult ViewTable(int? page, WeatherListViewModel wew)
        {            
            IQueryable<Weather> WeathersQ = db.Weathers.OrderBy(s => s.Date);
            if (wew.month != null && wew.year != null)
            {
                WeathersQ = db.Weathers.Where(p => p.Month == wew.month);
                WeathersQ = WeathersQ.Where(p => p.Year == wew.year);
            }
            if (wew.month == null && wew.year != null)
            {
                WeathersQ = db.Weathers.Where(p => p.Year == wew.year);
            }
            if (wew.month != null && wew.year == null)
            {
                TempData["msg"] = "<script>alert('Введите год');</script>";
            }
            var result = WeathersQ.ToList();


            ViewBag.Weathers1 = GetPagedNames(page, result);
            if (wew.Months==null)
            {
                wew.SetMonths();
            }
            if (wew.Years == null)
            {
                wew.SetYears();
            }

            WeatherListViewModel viewModel = new WeatherListViewModel
            {
                Weathers = WeathersQ,
                Months = wew.Months,
                Years = wew.Years
            };
            return View(viewModel);
        }

        

        public static void CreateWeather(IRow curRow)
        {

            string date = curRow.GetCell(0).StringCellValue;
            date += " " + curRow.GetCell(1).StringCellValue;

            Weather weather1 = new Weather
            {
                Date = Convert.ToDateTime(date)
            };
           weather1.Month =weather1.getMonth();
           weather1.Year =weather1.getYear();
           weather1.T = Convert.ToDouble(curRow.GetCell(2).NumericCellValue, CultureInfo.InvariantCulture);
           weather1.Humidity = Convert.ToDouble(curRow.GetCell(3).NumericCellValue, CultureInfo.InvariantCulture);
           weather1.Td = Convert.ToDouble(curRow.GetCell(4).NumericCellValue, CultureInfo.InvariantCulture);
           weather1.AtmoPress = curRow.GetCell(5).NumericCellValue;
           weather1.Wind = curRow.GetCell(6).ToString();
            if (curRow.GetCell(7).CellType == CellType.String) { }
            if (curRow.GetCell(7).CellType == CellType.Numeric)
            {
               weather1.WindSpeed = curRow.GetCell(7).NumericCellValue;
            }
            if (curRow.GetCell(8).CellType == CellType.String) { }
            if (curRow.GetCell(8).CellType == CellType.Numeric)
            {
               weather1.Clouds = curRow.GetCell(8).NumericCellValue;
            }
            if (curRow.GetCell(9).CellType == CellType.String) { }
            if (curRow.GetCell(9).CellType == CellType.Numeric)
            {
               weather1.h = curRow.GetCell(9).NumericCellValue;
            }
            if (curRow.GetCell(10).CellType == CellType.String) { }
            if (curRow.GetCell(10).CellType == CellType.Numeric)
            {
               weather1.VV = curRow.GetCell(10).NumericCellValue;
            }
            //MessageBox.Show(curRow.LastCellNum.ToString());
            if (curRow.LastCellNum == 12)
            {
               weather1.Other = curRow.GetCell(11).ToString();
            }
            db.Weathers.Add(weather1);
            
        }

        [HttpPost]
        public IActionResult LoadTable(IFormFileCollection excel)
        {
            foreach (var uploadedFile in excel)
            {
                if(uploadedFile != null)
                {
                    string excelName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", excelName);

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
                                    CreateWeather(curRow);
                                }

                            }
                            db.SaveChanges();
                            ViewBag.Message += "\t Таблицы из файла " + uploadedFile.FileName + " успешно загружены.   \n";

                        }
                        catch 
                        {
                            TempData["msg"] = "<script>alert('Неверный файл');</script>";
                            ViewBag.Message += "\t Неверный файл \n";
                            break;
                        }
                    }

                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
