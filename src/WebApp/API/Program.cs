using Application.Models;
using Application.Services;
using Core.interfaces;
using Persistence;
using Persistence.Repositories;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<MdService>();

builder.Services.AddScoped<JwtService>();

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddMdProcessor();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
