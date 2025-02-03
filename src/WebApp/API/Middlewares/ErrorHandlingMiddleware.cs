namespace API.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);

            if (context.Response.StatusCode == 404)
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/ErrorPages/404.html");
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync("wwwroot/ErrorPages/500.html");

            logger.LogError(ex, "An unhandled exception has occurred.");
        }
    }
}