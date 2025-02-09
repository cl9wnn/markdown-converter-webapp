namespace Application.Models;

public class ChangeRecordDto
{
    public Guid DocumentId { get; set; }
    public string? UserName { get; set; }
    public DateTime Date { get; set; }
}