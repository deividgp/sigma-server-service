namespace Application.DTOs;

public class SendAddMemberDTO
{
    public required string ServerName { get; set; }
    public required string ServerPassword { get; set; }
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
}
