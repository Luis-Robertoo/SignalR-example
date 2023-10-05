namespace Signal.Hubs;

public interface IDadosHub
{
    Task GetSala(string groupName);
    Task AddToGroup(string groupName);
    Task<string> WaitForMessage(string connectionId);
    Task SendToAll(string message);
}
