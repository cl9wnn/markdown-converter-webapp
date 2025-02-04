using Application.Models;
using Application.Services;
using Persistence;
using Persistence.Repositories;
using API.Extensions;
using API.Middlewares;
using Application.Interfaces;
using Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IDocumentsService, DocumentsService>();
builder.Services.AddScoped<IDocumentsRepository, DocumentsRepository>();

builder.Services.AddScoped<IDocumentAccessService, DocumentAccessService>();
builder.Services.AddScoped<IDocumentAccessRepository, DocumentAccessRepository>();

builder.Services.AddScoped<IMdService, MdService>();

builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<MinioService>();

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.Configure<MinIoSettings>(builder.Configuration.GetSection("MinIoSettings"));

builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddMdProcessor();
builder.Services.AddFilters();

var app = builder.Build();

app.Services.ApplyMigrations();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();

app.Run();