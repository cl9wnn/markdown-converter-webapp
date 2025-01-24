using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class PageController: ControllerBase
{
    
    [HttpGet("/")]
    public IActionResult ShowStartPage()
    {
        return File("StartPage/index.html", "text/html");
    }
    
    [HttpGet("/documents/{documentId:guid}")]
    public IActionResult ShowEditingPage()
    {
        return File("EditingPage/index.html", "text/html");
    }
    
    [HttpGet("/documents")]
    public IActionResult ShowDocumentsPage()
    {
        return File("DocumentsPage/index.html", "text/html");
    }
    
    [HttpGet("/forbidden")]
    public IActionResult ShowForbiddenPage()
    {
        return File("ErrorPages/403.html", "text/html");
    }
}