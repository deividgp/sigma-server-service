namespace Domain.Entities;

public class Channel : Entity<Guid>
{
    public required string Name { get; set; }
    public List<Message> Messages { get; set; } = [];
    public ChannelSettings ChannelSettings { get; set; } = new();
}
