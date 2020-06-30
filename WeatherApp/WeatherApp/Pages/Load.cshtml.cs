using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WeatherApp.Models;

namespace WeatherApp.Pages
{
    public class LoadModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Загрузка архивов погодных условий в городе Москве";
        }
        public void AddFile(IFormFileCollection uploads)
        {
            foreach (var uploadedFile in uploads)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroottry
                try
                {
                    XSSFWorkbook xssfwb;
                    Stream file = uploadedFile.OpenReadStream();
                    {
                        xssfwb = new XSSFWorkbook(file);
                    }

                    for (int i = 0; i < 12; i++)
                    {
                        ISheet sheet = xssfwb.GetSheetAt(i);
                        for (int row = 4; row <= sheet.LastRowNum; row++)
                        {
                            var curRow = sheet.GetRow(row);
                            HomeController.Create(curRow);
                        }
                        
                    }
                    //myLabel.Text += "\t Таблицы из файла " + uploadedFile.FileName + " успешно загружены.   \n";
                }
                    catch
                {
                    MessageBox.Show("Таблица неверного формата");
                }

            }
    }   } 
}
