namespace API.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
   public async Task InvokeAsync(HttpContext context)
   {
      context.Request.EnableBuffering();
      var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
      context.Request.Body.Position = 0;

      var headers = context.Request.Headers
         .Select(header => $"{header.Key} = {header.Value}")
         .Aggregate((current, next) => current + Environment.NewLine + next);

      logger.LogInformation(
         "Incoming Request: {method} {url} {newline}" +
         "Request Headers: {newline} {headers} {newline}" +
         "Request Body: {body}",
         context.Request.Method,
         context.Request.Path,
         Environment.NewLine,
         Environment.NewLine,
         headers,
         Environment.NewLine,
         body
      );

      await next(context);
   }
}