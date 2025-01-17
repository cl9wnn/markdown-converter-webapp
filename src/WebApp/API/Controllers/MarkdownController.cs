using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class MarkdownController: ControllerBase
{
    [Authorize]
    [HttpPost("api/convert")]
    public async Task<IActionResult> ConvertMdToHtmlAsync([FromBody] MarkdownRequest request)
    {
        return Ok();
    }
}