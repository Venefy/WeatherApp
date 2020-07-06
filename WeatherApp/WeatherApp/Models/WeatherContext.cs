using Microsoft.EntityFrameworkCore;

namespace WeatherApp.Models
{
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
