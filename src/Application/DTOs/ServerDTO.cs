namespace Application.DTOs;

public class ServerDTO
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Icon { get; set; }
    public Guid UserId { get; set; }
}
