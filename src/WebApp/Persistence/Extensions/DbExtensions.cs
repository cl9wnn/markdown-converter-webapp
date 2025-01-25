using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Persistence;

public static class DbExtensions
{
    public static IServiceCollection AddDataBase(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        serviceCollection.AddDbContext<WebDbContext>(o =>
        {
            o.UseNpgsql(connectionString);
        });
        return serviceCollection;
    }
}