namespace Domain.Entities;

public class PartialChannel : Entity<Guid>
{
    public required string Name { get; set; }
}
