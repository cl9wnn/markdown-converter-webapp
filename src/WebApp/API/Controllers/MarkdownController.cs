using API.Contracts;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class MarkdownController(MdService mdService): ControllerBase
{
    [Authorize]
    [HttpPost("api/convert")]
    public async Task<IActionResult> GetHtml([FromBody] MarkdownRequest request)
    {
        var htmlResult = await mdService.ConvertToHtmlAsync(request.RawMd!);
        
        return htmlResult.IsSuccess 
            ? Ok(new {Html = htmlResult.Data})
            : BadRequest(new { Error = htmlResult.ErrorMessage });
    }
}