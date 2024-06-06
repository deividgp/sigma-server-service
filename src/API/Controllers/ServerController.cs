namespace API.Controllers;

[ApiController]
public class ServerController(IServerService serverService) : ControllerBase
{
    private readonly IServerService _serverService = serverService;

    [HttpGet("/api/Server/Get/{serverId}")]
    public async Task<ActionResult> GetServer(Guid serverId)
    {
        Server? server = await _serverService.GetServer(serverId);

        if (server is null)
            return NotFound();

        return Ok(server);
    }
}
