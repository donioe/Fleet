using Fleet.Contracts.Models;
using FleetApi.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace FleetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FleetController : ControllerBase
{
    private readonly ILogger<FleetController> _logger;
    private readonly IFleetRepo _repo;
    private readonly FleetHub _fleetHub;

    public FleetController(ILogger<FleetController> logger, IFleetRepo repo, FleetHub fleetHub)
    {
        _logger = logger;
        _repo = repo;
        _fleetHub = fleetHub;
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<WeatherForecast> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //         {
    //             Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //             TemperatureC = Random.Shared.Next(-20, 55),
    //             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //         })
    //         .ToArray();
    // }
    
    [HttpGet(Name = "GetConnList")]
    public IEnumerable<string> GetConnList()
    {
        return _repo.GetConnectionsList();
    }

    [HttpPost(Name = "SendComment")]
    public async Task<ExecuteCommandResponse> SendCommand(string id)
    {
        var psCommand = new CommandArguments()
        {
            ExecutablePath = "ps",
            Arguments = "aux"
            
        };
        var com = await _fleetHub.SendCommand(id, psCommand);
        return com;
    }

}