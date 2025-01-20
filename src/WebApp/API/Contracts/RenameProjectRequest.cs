namespace API.Contracts;

public class RenameProjectRequest
{
    public string? NewName { get; set; }
    public Guid DocumentId { get; set; }
}