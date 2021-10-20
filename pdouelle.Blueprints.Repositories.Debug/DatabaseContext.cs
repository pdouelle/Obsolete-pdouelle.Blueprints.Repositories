using Microsoft.EntityFrameworkCore;

namespace pdouelle.Blueprints.Repositories.Debug
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}