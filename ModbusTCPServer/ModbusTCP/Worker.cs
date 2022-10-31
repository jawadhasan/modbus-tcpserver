using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ModbusTCP
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly PrinterClient _printerClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _printerClient = new PrinterClient("127.0.0.20"); //read from config
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            _printerClient.Start();

            return Task.CompletedTask;
        }
    }
}
