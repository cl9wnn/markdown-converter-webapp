using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class ValidateAuthorFilter(DocumentAccessService documentAccessService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasItems = context.HttpContext.Items.TryGetValue("documentId", out var documentIdObj) &
                       context.HttpContext.Items.TryGetValue("accountId", out var accountIdObj);

        if (!hasItems || documentIdObj == null || accountIdObj == null)
        {
            context.Result = new ForbidResult();
            return; 
        }

        var isParsed = Guid.TryParse(accountIdObj!.ToString(), out var accountId) & 
                       Guid.TryParse(documentIdObj!.ToString(), out var documentId);

        if (!isParsed)
        {
            context.Result = new ForbidResult();
            return; 
        }

        var isAuthor = await documentAccessService.IsAuthorAsync(documentId, accountId);

        if (!isAuthor)
        {
            context.Result = new BadRequestObjectResult(new { Error = "It is not your document!" });
            return; 
        }

        await next();
    }
}