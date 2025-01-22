using System.Web;
using API.Contracts;
using API.Extensions;
using API.Filters;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MarkdownController(MdService mdService): ControllerBase
{
    [ServiceFilter(typeof(UserExistsFilter))]
    [HttpPost("convert")]
    public async Task<IActionResult> GetHtml([FromBody] MarkdownRequest request)
    {
        var htmlResult = await mdService.ConvertToHtmlAsync(request.RawMd!);
        
        return htmlResult.IsSuccess 
            ? Ok(new {Html = htmlResult.Data})
            : BadRequest(new { Error = htmlResult.ErrorMessage });
    }
    
    [ServiceFilter(typeof(UserExistsFilter))]
    [ServiceFilter(typeof(DocumentExistsFilter))]
    [ServiceFilter(typeof(ValidateAuthorFilter))]
    [HttpPost("save/{documentId:guid}")]
    public async Task<IActionResult> SaveDocumentAsync(Guid documentId, [FromBody] string mdContent)
    {
        var saveResult = await mdService.SaveMarkdownAsync(documentId, mdContent);
        
        return saveResult.IsSuccess
            ? Ok()
            : BadRequest(new { Error = saveResult.ErrorMessage });
    }
    
    [ServiceFilter(typeof(UserExistsFilter))]
    [ServiceFilter(typeof(DocumentExistsFilter))]
    [ServiceFilter(typeof(ValidateAuthorFilter))]
    [HttpGet("get")]
    public async Task<IActionResult> GetMarkdownFile([FromQuery] string documentId)
    {
        var parsedDocumentId = Guid.Parse(documentId);
        var getResult = await mdService.GetMarkdownAsync(parsedDocumentId);

        return getResult.IsSuccess
            ? Ok(new { Content = getResult.Data })
            : BadRequest(new { Error = getResult.ErrorMessage });
    }
}