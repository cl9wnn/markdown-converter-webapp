namespace Core.Models;

public class Account
{
    public Guid AccountId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? PasswordHash { get; set; }

    private Account(Guid accountId, string? email, string? firstName, string? passwordHash)
    {
        AccountId = accountId;
        Email = email;
        FirstName = firstName;
        PasswordHash = passwordHash;
    }

    public static Account CreateAccount(Guid accountId, string email, string? firstName, string? passwordHash)
    {
        return new Account(accountId, email, firstName, passwordHash);
    }
}