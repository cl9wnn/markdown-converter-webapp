using Application.Interfaces.Services;
using Application.Services;
using Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class ValidatePermissionFilter(IDocumentAccessService documentAccessService, RequiredAccessLevel requiredPermissionType): IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!TryGetIdsFromContext(context, out var documentId, out var accountId))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!await HasRequiredPermission(documentId, accountId))
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }

    private bool TryGetIdsFromContext(ActionExecutingContext context, out Guid documentId, out Guid accountId)
    {
        var hasItems = context.HttpContext.Items.TryGetValue("documentId", out var documentIdObj) &
                       context.HttpContext.Items.TryGetValue("accountId", out var accountIdObj);

        if (!hasItems || documentIdObj == null || accountIdObj == null)
        {
            documentId = Guid.Empty;
            accountId = Guid.Empty;
            return false;
        }

        return Guid.TryParse(accountIdObj.ToString(), out accountId) &
               Guid.TryParse(documentIdObj.ToString(), out documentId);
    }

    private async Task<bool> HasRequiredPermission(Guid documentId, Guid accountId)
    {
        if (await documentAccessService.IsAuthorAsync(documentId, accountId))
            return true;

        var permissionId = await documentAccessService.GetUserPermissionAsync(documentId, accountId);
        
        return permissionId >= (int)requiredPermissionType;
    }
}