namespace Application.DTOs;

public class MemberRemoveDTO
{
    public required Guid ServerId { get; set; }
    public required Guid MemberId { get; set; }
}
