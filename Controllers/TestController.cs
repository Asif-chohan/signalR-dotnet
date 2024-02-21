using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.Hubs; // Adjust the namespace to match your project

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;

    public TestController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet("send")]
    public async Task<IActionResult> SendMessage()
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", "Hello from the server!");
        return Ok("Message sent to all clients");
    }
}
