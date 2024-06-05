namespace Domain.Entities;

public class Server : Entity<Guid>
{
    public required string Name { get; set; }
    public required string Password { get; set; }
    public string? Icon { get; set; }
    public ServerSettings ServerSettings { get; set; } = new();
    public List<Member> Members { get; set; } = [];
    public List<PartialChannel> Channels { get; set; } = [];
    public List<Role> Roles { get; set; } = [];
    public Guid OwnerId { get; set; }
}
