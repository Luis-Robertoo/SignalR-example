using Microsoft.AspNetCore.SignalR;
using Signal.Services;

namespace Signal.Hubs;

public class DadosHub : Hub, IDadosHub
{
    private readonly IUsersService _usersService;

    public DadosHub(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("Send", $"{Context.ConnectionId} saiu.");
        await _usersService.RemoveUser(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task GetSala(string groupName)
    {
        var sala = await _usersService.GetSalaForName(groupName);
        await Clients.Group(groupName).SendAsync("Send", sala);
    }

    public async Task AddToGroup(string groupName)
    {
        var sala = await _usersService.CreateOrUpdateSala(groupName, Context.ConnectionId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} entrou no grupo {groupName}.");

    }

    public async Task SendToGroup(string groupName, string message)
    {
        await Clients.Group(groupName).SendAsync("Send", $"{groupName} - {Context.ConnectionId} - Mensagem: {message}");
    }

    public async Task<string> WaitForMessage(string connectionId)
    {
        var message = await Clients.Client(connectionId).InvokeAsync<string>("mensagem", CancellationToken.None);
        return message;
    }

    public async Task SendToAll(string message)
    {
        await Clients.All.SendAsync("Send", $"Todos - API - Mensagem: {message}");
    }
}
