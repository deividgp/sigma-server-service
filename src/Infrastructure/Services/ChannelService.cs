namespace Infrastructure.Services;

public class ChannelService(
    IRepository<Channel, Guid> channelRepository,
    IRepository<Server, Guid> serverRepository
) : IChannelService
{
    private readonly IRepository<Channel, Guid> _channelRepository = channelRepository;
    private readonly IRepository<Server, Guid> _serverRepository = serverRepository;

    public async Task<Channel> AddChannel(ChannelCreateDTO channelCreate)
    {
        Channel channel = new() { Id = Guid.NewGuid(), Name = channelCreate.ChannelName };
        await _channelRepository.CreateAsync(channel);

        PartialChannel partialChannel =
            new() { Id = channel.Id, Name = channelCreate.ChannelName, };
        await _serverRepository.UpdateOneAsync(
            c => c.Id == channelCreate.ServerId,
            Builders<Server>.Update.Push(c => c.Channels, partialChannel)
        );

        return channel;
    }

    public async Task<Message> AddMessage(MessageCreateDTO messageCreate)
    {
        Message message =
            new()
            {
                Id = Guid.NewGuid(),
                Sender = messageCreate.Sender,
                Content = messageCreate.Content,
                Timestamp = DateTime.Now
            };

        await _channelRepository.UpdateOneAsync(
            c => c.Id == messageCreate.ChannelId,
            Builders<Channel>.Update.Push(c => c.Messages, message)
        );

        return message;
    }

    public async Task DeleteChannel(ChannelDeleteDTO channelDelete)
    {
        await _channelRepository.RemoveAsync(channelDelete.ChannelId);

        await _serverRepository.UpdateOneAsync(
            c => c.Id == channelDelete.ServerId,
            Builders<Server>.Update.PullFilter(
                c => c.Channels,
                c => c.Id == channelDelete.ChannelId
            )
        );
    }

    public async Task<Channel?> GetChannel(Guid channelId)
    {
        return await _channelRepository.GetByIdAsync(channelId);
    }
}
