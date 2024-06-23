using Fleet.Contracts;
using Fleet.Contracts.Models;
using Microsoft.AspNetCore.SignalR;

namespace FleetApi.Hubs;

public class FleetHub : Hub<IShip>
{
    private readonly IFleetRepo _fleetRepo;

    public FleetHub(IFleetRepo fleetRepo)
    {
        _fleetRepo = fleetRepo;
    }

    public async Task<ExecuteCommandResponse> SendCommand(string conn, CommandArguments commandArguments)
    {
        var response = new ExecuteCommandResponse()
        {
            Success = false,
            ExitCode = -1,
            Out = "<empty>",
            Error = "<empty>"
                
        };
        if (!_fleetRepo.IsExists(conn))
        {
            return response;
        }
        
        try
        {
            response = await Clients.Client(conn).ExecuteProcess(commandArguments).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            response.Error = e.Message;
        }

        return response;
    }

    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId; // get the connectionId
        var name = $"Client{_fleetRepo.GetNextId}";
        _fleetRepo.AddConnection(name, connectionId);
        return Task.CompletedTask;
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // remove connection
        // remove/keep pending messages
        
        return Task.CompletedTask;
    }
    
}