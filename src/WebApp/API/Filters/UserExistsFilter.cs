using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class UserExistsFilter(IAccountService accountService): IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accountIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
        
        if (!Guid.TryParse(accountIdClaim, out var accountId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var isExists = await accountService.DoesUserExistAsync(accountId);

        if (!isExists)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        context.HttpContext.Items.Add("accountId", accountId);
        
        await next();
    }
}