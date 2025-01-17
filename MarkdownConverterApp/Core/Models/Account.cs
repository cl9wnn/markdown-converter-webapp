namespace Core.Models;

public class Account
{
    public Guid AccountId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? PasswordHash { get; set; }
}