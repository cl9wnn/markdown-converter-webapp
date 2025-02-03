using API.Attributes;
using API.Extensions;
using API.Filters;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;
namespace API.Controllers;

[ApiController]
[Authorize]
[IsUserExists]
[Route("api/[controller]")]
public class DocumentsController(IDocumentsService documentsService): ControllerBase
{
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateDocumentAsync([FromBody] string name)
    {
        var accountId = HttpContext.GetAccountId();
        
        var createResult = await documentsService.CreateProjectAsync(accountId, name);
        
        return createResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = createResult.ErrorMessage });
    }
    
    [HttpGet("get")]
    public async Task<IActionResult> GetAllDocumentsAsync()
    {
        var accountId = HttpContext.GetAccountId();
        
        var getResult = await documentsService.GetUserDocumentsAsync(accountId);
        
        return getResult.IsSuccess
            ? Ok(new {Projects = getResult.Data})
            : BadRequest(new { Error = getResult.ErrorMessage });
    }

    [IsDocumentExists]
    [ValidatePermission(RequiredAccessLevel.Reader)]
    [HttpGet("{documentId:guid}")]
    public async Task<IActionResult> GetDocumentAsync(Guid documentId)
    {
        var getDocumentResult = await documentsService.GetDocumentAsync(documentId);
        
        return getDocumentResult.IsSuccess
            ? Ok(new {Project = getDocumentResult.Data})
            : BadRequest(new { Error = getDocumentResult.ErrorMessage });
    }
    
    [IsDocumentExists]
    [ValidatePermission(RequiredAccessLevel.Editor)]
    [HttpPost("{documentId:guid}/rename")]
    public async Task<IActionResult> RenameDocumentAsync(Guid documentId, [FromBody] string newName)
    {
        var renameResult = await documentsService.RenameProjectAsync(documentId, newName!);
        
        return renameResult.IsSuccess
            ? Ok(new {NewName = renameResult.Data})
            : BadRequest(new { Error = renameResult.ErrorMessage });
    }
    
    [IsDocumentExists]
    [ValidatePermission(RequiredAccessLevel.Author)]
    [HttpDelete("{documentId:guid}/delete")]
    public async Task<IActionResult> DeleteDocumentAsync(Guid documentId)
    {
        var deleteResult = await documentsService.DeleteProjectAsync(documentId);
        
        return deleteResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = deleteResult.ErrorMessage });
    }
}