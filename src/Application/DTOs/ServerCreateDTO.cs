namespace Application.DTOs;

public class ServerCreateDTO
{
    public required Guid OwnerId { get; set; }
    public required string OwnerUsername { get; set; }
    public required string ServerName { get; set; }
    public string? ServerPassword { get; set; }
}
