namespace Application.DTOs;

public class MessageCreateDTO
{
    public Guid ChannelId { get; set; }
    public required PartialUser Sender { get; set; }
    public required string Content { get; set; }
}
