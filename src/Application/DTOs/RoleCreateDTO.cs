namespace Application.DTOs;

public class RoleCreateDTO
{
    public required Guid ServerId { get; set; }
    public required string RoleName { get; set; }
}
