using ApplicationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.IServices
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetWeatherForecast();

        Task RefreshWeatherForecastAsync();
    }
}
