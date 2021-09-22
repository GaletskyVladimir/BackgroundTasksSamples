using ApplicationServices.IServices;
using ApplicationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private IEnumerable<WeatherForecast> _currentWeatherForecast;

        private readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastService()
        {
            RefreshWeatherForecastAsync();
        }

        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return _currentWeatherForecast;
        }

        public Task RefreshWeatherForecastAsync()
        {
            var rng = new Random();
            this._currentWeatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Task.CompletedTask;
        }
    }
}
