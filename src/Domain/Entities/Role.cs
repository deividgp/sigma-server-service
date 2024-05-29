namespace Domain.Entities;

public class Role : Entity<Guid>
{
    public required string Name { get; set; }
}
