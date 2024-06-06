namespace API.Controllers;

[ApiController]
public class ChannelController(IChannelService channelService) : ControllerBase
{
    private readonly IChannelService _channelService = channelService;

    [HttpGet("/api/Channel/Get/{channelId}")]
    public async Task<ActionResult> GetChannel(Guid channelId)
    {
        Channel? channel = await _channelService.GetChannel(channelId);

        if (channel is null)
            return NotFound();

        return Ok(channel);
    }

    [HttpGet("/api/Channel/GetMessages/{channelId}/{search}")]
    public async Task<ActionResult> GetMessages(Guid channelId, string search)
    {
        List<Message>? messages = await _channelService.GetMessages(new MessageGetRequestDTO()
        {
            ChannelId = channelId,
            Search = search
        });

        if (messages is null) return NotFound();

        return Ok(messages);
    }
}
