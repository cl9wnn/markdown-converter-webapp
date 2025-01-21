using API.Contracts;
using Application.Services;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DocumentsController(DocumentsService documentsService): ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateDocumentAsync([FromBody] string name)
    {
        var accountIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
        
        if (!Guid.TryParse(accountIdClaim, out var accountId))
            return Unauthorized();
        
        var createResult = await documentsService.CreateProjectAsync(accountId, name);
        
        return createResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = createResult.ErrorMessage });
    }
    
    [HttpGet("get")]
    public async Task<IActionResult> GetDocumentsAsync()
    {
        var accountIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
        
        if (!Guid.TryParse(accountIdClaim, out var accountId))
            return Unauthorized();
        
        var getResult = await documentsService.GetUserProjectsAsync(accountId);
        
        return getResult.IsSuccess
            ? Ok(new {Projects = getResult.Data})
            : BadRequest(new { Error = getResult.ErrorMessage });
    }
    
    [HttpGet("{documentId:guid}")]
    public async Task<IActionResult> GetDocumentAsync(Guid documentId)
    {
        var getDocumentResult = await documentsService.GetProjectAsync(documentId);
        
        return getDocumentResult.IsSuccess
            ? Ok(new {Project = getDocumentResult.Data})
            : BadRequest(new { Error = getDocumentResult.ErrorMessage });
    }

    
    [HttpPost("rename")]
    public async Task<IActionResult> RenameDocumentAsync([FromBody] RenameProjectRequest request)
    {
        if (request?.DocumentId == null || request.NewName == null)
            return BadRequest(new { Error = "Invalid request"});

        var renameResult = await documentsService.RenameProjectAsync(request.DocumentId, request.NewName!);
        
        return renameResult.IsSuccess
            ? Ok(new {NewName = renameResult.Data})
            : BadRequest(new { Error = renameResult.ErrorMessage });
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteDocumentAsync([FromBody] string projectIdRequest)
    {
        if (!Guid.TryParse(projectIdRequest, out var projectId))
            return BadRequest(new {Error = "Invalid request"} );
        
        var deleteResult = await documentsService.DeleteProjectAsync(projectId);
        
        return deleteResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = deleteResult.ErrorMessage });
    }
}