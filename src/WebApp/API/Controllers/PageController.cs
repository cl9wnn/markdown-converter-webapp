using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class PageController: ControllerBase
{
    [HttpGet("/documents/{documentId:guid}")]
    public IActionResult ShowMainPage()
    {
        return File("EditingPage/index.html", "text/html");
    }

    [HttpGet("/")]
    public IActionResult ShowStartPage()
    {
        return File("StartPage/index.html", "text/html");
    }
    
    [HttpGet("/documents")]
    public IActionResult ShowDocumentsPage()
    {
        return File("DocumentsPage/index.html", "text/html");
    }
}