using System.Text;
using API.Filters;
using Application.Models;
using Markdown.Abstract_classes;
using Markdown.Classes;
using Markdown.Classes.Parsers;
using Markdown.Classes.Renderers;
using Markdown.Classes.TagFactory;
using Markdown.Classes.Tags;
using Markdown.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions;

public static class ApiExtensions
{
    public static void AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        
        var authSettings = configuration.GetSection("AuthSettings").Get<AuthSettings>();

        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings!.SecretKey!))
                };

                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authorizationHeader = context.Request.Headers["Authorization"].ToString();
                        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = authorizationHeader.Substring("Bearer ".Length).Trim();
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void AddFilters(this IServiceCollection services)
    {
        services.AddScoped<UserExistsFilter>();
        services.AddScoped<DocumentExistsFilter>();
    }
    
    public static void AddMdProcessor(this IServiceCollection services)
    {
        
        services.AddSingleton<IEnumerable<TagElement>>(new List<TagElement>
        {
            new HeaderTag(),
            new BoldTag(),
            new ItalicTag(),
            new MarkedListTag(),
        });
        
        services.AddSingleton<DoubleTagFactory>();
        services.AddSingleton<SingleTagFactory>();


        services.AddSingleton<LineRenderer>();
        services.AddSingleton<ListRenderer>();

        services.AddSingleton<IParser<Token>, TokenParser>();
        services.AddSingleton<IParser<Line>, LineParser>();

        services.AddSingleton<IRenderer, HtmlRenderer>();

        services.AddSingleton<IMarkdownProcessor,MarkdownProcessor>();
    }
    
    public static void ApplyMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        try
        {
            var context = serviceProvider.GetRequiredService<WebDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ошибка при применении миграций.");
        }
    }
}