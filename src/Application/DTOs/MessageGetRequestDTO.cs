namespace Application.DTOs;

public class MessageGetRequestDTO
{
    public Guid ChannelId { get; set; }
    public required string Search { get; set; }
}
