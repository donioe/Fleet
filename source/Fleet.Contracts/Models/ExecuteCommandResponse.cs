namespace Fleet.Contracts.Models;

public class ExecuteCommandResponse
{
    public bool Success { get; set; }
    public int ExitCode { get; set; }
    public string Out { get; set; }
    public string Error { get; set; }
}