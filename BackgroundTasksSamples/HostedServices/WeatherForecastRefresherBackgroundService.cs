using ApplicationServices.IServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTasksSamples.HostedServices
{
    public class WeatherForecastRefresherBackgroundService : BackgroundService
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ILogger<WeatherForecastRefresherBackgroundService> _logger;
        private const int JobInterval = 5000;

        public WeatherForecastRefresherBackgroundService(IWeatherForecastService weatherForecastService, ILogger<WeatherForecastRefresherBackgroundService> logger)
        {
            this._weatherForecastService = weatherForecastService;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Staring {jobName}", nameof(WeatherForecastRefresherBackgroundService));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _weatherForecastService.RefreshWeatherForecastAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Backgound task {taskName} threw an exception", nameof(WeatherForecastRefresherBackgroundService));
                }

                await Task.Delay(JobInterval, stoppingToken);
            }

            _logger.LogInformation("Stopping {jobName}", nameof(WeatherForecastRefresherHostedService));
        }
    }
}
