using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using ShipApi.Configuration;

namespace ShipApi;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string _workerName;
    private readonly string _url;
    private readonly HubConnection _connection;

    
    public Worker(ILogger<Worker> logger, IOptions<FleetSettings>? fleetSettings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _url = fleetSettings?.Value.Url ?? throw new ArgumentNullException(nameof(fleetSettings));
        _workerName = Environment.MachineName;
        
        _connection = new HubConnectionBuilder()
            .WithUrl(_url)
            .WithAutomaticReconnect()
            .Build();
        
        _connection.Reconnecting += error =>
        {
            _logger.LogWarning("SignalR RcConnection {Ex}", error);
            return Task.CompletedTask;
        };
        
        _connection.Reconnected += connectionId =>
        {
            _logger.LogInformation("SignalR Reconnected {ConnectionId}", connectionId);
            return Task.CompletedTask;
        };
        
        _connection.Closed += async (error) =>
        {
            _logger.LogInformation("SignalR Closed {Ex}", error);
            await Task.Delay(new Random().Next(0,5) * 1000);
            await _connection.StartAsync();
        };
    }
    

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var result = await InitConnectWithRetryAsync(_connection, CancellationToken.None);
        while (!stoppingToken.IsCancellationRequested)
        {
            
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }


    private static async Task<bool> InitConnectWithRetryAsync(HubConnection connection, CancellationToken token)
    {
        // Keep trying to until we can start or the token is canceled.
        while (true)
        {
            try
            {
                await connection.StartAsync(token);
                Debug.Assert(connection.State == HubConnectionState.Connected);
                return true;
            }
            catch when (token.IsCancellationRequested)
            {
                return false;
            }
            catch
            {
                // Failed to connect, trying again in 5000 ms.
                Debug.Assert(connection.State == HubConnectionState.Disconnected);
                await Task.Delay(2000);
            }
        }
    }
}