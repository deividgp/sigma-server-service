namespace Application.Interfaces;

public interface IChannelService
{
    public Task<Channel> AddChannel(ChannelCreateDTO channelCreate);
    public Task DeleteChannel(ChannelDeleteDTO channelDelete);
    public Task<Message> AddMessage(MessageCreateDTO messageCreate);
    public Task<Channel?> GetChannel(Guid channelId);
    public Task<List<Message>?> GetMessages(MessageGetRequestDTO messageGetRequest);
}
