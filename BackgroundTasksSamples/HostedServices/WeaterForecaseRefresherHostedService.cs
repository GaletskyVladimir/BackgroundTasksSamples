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
    public class WeaterForecaseRefresherHostedService : IHostedService
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ILogger<WeaterForecaseRefresherHostedService> _logger;
        private const int JobInterval = 5000;

        public WeaterForecaseRefresherHostedService(IWeatherForecastService weatherForecastService, ILogger<WeaterForecaseRefresherHostedService> logger)
        {
            this._weatherForecastService = weatherForecastService;
            this._logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Staring {jobName}", nameof(WeaterForecaseRefresherHostedService));

            RefreshForecastAsync(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping {jobName}", nameof(WeaterForecaseRefresherHostedService));

            return Task.CompletedTask;
        }

        private async Task RefreshForecastAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _weatherForecastService.RefreshWeatherForecastAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Backgound task {taskName} threw an exception", nameof(WeaterForecaseRefresherHostedService));
                }

                await Task.Delay(JobInterval, cancellationToken);
            }
        }
    }
}
