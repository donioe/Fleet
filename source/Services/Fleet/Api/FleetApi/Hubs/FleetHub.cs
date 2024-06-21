using System.Collections.Concurrent;
using Fleet.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace FleetApi.Hubs;

public class FleetHub : Hub<IShip>
{
    private readonly FleetRepo _fleetRepo;

    public FleetHub()
    {
        _fleetRepo = new FleetRepo();
    }

    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId; // get the connectionId
        var name = $"Client{_fleetRepo.GetNextId}";
        _fleetRepo.AddNewConnection(name, connectionId);
        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // remove connection
        // remove/keep pending messages
        
        return Task.CompletedTask;
    }
    
}