using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class PageController: ControllerBase
{
    [HttpGet("/")]
    public IActionResult ShowMainPage()
    {
        return File("MainPage/index.html", "text/html");
    }
    
    [HttpGet("/documents")]
    public IActionResult ShowDocumentsPage()
    {
        return File("DocumentsPage/index.html", "text/html");
    }
}