using API.Contracts;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController(DocumentsService documentsService): ControllerBase
{
    
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateDocumentAsync([FromBody] SaveProjectRequest? request)
    {
        if (request?.Name == null || request.MdContent == null)
            return BadRequest(new { Error = "Invalid request" });
        
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        
        var createResult = await documentsService.CreateAsync(userId, request.Name, request.MdContent);
        
        return createResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = createResult.ErrorMessage });
    }
}