using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FvpWebAppWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var counter = 0;
            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Run(() =>
                {
                    _logger.LogInformation($"Worker running at: {DateTimeOffset.Now} counter: {counter}");
                    for (int i = 0; i < 10; i++)
                    {
                        Console.Write("A");
                    }
                    counter++;
                }).ConfigureAwait(true);

                //await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
                if (counter > 10)
                {
                    await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
                    await base.StopAsync(stoppingToken).ConfigureAwait(false);
                    Environment.Exit(0);
                };
            }
        }
    }
}
