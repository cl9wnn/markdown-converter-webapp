using API.Contracts;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarkdownController(MdService mdService): ControllerBase
{
    [HttpPost("convert")]
    public async Task<IActionResult> GetHtml([FromBody] MarkdownRequest request)
    {
        var htmlResult = await mdService.ConvertToHtmlAsync(request.RawMd!);
        
        return htmlResult.IsSuccess 
            ? Ok(new {Html = htmlResult.Data})
            : BadRequest(new { Error = htmlResult.ErrorMessage });
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> SaveDocumentAsync([FromBody] SaveProjectRequest request)
    {
        if (!Guid.TryParse(request.DocumentId, out var documentId) || request.MdContent == null)
            return BadRequest(new { Error = "Invalid request" });
        
        var saveResult = await mdService.SaveMarkdownAsync(documentId, request.MdContent);
        
        return saveResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = saveResult.ErrorMessage });
    }
    
    
    [HttpGet("get")]
    public async Task<IActionResult> GetMarkdownFile([FromQuery] string documentId)
    {
        if (!Guid.TryParse(documentId, out var parsedDocumentId))
            return BadRequest(new { Error = "Invalid request" });

        var getResult = await mdService.GetMarkdownAsync(parsedDocumentId);

        return getResult.IsSuccess
            ? Ok(new { Content = getResult.Data })
            : BadRequest(new { Error = getResult.ErrorMessage });
    }
}