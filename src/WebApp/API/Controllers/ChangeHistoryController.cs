using API.Attributes;
using Application.Interfaces;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[IsUserExists]
[IsDocumentExists]
[Route("api/documents")]
public class ChangeHistoryController(IChangeHistoryService changeHistoryService): ControllerBase
{
    
    [ValidatePermission(RequiredAccessLevel.Editor)]
    [HttpGet("{documentId:guid}/history")]
    public async Task<IActionResult> GetChangeHistory(Guid documentId)
    {
        var getResult = await changeHistoryService.GetChangeHistoryAsync(documentId);
        
        return getResult.IsSuccess
            ? Ok(new {changeHistory = getResult.Data})
            : BadRequest(new { Error = getResult.ErrorMessage });
    }
    
    [ValidatePermission(RequiredAccessLevel.Author)]
    [HttpPost("{documentId:guid}/history")]
    public async Task<IActionResult> ClearChangeHistory(Guid documentId)
    {
        var deleteResult = await changeHistoryService.ClearChangeHistoryAsync(documentId);
        
        return deleteResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = deleteResult.ErrorMessage });
    }
}