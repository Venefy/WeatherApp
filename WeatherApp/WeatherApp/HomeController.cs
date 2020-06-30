using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp
{
    public class HomeController : Controller
    {
        private static WeatherContext db;
        public HomeController(WeatherContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Weathers.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        public static void Add(Weather wew1)
        {
            db.Weathers.Add(wew1);
            db.SaveChanges();
        }
        public static void Create(IRow curRow)
        {

            string date = curRow.GetCell(0).StringCellValue;
            date += " " + curRow.GetCell(1).StringCellValue;

            Weather wew1 = new Weather
            {
                ID = curRow.RowNum,
                Date = Convert.ToDateTime(date)
            };

            wew1.T = Convert.ToDouble(curRow.GetCell(2).NumericCellValue, CultureInfo.InvariantCulture);
            wew1.Humidity = Convert.ToDouble(curRow.GetCell(3).NumericCellValue, CultureInfo.InvariantCulture);
            wew1.Td = Convert.ToDouble(curRow.GetCell(4).NumericCellValue, CultureInfo.InvariantCulture);
            wew1.AtmoPress = curRow.GetCell(5).NumericCellValue;
            wew1.Wind = curRow.GetCell(6).ToString();
            if (curRow.GetCell(7).CellType == CellType.String) { }
            if (curRow.GetCell(7).CellType == CellType.Numeric)
            {
                wew1.WindSpeed = curRow.GetCell(7).NumericCellValue;
            }
            if (curRow.GetCell(8).CellType == CellType.String) { }
            if (curRow.GetCell(8).CellType == CellType.Numeric)
            {
                wew1.Clouds = curRow.GetCell(8).NumericCellValue;
            }
            if (curRow.GetCell(9).CellType == CellType.String) { }
            if (curRow.GetCell(9).CellType == CellType.Numeric)
            {
                wew1.h = curRow.GetCell(9).NumericCellValue;
            }
            if (curRow.GetCell(10).CellType == CellType.String) { }
            if (curRow.GetCell(10).CellType == CellType.Numeric)
            {
                wew1.VV = curRow.GetCell(10).NumericCellValue;
            }
            //MessageBox.Show(curRow.LastCellNum.ToString());
            if (curRow.LastCellNum == 12)
            {
                wew1.Other = curRow.GetCell(11).ToString();
            }
            Add(wew1);
        }
    }
}
