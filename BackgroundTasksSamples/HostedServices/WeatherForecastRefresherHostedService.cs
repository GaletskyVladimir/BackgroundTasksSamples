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
    public class WeatherForecastRefresherHostedService : IHostedService
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ILogger<WeatherForecastRefresherHostedService> _logger;
        private const int JobInterval = 5000;
        private CancellationTokenSource _stoppingCts;
        private Task _executeTask;

        public WeatherForecastRefresherHostedService(IWeatherForecastService weatherForecastService, ILogger<WeatherForecastRefresherHostedService> logger)
        {
            this._weatherForecastService = weatherForecastService;
            this._logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Staring {jobName}", nameof(WeatherForecastRefresherHostedService));
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _executeTask = RefreshForecastAsync(_stoppingCts.Token);
            if (_executeTask.IsCompleted)
            {
                return _executeTask;
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping {jobName}", nameof(WeatherForecastRefresherHostedService));

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executeTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            }
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
                    _logger.LogError(ex, "Backgound task {taskName} threw an exception", nameof(WeatherForecastRefresherHostedService));
                }

                await Task.Delay(JobInterval, cancellationToken);
            }

            _logger.LogInformation("Finished loop");
        }
    }
}
