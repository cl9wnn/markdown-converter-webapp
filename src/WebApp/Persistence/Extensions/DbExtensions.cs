using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Persistence.Extensions;

public static class DbExtensions
{
    public static void AddDataBase(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresConnection");

        serviceCollection.AddDbContext<WebDbContext>(o =>
        {
            o.UseNpgsql(connectionString);
        });
    }

    public static void AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnection = configuration.GetConnectionString("MongoConnection");

        services.AddSingleton<IMongoClient>(serviceProvider => new MongoClient(mongoConnection));

        services.AddSingleton<IMongoDatabase>(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            var databaseName = new MongoUrl(mongoConnection).DatabaseName 
                               ?? throw new InvalidOperationException("Database name not found in MongoConnection string.");
            
            return client.GetDatabase(databaseName);
        });
    }
    
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();

        if (redisSettings == null || string.IsNullOrEmpty(redisSettings.Host))
            throw new ArgumentException("Redis settings are missing or invalid.");

        var redisConfiguration = $"{redisSettings.Host}:{redisSettings.Port}";

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration;
            options.InstanceName = "MdProcessor_"; 
        });
    }
}