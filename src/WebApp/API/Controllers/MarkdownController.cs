using API.Attributes;
using API.Contracts;
using Application.Interfaces;
using Core.Enums;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Authorize]
[IsUserExists]
[IsDocumentExists]
[Route("api/[controller]")]
public class MarkdownController(IMdService mdService): ControllerBase
{
    [ValidatePermission(RequiredAccessLevel.Reader)]
    [HttpPost("convert/{documentId:guid}")]
    public async Task<IActionResult> GetHtml(Guid documentId, [FromBody] MarkdownRequest request)
    {
        var htmlResult = await mdService.ConvertToHtmlAsync(request.RawMd!);
        
        return htmlResult.IsSuccess 
            ? Ok(new {Html = htmlResult.Data})
            : BadRequest(new { Error = htmlResult.ErrorMessage });
    }
    
    [ValidatePermission(RequiredAccessLevel.Editor)]
    [HttpPost("save/{documentId:guid}")]
    public async Task<IActionResult> SaveDocumentAsync(Guid documentId, [FromBody] string mdContent)
    {
        var saveResult = await mdService.SaveMarkdownAsync(documentId, mdContent);
        
        return saveResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = saveResult.ErrorMessage });
    }
    
    [ValidatePermission(RequiredAccessLevel.Reader)]
    [HttpGet("get/{documentId:guid}")]
    public async Task<IActionResult> GetMarkdownFile(Guid documentId)
    {
        var getResult = await mdService.GetMarkdownAsync(documentId);

        return getResult.IsSuccess
            ? Ok(new { Content = getResult.Data })
            : BadRequest(new { Error = getResult.ErrorMessage });
    }
}