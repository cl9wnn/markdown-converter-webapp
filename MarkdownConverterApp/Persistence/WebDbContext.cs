using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Entities;

namespace Persistence;

public class WebDbContext(DbContextOptions<WebDbContext> options): DbContext(options)
{ 
    public DbSet<AccountEntity> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}