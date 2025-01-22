namespace API.Extensions;

public static class HttpContextExtensions
{
    public static Guid? GetAccountId(this HttpContext context)
    {
        return context.Items.TryGetValue("accountId", out var accountIdObj) && accountIdObj is Guid accountId
            ? accountId
            : null;
    }
}