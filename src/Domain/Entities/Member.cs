namespace Domain.Entities;

public class Member : Entity<Guid>
{
    public required string Username { get; set; }
    public List<Role> Roles { get; set; } = [];
}
