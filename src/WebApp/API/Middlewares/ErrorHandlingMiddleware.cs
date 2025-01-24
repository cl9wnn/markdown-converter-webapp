namespace API.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);
        
        if (context.Response.StatusCode == 404)
        {
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync("wwwroot/ErrorPages/404.html"); 
        }
    }
}