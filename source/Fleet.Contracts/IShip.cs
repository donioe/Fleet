using Fleet.Contracts.Models;

namespace Fleet.Contracts;

public interface IShip
{
    Task<ExecuteCommandResponse> ExecuteProcess(CommandArguments commandArguments);
    
    //Task<ExecuteCommandResponse> WriteFile(CommandArguments commandArguments);
}