namespace Application.DTOs;

public class RoleDeleteDTO
{
    public required Guid ServerId { get; set; }
    public required Guid RoleId { get; set; }
}
