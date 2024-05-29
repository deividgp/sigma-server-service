namespace Application.DTOs;

public class ChannelCreateDTO
{
    public Guid ServerId { get; set; }
    public required string ChannelName { get; set; }
}
