namespace API.Controllers;

[ApiController]
public class ChannelController(IChannelService channelService) : ControllerBase
{
    private readonly IChannelService _channelService = channelService;

    [HttpGet("/api/Channel/Get")]
    public async Task<ActionResult> GetChannel(Guid channelId)
    {
        Channel? channel = await _channelService.GetChannel(channelId);

        if (channel is null)
            return NotFound();

        return Ok(channel);
    }

    [HttpGet("/api/Channel/GetMessages")]
    public async Task<ActionResult> GetMessages(MessageGetRequestDTO messageGetRequest)
    {
        return Ok(await _channelService.GetMessages(messageGetRequest));
    }
}
