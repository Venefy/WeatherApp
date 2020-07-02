using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace WeatherApp.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public IFormFile myfile { set; get; }
    }
}
