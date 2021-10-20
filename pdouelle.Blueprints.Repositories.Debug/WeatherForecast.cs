using System;
using pdouelle.Entity;

namespace pdouelle.Blueprints.Repositories.Debug
{
    public class WeatherForecast : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }
        public string Summary { get; set; }
    }
}