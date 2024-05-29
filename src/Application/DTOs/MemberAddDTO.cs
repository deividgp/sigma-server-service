namespace Application.DTOs;

public class MemberAddDTO
{
    public required Guid ServerId { get; set; }
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
}
