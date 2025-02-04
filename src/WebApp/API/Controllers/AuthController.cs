using API.Contracts;
using API.Validators;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAccountService accountService): ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest? request)
    {
        if (request == null)
            return BadRequest("Invalid request");
        
        var validator = new UserValidator();
        var validationResult = await validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(errors => errors.ErrorMessage));
        
        var registerResult = await accountService.RegisterAsync(request.Email!, request.Password!, request.FirstName!);

        return registerResult.IsSuccess 
            ? Ok(new { Token = registerResult.Data })
            : BadRequest(new { Error = registerResult.ErrorMessage });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest? request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return Unauthorized();
        
        var tokenResult = await accountService.LoginAsync(request.Email, request.Password);

        if (!tokenResult!.IsSuccess)
            return BadRequest(new { Error = tokenResult.ErrorMessage }); 
        
        return Ok(new { Token = tokenResult.Data });
    }
}