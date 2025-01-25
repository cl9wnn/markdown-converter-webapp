using API.Attributes;
using API.Contracts;
using API.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DocumentAccessController(IDocumentAccessService documentAccessService, IAccountService accountService): ControllerBase
{
    
    [ServiceFilter(typeof(UserExistsFilter))]
    [ServiceFilter(typeof(DocumentExistsFilter))]
    [ValidatePermission(RequiredAccessLevel.Author)]
    [HttpPost("{documentId:guid}/set-permission")]
    public async Task<IActionResult> SetPermissionAsync(Guid documentId, [FromBody] SetPermissionRequest request)
    {
        if (!Enum.IsDefined(typeof(PermissionType), request.PermissionType) || request.Email == null)
            return BadRequest(new { Error = "Invalid permissionType or email" });
                
        var accountResult = await accountService.GetAccountIdByEmailAsync(request.Email!);

        if (!accountResult.IsSuccess)
        {
            return BadRequest(new { Error = accountResult.ErrorMessage });
        }
        
        var setPermissionResult = await documentAccessService
            .SetUserPermissionAsync(request.PermissionType, documentId, accountResult.Data);
        
        return setPermissionResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = setPermissionResult.ErrorMessage });
    }

    [ServiceFilter(typeof(UserExistsFilter))]
    [ServiceFilter(typeof(DocumentExistsFilter))]
    [ValidatePermission(RequiredAccessLevel.Author)]
    [HttpGet("{documentId:guid}/get-permission")]
    public async Task<IActionResult> GetDocumentPermissionListAsync(Guid documentId)
    {
        var getResult = await documentAccessService.GetDocumentPermissionListAsync(documentId);
        
        return getResult.IsSuccess
            ? Ok(new {Permissons = getResult.Data} )
            : BadRequest(new { Error = getResult.ErrorMessage });
    }
    
    [ServiceFilter(typeof(UserExistsFilter))]
    [ServiceFilter(typeof(DocumentExistsFilter))]
    [ValidatePermission(RequiredAccessLevel.Author)]
    [HttpPost("{documentId:guid}/clear-permission")]
    public async Task<IActionResult> ClearDocumentPermissionAsync(Guid documentId, [FromBody] string email)
    {
        var clearResult = await documentAccessService.ClearPermissionAsync(documentId, email);
        
        return clearResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = clearResult.ErrorMessage });
    }
}