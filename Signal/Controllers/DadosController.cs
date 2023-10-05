using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Signal.Hubs;
using Signal.Services;

namespace Signal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DadosController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly IHubContext<DadosHub> _hubContext;

    public DadosController(IUsersService usersService, IHubContext<DadosHub> hubContext)
    {
        _usersService = usersService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult> Jogo([FromQuery] string message)
    {
        await _hubContext.Clients.All.SendAsync("Send", $"Todos - API - Mensagem: {message}");
        return Ok();
    }

}
