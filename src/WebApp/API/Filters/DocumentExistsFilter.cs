using System.Text.Json;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace API.Filters;

public class DocumentExistsFilter(IDocumentsService documentsService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.RouteValues.TryGetValue("documentId", out var documentIdValue) && 
            await IsDocumentExistsAsync(context, documentIdValue!.ToString()!))
        {
            await next();
            return;
        }
    
        if (context.HttpContext.Request.Query.TryGetValue("documentId", out var documentIdQueryValue) && 
            await IsDocumentExistsAsync(context, documentIdQueryValue!.ToString()!))
        {
            await next(); 
            return;
        }
    
        if (context.HttpContext.Request.ContentLength > 0)
        {
            var documentIdFromBody = await GetDocumentIdFromBodyAsync(context);
            if (documentIdFromBody != null && await IsDocumentExistsAsync(context, documentIdFromBody))
            {
                await next(); 
                return;
            }
        }
        
        context.Result = new BadRequestObjectResult(new { Error = "Document not found" });
    }

    private async Task<bool> IsDocumentExistsAsync(ActionExecutingContext context, string requestDocumentId)
    {
        if (!Guid.TryParse(requestDocumentId, out var documentId))
            return false;

        var isExists = await documentsService.IsDocumentExistsAsync(documentId);
        
        if (!isExists)
            return false;
        
        context.HttpContext.Items.Add("documentId", documentId);
        return true;
    }
    
    private async Task<string?> GetDocumentIdFromBodyAsync(ActionExecutingContext context)
    {
        try
        {
            context.HttpContext.Request.EnableBuffering(); 
            using var reader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.HttpContext.Request.Body.Position = 0; 

            using var json = JsonDocument.Parse(body);
            if (json.RootElement.TryGetProperty("DocumentId", out var documentIdProperty))
            {
                return documentIdProperty.GetString();
            }
        }
        catch
        {
            // ignored
        }

        return null;
    }
}