using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerExample
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HubConnection _connection;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;

            _configuration = configuration;
            _connection = new HubConnectionBuilder()
                .WithUrl(new Uri(new Uri(_configuration["BaseAddress"]), "hub"))
                .Build();
            _connection.ServerTimeout = TimeSpan.FromSeconds(45);
            _connection.Closed += ClosedAsync;
            _connection.On("MessageEvent", (string message) =>
            {
                _logger.LogInformation($"Message:{message}");
            });
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await ConnectedAsync();
            await base.StartAsync(cancellationToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                object[] value = {};
                await _connection.InvokeCoreAsync("MailCollectionEvent", value, stoppingToken);
                await _connection.InvokeCoreAsync("ProofOfDeliveryEvent", value, stoppingToken);

                await Task.Delay((5 * 60000), stoppingToken);
            }
        }

        private async Task ClosedAsync(Exception error)
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await ConnectedAsync();
        }
        private async Task ConnectedAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }
        }
    }
}
